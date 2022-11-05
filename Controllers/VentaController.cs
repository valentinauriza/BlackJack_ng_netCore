namespace PintoBello_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PintoBello_API.Models;
using PintoBello_API.Request;
using PintoBello_API.Response;
using PintoBello_API.Response.Cliente;

public class VentaController : ControllerBase
{
    private readonly pintureriaContext _context;

    public VentaController(pintureriaContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/facturas")]
    public async Task<ActionResult<bool>> nuevaFactura([FromBody] FacturaRequest request)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            using (_context)
            {
                using (var transaccion = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var factura = new Factura();
                        factura.Total = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
                        factura.Fecha = DateTime.Now;
                        factura.IdCliente = request.IdCliente;
                        factura.IdEmpleado = request.IdEmpleado;
                        factura.IdFormasPago = request.IdFormasPago;

                        _context.Facturas.Add(factura);
                        _context.SaveChanges();

                        foreach (var item in request.Detalles)
                        {
                            var detalle = new DetalleFactura();
                            detalle.Cantidad = item.Cantidad;
                            detalle.PrecioUnitario = item.PrecioUnitario;
                            detalle.Importe = item.Importe;
                            detalle.IdProducto = item.IdProducto;
                            detalle.IdFactura = factura.IdFactura;

                            _context.DetalleFacturas.Add(detalle);
                            _context.SaveChanges();
                        }

                        var nuevo = new Pedido()
                        {
                            Fecha = factura.Fecha,
                            IdFactura = factura.IdFactura
                        };
                        await _context.AddAsync(nuevo);
                        await _context.SaveChangesAsync();

                        transaccion.Commit();
                        respuesta.Exito = 1;
                    }
                    catch (Exception)
                    {
                        transaccion.Rollback();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            respuesta.Mensaje = ex.Message;
        }
        return Ok(respuesta);
    }

    [HttpGet]
    [Route("api/facturas/obtener/{id}")]
    public async Task<ActionResult<RdoFactura>> getFacturaId(int id)
    {
        try
        {
            var result = new RdoFactura();

            var det = await _context.DetalleFacturas.Where(d => d.IdFactura.Equals(id)).
            Include(d => d.IdFacturaNavigation).Include(d => d.IdProductoNavigation).ToListAsync();

            if (det != null)
            {
                var fact = await _context.Facturas.Where(f => f.IdFactura.Equals(id)).Include(f => f.IdEmpleadoNavigation)
            .Include(f => f.IdClienteNavigation).Include(f => f.IdFormasPagoNavigation)
            .FirstOrDefaultAsync();

                result.IdFactura = id;
                result.Empleado = fact.IdEmpleadoNavigation.Nombre;
                result.Cliente = fact.IdClienteNavigation.Nombre;
                result.FormaPago = fact.IdFormasPagoNavigation.Descripcion;

                foreach (var d in det)
                {
                    var resultAux = new RdoDetalle
                    {
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Importe = d.Importe,
                        Producto = d.IdProductoNavigation.Nombre
                    };
                    result.listaDetalles.Add(resultAux);
                }
                result.Total = fact.Total;
            }
            return result;
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }


    //ESTE NO FUNCIONA
    [HttpGet]
    [Route("api/pedidos/obtener/{id}")]
    public async Task<ActionResult<RdoPedido>> getPedidoId(int id)
    {
        try
        {
            var result = new RdoPedido();
            var ped = await _context.Pedidos.Where(d => d.IdPedido.Equals(id)).Include(d => d.IdFacturaNavigation).
            FirstOrDefaultAsync();

            if (ped != null)
            {
                var query = from pedi in _context.Pedidos
                            join fact in _context.Facturas on pedi.IdFactura equals fact.IdFactura
                            join defa in _context.DetalleFacturas on fact.IdFactura equals defa.IdFactura
                            join empl in _context.Empleados on fact.IdEmpleado equals empl.IdEmpleado
                            join cli in _context.Clientes on fact.IdCliente equals cli.IdCliente
                            join pro in _context.Productos on defa.IdProducto equals pro.IdProducto
                            where pedi.IdPedido == id
                            select new { Factura = fact, Pedido = pedi, DetalleFactura = defa, Empleado = empl, Cliente = cli, Producto = pro };

                var det = query.ToList();

                foreach (var d in det)
                {
                    result.Empleado = d.Factura.IdEmpleadoNavigation.Nombre + " " + d.Factura.IdEmpleadoNavigation.Apellido;
                    result.Cliente = d.Factura.IdClienteNavigation.Nombre + " " + d.Factura.IdClienteNavigation.Apellido;
                    result.Fecha = d.Factura.Fecha;

                    var resultAux = new RdoPed
                    {
                        Cantidad = d.DetalleFactura.Cantidad,
                        Producto = d.DetalleFactura.IdProductoNavigation.Nombre
                    };
                    result.listaPedidos.Add(resultAux);
                }
            }
            return result;
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}