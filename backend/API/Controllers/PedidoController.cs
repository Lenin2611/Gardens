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
    public class PedidoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;

        public PedidoController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
        {
            var results = await _unitOfWork.Pedidos.GetAllAsync();
            return _mapper.Map<List<PedidoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Get(int id)
        {
            var result = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<PedidoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoDto>> Post(PedidoDto resultDto)
        {
            var result = _mapper.Map<Pedido>(resultDto);
            if (resultDto.FechaPedido == DateOnly.MinValue)
            {
                resultDto.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
                result.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
            }
            if (resultDto.FechaEsperada == DateOnly.MinValue)
            {
                resultDto.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
                result.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
            }
            if (resultDto.FechaEntrega == DateOnly.MinValue)
            {
                resultDto.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
                result.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pedidos.Add(result);
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
        public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody] PedidoDto resultDto)
        {
            var exists = await _unitOfWork.Pedidos.GetByIdAsync(id);
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
            if (resultDto.FechaPedido == DateOnly.MinValue)
            {
                exists.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
            }
            if (resultDto.FechaEsperada == DateOnly.MinValue)
            {
                exists.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
            }
            if (resultDto.FechaEntrega == DateOnly.MinValue)
            {
                exists.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
            }
            // The context is already tracking result, so no need to attach it
            await _unitOfWork.SaveAsync();
            // Return the updated entity
            return _mapper.Map<PedidoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Pedidos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("pedidotarde")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoIdClienteIdFechaEntregaDto>>> GetPedidoTarde()
        {
            var result = await _unitOfWork.Pedidos.GetPedidosTardeAsync();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoIdClienteIdFechaEntregaDto>>(result);
        }

        [HttpGet("pedido2diasantesAddDate")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoIdClienteIdFechaEntregaDto>>> GetPedido2DiasAntesAddDate()
        {
            var result = await _unitOfWork.Pedidos.GetPedidos2DiasAntesAddDateAsync();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoIdClienteIdFechaEntregaDto>>(result);
        }

        [HttpGet("pedido2diasantesdatediff")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoIdClienteIdFechaEntregaDto>>> GetPedido2DiasAntesDateDiff()
        {
            var result = await _unitOfWork.Pedidos.GetPedidos2DiasAntesDateDiffAsync();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoIdClienteIdFechaEntregaDto>>(result);
        }

        [HttpGet("pedido2diasantesresta")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoIdClienteIdFechaEntregaDto>>> GetPedido2DiasAntesResta()
        {
            var result = await _unitOfWork.Pedidos.GetPedidos2DiasAntesOperadorAsync();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoIdClienteIdFechaEntregaDto>>(result);
        }

        [HttpGet("pedidorechazado2009")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoDto>>> GetPedidoRechazados2009()
        {
            var result = await _unitOfWork.Pedidos.GetPedidosRechazados2009Async();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoDto>>(result);
        }

        [HttpGet("pedidoenero")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PedidoDto>>> GetPedidoEnero()
        {
            var result = await _unitOfWork.Pedidos.GetPedidosEnero();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<PedidoDto>>(result);
        }

        [HttpGet("pedidoscantidadestado")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PagoCantidadEstadoDto>>> GetCantidadPedidos()
        {
            var results = await (from pedido in _context.Pedidos
                        join estado in _context.Estados on pedido.IdEstadoFk equals estado.Id
                        select new PagoCantidadEstadoDto
                        {
                            Estado = estado.Nombre,
                            Count = 0
                            
                        })
                        .ToListAsync();
            var pagoEstados = new List<PagoCantidadEstadoDto>();
            foreach (var p in results)
            {
                var existingCountry = pagoEstados.FirstOrDefault(x => x.Estado == p.Estado);
                
                if (existingCountry != null)
                {
                    existingCountry.Count += 1;
                }
                else
                {
                    pagoEstados.Add(new PagoCantidadEstadoDto
                    {
                        Estado = p.Estado,
                        Count = 1
                    });
                }
            }
            return pagoEstados.OrderByDescending(x => x.Count).ToList();
        }
    }
}