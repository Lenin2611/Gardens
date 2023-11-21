using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.DtosSec;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;

namespace API.Controllers
{
    public class EmpleadoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;

        public EmpleadoController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
        {
            var results = await _unitOfWork.Empleados.GetAllAsync();
            return _mapper.Map<List<EmpleadoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Get(int id)
        {
            var result = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<EmpleadoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmpleadoDto>> Post(EmpleadoDto resultDto)
        {
            var result = _mapper.Map<Empleado>(resultDto);
            _unitOfWork.Empleados.Add(result);
            await _unitOfWork.SaveAsync();
            if (result == null)
            {
                return BadRequest();
            }
            resultDto.Id = result.Id;
            return CreatedAtAction(nameof(Post), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto resultDto)
        {
            var exists = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == 0)
            {
                resultDto.Id = id;
            }
            if (resultDto.Id != id)
            {
                return BadRequest();
            }
            // Update the properties of the existing entity with values from resultDto
            _mapper.Map(resultDto, exists);
            // The context is already tracking result, so no need to attach it
            await _unitOfWork.SaveAsync();
            // Return the updated entity
            return _mapper.Map<EmpleadoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Empleados.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("empleados7")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmpleadoNombreApellidosEmailDto>>> GetEmpleadosByJefe()
        {
            var result = await _unitOfWork.Empleados.GetEmpleadoByJefe(7);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<EmpleadoNombreApellidosEmailDto>>(result);
        }

        [HttpGet("empleadojefe")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmpleadoPuestoNombreApellidosEmailDto>>> GetEmpleadoPuestoNombreApellidosEmail()
        {
            var results = await (from puesto in _context.Puestos
                        join empleado in _context.Empleados on puesto.Id equals empleado.IdPuestoFk
                        where puesto.Nombre.ToLower().Trim() == "director general"
                        select new EmpleadoPuestoNombreApellidosEmailDto
                        {
                            Puesto = puesto.Nombre,
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email
                        })
                        .ToListAsync();
            return results;
        }

        [HttpGet("empleadosnorepresentante")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmpleadoNombreApellidoPuestoDto>>> GetEmpleadoPuestoNombreApellido()
        {
            var results = await (from puesto in _context.Puestos
                        join empleado in _context.Empleados on puesto.Id equals empleado.IdPuestoFk
                        where puesto.Nombre.ToLower().Trim() != "representante de ventas"
                        select new EmpleadoNombreApellidoPuestoDto
                        {
                            Puesto = puesto.Nombre,
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                        })
                        .ToListAsync();
            return results;
        }

        [HttpGet("cantidadempleados")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCantidadClientes()
        {
            var query = (from empleado in _context.Empleados
                    select new
                    {
                        Cantidad = _context.Empleados.Count()
                    }).Distinct();

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("empleadosConJefes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosConJefes()
        {
            var query = from empleado in _context.Empleados
                        join jefe in _context.Empleados on empleado.IdJefeFk equals jefe.Id into jefes
                        from jefeEmpleado in jefes.DefaultIfEmpty()
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email,
                            NombreJefe = jefeEmpleado != null ? jefeEmpleado.Nombre : "Sin jefe"
                        };

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("empleadosJefesDeJefe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosJefesJefeDeJefe()
        {
            var query = from empleado in _context.Empleados
                        join jefe in _context.Empleados on empleado.IdJefeFk equals jefe.Id into jefes
                        from jefeEmpleado in jefes.DefaultIfEmpty()
                        join jefeDeJefe in _context.Empleados on jefeEmpleado.IdJefeFk equals jefeDeJefe.Id into jefesDeJefe
                        from jefeDeJefeEmpleado in jefesDeJefe.DefaultIfEmpty()
                        select new
                        {
                            NombreEmpleado = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email,
                            NombreJefe = jefeEmpleado != null ? jefeEmpleado.Nombre : "Sin jefe",
                            NombreJefeDeJefe = jefeDeJefeEmpleado != null ? jefeDeJefeEmpleado.Nombre : "Sin jefe de jefe"
                        };

            List<object> result = query.ToList<object>();
            return Ok(result);
        }
        [HttpGet("empleadosSinOficinaLeftJoin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinOficinaLeftJoin()
        {
            var query = from empleado in _context.Empleados
                        join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id into oficinasEmpleado
                        from oficinaEmpleado in oficinasEmpleado.DefaultIfEmpty()
                        where oficinaEmpleado == null
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email
                        };
            List<object> result = query.ToList<object>();
            return Ok(result);
        }
        [HttpGet("empleadosSinClienteLeftJoin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinClienteLeftJoin()
        {
            var query = from empleado in _context.Empleados
                        join cliente in _context.Clientes on empleado.Id equals cliente.IdEmpleadoRepresentanteVentasFk into clientesEmpleado
                        from clienteEmpleado in clientesEmpleado.DefaultIfEmpty()
                        where clienteEmpleado == null
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email
                        };
            List<object> result = query.ToList<object>();
            return Ok(result);
        }

        [HttpGet("empleadosSinClienteYOficina")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinClienteYOficina()
        {
            var query = from empleado in _context.Empleados
                        join cliente in _context.Clientes on empleado.Id equals cliente.IdEmpleadoRepresentanteVentasFk into clientesEmpleado
                        from clienteEmpleado in clientesEmpleado.DefaultIfEmpty()
                        join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id
                        where clienteEmpleado == null
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email,
                            Oficina = oficina
                        };

            List<object> result = query.ToList<object>();

            return Ok(result);
        }

        [HttpGet("empleadosSinOficinaYCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinOficinaYCliente()
        {
            var queryOficina = from empleado in _context.Empleados
                            join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id into oficinasEmpleado
                            from oficinaEmpleado in oficinasEmpleado.DefaultIfEmpty()
                            where oficinaEmpleado == null
                            select new
                            {
                                Empleado = empleado.Nombre,
                                Apellido = empleado.Apellido,
                                Email = empleado.Email,
                                Tipo = "Sin oficina"
                            };

            var queryCliente = from empleado in _context.Empleados
                            join cliente in _context.Clientes on empleado.Id equals cliente.IdEmpleadoRepresentanteVentasFk into clientesEmpleado
                            from clienteEmpleado in clientesEmpleado.DefaultIfEmpty()
                            where clienteEmpleado == null
                            select new
                            {
                                Empleado = empleado.Nombre,
                                Apellido = empleado.Apellido,
                                Email = empleado.Email,
                                Tipo = "Sin cliente"
                            };

            var result = queryOficina.Union(queryCliente);

            List<object> resultList = result.ToList<object>();

            return Ok(resultList);
        }
        [HttpGet("empleadosSinClientesConNombreJefe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinClientesConNombreJefe()
        {
            var query = from empleado in _context.Empleados
                        where !_context.Clientes.Any(cliente => cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id)
                        select new
                        {
                            Empleado = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            Email = empleado.Email,
                            NombreJefe = empleado.IdJefeFkNavigation != null ? empleado.IdJefeFkNavigation.Nombre : "Sin jefe"
                        };

            List<object> result = query.ToList<object>();
            return Ok(result);
        }
        [HttpGet("empleadosSinClientes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinClientes()
        {
            var query = from empleado in _context.Empleados
                        where !_context.Clientes.Any(cliente => cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id)
                        join puesto in _context.Puestos on empleado.IdPuestoFk equals puesto.Id
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellido1 = empleado.Apellido,
                            Cargo = puesto.Nombre
                        };

            var empleadosSinClientes = query.ToList();

            return Ok(empleadosSinClientes);
        }
        [HttpGet("empleadosSinClientesRepresentados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosSinClientesRepresentados()
        {
            var query = from empleado in _context.Empleados
                        where !_context.Clientes.Any(cliente => cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id)
                        join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellidos = empleado.Apellido,
                            Puesto = empleado.IdPuestoFkNavigation.Nombre,
                            TelefonoOficina = oficina.Telefono
                        };

            var empleadosSinClientesRepresentados = query.ToList();

            return Ok(empleadosSinClientesRepresentados);
        }

        [HttpGet("empleadosAlberto")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<EmpleadoSoriaDto>>> GetEmpleadoByAlberto()
        {
            var result = await _unitOfWork.Empleados.GetEmpleadoByAlberto();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<EmpleadoSoriaDto>>(result);
        }
    }
}