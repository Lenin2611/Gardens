using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPais : IGenericRepositoryInt<Pais>
    {
        // Task<Pais> GetByPais(string pais);
        Task<List<Cliente>> GetClientesAsync(string pais);
    }
}