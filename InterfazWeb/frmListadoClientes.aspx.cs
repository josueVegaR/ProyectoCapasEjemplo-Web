using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Entidades;
using LogicaNegocio;
using System.Configuration;

namespace InterfazWeb
{
    public partial class frmListadoClientes : System.Web.UI.Page
    {
        //******** MÉTODOS:
        private void Limpiar()
        {
            txtNombre.Text = string.Empty;
            txtNombre.Focus();
        }
        private void CargarClientes(string condicion="")
        {
            DataSet DS;
            LogicaCliente logicacliente = new LogicaCliente(Config.getConnectionString);
            try
            {
                DS = logicacliente.ListarClientes(condicion, "");

                //Se verifica que el DS tenga algo:
                if (DS != null)
                {
                    GrdLista.DataSource = DS;
                    GrdLista.DataMember = DS.Tables[0].TableName;
                    //Para que se muestren los datos en pantalla:
                    GrdLista.DataBind();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BorrarCliente(int id)
        {
            LogicaCliente logicaCliente = new LogicaCliente(Config.getConnectionString);
            Cliente cliente = new Cliente();
            try
            {
                cliente.Id_Cliente = id;
                logicaCliente.EliminarConSP(cliente);
                Session["_mensaje"] = logicaCliente.Mensaje; // Aquí ya la lógica tiene lo que le devolvió el SP.
                Limpiar();
                CargarClientes();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //El evento Load se carga cuando se abre la página por primera vez, así como 
            // cuando se hace postback.
            // Si se llama el método CargarClientes sin el "isPostBack", entonces se carga siempre.
            try
            {
                if (!IsPostBack)
                {
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                // Mensaje para mostrar el usuario.
                Session["_mensaje"] = $"Error al cargar los clientes{ex.Message}";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string condicion = $"Nombre like '%{txtNombre.Text}%'";
                CargarClientes(condicion);
            }
            catch (Exception ex)
            {
                // Mensaje para mostrar el usuario.
                Session["_mensaje"] = $"Error al cargar los clientes{ex.Message}";
            }
        }

        protected void GrdLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdLista.PageIndex = e.NewPageIndex;
                // e es el parámetro del evento que me dice en qué página estoy actualmente.
                
                CargarClientes();
            }
            catch (Exception ex)
            {
                // Mensaje para mostrar el usuario.
                Session["_mensaje"] = $"Error al cargar los clientes{ex.Message}";
            }
        }

        protected void lnkEliminar_Command(object sender, CommandEventArgs e) //mediante la variable "e", leo el id:
        {
            try
            {
                BorrarCliente(int.Parse(e.CommandArgument.ToString()));
            }
            catch (Exception ex)
            {
                Session["_mensaje"] = $"Error al eliminar el cliente {ex.Message}";
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //Una forma de hacerlo sería así:
            //Session["_mensaje"] = null;
            // Sin embargo, se utiliza el método Remove:
            Session.Remove("Id_Cliente");

            //Si se tenía una variable llamada Id_Cliente, se borra del todo la variable.

            Response.Redirect("frmClientes.aspx");
        }

        protected void lnkModificar_Command(object sender, CommandEventArgs e)
        {
            Session["Id_Cliente"] = e.CommandArgument.ToString(); // Se recupera el ID cliente del parámetro "e". Se guarda el la variable de Sesión Id_Cliente.
            Response.Redirect("frmClientes.aspx");
        }
    }
}