using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ABMProductos
{
    class Daatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader dr;
        string cadenaConexion;

        public SqlDataReader Dr { get => dr; set => dr = value; }
        public string CadenaConexion { get => cadenaConexion; set => cadenaConexion = value; }

        public Daatos()
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = null;
        }

        public Daatos(string cadenaConexion)
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = cadenaConexion;
        }
        public void Conectar()
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }
        public void Desconectar()
        {
            conexion.Close();
            conexion.Dispose();  // libera la memoria 
        }

        public DataTable ConsultarTabla(string nomTabla)
        {
            DataTable dt = new DataTable();
            Conectar();
            comando.CommandText = $"select * from {nomTabla}";
            dt.Load(comando.ExecuteReader());
            Desconectar();
            return dt;
        }

        public void LeerTabla(string nomTabla)
        {
            Conectar();
            comando.CommandText = $"select * from {nomTabla}";
            dr = comando.ExecuteReader();  // no cerramos conexion x q necesita q este abierta el datareader
        }

        public void Actualizar(string strSQL)
        {
            Conectar();
            comando.CommandText = strSQL;
            comando.ExecuteNonQuery();
            Desconectar();
        }
    }
}
