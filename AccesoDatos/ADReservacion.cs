using System;
using System.Collections.Generic;
using System.Text;
using Entidades;
using System.Data.SqlClient;
using System.Data;

namespace AccesoDatos
{
    public class ADReservacion
    {
        //Atributos:
        string _Cadenaconexion;
        private string _mensaje;

        //Constructor:
        public ADReservacion(string CadenaConexion)
        {
            _Cadenaconexion = CadenaConexion;
            _mensaje = string.Empty;
        }

        //Propiedad:
        public string Mensaje { get => _mensaje; }

        //Métodos:

        //Método para insertar:
        public int Insertar(EntidadReservacion EntidadReserva)
        {
            SqlConnection sqlConexion = new SqlConnection(_Cadenaconexion);
            SqlCommand sqlCommand = new SqlCommand();
            int NumReservacion = 0;
            string Sentencia;
            sqlCommand.Connection = sqlConexion;
            try
            {
                Sentencia = "INSERT INTO RESERVACIONES(ID_CLIENTE,FECHAINGRESO,FECHASALIDA,CANTIDADPERSONAS,TIPOHABITACION,PRECIOXN,CANCELADA) VALUES (@IDCLIENTE,@FECHAINGRESO,@FECHASALIDA,@CANTIDAD,@TIPO,@PRECIO,@CANCELADA) SELECT @@Identity";
                sqlCommand.Parameters.AddWithValue("@IDCLIENTE", EntidadReserva.Cliente.Id_Cliente);
                sqlCommand.Parameters.AddWithValue("@FECHAINGRESO", EntidadReserva.FechaIngreso);
                sqlCommand.Parameters.AddWithValue("@FECHASALIDA", EntidadReserva.FechaSalida);
                sqlCommand.Parameters.AddWithValue("@CANTIDAD", EntidadReserva.CantidadPersonas);
                sqlCommand.Parameters.AddWithValue("@TIPO", EntidadReserva.TipoHabitacion);
                sqlCommand.Parameters.AddWithValue("@PRECIO", EntidadReserva.Precioxn);
                sqlCommand.Parameters.AddWithValue("@CANCELADA", EntidadReserva.Cancelada);
                sqlCommand.CommandText = Sentencia;
                sqlConexion.Open();
                NumReservacion = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConexion.Close();
            }
            catch (Exception)
            {
                NumReservacion = -1;
                throw;
            }
            finally
            {
                sqlConexion.Dispose();
                sqlCommand.Dispose();
            }
            return NumReservacion;
        }//fin de insertar

        //Método para Modificar:
        public int Modificar(EntidadReservacion EntidadReserva)
        {
            int resultado = -1;
            SqlConnection Conexion = new SqlConnection(_Cadenaconexion);
            SqlCommand Command = new SqlCommand();
            string Sentencia = string.Empty;
            Command.Connection = Conexion;
            Command.Parameters.AddWithValue("@NUMRESER", EntidadReserva.NumReservacion);
            Command.Parameters.AddWithValue("@IDCLIENTE", EntidadReserva.Cliente.Id_Cliente);
            Command.Parameters.AddWithValue("@FECHAINGRESO", EntidadReserva.FechaIngreso);
            Command.Parameters.AddWithValue("@FECHASALIDA", EntidadReserva.FechaSalida);
            Command.Parameters.AddWithValue("@CANTIDAD", EntidadReserva.CantidadPersonas);
            Command.Parameters.AddWithValue("@TIPO", EntidadReserva.TipoHabitacion);
            Command.Parameters.AddWithValue("@PRECIO", EntidadReserva.Precioxn);
            Sentencia = "UPDATE RESERVACIONES SET ID_CLIENTE=@IDCLIENTE,FECHAINGRESO=@FECHAINGRESO, FECHASALIDA=@FECHASALIDA,CANTIDADPERSONAS=@CANTIDAD,TIPOHABITACION=@TIPO,PRECIOXN=@PRECIO WHERE NUMRESERVACION=@NUMRESER";
            try
            {
                Command.CommandText = Sentencia;
                Conexion.Open();
                Command.ExecuteNonQuery();
                resultado = 1;
                Conexion.Close();
            }
            catch (Exception)
            {
                resultado = -1;
                throw;
            }
            finally
            {
                Command.Dispose();
                Conexion.Dispose();
            }

            return resultado;
        }// fin de modificar

