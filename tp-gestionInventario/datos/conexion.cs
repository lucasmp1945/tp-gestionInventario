using System;
using System.Data.SqlClient;

namespace tp_gestionInventario.datos
{
    public class Conexion
    {
        private readonly string cadena = "Server=localhost;Database=inventario;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }
    }
}
