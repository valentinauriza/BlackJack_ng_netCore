using System.ComponentModel.DataAnnotations;

namespace PintoBello_API.Request;

public class FacturaRequest
{
    public int IdCliente { get; set; }
    public int IdEmpleado { get; set; }
    public int IdFormasPago { get; set; }
    public DateTime Fecha { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Deben existir Detalles")]
    public List<Detalles>? Detalles { get; set; }

    public FacturaRequest()
    {
        this.Detalles = new List<Detalles>();
    }
}

public class Detalles
{

    public uint IdFactura { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Importe { get; set; }
    public int IdProducto { get; set; }

}