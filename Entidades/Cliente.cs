using System;

namespace Entidades
{
    public class Cliente
    {

        #region Atributos

        private int id_cliente;
        private string nombre;
        private string telefono;
        private string direccion;
        private bool existe;

        #endregion

        #region Constructores

        public Cliente()
        {
            id_cliente = 0;
            nombre = string.Empty;
            telefono = string.Empty;
            direccion = string.Empty;
            existe = false;
        }

        public Cliente(int id, string nombreC, string telefonoC, string direccionC)
        {
            id_cliente = id;
            nombre = nombreC;
            telefono = telefonoC;
            direccion = direccionC;
            existe = true;
        }

        #endregion

        #region Propiedades

        public int Id_Cliente{

            set => id_cliente = value;
            get => id_cliente;

        }


        public string Nombre {

            get { return nombre; }
            set { nombre = value; }
        }


        public string Telefono
        {

            get { return telefono; }
            set { telefono = value; }
        }


        public string Direccion
        {

            get { return direccion; }
            set { direccion = value; }
        }

        public bool Existe
        {

            set => existe = value;
            get => existe;

        }


        #endregion

        #region Metodos

        public override string ToString()
        {
            return string.Format("{0} - {1}",id_cliente,nombre);
        }

        #endregion

    }
}
