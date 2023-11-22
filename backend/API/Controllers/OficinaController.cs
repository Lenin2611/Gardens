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
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class OficinaController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;


        public OficinaController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> Get(int pageIndex = 1, int pageSize = 1)
        {
            var results = await _unitOfWork.Oficinas.GetAllAsync(pageIndex, pageSize);
            return _mapper.Map<List<OficinaDto>>(results.registros);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OficinaDto>> Get(string id)
        {
            var result = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<OficinaDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OficinaDto>> Post(OficinaDto resultDto)
        {
            var result = _mapper.Map<Oficina>(resultDto);
            _unitOfWork.Oficinas.Add(result);
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
        public async Task<ActionResult<OficinaDto>> Put(string id, [FromBody] OficinaDto resultDto)
        {
            var exists = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == string.Empty)
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
            return _mapper.Map<OficinaDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Oficinas.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("oficinasSinEmpleadosRepresentantesDeFrutales")]
        [ApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOficinasSinEmpleadosRepresentantesDeFrutales()
        {
            var query = from oficina in _context.Oficinas
                        where !_context.Empleados.Any(empleado =>
                                empleado.IdOficinaFk == oficina.Id &&
                                _context.Clientes.Any(cliente =>
                                    cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id &&
                                    _context.Pedidos.Any(pedido =>
                                        pedido.IdClienteFk == cliente.Id &&
                                        _context.Detallepedidos.Any(detalle =>
                                            detalle.IdPedidoFk == pedido.Id &&
                                            detalle.IdProductoFkNavigation.IdGamaProductoFk == "Frutales"
                                        )
                                    )
                                )
                        )
                        select new
                        {
                            Id = oficina.Id,
                            Telefono = oficina.Telefono
                        };

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("oficinasSinRepresentantesVentasFrutales")]
        [ApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOficinasSinRepresentantesVentasFrutales()
        {
            var query = from oficina in _context.Oficinas
                        where !_context.Empleados.Any(empleado =>
                            _context.Clientes.Any(cliente =>
                                cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id &&
                                cliente.Pedidos.Any(pedido =>
                                    pedido.Detallepedidos.Any(detalle =>
                                        detalle.IdProductoFkNavigation.IdGamaProductoFk == "Frutales"
                                    )
                                )
                            ) && empleado.IdOficinaFk == oficina.Id
                        )
                        select oficina;

            var oficinasSinRepresentantesVentasFrutales = query.ToList();

            return Ok(oficinasSinRepresentantesVentasFrutales);
        }
    }
}