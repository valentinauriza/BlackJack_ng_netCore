namespace PintoBello_API.Request;

public class ProductoRequest
{
    public string Nombre { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public DateTime FechaVencimineto { get; set; }
    public string Tamaño { get; set; } = null!;
    public int IdMarca { get; set; }
    public int IdTipoProducto { get; set; }
    public int IdProveedor { get; set; }
}