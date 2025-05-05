using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp_gestionInventario.models
{
    public class Producto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public string descripcion { get; set; }
    }
}
