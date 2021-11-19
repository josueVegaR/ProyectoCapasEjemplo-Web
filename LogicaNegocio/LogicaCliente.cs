using System;
using System.Collections.Generic;
using System.Text;
using Entidades;
using AccesoDatos;
using System.Data;

namespace LogicaNegocio
{
    public class LogicaCliente
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
        public LogicaCliente() //opcional por si despues otro dv quiera setear la cadena de conexion
        {
            _cadenaConexion = string.Empty;
            _mensaje = string.Empty;
        }

        public LogicaCliente(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
            _mensaje = string.Empty;
        }
        #endregion

        #region Métodos

        public int Insertar(Cliente cliente) {
            int id_cliente = 0;
            ADClientes accesoDatos = new ADClientes(_cadenaConexion);
            try
            {
                id_cliente = accesoDatos.Insertar(cliente);
            }
            catch(Exception) {
                throw;
            }
            return id_cliente;
        }

        public int Modificar(Cliente cliente) {
            int filasAfectadas = 0;
            ADClientes accesoDatos = new ADClientes(_cadenaConexion);
            try
            {
                filasAfectadas = accesoDatos.Modificar(cliente);
            }
            catch (Exception )
            {
                throw;
            }
            return filasAfectadas;
        }

        public DataSet ListarClientes(string condicion, string orden) {
            DataSet DS;
            ADClientes accesoDatos = new ADClientes(_cadenaConexion);//se instancia el acceso a los datos

            try 
            {
                DS = accesoDatos.ListarClientes(condicion, orden);
            } 
            catch (Exception )
            {
                throw;
            }

            return DS;
        }

        public List<Cliente> ListarClientes(string condicion = "") {
            List<Cliente> listaClientes;
            ADClientes accesoDatos = new ADClientes(_cadenaConexion);
            try 
            {
                listaClientes = accesoDatos.ListarClientes(condicion);
            } 
            catch (Exception ) 
            {
                throw;
            }

            return listaClientes;
        }

        public Cliente ObtenerCliente(int id) {
            Cliente cliente;
            ADClientes accesoDatos = new ADClientes(_cadenaConexion);
            try 
            {
                cliente = accesoDatos.ObtenerCliente(id);
            } 
            catch (Exception ) 
            {
                throw;
            }

            return cliente;
        }

        public int EliminarConSP(Cliente cliente) //aqui antes de eliminar se podria verificar si es posible eliminar a un usuario dependiendo de la logica de negocio
        {
            int resultado;
            ADClientes accesoDatosC = new ADClientes(_cadenaConexion);
            ADReservacion accesoDatosR = new ADReservacion(_cadenaConexion);
            EntidadReservacion reservacion;
            try
            {
                reservacion = accesoDatosR.ObtenerRegistro($"ID_CLIENTE = {cliente.Id_Cliente}");
                if (reservacion.Existe == false)
                {
                    resultado = accesoDatosC.EliminarRegistroConSP(cliente);
                    _mensaje = accesoDatosC.Mensaje;
                }
                else
                {
                    _mensaje = "El cliente no se puede eliminar ya que tiene reservas asociadas.";
                    resultado = -1;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return resultado;
        }

        #endregion
    }//fin clase
}//fin namespace
