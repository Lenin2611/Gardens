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
    public class PedidoRepository : GenericRepositoryInt<Pedido>, IPedido
    {
        private readonly GardensContext _context;

        public PedidoRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetPedidosTardeAsync() // 2611
        {
            return await _context.Pedidos
                        .Where(x => x.FechaEntrega > x.FechaEsperada)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidos2DiasAntesAddDateAsync() // 2611
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pedido p
                WHERE ADDDATE(p.FechaEntrega, 2) <= p.FechaEsperada AND p.FechaEntrega != '1900-01-01'"
            ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Pedido>> GetPedidos2DiasAntesDateDiffAsync() // 2611
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pedido p
                WHERE (DATEDIFF(p.FechaEsperada, p.FechaEntrega) > '2') AND (p.FechaEntrega != '1900-01-01')"
            ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Pedido>> GetPedidos2DiasAntesOperadorAsync() // 2611
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pedido p
                WHERE (p.FechaEsperada - p.FechaEntrega > '2') AND (p.FechaEntrega != '1900-01-01')"
            ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Pedido>> GetPedidosRechazados2009Async() // 2611
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pedido p
                JOIN Estado e ON e.Id = p.IdEstadoFk
                WHERE e.Nombre = 'rechazado'"
            ).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Pedido>> GetPedidosEnero(int año) // 2611
        {
            var result = await _context.Pedidos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pedido p
                WHERE MONTH(p.FechaEntrega) = '1' AND YEAR(p.FechaEntrega) = {0} AND (p.FechaEntrega != '1900-01-01')", año
            ).ToListAsync();
            return result;
        }
    }
}