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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarLista();
                }
            }
            catch (Exception)
            {

                Session["_mensaje"] = "No se registran reservas.";
            }
        }
    }
}