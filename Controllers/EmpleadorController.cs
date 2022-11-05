namespace PintoBello_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PintoBello_API.Models;
using PintoBello_API.Request;
using PintoBello_API.Response;

public class EmpleadorController : ControllerBase
{
    private readonly pintureriaContext _context;

    public EmpleadorController(pintureriaContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/empleado/listado")]
    public async Task<ActionResult<RdoEmplUnico>> getEmpleados()
    {
        try
        {
            var result = new RdoEmpleado();
            var empleados = await _context.Empleados.Include(c => c.IdTipoEmpleadoNavigation).ToListAsync();

            if (empleados != null)
            {
                foreach (var e in empleados)
                {
                    var resultAux = new RdoEmplUnico
                    {
                        IdEmpleado = e.IdEmpleado,
                        tipoEmpleado = e.IdTipoEmpleadoNavigation.Tipo,
                        Legajo = e.Legajo,
                        Nombre = e.Nombre,
                        Apellido = e.Apellido,
                        Dni = e.Dni,
                        Telefono = e.Telefono,
                        Mail = e.Mail,
                        Usuario = e.Usuario
                    };
                    result.empleadosLista.Add(resultAux);
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
    [Route("api/empleado/obtenerId/{id}")]
    public async Task<ActionResult<RdoEmplUnico>> getEmpleadoId(int id)
    {
        try
        {
            var result = new RdoEmplUnico();
            var empleado = await _context.Empleados.Where(c => c.IdEmpleado.Equals(id)).Include(c => c.IdTipoEmpleadoNavigation).
            FirstOrDefaultAsync();

            if (empleado != null)
            {
                result.IdEmpleado = empleado.IdEmpleado;
                result.tipoEmpleado = empleado.IdTipoEmpleadoNavigation.Tipo;
                result.Legajo = empleado.Legajo;
                result.Nombre = empleado.Nombre;
                result.Apellido = empleado.Apellido;
                result.Dni = empleado.Dni;
                result.Telefono = empleado.Telefono;
                result.Mail = empleado.Mail;
                result.Usuario = empleado.Usuario;
            }
            return result;
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/empleado/alta")]
    public async Task<ActionResult<bool>> postEmpleado([FromBody] EmpleadoRequest cmd)
    {
        try
        {
            var result = new RdoEmplUnico();
            var empleado = await _context.Empleados.Where(c => c.Dni.Equals(cmd.Dni)).FirstOrDefaultAsync();

            if (empleado == null)
            {
                var nuevo = new Empleado()
                {
                    IdTipoEmpleado = cmd.IdTipoEmpleado,
                    Legajo = cmd.Legajo,
                    Nombre = cmd.Nombre,
                    Apellido = cmd.Apellido,
                    Dni = cmd.Dni,
                    Telefono = cmd.Telefono,
                    Mail = cmd.Mail,
                    Usuario = cmd.Usuario,
                    Contrasena = cmd.Contrasena
                };
                await _context.AddAsync(nuevo);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok("El empleado ya existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPut]
    [Route("api/empleado/update/{id}")]
    public async Task<ActionResult<bool>> editEmpleado([FromBody] EmpleadoEditRequest cmd, int id)
    {
        try
        {
            var empleado = await _context.Empleados.Where(c => c.IdEmpleado.Equals(id)
            ).FirstOrDefaultAsync();

            if (empleado != null)
            {
                empleado.Nombre = cmd.Nombre;
                empleado.Apellido = cmd.Apellido;
                empleado.Mail = cmd.Mail;
                empleado.Telefono = cmd.Telefono;

                _context.Update(empleado);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok("No existen empleados con esas credenciales");
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPut]
    [Route("api/empleado/updateUsuario/{id}")]
    public async Task<ActionResult<bool>> editUsuEmpleado([FromBody] EmpleadoPassRequest cmd, int id)
    {
        try
        {
            var empleado = await _context.Empleados.Where(c => c.IdEmpleado.Equals(id)
            ).FirstOrDefaultAsync();

            if (empleado != null)
            {
                empleado.Usuario = cmd.Usuario;
                empleado.Contrasena = cmd.Contrasena;

                _context.Update(empleado);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok("No existen empleados con esas credenciales");
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    /*  Esto si la base de datos tuviera un campo Activo en el campo
    
        [HttpPut]
        [Route("api/empleado/baja/{id}")]
        public async Task<ActionResult<bool>> deleteEmpleado(int id)
        {
            try
            {
                var empleado = await _context.Empleados.Where(c => c.IdEmpleado.Equals(id)).FirstOrDefaultAsync();
                if (empleado != null)
                {
                    //empleado.estado = 1 --> inactivo

                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception e)
            {
                return BadRequest("No se puede realizar esta acción");
            }
    }*/
}
