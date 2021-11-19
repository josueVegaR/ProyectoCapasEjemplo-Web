using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using LogicaNegocio;
using System.Data;

namespace InterfazWeb
{
    public partial class frmReserva : System.Web.UI.Page
    {
        private void CargarClientes(string condicion = "")
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

        private void LimpiarCampos()
        {
            txtidSeleccionado.Text = string.Empty;
            txtcliente.Text = string.Empty;
            txtnombrecliente.Text = string.Empty;
            txtfechaActual.Text = DateTime.Today.ToString("d");
            txtfechaI.Text = string.Empty;
            txtfechaS.Text = string.Empty;
            txtpersonas.Text = string.Empty;
            txtnumreserva.Text = string.Empty;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EntidadReservacion reservacion;
            LogicaReservaciones logicaReservaciones = new LogicaReservaciones(Config.getConnectionString);
            try
            {
                if (!IsPostBack)
                {
                    CargarClientes();
                    Session["_mensaje"] = null;
                    if (Session["Id_Reserva"] != null)
                    {
                        reservacion = logicaReservaciones.ObtenerReserva($"NUMRESERVACION = {Session["Id_Reserva"]}");
                        txtnumreserva.Text = reservacion.NumReservacion.ToString();
                        txtidSeleccionado.Text = reservacion.Cliente.Id_Cliente.ToString();
                        txtcliente.Text = reservacion.Cliente.Nombre;
                        txtfechaI.Text = reservacion.FechaIngreso.ToString("yyyy-MM-dd"); //Formato para Fechas 
                        txtfechaS.Text = reservacion.FechaSalida.ToString("yyyy-MM-dd");
                        txtpersonas.Text = reservacion.CantidadPersonas.ToString();
                        cbotipo.SelectedValue = reservacion.TipoHabitacion;
                        txtfechaActual.Text = DateTime.Today.ToString("d"); // Formato de fecha corta.
                    }
                    else
                    {
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception)
            {
                Session["_mensaje"] = "Error al cargar la reserva.";
                Response.Redirect("frmListadoReservas.aspx");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string condicion = $"nombre like '%{txtnombrecliente.Text.Trim()}%'";
                CargarClientes(condicion);

                //La clase Script Manager me permite correr un archivo/código JS que quiera ejecutar desde el servidor:
                string javaScript = "AbrirModal();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);
            }
            catch (Exception)
            {
                Session["_mensaje"] = "Error al buscar el cliente.";
            }
        }

        protected void lnkSeleccionar_Command(object sender, CommandEventArgs e)
        {
            //Se declara una variable para recuperar el ID del command Argument
            int id = int.Parse(e.CommandArgument.ToString());
            Cliente cliente;
            LogicaCliente logicaCliente = new LogicaCliente(Config.getConnectionString);

            try
            {
                cliente = logicaCliente.ObtenerCliente(id);
                if (cliente != null)
                {
                    txtidSeleccionado.Text = cliente.Id_Cliente.ToString();
                    txtcliente.Text = cliente.Nombre;
                }
            }
            catch (Exception)
            {
                Session["_mensaje"] = "Error al seleccionar el cliente.";
            }


        }
    }
}