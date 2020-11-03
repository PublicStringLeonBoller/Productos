using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABMProductos
{
    class Producto
    {
        int codigo;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }
        double precio;

        public double Precio
        {
            get { return precio; }
            set { precio = value; }
        }
        int tipo;

        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        int marca;

        public int Marca
        {
            get { return marca; }
            set { marca = value; }
        }
        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        override public string ToString()
        {
            return $"{Codigo} - {Detalle}";
        }

        public string ShowProducto()
        {
            return $"{Codigo} - {Detalle}";
        }
    }
}
