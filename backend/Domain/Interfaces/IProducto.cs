using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProducto : IGenericRepositoryString<Producto>
    {
        Task<List<Producto>> GetProductoOrnamentales100();
        Task<List<Producto>> GetProductoMax();
        Task<List<Producto>> GetProductoMin();
        Task<List<Producto>> GetPrecioMax();
    }
}