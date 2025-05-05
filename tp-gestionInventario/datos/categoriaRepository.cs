using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tp_gestionInventario.models;

namespace tp_gestionInventario.datos
{
    internal class categoriaRepository
    {
        private readonly Conexion conexion = new Conexion();
        public List<Categoria> getAll()
        {
            List<Categoria> lista = new List<Categoria>();

            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT * FROM categorias";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categoria c = new Categoria
                            {
                                idCategoria = reader.GetInt32(0),
                                descrip = reader.GetString(1)
                            };

                            lista.Add(c);
                        }
                    }
                }
            }

            return lista;
        }
    }
}
