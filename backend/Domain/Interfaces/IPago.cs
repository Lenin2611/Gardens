using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPago : IGenericRepositoryString<Pago>
    {
        Task<List<Cliente>> GetClientesByYearAsync(int año);
        Task<List<Cliente>> GetClientesByDateAsync(int año);
        Task<List<Cliente>> GetClientesByOtherAsync(int año);
        decimal GetPagosMedia2009();
    }
}