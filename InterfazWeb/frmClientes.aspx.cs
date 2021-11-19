using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using LogicaNegocio;

namespace InterfazWeb
{
    public partial class frmClientes : System.Web.UI.Page
    {
        //Métodos ************

        private void Limpiar()
        {
            txtIDCliente.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
        }

        //Se crea un método para generar un cliente que luego voy a utilizar para borrar y eliminar el mismo:
        private Cliente GenerarEntidadCliente()
        {
            Cliente cliente = new Cliente(); //Se crea la instancia de la entidad Cliente que voy a utilizar para asignarle valores.

            //Verifico que en la sesión exista un cliente:
            if (Session["Id_Cliente"] != null)
            {
                cliente.Id_Cliente = int.Parse(Session["Id_Cliente"].ToString());
                cliente.Existe = true;
            }
            else
            {
                cliente.Id_Cliente = -1;
                cliente.Existe = false;
            }

            //Ahora asigno el nombre, teléfono y dirección a la nueva entidad.
            cliente.Nombre = txtNombre.Text;
            cliente.Telefono = txtTelefono.Text;
            cliente.Direccion = txtDireccion.Text;

            //Devuelvo el cliente:
            return cliente;
        }


        //********************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Variables:
                Cliente cliente;
                LogicaCliente logicaCliente = new LogicaCliente(Config.getConnectionString);
                int id; //Para guardar el ID de la entidad cliente.

                if (!IsPostBack) //solamente se ejecuta la primera vez que carga la página. Esto debido a que evito que haga algo si es un postback.
                {
                    Session["_mensaje"] = null;
                    // Si la variable de sesión tiene algo, se borra. Si no tiene nada la crea, o no hace nada si no existe.

                    if (Session["Id_Cliente"] != null)
                    {
                        id = int.Parse(Session["Id_Cliente"].ToString()); //Sacar el ID de la variable de Sesión
                        cliente = logicaCliente.ObtenerCliente(id); //Se asigna el cliente que me devuelve el método de la lógica.
                        
                        //Ahora verifico que lo que me haya devuelto el método de la lógica no sea un cliente nulo:
                        if (cliente != null)
                        {
                            //Si no es nulo, asigno valores a los campos del Form:
                            txtIDCliente.Text = cliente.Id_Cliente.ToString();
                            txtNombre.Text = cliente.Nombre;
                            txtTelefono.Text = cliente.Telefono;
                            txtDireccion.Text = cliente.Direccion;
                        }
                        else
                        {
                            Session["_mensaje"] = "Error al cargar el cliente.";
                        }
                    }
                    else
                    {
                        //Si la variable de Sesión Id_Cliente es nula, limpio.
                        Limpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                Session["_mensaje"] = $"Error al cargar los datos. {ex.Message}";
                Response.Redirect("frmListadoClientes.aspx");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Redireccionar la página al otro form.
            Response.Redirect("frmListadoClientes.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Hago el llamado a la entidad y a la Lógica:
            Cliente cliente;
            LogicaCliente logicaCliente = new LogicaCliente(Config.getConnectionString);
            int resultado; //Para guardar el ID devuelto por el método de la lógica:

            try
            {
                cliente = GenerarEntidadCliente();
                if (cliente.Existe) //Si existe lo modifico.
                {
                    resultado = logicaCliente.Modificar(cliente);
                }
                else // sino existe lo creo.
                {
                    resultado = logicaCliente.Insertar(cliente);
                }

                //La variable resultado guarda el valor que devuelve el método de la lógica. Se guarda el valor para recuperarlo, aunque no lo usamos.

                Session["_mensaje"] = "Operación realizada satisfactoriamente.";
                Response.Redirect("frmListadoClientes.aspx", false); // Acá se le pone el falso para que el sistema no crea que hubo una excepción.
            }
            catch (Exception ex)
            {
                Session["_mensaje"] = $"Error al guardar datos. {ex.Message}";
                Response.Redirect("frmListadoClientes.aspx");
            }
        }
    }
}