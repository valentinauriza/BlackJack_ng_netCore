using System;
using System.Collections.Generic;

namespace PintoBello_API.Models
{
    public partial class TipoProducto
    {
        public TipoProducto()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdTipoProducto { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
