using System;
using System.Data; // el using que se va a utilizar para realizar el acceso a la base de datos
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Entidades;
namespace AccesoDatos
{
    public class ADClientes
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
        public ADClientes() //opcional por si despues otro dv quiera setear la cadena de conexion
        {
            _cadenaConexion = string.Empty;
            _mensaje = string.Empty;
        }

        public ADClientes(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
            _mensaje = string.Empty;
        }
        #endregion

        #region Metodos

        public int Insertar(Cliente cliente) { // inserta y devuelve el id generado en la insersion en la tabla
            SqlConnection conexion = new SqlConnection(_cadenaConexion);//se envia la cadena de conexion para establecerla con la base de datos
            SqlCommand comando = new SqlCommand();//objeto que se usa para ejecutar comandos de SQL
            int id = 0; //variable qeu guarda el id ingresado a la base de datos se devolvera en la funcion
            string sentencia = "INSERT INTO TBL_CLIENTES(NOMBRE,TELEFONO,DIRECCION) VALUES(@NOMBRE,@TELEFONO,@DIRECCION) select @@identity";
            comando.Connection = conexion;//aqui se le dice al comando SQL cual es la base de datos en la que se realizara la sentencia SQL
            comando.Parameters.AddWithValue("@NOMBRE", cliente.Nombre); // se especifican las variables que el comando va a necesitar para poder ejecutar la sentencia SQL
            comando.Parameters.AddWithValue("@TELEFONO", cliente.Telefono);
            comando.Parameters.AddWithValue("@DIRECCION", cliente.Direccion);
            comando.CommandText = sentencia;
            //hasta aqui el objeto command esta preparado para ejecutar la consulta que se desea


            //aqui ya se empieza a ejecutar la consulta SQL
            try
            {
                conexion.Open();
                id = Convert.ToInt32(comando.ExecuteScalar());
                conexion.Close();
            }
            catch
            {
                throw;
            }
            finally {
                conexion.Dispose();
                comando.Dispose();
            }

            return id;
        }

        public int Modificar(Cliente cliente) {
            int filasAfectadas = -1;
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlCommand comando = new SqlCommand();
            string sentencia = "UPDATE TBL_CLIENTES SET NOMBRE=@NOMBRE, TELEFONO=@TELEFONO, DIRECCION=@DIRECCION WHERE ID_CLIENTE=@ID_CLIENTE";
            comando.CommandText = sentencia;
            comando.Connection = conexion;
            comando.Parameters.AddWithValue("@ID_CLIENTE", cliente.Id_Cliente);
            comando.Parameters.AddWithValue("@NOMBRE", cliente.Nombre);
            comando.Parameters.AddWithValue("@TELEFONO", cliente.Telefono);
            comando.Parameters.AddWithValue("@DIRECCION", cliente.Direccion);

            try
            {
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception )
            {
                throw;
            }
            finally 
            {
                conexion.Dispose();
                comando.Dispose();
            }
            return filasAfectadas;
        }


        public DataSet ListarClientes(string condicion, string orden)
        {  //devuelve un dataset de clientes para mostrarlo en un datagridView

            DataSet datos = new DataSet(); //lugar donde se va a guardar la tabla que vendra de la consulta del sql
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlDataAdapter adapter;
            string sentencia = "SELECT ID_CLIENTE, NOMBRE, TELEFONO,DIRECCION FROM TBL_CLIENTES";

            if (!string.IsNullOrEmpty(condicion))
            { //si la condicion no esta vacia entonces concatene esa condicion a la sentencia
                sentencia = string.Format("{0} where {1}", sentencia, condicion);
            }

            if (!string.IsNullOrEmpty(orden))
            {//si orden no esta vacia entonces concatene ese orden a la sentencia
                sentencia = string.Format("{0} order by {1}", sentencia, orden);
            }

            try
            {
                adapter = new SqlDataAdapter(sentencia, conexion);//se realiza la conexion y se prepara el adaptador para ejecutar la sentencia
                adapter.Fill(datos, "Clientes");//el adaptador llena el dataset y le pone nombre 
            }
            catch (Exception ) 
            {
                throw;
            }

            return datos; //devuelve el dataset
        }

