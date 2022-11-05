namespace PintoBello_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PintoBello_API.Models;
using PintoBello_API.Request;
using PintoBello_API.Response;
public class ProveedoresController : ControllerBase
{
    private readonly pintureriaContext _context;

    public ProveedoresController(pintureriaContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/proveedores/listado")]
    public async Task<ActionResult<RdoProveedores>> getProveedores()
    {
        try
        {
            var result = new RdoProveedores();
            var proveedores = await _context.Proveedors.ToListAsync();

            if (proveedores != null)
            {
                foreach (var p in proveedores)
                {
                    var resultAux = new RdoProv
                    {
                        Nombre = p.Nombre,
                        Apellido = p.Apellido,
                        Dni = p.Dni,
                        Telefono = p.Telefono,
                        Mail = p.Mail
                    };
                    result.listaProveedores.Add(resultAux);
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

    [HttpGet]
    [Route("api/proveedores/obtenerId/{id}")]
    public async Task<ActionResult<RdoProv>> getClienteId(int id)
    {
        try
        {
            var result = new RdoProv();
            var prov = await _context.Proveedors.Where(p => p.IdProveedor.Equals(id)).FirstOrDefaultAsync();

            if (prov != null)
            {
                result.Nombre = prov.Nombre;
                result.Apellido = prov.Apellido;
                result.Dni = prov.Dni;
                result.Telefono = prov.Telefono;
                result.Mail = prov.Mail;
            }
            return result;
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/proveedores/alta")]
    public async Task<ActionResult<bool>> altaProveedor([FromBody] ProveedorRequest cmd)
    {
        try
        {
            var prov = await _context.Proveedors.Where(c => c.Dni.Equals(cmd.Dni)).FirstOrDefaultAsync();

            if (prov == null)
            {
                var nuevo = new Proveedor()
                {
                    Nombre = cmd.Nombre,
                    Apellido = cmd.Apellido,
                    Dni = cmd.Dni,
                    Telefono = cmd.Telefono,
                    Mail = cmd.Mail
                };
                await _context.AddAsync(nuevo);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok("El proveedor ya existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPut]
    [Route("api/proveedores/update/{id}")]
    public async Task<ActionResult<bool>> editProveedor([FromBody] ProveedorEditRequest cmd, int id)
    {
        try
        {
            var proveedor = await _context.Proveedors.Where(c => c.IdProveedor.Equals(id)).FirstOrDefaultAsync();

            if (proveedor != null)
            {
                proveedor.Nombre = cmd.Nombre;
                proveedor.Apellido = cmd.Apellido;
                proveedor.Telefono = cmd.Telefono;
                proveedor.Mail = cmd.Mail;

                _context.Update(proveedor);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok("No existen proveedores con esas credenciales");
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}
