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
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class PaisController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;

        public PaisController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PaisDto>>> Get()
        {
            var results = await _unitOfWork.Paises.GetAllAsync();
            return _mapper.Map<List<PaisDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaisDto>> Get(int id)
        {
            var result = await _unitOfWork.Paises.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<PaisDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaisDto>> Post(PaisDto resultDto)
        {
            var result = _mapper.Map<Pais>(resultDto);
            _unitOfWork.Paises.Add(result);
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
        public async Task<ActionResult<PaisDto>> Put(int id, [FromBody] PaisDto resultDto)
        {
            var exists = await _unitOfWork.Paises.GetByIdAsync(id);
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
            return _mapper.Map<PaisDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Paises.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Paises.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // [HttpGet("clientes{pais}")] // 2611
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<DireccionClienteDto>> GetClientesByPais(string pais)
        // {
        //     var result = await _unitOfWork.Paises.GetByPais(pais);
        //     if (result == null)
        //     {
        //         return NotFound();
        //     }

        //     return _mapper.Map<DireccionClienteDto>(result);
        // }

        [HttpGet("clientesespaña")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteNombreDto>>> GetClientesByPais()
        {
            var result = await _unitOfWork.Paises.GetClientesAsync("españa");
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteNombreDto>>(result);
        }

        [HttpGet("oficinasespaña")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OficinaIdTelefonoDto>>> GetOficinasByPais()
        {
            var results = await (from oficina in _context.Oficinas
                                 join direccion in _context.Direcciones on oficina.Id equals direccion.IdOficinaFk
                                 join ciudad in _context.Ciudades on direccion.IdCiudadFk equals ciudad.Id
                                 join region in _context.Regiones on ciudad.IdRegionFk equals region.Id
                                 join pais in _context.Paises on region.IdPaisFk equals pais.Id
                                 where pais.Nombre == "España"
                                 select new OficinaIdTelefonoDto
                                 {
                                     Id = oficina.Id,
                                     Telefono = oficina.Telefono,
                                     Ciudad = ciudad.Nombre
                                 })
                        .ToListAsync();
            return results;
        }
    }
}