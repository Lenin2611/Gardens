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
    public class ProductoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;


        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            var results = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<List<ProductoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(string id)
        {
            var result = await _unitOfWork.Productos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProductoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto resultDto)
        {
            var result = _mapper.Map<Producto>(resultDto);
            _unitOfWork.Productos.Add(result);
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
        public async Task<ActionResult<ProductoDto>> Put(string id, [FromBody] ProductoDto resultDto)
        {
            var exists = await _unitOfWork.Productos.GetByIdAsync(id);
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
            return _mapper.Map<ProductoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _unitOfWork.Productos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Productos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("productosornamentales100")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductoNormalDto>>> GetProductosOrnamentales100()
        {
            var result = await _unitOfWork.Productos.GetProductoOrnamentales100();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductoNormalDto>>(result);
        }
        [HttpGet("productosSinPedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductosSinPedidos()
        {
            var query = from producto in _context.Productos
                        where !_context.Detallepedidos.Any(detalle => detalle.IdProductoFk == producto.Id)
                        select producto;

            List<Producto> result = query.ToList();

            return Ok(result);
                }

        // [HttpGet("productosSinPedidosDetalle")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public IActionResult GetProductosSinPedidosDetalle()
        // {
        //     var query = from producto in _context.Productos
        //                 where !_context.Detallepedidos.Any(detalle => detalle.IdProductoFk == producto.Id)
        //                 select new
        //                 {
        //                     Nombre = producto.Nombre,
        //                     Descripcion = producto.Descripcion,
        //                     Imagen = producto.
        //                 };

        //     List<object> result = query.ToList<object>();

        //     return Ok(result);
        // }

        [HttpGet("top20ProductosMasVendidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTop20ProductosMasVendidos()
        {
            var query = from detalle in _context.Detallepedidos
                        group detalle by detalle.IdProductoFk into productosVendidos
                        orderby productosVendidos.Sum(detalle => detalle.Cantidad) descending
                        select new
                        {
                            IdProducto = productosVendidos.Key,
                            TotalUnidadesVendidas = productosVendidos.Sum(detalle => detalle.Cantidad)
                        };

            var top20 = query.Take(20).ToList();

            return Ok(top20);
        }
        [HttpGet("ProductosSinPedidosEXISTS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductosSinPedidosEXISTS()
        {
            var query = from producto in _context.Productos
                        where !(_context.Detallepedidos.Any(detalle => detalle.IdProductoFk == producto.Id))
                        select producto;

            var productosSinPedidos = query.ToList();

            return Ok(productosSinPedidos);
        }
        [HttpGet("ProductosConPedidosEXISTS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductosConPedidosEXISTS()
        {
            var query = from producto in _context.Productos
                        where _context.Detallepedidos.Any(detalle => detalle.IdProductoFk == producto.Id)
                        select producto;

            var productosConPedidos = query.ToList();

            return Ok(productosConPedidos);
        }

        [HttpGet("productobaratocaro")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoPrecioVentaBaratoCaroDto>> GetProductoBaratoCaro()
        {
            var results = await _context.Productos.ToListAsync();
            var max = results.OrderByDescending(x => x.PrecioVenta).FirstOrDefault();
            var min = results.OrderBy(x => x.PrecioVenta).FirstOrDefault();
            return new ProductoPrecioVentaBaratoCaroDto
            {
                NombreBarato = min.Nombre,
                Barato = min.PrecioVenta,
                NombreCaro = max.Nombre,
                Caro = max.PrecioVenta
            };
        }

        [HttpGet("productoMax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductoMaxDto>>> GetProductoMax()
        {
            var result = await _unitOfWork.Productos.GetProductoMax();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductoMaxDto>>(result);
        }

        [HttpGet("productoMin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductoMinDto>>> GetProductoMin()
        {
            var result = await _unitOfWork.Productos.GetProductoMin();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductoMinDto>>(result);
        }

        [HttpGet("productoPrecioMax")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductoPrecioMaxDto>>> GetPrecioMax()
        {
            var result = await _unitOfWork.Productos.GetPrecioMax();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductoPrecioMaxDto>>(result);
        }
    }
}