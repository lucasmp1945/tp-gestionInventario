using System;
using System.Data.SqlClient;
using System.IO;

namespace tp_gestionInventario.datos
{
    public class Conexion
    {
        private readonly string cadena = "Server=localhost;Database=inventario;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }

        public static void EjecutarScriptMigracion()
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "DBScript.sql");
            string script = File.ReadAllText(scriptPath);

            using (SqlConnection conn = new SqlConnection("Server=localhost;Integrated Security=true;"))
            {
                conn.Open();
                foreach (string command in script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
