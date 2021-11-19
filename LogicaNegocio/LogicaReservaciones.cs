using System;
using System.Collections.Generic;
using System.Text;
using Entidades;
using AccesoDatos;
using System.Data;

namespace LogicaNegocio
{
    public class LogicaReservaciones
    {
        #region Atributos
        private string _cadenaConexion;
        private string _mensaje;
        #endregion

        #region Propiedades
        public string Mensaje
        {
            get => _mensaje;
        }

        public string CadenaConexion//set opcional en caso de que en el otro constructor no se le pase la cadenaConexion
        {
            set => _cadenaConexion = value;
        }
        #endregion

        #region Constructor
        public LogicaReservaciones() //opcional por si despues otro dv quiera setear la cadena de conexion
        {
            _cadenaConexion = string.Empty;
            _mensaje = string.Empty;
        }

        public LogicaReservaciones(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
            _mensaje = string.Empty;
        }
        #endregion

        #region Métodos
        public int InsertarReserva(EntidadReservacion Reservacion)
        {
            int Resultado;
            ADReservacion AccesoDatosR = new ADReservacion(_cadenaConexion);
            try
            {
                if (Reservacion.TipoHabitacion == "Standard")
                {
                    Reservacion.Precioxn = 80;
                }
                else if (Reservacion.TipoHabitacion == "Junior")
                {
                    Reservacion.Precioxn = 120;
                }
                else
                {
                    Reservacion.Precioxn = 180;
                }
                Reservacion.Cancelada = false;
                Resultado = AccesoDatosR.Insertar(Reservacion);
                if (Resultado > 0)
                {
                    _mensaje = "Operación realizada satisfactoriamente";
                }
                else
                {
                    _mensaje = "Imposible realizar la operación";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public int ModificarReserva(EntidadReservacion Reservacion)
        {
            int Resultado;
            ADReservacion AccesoDatosR = new ADReservacion(_cadenaConexion);
            try
            {
                if (!Reservacion.Cancelada)
                {
                    if (Reservacion.TipoHabitacion == "Standard")
                    {
                        Reservacion.Precioxn = 80;
                    }
                    else if (Reservacion.TipoHabitacion == "Junior")
                    {
                        Reservacion.Precioxn = 120;
                    }
                    else
                    {
                        Reservacion.Precioxn = 180;
                    }
                    Resultado = AccesoDatosR.Modificar(Reservacion);
                    _mensaje = "Reserva modificada satisfactoriamente";
                }
                else
                {
                    Resultado = -1;
                    _mensaje = "Imposible modificar la reserva debido a que ya canceló la factura";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public int FacturarReserva(int Reservacion)
        {
            int Resultado;
            ADReservacion AccesoDatosR = new ADReservacion(_cadenaConexion);
            try
            {
                Resultado = AccesoDatosR.FacturarReserva(Reservacion);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public int EliminarReserva(EntidadReservacion Reservacion)
        {
            int Resultado = 0;
            ADReservacion adReserva = new ADReservacion(_cadenaConexion);
            try
            {
                if (adReserva.ObtenerRegistro($"NUMRESERVACION={Reservacion.NumReservacion}").Cancelada) //Si cancelada está en True:
                {
                    _mensaje = "Imposible eliminar la reserva ya que la factura ya se canceló";
                    Resultado = -1;
                }
                else //Si la reserva aún no se ha cancelado:
                {
                    Resultado = adReserva.Eliminar(Reservacion);
                    if (Resultado > 0)
                        _mensaje = "La reserva se eliminó satisfactoriamente";
                    else
                        _mensaje = "No se pudo completar la operación";
                }

            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }

        public EntidadReservacion ObtenerReserva(string Condicion)
        {
            EntidadReservacion EntidadReserva;
            ADReservacion AccesoDatosR = new ADReservacion(_cadenaConexion);
            try
            {
                EntidadReserva = AccesoDatosR.ObtenerRegistro(Condicion);
            }
            catch (Exception)
            {

                throw;
            }
            return EntidadReserva;
        }
        
        public DataSet ListarRegistros(string Condicion)
        {
            DataSet DS;
            ADReservacion AccesoDatosR = new ADReservacion(_cadenaConexion);
            try
            {
                DS = AccesoDatosR.ListarRegistros(Condicion);
            }
            catch (Exception)
            {

                throw;
            }
            return DS;
        }

        #endregion
    }
}