        //Cancelar Factura:
        public int FacturarReserva(int NumReservacion)
        {
            int resultado = -1;
            SqlConnection Conexion = new SqlConnection(_Cadenaconexion);
            SqlCommand Command = new SqlCommand();
            string Sentencia = string.Empty;
            Command.Connection = Conexion;
            Command.Parameters.AddWithValue("@NUMRESER", NumReservacion);
            Sentencia = "UPDATE RESERVACIONES SET CANCELADA=1 WHERE NUMRESERVACION=@NUMRESER";
            try
            {
                Command.CommandText = Sentencia;
                Conexion.Open();
                Command.ExecuteNonQuery();
                resultado = 1;
                Conexion.Close();
            }
            catch (Exception)
            {
                resultado = -1;
                throw;
            }
            finally
            {
                Command.Dispose();
                Conexion.Dispose();
            }

            return resultado;
        }// fin de Facturar

        //Eliminar:
        public int Eliminar(EntidadReservacion EntidadReserva)
        {
            int vln_resultado = -1;
            SqlConnection vlo_Conexion = new SqlConnection(_Cadenaconexion);
            SqlCommand vlo_Command = new SqlCommand();
            string vlc_Sentencia = string.Empty;
            vlo_Command.Connection = vlo_Conexion;
            vlo_Command.Parameters.AddWithValue("@NUMRESER", EntidadReserva.NumReservacion);
            vlc_Sentencia = "DELETE FROM RESERVACIONES WHERE NUMRESERVACION=@NUMRESER";
            vlo_Command.CommandText = vlc_Sentencia;
            try
            {
                vlo_Conexion.Open();
                vlo_Command.ExecuteNonQuery();
                vln_resultado = 1;
                vlo_Conexion.Close();
            }
            catch (Exception)
            {
                vln_resultado = -1;
                throw;
            }
            finally
            {
                vlo_Command.Dispose();
                vlo_Conexion.Dispose();
            }
            return vln_resultado;
        }//fin eliminar

        //Método para usar la vista creada:
        public EntidadReservacion ObtenerRegistro(string Condicion)
        {
            EntidadReservacion EntidadReserva = new EntidadReservacion();
            SqlConnection Conexion = new SqlConnection(_Cadenaconexion);
            SqlCommand Command = new SqlCommand();
            SqlDataReader DataReader;
            string Sentencia = string.Empty;

            Command.Connection = Conexion;

            Sentencia = "Select NUMRESERVACION,ID_CLIENTE,NOMBRE,TELEFONO,FECHAINGRESO,FECHASALIDA,CANTIDADPERSONAS,TIPOHABITACION,PRECIOXN,CANCELADA from CONSULTA_RESERVA";
            if (!string.IsNullOrEmpty(Condicion))
            {
                Sentencia = string.Format("{0} WHERE {1}", Sentencia, Condicion);
            }
            try
            {
                Command.CommandText = Sentencia;
                Conexion.Open();
                DataReader = Command.ExecuteReader();

                if (DataReader.HasRows)
                {
                    DataReader.Read();
                    EntidadReserva.NumReservacion = DataReader.GetInt32(0);
                    EntidadReserva.Cliente.Id_Cliente = DataReader.GetInt32(1);
                    EntidadReserva.Cliente.Nombre = DataReader.GetString(2);
                    EntidadReserva.Cliente.Telefono = DataReader.GetString(3);
                    EntidadReserva.FechaIngreso = DataReader.GetDateTime(4);
                    EntidadReserva.FechaSalida = DataReader.GetDateTime(5);
                    EntidadReserva.CantidadPersonas = DataReader.GetInt32(6);
                    EntidadReserva.TipoHabitacion = DataReader.GetString(7);
                    EntidadReserva.Precioxn = DataReader.GetInt32(8);
                    EntidadReserva.Cancelada = DataReader.GetBoolean(9);
                    EntidadReserva.Existe = true;
                }
                Conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Command.Dispose();
                Conexion.Dispose();
            }
            return EntidadReserva;
        }//FIN OBTENER REGISTRO

        public DataSet ListarRegistros(string Condicion)
        {
            DataSet dataSet = new DataSet();
            SqlConnection Conexion = new SqlConnection(_Cadenaconexion);
            SqlDataAdapter DataAdapter;
            string Sentencia = string.Empty;

            try
            {
                Sentencia = "Select NUMRESERVACION,ID_CLIENTE,NOMBRE,FECHAINGRESO,FECHASALIDA,NOCHES,CANTIDADPERSONAS,TIPOHABITACION,PRECIOXN,SUBTOTAL,ICT,IVA,CANCELADA from CONSULTA_RESERVA";
                if (!string.IsNullOrEmpty(Condicion))
                {
                    Sentencia = string.Format("{0} WHERE {1}", Sentencia, Condicion);
                }
                DataAdapter = new SqlDataAdapter(Sentencia, Conexion);
                DataAdapter.Fill(dataSet, "Reservaciones");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Conexion.Dispose();
            }
            return dataSet;
        }
    }
}
