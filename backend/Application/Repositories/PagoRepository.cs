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
    public class PagoRepository : GenericRepositoryString<Pago>, IPago
    {
        private readonly GardensContext _context;

        public PagoRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetClientesByYearAsync(int año)
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT DISTINCT c.*
                FROM Cliente c
                JOIN Pago p ON c.Id = p.IdClienteFk
                WHERE YEAR(p.FechaPago) = {0}", año
            ).ToListAsync();
            return result;
        }

        public async Task<List<Cliente>> GetClientesByDateAsync(int año)
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT DISTINCT c.*
                FROM Cliente c
                JOIN Pago p ON c.Id = p.IdClienteFk
                WHERE DATE_FORMAT(p.FechaPago, '%X') = {0}", año
            ).ToListAsync();
            return result;
        }

        public async Task<List<Cliente>> GetClientesByOtherAsync(int año)
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT DISTINCT c.*
                FROM Cliente c
                JOIN Pago p ON c.Id = p.IdClienteFk
                WHERE EXTRACT(YEAR FROM p.FechaPago) = {0}", año
            ).ToListAsync();
            return result;
        }

        public decimal GetPagosMedia2009()
        {
            var result = _context.Pagos.FromSqlRaw
            (
                @"SELECT p.*
                FROM Pago p
                WHERE EXTRACT(YEAR FROM p.FechaPago) = 2009"
            );
            decimal suma = 0;
            int i = 0;
            foreach (var p in result)
            {
                i += 1;
                suma += p.Total;
            }
            decimal x = suma / i;
            return x;
        }
    }
}