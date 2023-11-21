using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    public class DetallepedidoController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;


        public DetallepedidoController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DetallepedidoDto>>> Get()
        {
            var results = await _unitOfWork.Detallepedidos.GetAllAsync();
            return _mapper.Map<List<DetallepedidoDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetallepedidoDto>> Get(int id)
        {
            var result = await _unitOfWork.Detallepedidos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<DetallepedidoDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetallepedidoDto>> Post(DetallepedidoDto resultDto)
        {
            var result = _mapper.Map<Detallepedido>(resultDto);
            _unitOfWork.Detallepedidos.Add(result);
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
        public async Task<ActionResult<DetallepedidoDto>> Put(int id, [FromBody] DetallepedidoDto resultDto)
        {
            var exists = await _unitOfWork.Detallepedidos.GetByIdAsync(id);
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
            return _mapper.Map<DetallepedidoDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Detallepedidos.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Detallepedidos.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }



        [HttpGet("sumaCantidadProductosPorPedido")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSumaCantidadProductosPorPedido()
        {
            var query = from detalle in _context.Detallepedidos
                        group detalle by detalle.IdPedidoFk into productosPorPedido
                        select new
                        {
                            IdPedido = productosPorPedido.Key,
                            CantidadTotal = productosPorPedido.Sum(detalle => detalle.Cantidad)
                        };

            var sumaCantidadPorPedido = query.ToList();

            return Ok(sumaCantidadPorPedido);
        }

        [HttpGet("facturacionTotal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFacturacionTotal()
        {
            var query = from detalle in _context.Detallepedidos
                        where detalle.Cantidad.HasValue && detalle.PrecioUnidad.HasValue
                        select new
                        {
                            BaseImponible = detalle.Cantidad.Value * detalle.PrecioUnidad.Value,
                            IVA = (detalle.Cantidad.Value * detalle.PrecioUnidad.Value) * 0.21m // 21% de IVA
                        };

            decimal baseImponibleTotal = query.Sum(d => d.BaseImponible);
            decimal ivaTotal = query.Sum(d => d.IVA);
            decimal totalFacturado = baseImponibleTotal + ivaTotal;

            var facturacion = new
            {
                BaseImponibleTotal = baseImponibleTotal,
                IVATotal = ivaTotal,
                TotalFacturado = totalFacturado
            };

            return Ok(facturacion);
        }

        [HttpGet("facturacionTotalPorProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFacturacionTotalPorProducto()
        {
            var query = from detalle in _context.Detallepedidos
                        where detalle.Cantidad.HasValue && detalle.PrecioUnidad.HasValue
                        group detalle by detalle.IdProductoFk into productos
                        select new
                        {
                            IdProducto = productos.Key,
                            BaseImponibleTotal = productos.Sum(d => d.Cantidad.Value * d.PrecioUnidad.Value),
                            IVATotal = productos.Sum(d => (d.Cantidad.Value * d.PrecioUnidad.Value) * 0.21m),
                            TotalFacturado = productos.Sum(d => (d.Cantidad.Value * d.PrecioUnidad.Value) * 1.21m)
                        };

            var facturacionPorProducto = query.ToList();

            return Ok(facturacionPorProducto);
        }

        [HttpGet("facturacionPorCodigoOR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFacturacionPorCodigoOR()
        {
            var query = from detalle in _context.Detallepedidos
                        where detalle.Cantidad.HasValue && detalle.PrecioUnidad.HasValue
                            && detalle.IdProductoFk.StartsWith("OR")
                        group detalle by detalle.IdProductoFk into productos
                        select new
                        {
                            IdProducto = productos.Key,
                            BaseImponibleTotal = productos.Sum(d => d.Cantidad.Value * d.PrecioUnidad.Value),
                            IVATotal = productos.Sum(d => (d.Cantidad.Value * d.PrecioUnidad.Value) * 0.21m),
                            TotalFacturado = productos.Sum(d => (d.Cantidad.Value * d.PrecioUnidad.Value) * 1.21m)
                        };

            var facturacionPorCodigoOR = query.ToList();

            return Ok(facturacionPorCodigoOR);
        }

        [HttpGet("ventasProductosMas3000Euros")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVentasProductosMas3000Euros()
        {
            var query = from detalle in _context.Detallepedidos
                        where detalle.Cantidad.HasValue && detalle.PrecioUnidad.HasValue
                        group detalle by detalle.IdProductoFk into productos
                        let totalFacturado = productos.Sum(d => d.Cantidad.Value * d.PrecioUnidad.Value)
                        let totalConIVA = totalFacturado * 1.21m // Total facturado con 21% IVA
                        where totalConIVA > 3000
                        select new
                        {
                            IdProducto = productos.Key,
                            TotalFacturado = totalFacturado,
                            TotalConIVA = totalConIVA
                        };

            var productosMas3000Euros = query.ToList();

            var ventasMas3000Euros = productosMas3000Euros.Select(producto =>
                new
                {
                    NombreProducto = _context.Productos.FirstOrDefault(p => p.Id == producto.IdProducto)?.Nombre,
                    UnidadesVendidas = _context.Detallepedidos.Where(d => d.IdProductoFk == producto.IdProducto).Sum(d => d.Cantidad),
                    producto.TotalFacturado,
                    producto.TotalConIVA
                }
            ).ToList();

            return Ok(ventasMas3000Euros);
        }

        [HttpGet("sumaTotalPagosPorAnio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSumaTotalPagosPorAnio()
        {
            var query = from pago in _context.Pagos
                        group pago by pago.FechaPago.Year into pagosPorAnio
                        select new
                        {
                            Anio = pagosPorAnio.Key,
                            SumaTotalPagos = pagosPorAnio.Sum(pago => pago.Total)
                        };
            var sumaTotalPorAnio = query.ToList();
            return Ok(sumaTotalPorAnio);
        }
        
        [HttpGet("productoMasVendido")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductoMasVendido()
        {
            var query = from detalle in _context.Detallepedidos
                        group detalle by detalle.IdProductoFkNavigation.Nombre into productos
                        orderby productos.Sum(p => p.Cantidad) descending
                        select productos.Key;

            var productoMasVendido = query.FirstOrDefault();

            return Ok(productoMasVendido);
        }
    }
}