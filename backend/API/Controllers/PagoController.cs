using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.DtosSec;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;

namespace API.Controllers

{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class PagoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;

        public PagoController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> Get(int pageIndex = 1, int pageSize = 1)
        {
            var results = await _unitOfWork.Pagos.GetAllAsync(pageIndex, pageSize);
            return _mapper.Map<List<PagoDto>>(results.registros);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Get(string id)
        {
            var result = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<PagoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> Post(PagoDto resultDto)
        {
            var result = _mapper.Map<Pago>(resultDto);
            if (resultDto.FechaPago == DateOnly.MinValue)
            {
                resultDto.FechaPago = DateOnly.FromDateTime(DateTime.Now);
                result.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pagos.Add(result);
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
        public async Task<ActionResult<PagoDto>> Put(string id, [FromBody] PagoDto resultDto)
        {
            var exists = await _unitOfWork.Pagos.GetByIdAsync(id);
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
            if (resultDto.FechaPago == DateOnly.MinValue)
            {
                exists.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            }
            // The context is already tracking result, so no need to attach it
            await _unitOfWork.SaveAsync();
            // Return the updated entity
            return _mapper.Map<PagoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Pagos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("clientes2008")] // 2611
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteIdDto>>> GetClientesByYear()
        {
            var result = await _unitOfWork.Pagos.GetClientesByYearAsync(2008);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteIdDto>>(result);
        }

        [HttpGet("clientes2008{año}")] // 2611
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteIdDto>>> GetClientesByDate(int año)
        {
            var result = await _unitOfWork.Pagos.GetClientesByDateAsync(año);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteIdDto>>(result);
        }

        [HttpGet("clientesother{año}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteIdDto>>> GetClientesByOther(int año)
        {
            var result = await _unitOfWork.Pagos.GetClientesByOtherAsync(año);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteIdDto>>(result);
        }

        [HttpGet("pagopaypal2008")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PagoIDFechaTotalDto>>> GetOficinasYCiudades()
        {
            var results = await (from pago in _context.Pagos
                                join formapago in _context.Formapagos on pago.IdFormaPagoFk equals formapago.Id
                                where formapago.Nombre.Trim().ToLower() == "paypal" && pago.FechaPago.Year == 2008
                                select new PagoIDFechaTotalDto
                                {
                                    Id = pago.Id,
                                    FechaPago = pago.FechaPago,
                                    Total = pago.Total,
                                    FormaPago = formapago.Nombre
                                })
                        .ToListAsync();
            return results;
        }

        [HttpGet("pagos2009media")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPagosMedia2009()
        {
            var query = (from empleado in _context.Pagos
                    select new
                    {
                        Cantidad = _unitOfWork.Pagos.GetPagosMedia2009()
                    }).Distinct();

            List<object> result = query.ToList<object>();

            return Ok(result);
        }

        [HttpGet("fechasPrimerUltimoPagoClientes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFechasPrimerUltimoPagoClientes()
        {
            var query = from cliente in _context.Clientes
                        join pago in _context.Pagos on cliente.Id equals pago.IdClienteFk into pagosCliente
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            ApellidosCliente = cliente.ApellidoContacto,
                            FechaPrimerPago = pagosCliente.Min(p => (DateOnly?)p.FechaPago),
                            FechaUltimoPago = pagosCliente.Max(p => (DateOnly?)p.FechaPago)
                        };

            var fechasPrimerUltimoPagoClientes = query.ToList();

            return Ok(fechasPrimerUltimoPagoClientes);
        }

        [HttpGet("pagosOrdenadosPorFecha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPagosOrdenadosPorFecha()
        {
            var query = from pago in _context.Pagos
                        orderby pago.FechaPago descending
                        select new
                        {
                            Id = pago.Id,
                            FechaPago = pago.FechaPago,
                            Total = pago.Total,
                            IdClienteFk = pago.IdClienteFk
                        };

            var pagosOrdenadosPorFecha = query.ToList();

            return Ok(pagosOrdenadosPorFecha);
        }
    }


}