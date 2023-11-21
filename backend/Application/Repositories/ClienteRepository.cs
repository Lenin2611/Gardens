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
    public class ClienteRepository : GenericRepositoryInt<Cliente>, ICliente
    {
        private readonly GardensContext _context;

        public ClienteRepository(GardensContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetClientesMadrid11O30()
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT DISTINCT c.*
                FROM Cliente c
                JOIN Direccion d ON c.IdDireccionFk = d.Id
                JOIN Ciudad ci ON d.IdCiudadFk = ci.Id
                WHERE ci.Nombre = 'Madrid' AND c.IdEmpleadoRepresentanteVentasFk = 11 OR c.IdEmpleadoRepresentanteVentasFk = 30"
            ).ToListAsync();
            return result;
        }
        public async Task<List<Cliente>> GetClienteAndRepresentanteDatos()
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT c.*, e.Nombre AS NombreEmpleadoRepresentante, e.Apellido AS ApellidoRepresentante
                FROM Cliente c
                INNER JOIN Direccion d ON c.IdDireccionFk = d.Id
                INNER JOIN Ciudad ci ON d.IdCiudadFk = ci.Id
                INNER JOIN Empleado e ON c.IdEmpleadoRepresentanteVentasFk = e.Id
                "
            ).ToListAsync();

            var datosclientesrrepresentante = new List<Cliente>();
            foreach(var d in result)
            {
                var dato = new Cliente
                {
                    Id = d.Id,
                    Nombre = d.Nombre,
                    NombreContacto = d.NombreContacto,
                    ApellidoContacto = d.ApellidoContacto
                };
                datosclientesrrepresentante.Add(dato);
            }

            return datosclientesrrepresentante;
        }
        public async Task<List<Cliente>> GetClientesPagosRepresentantes()
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT c.*, e.Nombre AS NombreEmpleadoRepresentante
                FROM Cliente c
                    INNER JOIN Pago p ON c.Id = p.IdClienteFk
                    INNER JOIN Direccion d ON c.IdDireccionFk = d.Id
                    INNER JOIN Ciudad ci ON d.IdCiudadFk = ci.Id
                    INNER JOIN Empleado e ON c.IdEmpleadoRepresentanteVentasFk = e.Id
                    WHERE p.IdClienteFk IS NOT NULL
                "
            ).ToListAsync();
            var clientesPagan = new List<Cliente>();
            foreach (var c in result)
            {
                if (clientesPagan.Any(x => x.Id == c.Id))
                {
                    continue;
                }
                else
                {
                    var cliente = new Cliente
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        NombreContacto = c.NombreContacto
                    };
                    clientesPagan.Add(c);
                }
            }
            return result;
        }

        public async Task<List<Cliente>> GetClientesNoPagosRepresentantes()
        {
            var result = await _context.Clientes.FromSqlRaw
            (
                @"SELECT c.*, e.Nombre AS NombreEmpleadoRepresentante
                FROM Cliente c
                    INNER JOIN Pago p ON c.Id = p.IdClienteFk
                    INNER JOIN Direccion d ON c.IdDireccionFk = d.Id
                    INNER JOIN Ciudad ci ON d.IdCiudadFk = ci.Id
                    INNER JOIN Empleado e ON c.IdEmpleadoRepresentanteVentasFk = e.Id
                    WHERE p.IdClienteFk IS NULL
                "
            ).ToListAsync();
            var clientesPagan = new List<Cliente>();
            foreach (var c in result)
            {
                if (clientesPagan.Any(x => x.Id == c.Id))
                {
                    continue;
                }
                else
                {
                    var cliente = new Cliente
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        NombreContacto = c.NombreContacto
                    };
                    clientesPagan.Add(c);
                }
            }
            return result;
        }

        public async Task<List<Cliente>> GetClienteMaxCredito()
        {
            var result = await _context.Clientes.FromSqlRaw(
                @"SELECT * FROM Cliente c
                WHERE c.limiteCredito = (SELECT MAX(limiteCredito) FROM Cliente)"
            ).ToListAsync();
            return result;
        }

        public async Task<List<Cliente>> GetClienteSinPago()
        {
            var result = await _context.Clientes.FromSqlRaw(
                @"SELECT DISTINCT c.*
                FROM Cliente c
                WHERE c.Id NOT IN (
                    SELECT p.IdClienteFk
                    FROM Pago p
                    WHERE COALESCE(p.Total, 0) <> 0
        )"
            ).ToListAsync();
            return result;
        }
    }
}