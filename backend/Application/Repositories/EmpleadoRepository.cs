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
    public class EmpleadoRepository : GenericRepositoryInt<Empleado>, IEmpleado
    {
        private readonly GardensContext _context;

        public EmpleadoRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetEmpleadoByJefe(int id)
        {
            return await _context.Empleados.Where(x => x.IdJefeFk == id)
                        .ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadoByAlberto()
        {
            var result = await _context.Empleados.FromSqlRaw(
                @"SELECT e.*
                FROM empleado e
                WHERE e.IdJefeFk = (SELECT id FROM empleado WHERE nombre = 'alberto' AND apellido = 'soria')"
            ).ToListAsync();
            return result;
        }
    }
}