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
using tp_gestionInventario.models;

namespace tp_gestionInventario
{
    public partial class Productos : Form
    {
        private bool esNuevo = false;

        public Productos()

        {
            InitializeComponent();
            cargarProductos();
            enabled(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            clear();
            enabled(true);
            this.esNuevo = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            this.esNuevo = false;
            enabled(true);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            clear();
            enabled(false);
            this.esNuevo = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            var repo = new productoRepository();
            var prod = crearObjProducto();
            bool rta;

            if (this.esNuevo)
            {
                rta = repo.insert(prod);
                if (rta)
                {
                    MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                rta = repo.update(prod);
                if (rta)
                {
                    MessageBox.Show("Producto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (rta)
            {
                clear();
                cargarProductos();
                enabled(false); 
                esNuevo = false;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cod = txtCodigo.Text.Trim();

            if (string.IsNullOrEmpty(cod))
            {
                MessageBox.Show("Seleccioná un producto para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Estás seguro de eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var repo = new productoRepository();
                bool rta = repo.delete(cod);

                if (rta)
                {
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    cargarProductos();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el producto. Verificá el código.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cargarProductos()
        {
            var repo = new productoRepository();
            var lista = repo.getAll();

            dgvProductos.DataSource = lista;
        }

        private void clear()
        {
            txtCodigo.Clear();
            txtNombe.Clear();
            nudStock.Value = 0;
            nudPrecio.Value = 0;
            txtDescripcion.Clear();
        }

        private void enabled(bool valor)
        {
            btnCancelar.Enabled = valor;
            btnGuardar.Enabled = valor;
            txtCodigo.Enabled = valor;
            txtNombe.Enabled = valor;
            txtDescripcion.Enabled = valor;
            nudPrecio.Enabled = valor;
            nudStock.Enabled = valor;
        }

        private Producto crearObjProducto()
        {
            Producto p = new Producto();
            p.codigo = txtCodigo.Text;
            p.nombre = txtNombe.Text;
            p.descripcion = txtDescripcion.Text;
            p.precio = Convert.ToDecimal(nudPrecio.Value);
            p.stock = Convert.ToInt32(nudStock.Value);
            return p;
        }

        private void dgvProcutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.esNuevo = false;
            enabled(false);

            if (e.RowIndex >= 0) 
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtCodigo.Text = fila.Cells["codigo"].Value.ToString();
                txtNombe.Text = fila.Cells["nombre"].Value.ToString();
                nudPrecio.Value = Convert.ToDecimal(fila.Cells["precio"].Value);
                nudStock.Value = Convert.ToInt32(fila.Cells["stock"].Value);
                txtDescripcion.Text = fila.Cells["descripcion"].Value.ToString();
            }
        }


        private void btnFiltrar_Click(object sender, EventArgs e)
        {

        }
    }
}
