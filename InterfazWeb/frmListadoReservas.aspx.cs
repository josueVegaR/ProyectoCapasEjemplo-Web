using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LogicaNegocio;
using Entidades;

namespace InterfazWeb
{
    public partial class frmListadoReservas : System.Web.UI.Page
    {
        //Métodos:
        private void CargarLista(string condicion = "") // esta lista carga las reservaciones que tienen el cancelado en 0.
            //se deben mostrar únicamente las reservas que no se han cancelado/pagado.
        {
            string condicionBase = "cancelada=0"; // Esta es la condición que no puede faltar, que no se haya facturado la reservación.
            DataSet DS;
            LogicaReservaciones logicaReservaciones = new LogicaReservaciones(Config.getConnectionString);

            try
            {
                if (!string.IsNullOrEmpty(condicion)) // Si la condición no está vacía:
                {
                    condicionBase = $"{condicionBase} and {condicion}";
                }
                DS = logicaReservaciones.ListarRegistros(condicionBase);
                if (DS != null)
                {
                    GrdLista.DataSource = DS;
                    GrdLista.DataMember = DS.Tables[0].TableName;
                    GrdLista.DataBind();
                }

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void Limpiar()
        {
            txtNombre.Text = string.Empty;
            txtnumreserva.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarLista();
                    Limpiar();
                }
            }
            catch (Exception)
            {

                Session["_mensaje"] = "No se registran reservas.";
            }
        }

        protected void GrdLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdLista.PageIndex = e.NewPageIndex;
                CargarLista();
            }
            catch (Exception ex)
            {
                Session["_mensaje"] = $"Error al cargar las reservas. {ex.Message}";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string condicion = $"nombre like '%{txtNombre.Text.Trim()}%' and NUMRESERVACION like'%{txtnumreserva.Text.Trim()}%'";
                CargarLista(condicion);
            }
            catch (Exception)
            {
                Session["_mensaje"] = "Error al buscar la reserva.";
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //No puede haber un ID de reserva en sesión, si tiene algo se borra.
            Session.Remove("Id_Reserva");
            //Se llama al formulario de reserva:
            Response.Redirect("frmReserva.aspx");
        }

        protected void lnkModificar_Command(object sender, CommandEventArgs e)
        {
            //En este caso necesitamos el ID de reserva para modificarlo:
            Session["Id_Reserva"] = e.CommandArgument.ToString();
            Response.Redirect("frmReserva.aspx");
        }
    }
}