        public List<Cliente> ListarClientes(string condicion = "") 
        {//devuelve una lista de clientes en lugar de un dataset 
            DataSet DS = new DataSet();//se define el dataset donde se almacenara la informacion de la tabla de sql
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlDataAdapter adapter;
            List<Cliente> clientes;//la lista de clientes que se devolvera
            string sentencia = "SELECT ID_CLIENTE, NOMBRE,TELEFONO,DIRECCION FROM TBL_CLIENTES";

            if (!string.IsNullOrEmpty(condicion)) //si la condicion no viene vacia entonces concatenela con la sentencia
            {
                sentencia = string.Format("{0} where {1}", sentencia, condicion);
            }

            try
            {
                adapter = new SqlDataAdapter(sentencia, conexion);
                adapter.Fill(DS, "Clientes");//se llena el dataset y se le pone nombre

                //sentencia linQ para convertir el dataset en una lista
                clientes = (from DataRow fila in DS.Tables["Clientes"].Rows
                            select new Cliente()
                            {
                                Id_Cliente = (int)fila[0],
                                Nombre = fila[1].ToString(),
                                Telefono = fila[2].ToString(),
                                Direccion = fila[3].ToString(),
                                Existe = true
                            }).ToList();//al final convierte lo del data set en puras instancias de clientes y llena la lista con puros clientes que hay en las tablas
            }
            catch (Exception )
            {
                throw;
            }

            return clientes;//devuelve la lista con los clientes
        }


        public Cliente ObtenerCliente(int id) { //devuelve un cliente cuando se busca
            Cliente cliente = null;
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlCommand comando = new SqlCommand();
            SqlDataReader dataReader;//el data reader no tiene constructor para llenarlo es mediante un execute
            string sentencia = string.Format("SELECT ID_CLIENTE,NOMBRE,TELEFONO,DIRECCION FROM TBL_CLIENTES WHERE ID_CLIENTE ={0}", id);//si el id fuera texto se debe poner el 0 entre comillas asi '{0}' dentro del string
            comando.Connection = conexion;
            comando.CommandText = sentencia;

            try 
            {
                conexion.Open();
                dataReader = comando.ExecuteReader();

                if (dataReader.HasRows) 
                {
                    cliente = new Cliente();
                    dataReader.Read();//lee fila por fila del data reader
                    cliente.Id_Cliente = dataReader.GetInt32(0);//esta columna es de tipo integer y esta en la posicion 0
                    cliente.Nombre = dataReader.GetString(1);
                    cliente.Telefono = dataReader.GetString(2);
                    cliente.Direccion = dataReader.GetString(3);
                    cliente.Existe = true;
                }
                conexion.Close();

                //con un while y si quiero obtener una lista de clientes seria asi
                //while (dataReader.HasRows) al igual que en el if lea los datos crea una instancia y metala en una lista para devovleer una lista de clientes

            } 
            catch (Exception ) 
            {
                throw;
            }
            return cliente;
        
        }

        public int EliminarRegistroConSP(Cliente cliente) { //eliminar registro con Stored Procedure
            int resultado = -1;
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlCommand comando = new SqlCommand();
            comando.CommandText = "Eliminar"; //nombre del procedimiento almacenado
            comando.CommandType = CommandType.StoredProcedure;//se especifica que tipo de comando es, en este caso es un procedimiento almacenado
            comando.Connection = conexion;
            //parametro de entrada para el SP
            comando.Parameters.AddWithValue("@idcliente", cliente.Id_Cliente);
            //parametro de salida del SP
            comando.Parameters.Add("@msj", SqlDbType.VarChar,50).Direction=ParameterDirection.Output;//definicion del parametro de salida del procedimiento almacenado
            comando.Parameters.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;//se declara otro parametro de retorno del SP que obtenga el retorno del SP

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery(); //ejecuta el SP y se llenan las variables de retorno del SP
                resultado = Convert.ToInt32(comando.Parameters["@retorno"].Value); //obtengo la variable de retorno
                //se va a leer el parametro de salida del SP
                _mensaje = comando.Parameters["@msj"].Value.ToString();//obtiene el mensaje que se devolvio del SP
                conexion.Close();
            }
            catch (Exception )
            {

                throw;
            }

            return resultado;

        }

        #endregion


    }//fin de clase
}// de namespace
