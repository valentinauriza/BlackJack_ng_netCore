namespace PintoBello_API.Response;

public class RdoProveedores
{
    public List<RdoProv> listaProveedores { get; set; } = new List<RdoProv>();
}

public class RdoProv
{
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Mail { get; set; } = null!;
}