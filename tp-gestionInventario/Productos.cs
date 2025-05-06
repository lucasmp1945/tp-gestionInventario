using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
        private List<Producto> productosCargados = new List<Producto>();

        public Productos()

        {
            InitializeComponent();
            cargarProductos();
            cargarCategorias();
            enabled(false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            clear();
            enabled(true);
            this.esNuevo = true;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Debe seleccionar un producto para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.esNuevo = false;
            enabled(true);
            btnNuevo.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            clear();
            enabled(false);
            this.esNuevo = false;
            btnNuevo.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!validarDatos())
                return;

            var repo = new productoRepository();
            var prod = crearObjProducto();
            bool rta;

            if (this.esNuevo)
            {
                var existente = buscarProducto(prod.codigo);
                if (existente != null)
                {
                    MessageBox.Show($"Ya existe un producto con ese código.\nNombre: {existente.nombre}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                btnNuevo.Enabled = true;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
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
            this.productosCargados =  repo.getAll();

            dgvProductos.DataSource = this.productosCargados;
            dgvProductos.Columns["idCategoria"].Visible = false;


        }




        private Producto buscarProducto(string codigo)
        {
            var repo = new productoRepository();
            var prod = repo.getByCodigo(codigo);
            return prod;
        }

        private void cargarCategorias()
        {
            var repo = new categoriaRepository();
            var lista = repo.getAll();

            cmbCategorias.DataSource = lista;
            cmbCategorias.DisplayMember = "descrip";
            cmbCategorias.ValueMember = "idCategoria";

            var listaFiltro = new List<Categoria>();
            listaFiltro.Add(new Categoria { idCategoria = 0, descrip = "-- Seleccione --" });
            listaFiltro.AddRange(lista);

            cmbFiltroCat.DataSource = listaFiltro;
            cmbFiltroCat.DisplayMember = "descrip";
            cmbFiltroCat.ValueMember = "idCategoria";
            cmbFiltroCat.SelectedIndex = 0; 
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
            cmbCategorias.Enabled = valor;
        }

        private Producto crearObjProducto()
        {
            Producto p = new Producto();
            p.codigo = txtCodigo.Text;
            p.nombre = txtNombe.Text;
            p.descripcion = txtDescripcion.Text;
            p.precio = Convert.ToDecimal(nudPrecio.Value);
            p.stock = Convert.ToInt32(nudStock.Value);
            p.idCategoria = Convert.ToInt32(cmbCategorias.SelectedValue);
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
                cmbCategorias.SelectedValue = Convert.ToInt32(fila.Cells["idCategoria"].Value);

            }
        }


        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (productosCargados == null)
                return;

            int idCategoria = Convert.ToInt32(cmbFiltroCat.SelectedValue);
            string filtro = txtFiltro.Text.Trim().ToLower();

            IEnumerable<Producto> filtrados = productosCargados;

            if (idCategoria != 0)
            {
                filtrados = filtrados.Where(p => p.idCategoria == idCategoria);
            }

            if (!string.IsNullOrEmpty(filtro))
            {
                filtrados = filtrados.Where(p =>
                    (!string.IsNullOrEmpty(p.codigo) && p.codigo.ToLower().Contains(filtro)) ||
                    (!string.IsNullOrEmpty(p.nombre) && p.nombre.ToLower().Contains(filtro))
                );
            }

            dgvProductos.DataSource = filtrados.ToList();
            dgvProductos.Columns["idCategoria"].Visible = false;
        }


        private bool validarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El campo 'Código' es obligatorio.");
                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombe.Text))
            {
                MessageBox.Show("El campo 'Nombre' es obligatorio.");
                txtNombe.Focus();
                return false;
            }

            if (nudPrecio.Value <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a 0.");
                nudPrecio.Focus();
                return false;
            }

            if (nudStock.Value < 0)
            {
                MessageBox.Show("El stock no puede ser negativo.");
                nudStock.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("El campo 'Descripción' es obligatorio.");
                txtDescripcion.Focus();
                return false;
            }

            if (cmbCategorias.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una categoría.");
                cmbCategorias.Focus();
                return false;
            }

            return true;
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {

        }

        private void dgvProductos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                var s = Convert.ToInt32(fila.Cells["stock"].Value);
                if (s == 0)
                {
                    fila.DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else if(s > 0 && s <=10)
                {
                    fila.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                }
            }
        }
    }
}
