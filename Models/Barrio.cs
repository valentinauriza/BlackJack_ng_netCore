using System;
using System.Collections.Generic;

namespace PintoBello_API.Models
{
    public partial class Barrio
    {
        public Barrio()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdBarrios { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
