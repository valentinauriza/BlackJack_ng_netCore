using System;
using System.Collections.Generic;

namespace PintoBello_API.Models
{
    public partial class TipoEmpleado
    {
        public TipoEmpleado()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int IdTipoEmpleado { get; set; }
        public string Tipo { get; set; } = null!;

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
