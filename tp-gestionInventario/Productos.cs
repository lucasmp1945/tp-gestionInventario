using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tp_gestionInventario.datos;

namespace tp_gestionInventario
{
    public partial class Productos : Form
    {
        public Productos()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion conn = new Conexion();
                using (SqlConnection conexion = conn.ObtenerConexion())
                {
                    conexion.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
