using System;
using System.Collections.Generic;

namespace PintoBello_API.Models
{
    public partial class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public int IdFactura { get; set; }

        public virtual Factura IdFacturaNavigation { get; set; } = null!;
    }
}
