using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LogicaNegocio;
using Entidades;

namespace Interfaz
{
    public partial class FrmBuscarClientes : Form
    {

        //creacion un nuevo evento para el formulario

        public event EventHandler Aceptar;//se crea el evento nuevo

        int id_Cliente; //se declara globalmente para que tenga acceso


        public FrmBuscarClientes()
        {
            InitializeComponent();
        }



        private void CargarListaArray(string condicion = "")
        { //carga el datagridview con la informacion de la lista 
            LogicaCliente logica = new LogicaCliente(Configuracion.getConnectionString);
            List<Cliente> clientes;

            try
            {
                clientes = logica.ListarClientes(condicion);
                if (clientes.Count > 0) //si la lista tiene algo entonces...
                {
                    GrdLista.DataSource = clientes;//cargue en el datagridview lo que tiene la lista
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void Seleccionar() {  //metodo que permite seleccionar el id de la fila del datagridview que se escoja

            if (GrdLista.SelectedRows.Count > 0) //si ha seleccionado una fila
            {
                id_Cliente = (int)GrdLista.SelectedRows[0].Cells[0].Value;
                Aceptar(id_Cliente,null);//le manda el id al evento aceptar que esta en el otro form
                Close();
            }
        }

        private void FrmBuscarClientes_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    CargarListaArray();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string condicion = string.Empty; //condicion que se usara para realizar el filtrado en los datos para recuperar el cliente deseado
            try
            {
                if (!string.IsNullOrEmpty(txtNombre.Text))//si no esta vacio
                {
                    condicion = string.Format("Nombre like '%{0}%'", txtNombre.Text.Trim()); //donde en el nombre sea algo como lo que se escriba en el txtNombre el trim lo usa para quitar espacios
                }

                 CargarListaArray(condicion);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Seleccionar();
        }

        private void GrdLista_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Seleccionar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Aceptar(-1, null); //evento aceptar
            Close();
        }
    }
}
