using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmpleado : IGenericRepositoryInt<Empleado>
    {
        Task<IEnumerable<Empleado>> GetEmpleadoByJefe(int id);
        Task<List<Empleado>> GetEmpleadoByAlberto();
    }
}