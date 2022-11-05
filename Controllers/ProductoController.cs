namespace PintoBello_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PintoBello_API.Models;
using PintoBello_API.Request;
using PintoBello_API.Response;

public class ProductoController : ControllerBase
{
    private readonly pintureriaContext _context;

    public ProductoController(pintureriaContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/productos/listado")]
    public async Task<ActionResult<RdoProductoss>> getProductos()
    {
        try
        {
            var result = new RdoProductoss();
            var productos = await _context.Productos.Include(c => c.IdMarcaNavigation).Include(c => c.IdProveedorNavigation).
            Include(c => c.IdTipoProductoNavigation).ToListAsync();

            var pr = productos.Distinct().ToArray();
            var cantidad = pr.Length;

            if (pr != null)
            {
                //PARA QUE SE ENTIENDA: quiero agrupar todos los que tienen el mismo nombre, marca y tamaño
                //para sacar stock porque en la base de datos no hay un campo con eso
                var prod = pr.GroupBy(x => new { x.Nombre, x.Marca, x.Tamaño })
                .Select(g =>
                new RdoListaP
                {
                    Marca = g.Key.Marca,
                    Tamaño = g.Key.Tamaño,
                    Nombre = g.Key.Nombre,
                    stock = g.Count(),
                    TipoProducto = pr.Where(x => x.Nombre == g.Key.Nombre &&
                     x.Marca == g.Key.Marca && x.Tamaño == g.Key.Tamaño).First().IdTipoProductoNavigation.Descripcion ?? "",
                    Proveedor = pr.Where(x => x.Nombre == g.Key.Nombre &&
                     x.Marca == g.Key.Marca && x.Tamaño == g.Key.Tamaño).First().IdProveedorNavigation.Nombre
                });
                result.listaProd = prod.ToList();
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpGet]
    [Route("api/productos/listado/{nombre}+{marca}+{tam}")]
    public async Task<ActionResult<RdoProductos>> getProductosPorNombre(string nombre, string marca, int tam)
    {
        try
        {
            //Con esto quiero traer todos los productos que estén relacionados con un ítem de la lista anterior
            var result = new RdoProductos();
            var productos = await _context.Productos.Where(c => c.Nombre.Equals(nombre) && c.Marca.Equals(marca)
            && c.Tamaño.Equals(tam)).Include(c => c.IdMarcaNavigation).Include(c => c.IdProveedorNavigation).
            Include(c => c.IdTipoProductoNavigation).ToListAsync();

            if (productos != null)
            {
                foreach (var p in productos)
                {
                    var resultAux = new RdoProd
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        Marca = p.Marca,
                        FechaVencimineto = p.FechaVencimineto,
                        Tamaño = p.Tamaño,
                        TipoProducto = p.IdTipoProductoNavigation.Descripcion,
                        Proveedor = p.IdProveedorNavigation.Nombre
                    };
                    result.listaProductos.Add(resultAux);
                }
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/productos/alta")]
    public async Task<ActionResult<bool>> nuevoProducto([FromBody] ProductoRequest cmd)
    {
        try
        {
            var nuevo = new Producto()
            {
                Nombre = cmd.Nombre,
                Marca = cmd.Marca,
                FechaVencimineto = cmd.FechaVencimineto,
                Tamaño = cmd.Tamaño,
                IdMarca = cmd.IdMarca,
                IdTipoProducto = cmd.IdTipoProducto,
                IdProveedor = cmd.IdProveedor
            };
            await _context.AddAsync(nuevo);
            await _context.SaveChangesAsync();

            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    //-----------------------------TIPO PRODUCTO-----------------------------//
    [HttpGet]
    [Route("api/tipoProducto/listado")]
    public async Task<ActionResult<RdoTipoProducto>> getTiposProd()
    {
        try
        {
            var result = new RdoTipoProducto();
            var tipos = await _context.TipoProductos.ToListAsync();

            if (tipos != null)
            {
                foreach (var t in tipos)
                {
                    var resultAux = new RdoTipo
                    {
                        IdTipoProducto = t.IdTipoProducto,
                        Descripcion = t.Descripcion
                    };
                    result.listaFormasPago.Add(resultAux);
                }
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}
