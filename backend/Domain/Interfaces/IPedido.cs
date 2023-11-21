using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPedido : IGenericRepositoryInt<Pedido>
    {
        Task<IEnumerable<Pedido>> GetPedidosTardeAsync();
        Task<IEnumerable<Pedido>> GetPedidos2DiasAntesAddDateAsync();
        Task<IEnumerable<Pedido>> GetPedidos2DiasAntesDateDiffAsync();
        Task<IEnumerable<Pedido>> GetPedidos2DiasAntesOperadorAsync();
        Task<IEnumerable<Pedido>> GetPedidosRechazados2009Async();
        Task<IEnumerable<Pedido>> GetPedidosEnero(int año);
    }
}