using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tp_gestionInventario.models;

namespace tp_gestionInventario.datos
{
    internal class productoRepository
    {
        private readonly Conexion conexion = new Conexion();

        public List<Producto> getAll()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "SELECT p.*, c.descrip AS categoria FROM productos p LEFT JOIN categorias c ON c.idCategoria = p.idCategoria";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto p = new Producto
                        {
                            codigo = reader["codigo"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            precio = Convert.ToDecimal(reader["precio"]),
                            stock = Convert.ToInt32(reader["stock"]),
                            descripcion = reader["descripcion"].ToString(),
                            idCategoria = Convert.ToInt32(reader["idCategoria"]),
                            categoria = reader["categoria"].ToString()

                        };
                        lista.Add(p);
                    }
                }
            }

            return lista;
        }

        public Producto getByCodigo(string codigo)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();

                string query = "SELECT codigo, nombre, precio, stock, descripcion, idCategoria FROM productos WHERE codigo = @codigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Producto p = new Producto
                            {
                                codigo = reader["codigo"].ToString(),
                                nombre = reader["nombre"].ToString(),
                                precio = Convert.ToDecimal(reader["precio"]),
                                stock = Convert.ToInt32(reader["stock"]),
                                descripcion = reader["descripcion"].ToString(),
                                idCategoria = Convert.ToInt32(reader["idCategoria"])
                            };

                            return p;
                        }
                    }
                }
            }

            return null;
        }


        public bool insert(Producto p)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();

                string query = "INSERT INTO productos (codigo, nombre, precio, stock, descripcion, idCategoria) VALUES (@codigo, @nombre, @precio, @stock, @descripcion, @idCategoria)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", p.codigo);
                    cmd.Parameters.AddWithValue("@nombre", p.nombre);
                    cmd.Parameters.AddWithValue("@precio", p.precio);
                    cmd.Parameters.AddWithValue("@stock", p.stock);
                    cmd.Parameters.AddWithValue("@descripcion", p.descripcion);
                    cmd.Parameters.AddWithValue("@idCategoria", p.idCategoria);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool update(Producto p)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE productos SET nombre=@nombre, precio=@precio, stock=@stock, descripcion=@descripcion, idCategoria=@idCategoria WHERE codigo=@codigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", p.codigo);
                    cmd.Parameters.AddWithValue("@nombre", p.nombre);
                    cmd.Parameters.AddWithValue("@precio", p.precio);
                    cmd.Parameters.AddWithValue("@stock", p.stock);
                    cmd.Parameters.AddWithValue("@descripcion", p.descripcion);
                    cmd.Parameters.AddWithValue("@idCategoria", p.idCategoria);


                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public bool delete(string codigo)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM productos WHERE codigo=@codigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}
