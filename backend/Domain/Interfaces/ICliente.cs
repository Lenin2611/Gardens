using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICliente : IGenericRepositoryInt<Cliente>
    {
        Task<List<Cliente>> GetClientesMadrid11O30();
        Task<List<Cliente>> GetClienteAndRepresentanteDatos();
        Task<List<Cliente>> GetClientesNoPagosRepresentantes();
        Task<List<Cliente>> GetClienteMaxCredito();
        Task<List<Cliente>> GetClienteSinPago();
    }
}