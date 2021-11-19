using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Entidades;
using LogicaNegocio;

namespace Interfaz
{
    public partial class FrmClientes : Form
    {

        Cliente clienteRegistrado;//sirve para cargar informacion del cliente en el formulario asi se sabe si tiene algo se modificaria la info y si esta vacio seria un objeto nuevo
        FrmBuscarClientes formBuscar;
        public FrmClientes()
        {
            InitializeComponent();
        }
        //metodo de limpiar
        private void Limpiar() {

            TxtIdCliente.Text = string.Empty;
            TxtNombre.Text = string.Empty;
            TxtTelefono.Text = string.Empty;
            TxtDireccion.Text = string.Empty;
            TxtNombre.Focus();

        }

        private Cliente GenerarEntidad() { //genera entidad para crear o modificar la informacion 
            Cliente cliente;
            if (!string.IsNullOrEmpty(TxtIdCliente.Text))
            {
                cliente = clienteRegistrado;
            }
            else 
            {
                cliente = new Cliente();
            }

            cliente.Nombre = TxtNombre.Text;
            cliente.Telefono = TxtTelefono.Text;
            cliente.Direccion = TxtDireccion.Text;
            return cliente;
        }

        private void CargarListaArray(string condicion = "")
        { //carga el datagridview con la informacion de la lista 
            LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);
            List<Cliente> clientes;

            try 
            {
                clientes = logica.ListarClientes(condicion);
                if (clientes.Count>0) //si la lista tiene algo entonces...
                {
                    GrdLista.DataSource = clientes;//cargue en el datagridview lo que tiene la lista
                }
            } 
            catch (Exception ex) 
            {
                throw;
            }

        }

        private void CargarListaDataSet(string condicion = "", string orden = "") 
        { //carga el datagridview con el dataset
            LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);
            DataSet DSClientes;

            try 
            {
                DSClientes = logica.ListarClientes(condicion, orden);
                if (DSClientes.Tables["Clientes"].Rows.Count > 0) //si tiene algo el data set entonces carguelo en el datagridview
                {
                    GrdLista.DataSource = DSClientes;
                    GrdLista.DataMember = DSClientes.Tables["Clientes"].TableName;//en la tabla con nombre Clientes del dataset
                }
            } 
            catch (Exception ex) 
            {
                throw;
            }

            //foreach (DataRow row in DSClientes.Tables[0].Rows) {  ASI SE RECORRE UN DATA SET CON UN FOR EACH
            //    MessageBox.Show(row[0].ToString());
            //}

            //for (int i = 0; i < DSClientes.Tables["Clientes"].Rows.Count; i++) {       ASI SE RECORRE UN DATA SET CON UN FOR CONVENCIONAL
            //    MessageBox.Show(DSClientes.Tables[0].Rows[i][0].ToString());
            //}


        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Cliente entidad = new Cliente(-1, TxtNombre.Text, TxtTelefono.Text, TxtDireccion.Text);
            //LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);
            //logica.Insertar(entidad);

            LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);
            Cliente cliente;
            int resultado;
            try 
            {
                if (!string.IsNullOrEmpty(TxtNombre.Text) &&
                    !string.IsNullOrEmpty(TxtTelefono.Text) &&
                    !string.IsNullOrEmpty(TxtDireccion.Text))
                {
                    cliente = GenerarEntidad();
                    if (!cliente.Existe)
                    {
                        resultado = logica.Insertar(cliente);
                    }
                    else
                    {
                        resultado = logica.Modificar(cliente);
                    }
                    if (resultado > 0)
                    {
                        Limpiar();
                        MessageBox.Show("Operacion Realizada con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarListaDataSet();
                    }
                    else 
                    {
                        MessageBox.Show("No se realizó ninguna modificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else 
                {
                    MessageBox.Show("Los datos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void CargarCliente(int id) {
            Cliente cliente;
            LogicaCliente traerCliente = new LogicaCliente(Configuracion.getConnectionString);
            try
            {
                cliente = traerCliente.ObtenerCliente(id);
                if (cliente != null)
                {
                    TxtIdCliente.Text = cliente.Id_Cliente.ToString();
                    TxtNombre.Text = cliente.Nombre;
                    TxtTelefono.Text = cliente.Telefono;
                    TxtDireccion.Text = cliente.Direccion;
                    clienteRegistrado = cliente; //se setea el cliente registrado para poder darle la opcion al usuario de modificar la informacion del cliente
                }
                else 
                {
                    MessageBox.Show("El cliente no se encuentra en la base de datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarListaDataSet(); //en el load del formulario se llama a carga el gridview con el data set //vuelve a cargar la lista para que vea que el cliente de verdad no esta
                }
            } 
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            try 
            {
                CargarListaDataSet(); //en el load del formulario se llama a carga el gridview con el data set
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GrdLista_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = 0;
            try
            {
                
                id = (int)GrdLista.SelectedRows[0].Cells[0].Value;//se recupero el ID
                CargarCliente(id);//ya con el ID recuperado se puede llamar a la funcion que carga el cliente desde la base de datos en el formulario
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Cliente cliente;
            int resultado;
            LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);

            try
            {
                if (!string.IsNullOrEmpty(TxtIdCliente.Text))
                {
                    cliente = logica.ObtenerCliente(int.Parse(TxtIdCliente.Text));//busca primero el cliente antes de borrarlo para ver si existe
                    if (cliente != null) 
                    {
                        resultado = logica.EliminarConSP(cliente);//si el cliente no es nulo puede borrarlo

                        MessageBox.Show(logica.Mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information); //imprime el mensaje que el SP mandó
                        Limpiar();
                        CargarListaDataSet();
                    }
                    else
                    {
                        MessageBox.Show("El cliente no existe", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Limpiar();
                        CargarListaDataSet();
                    }
                }
                else
                {
                    MessageBox.Show("Debe Seleccionar un cliente antes de eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            formBuscar = new FrmBuscarClientes();
            formBuscar.Aceptar += new EventHandler(Aceptar);//se especifica que se quiere usar el evento ACEPTAR
            formBuscar.ShowDialog();
        }

        private void Aceptar(object id, EventArgs e)//implementa el evento aceptar y recibe un id el cual se manda desde el formulario que se abre y aqui se carga el cliente
        {
            try
            {
                int idCliente = (int)id;
                if (idCliente != -1)
                {
                    CargarCliente(idCliente);
                }
                else
                {
                    Limpiar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
