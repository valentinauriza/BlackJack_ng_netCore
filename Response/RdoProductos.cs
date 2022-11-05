namespace PintoBello_API.Response;

public class RdoProductos
{
    public List<RdoProd> listaProductos { get; set; } = new List<RdoProd>();
}

public class RdoProductoss
{
    public List<RdoListaP> listaProd { get; set; } = new List<RdoListaP>();
}
public class RdoListaP
{
    public int stock { get; set;}
    public string Nombre { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public string Tamaño { get; set; } = null!;
    public string TipoProducto { get; set; } = null!;
    public string Proveedor { get; set; } = null!;
}

public class RdoProd
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public DateTime FechaVencimineto { get; set; }
    public string Tamaño { get; set; } = null!;
    public string TipoProducto { get; set; } = null!;
    public string Proveedor { get; set; } = null!;
}