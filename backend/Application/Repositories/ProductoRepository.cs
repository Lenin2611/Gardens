using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class ProductoRepository : GenericRepositoryString<Producto>, IProducto
    {
        private readonly GardensContext _context;

        public ProductoRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetProductoOrnamentales100()
        {
            var result = await _context.Productos.FromSqlRaw
            (
                @"SELECT DISTINCT p.*
                FROM Producto p
                JOIN Gamaproducto g ON p.IdGamaProductoFk = g.Id
                WHERE g.Id = 'Ornamentales' AND p.Cantidad > 100
                ORDER BY p.PrecioVenta DESC"
            ).ToListAsync();
            return result;
        }

        public async Task<List<Producto>> GetProductoMax()
        {
            var result = await _context.Productos.FromSqlRaw(
                @"SELECT p.* FROM Producto p
                WHERE p.cantidad = (SELECT MAX(cantidad) FROM Producto)"
            ).ToListAsync();
            return result;
        }

        public async Task<List<Producto>> GetProductoMin()
        {
            var result = await _context.Productos.FromSqlRaw(
                @"SELECT p.* FROM Producto p
                WHERE p.cantidad = (SELECT MIN(cantidad) FROM Producto)"
            ).ToListAsync();
            return result;
        }

        public async Task<List<Producto>> GetPrecioMax()
        {
            var result = await _context.Productos.FromSqlRaw(
                @"SELECT * FROM Producto p WHERE p.precioVenta = (SELECT MAX(precioVenta) FROM Producto)"
            ).ToListAsync();
            return result;
        }
    }
}