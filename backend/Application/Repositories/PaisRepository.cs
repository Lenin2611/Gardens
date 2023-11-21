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
    public class PaisRepository : GenericRepositoryInt<Pais>, IPais
    {
        private readonly GardensContext _context;

        public PaisRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        // public async Task<Pais> GetByPais(string pais)
        // {
        //     var result = await _context.Paises.Where(x => x.Nombre.Trim().ToLower() == pais.Trim().ToLower())
        //     .Include(p => p.Regiones)
        //     .ThenInclude(r => r.Ciudades)
        //     .ThenInclude(c => c.Direcciones)
        //     .ThenInclude(d => d.Clientes)
        //     .FirstOrDefaultAsync();
        //     return result;
        // }

        public async Task<List<Cliente>> GetClientesAsync(string pais)
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT c.*
                FROM Pais p
                JOIN Region r ON p.Id = r.IdPaisFk
                JOIN Ciudad ci ON r.Id = ci.IdRegionFk
                JOIN Direccion d ON ci.Id = d.IdCiudadFk
                JOIN Cliente c ON d.Id = c.IdDireccionFk
                WHERE p.Nombre = {0}", pais
            ).ToListAsync();
            return result;
        }
    }
}