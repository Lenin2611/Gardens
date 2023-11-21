using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.DtosSec;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;

namespace API.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardensContext _context;

        public ClienteController(IUnitOfWork unitOfWork, IMapper mapper, GardensContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {
            var results = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<List<ClienteDto>>(results);
        }

        [HttpGet("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Get(int id)
        {
            var result = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<ClienteDto>(result);
        }

        [HttpPost] // 2611
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Post(ClienteDto resultDto)
        {
            var result = _mapper.Map<Cliente>(resultDto);
            _unitOfWork.Clientes.Add(result);
            await _unitOfWork.SaveAsync();
            if (result == null)
            {
                return BadRequest();
            }
            resultDto.Id = result.Id;
            return CreatedAtAction(nameof(Post), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto resultDto)
        {
            var exists = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (exists == null)
            {
                return NotFound();
            }
            if (resultDto.Id == 0)
            {
                resultDto.Id = id;
            }
            if (resultDto.Id != id)
            {
                return BadRequest();
            }
            // Update the properties of the existing entity with values from resultDto
            _mapper.Map(resultDto, exists);
            // The context is already tracking result, so no need to attach it
            await _unitOfWork.SaveAsync();
            // Return the updated entity
            return _mapper.Map<ClienteDto>(exists);
        }

        [HttpDelete("{id}")] // 2611
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Clientes.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("clientesmadrid11o30")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteNombreDto>>> GetClientesMadrid11O30()
        {
            var result = await _unitOfWork.Clientes.GetClientesMadrid11O30();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteNombreDto>>(result);
        }

        [HttpGet("cantidadclientesbypais")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientesCantidadPaisDto>>> GetCantidadClientesById()
        {
            var results = await (from cliente in _context.Clientes
                        join direccion in _context.Direcciones on cliente.IdDireccionFk equals direccion.Id
                        join ciudad in _context.Ciudades on direccion.IdCiudadFk equals ciudad.Id
                        join region in _context.Regiones on ciudad.IdRegionFk equals region.Id
                        join pais in _context.Paises on region.IdPaisFk equals pais.Id
                        select new ClientesCantidadPaisDto
                        {
                            Pais = pais.Nombre,
                            Count = 0
                            
                        })
                        .ToListAsync();
            var paisesClientes = new List<ClientesCantidadPaisDto>();
            foreach (var p in results)
            {
                var existingCountry = paisesClientes.FirstOrDefault(x => x.Pais == p.Pais);
                
                if (existingCountry != null)
                {
                    existingCountry.Count += 1;
                }
                else
                {
                    paisesClientes.Add(new ClientesCantidadPaisDto
                    {
                        Pais = p.Pais,
                        Count = 1
                    });
                }
            }
            return paisesClientes;
        }

        [HttpGet("cantidadclientes")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public int GetCantidadClientes()
        {
            int result = _context.Clientes.Count();
            return result;
        }

        [HttpGet("cantidadclientesmadrid")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public int GetCantidadClientesMadrid()
        {
            var results = (from cliente in _context.Clientes
                        join direccion in _context.Direcciones on cliente.IdDireccionFk equals direccion.Id
                        join ciudad in _context.Ciudades on direccion.IdCiudadFk equals ciudad.Id
                        where ciudad.Nombre == "Madrid"
                        select new ClienteNombreDto
                        {
                            Nombre = cliente.Nombre
                        }).ToList();
            int suma = 0;
            foreach(var c in results)
            {
                suma += 1;
            }
            return suma;
        }

        [HttpGet("cantidadclientesciudadm")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientesCantidadCiudadDto>>> GetCantidadClientesByCiudadM()
        {
            var results = await (from cliente in _context.Clientes
                        join direccion in _context.Direcciones on cliente.IdDireccionFk equals direccion.Id
                        join ciudad in _context.Ciudades on direccion.IdCiudadFk equals ciudad.Id
                        where ciudad.Nombre.StartsWith("M")
                        select new ClientesCantidadCiudadDto
                        {
                            Ciudad = ciudad.Nombre,
                            Count = 0
                            
                        })
                        .ToListAsync();
            var ciudadesClientes = new List<ClientesCantidadCiudadDto>();
            foreach (var p in results)
            {
                var existingCiudad = ciudadesClientes.FirstOrDefault(x => x.Ciudad == p.Ciudad);
                
                if (existingCiudad != null)
                {
                    existingCiudad.Count += 1;
                }
                else
                {
                    ciudadesClientes.Add(new ClientesCantidadCiudadDto
                    {
                        Ciudad = p.Ciudad,
                        Count = 1
                    });
                }
            }
            return ciudadesClientes;
        }

        [HttpGet("cantidadclientessinrepresentante")] // 2611
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCantidadClientesSinRepresentante()
        {
            var results = await (from cliente in _context.Clientes
                join empleado in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals empleado.Id
                join puesto in _context.Puestos on empleado.IdPuestoFk equals puesto.Id
                where puesto.Nombre == "Representante de Ventas"
                select cliente.Nombre
            )
            .ToListAsync();
            int x = results.Count();
            return x;
        }

        [HttpGet("clienteDatosRepresentanteVenta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteRepresentanteVentasDto>>> GetClienteAndRepresentanteDatos()
        {
            var result = await _unitOfWork.Clientes.GetClienteAndRepresentanteDatos();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteRepresentanteVentasDto>>(result);
        }
        [HttpGet("clientesPagosRepresentantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientePagosRepresentanteDto>>> GetClientesPagosRepresentantes()
        {
            var result = await _unitOfWork.Clientes.GetClienteAndRepresentanteDatos();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClientePagosRepresentanteDto>>(result);
        }
        [HttpGet("clientesNoPagosRepresentantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClientePagosRepresentanteDto>>> GetClientesNoPagosRepresentantes()
        {
            var result = await _unitOfWork.Clientes.GetClientesNoPagosRepresentantes();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClientePagosRepresentanteDto>>(result);
        }
        [HttpGet("clientesPagosRepresentantesCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesPagosRepresentantesCiudad()
        {
        var query = (from pago in _context.Pagos
                    join cliente in _context.Clientes on pago.IdClienteFk equals cliente.Id
                    join representante in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals representante.Id
                    join direccionRepresentante in _context.Direcciones on representante.IdDireccionFk equals direccionRepresentante.Id
                    join ciudadRepresentante in _context.Ciudades on direccionRepresentante.IdCiudadFk equals ciudadRepresentante.Id
                    select new
                    {
                        NombreCliente = cliente.Nombre,
                        NombreRepresentante = representante.Nombre,
                        CiudadRepresentante = ciudadRepresentante.Nombre
                    }).Distinct();

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("clientesNoPagosRepresentantesCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagosDetalles()
        {
            var query = (from cliente in _context.Clientes
                        join representante in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals representante.Id
                        join direccionRepresentante in _context.Direcciones on representante.IdDireccionFk equals direccionRepresentante.Id
                        join ciudadRepresentante in _context.Ciudades on direccionRepresentante.IdCiudadFk equals ciudadRepresentante.Id
                        where !_context.Pagos.Any(p => p.IdClienteFk == cliente.Id)
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            NombreRepresentante = representante.Nombre,
                            CiudadRepresentante = ciudadRepresentante.Nombre
                        }).Distinct();

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("clientesRepresentantesCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesRepresentantesCiudad()
        {
            var query = from cliente in _context.Clientes
                        join representante in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals representante.Id
                        join direccionRepresentante in _context.Direcciones on representante.IdDireccionFk equals direccionRepresentante.Id
                        join ciudadRepresentante in _context.Ciudades on direccionRepresentante.IdCiudadFk equals ciudadRepresentante.Id
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            NombreRepresentante = representante.Nombre,
                            CiudadRepresentante = ciudadRepresentante.Nombre
                        };

            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("clientesConPedidosRetrasados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPedidosRetrasados()
        {
            var query = from pedido in _context.Pedidos
                        where pedido.FechaEntrega > pedido.FechaEsperada
                        select new
                        {
                            NombreCliente = pedido.IdClienteFkNavigation.Nombre
                        };

            // Ejecutar la consulta y obtener los resultados
            List<object> result = query.ToList<object>();

            return Ok(result);
        }

        [HttpGet("clientesSinPagosLeftJoin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagosLeftJoin()
        {
            var query = from cliente in _context.Clientes
                        join pago in _context.Pagos on cliente.Id equals pago.IdClienteFk into pagosCliente
                        from pagoCliente in pagosCliente.DefaultIfEmpty()
                        where pagoCliente == null
                        select new
                        {
                            NombreCliente = cliente.Nombre
                        };
            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("clientesSinPedidosLeftJoin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPedidosLeftJoin()
        {
            var query = from cliente in _context.Clientes
                        join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk into pedidosCliente
                        from pedidoCliente in pedidosCliente.DefaultIfEmpty()
                        where pedidoCliente == null
                        select new
                        {
                            NombreCliente = cliente.Nombre
                            // Puedes añadir más detalles aquí si los necesitas
                        };

            // Ejecutar la consulta y obtener los resultados
            List<object> result = query.ToList<object>();

            return Ok(result);
        }
        [HttpGet("clientesSinPagosYPedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagosYPedidos()
        {
            var queryPagos = from cliente in _context.Clientes
                            join pago in _context.Pagos on cliente.Id equals pago.IdClienteFk into pagosCliente
                            from pagoCliente in pagosCliente.DefaultIfEmpty()
                            where pagoCliente == null
                            select new
                            {
                                NombreCliente = cliente.Nombre,
                                Tipo = "Sin pago"
                            };

            var queryPedidos = from cliente in _context.Clientes
                            join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk into pedidosCliente
                            from pedidoCliente in pedidosCliente.DefaultIfEmpty()
                            where pedidoCliente == null
                            select new
                            {
                                NombreCliente = cliente.Nombre,
                                Tipo = "Sin pedido"
                            };

            var result = queryPagos.Union(queryPedidos);
            List<object> resultList = result.ToList<object>();

            return Ok(resultList);
        }
        [HttpGet("clientesConPedidoSinPago")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPedidoSinPago()
        {
            var query = from cliente in _context.Clientes
                        where _context.Pedidos.Any(pedido => pedido.IdClienteFk == cliente.Id) &&
                            !_context.Pagos.Any(pago => pago.IdClienteFk == cliente.Id)
                        select cliente;

            List<Cliente> result = query.ToList();

            return Ok(result);
        }

        [HttpGet("clientesSinPagos")] // Implicit Exists
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagos()
        {
            var query = from cliente in _context.Clientes
                        where !(_context.Pagos.Any(pago => pago.IdClienteFk == cliente.Id))
                        select cliente;

            List<Cliente> clientesSinPagos = query.ToList();

            return Ok(clientesSinPagos);
        }

        [HttpGet("clientesConPagos")] // Implicit Exists
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPagos()
        {
            var query = from cliente in _context.Clientes
                        where _context.Pagos.Any(pago => pago.IdClienteFk == cliente.Id)
                        select cliente;

            List<Cliente> clientesConPagos = query.ToList();

            return Ok(clientesConPagos);
        }
        [HttpGet("clientesConCantidadPedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConCantidadPedidos()
        {
            var query = from cliente in _context.Clientes
                        join pedido in _context.Pedidos on cliente.Id equals pedido.IdClienteFk into clientePedidos
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            CantidadPedidos = clientePedidos.Count()
                        };

            var resultado = query.ToList();

            return Ok(resultado);
        }

        [HttpGet("totalPagadoPorCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTotalPagadoPorCliente()
        {
            var query = from cliente in _context.Clientes
                        join pago in _context.Pagos on cliente.Id equals pago.IdClienteFk into clientePagos
                        from pagoCliente in clientePagos.DefaultIfEmpty()
                        group pagoCliente by cliente.Nombre into pagosPorCliente
                        select new
                        {
                            NombreCliente = pagosPorCliente.Key,
                            TotalPagado = pagosPorCliente.Sum(p => p.Total)
                        };

            var resultado = query.ToList();

            return Ok(resultado);
        }
        [HttpGet("clientesConPedidosEn2008")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPedidosEn2008()
        {
            var query = from pedido in _context.Pedidos
                        where pedido.FechaPedido.Year == 2008
                        join cliente in _context.Clientes on pedido.IdClienteFk equals cliente.Id
                        orderby cliente.Nombre
                        select cliente.Nombre;

            var clientesEn2008 = query.ToList();

            return Ok(clientesEn2008);
        }
        [HttpGet("clientesSinPagosInfoRepresentante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagosInfoRepresentante()
        {
            var query = from cliente in _context.Clientes
                        where !_context.Pagos.Any(p => p.IdClienteFk == cliente.Id)
                        join empleado in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals empleado.Id into representantes
                        from representante in representantes
                        join oficina in _context.Oficinas on representante.IdOficinaFk equals oficina.Id
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            NombreRepresentante = representante.Nombre,
                            ApellidoRepresentante = representante.Apellido,
                            TelefonoOficinaRepresentante = oficina.Telefono
                        };

            var clientesSinPagosInfoRepresentante = query.ToList();

            return Ok(clientesSinPagosInfoRepresentante);
        }
        [HttpGet("clientesInfoRepresentanteYCiudad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesInfoRepresentanteYCiudad()
        {
            var query = from cliente in _context.Clientes
                        join empleado in _context.Empleados on cliente.IdEmpleadoRepresentanteVentasFk equals empleado.Id
                        join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id
                        join ciudad in _context.Ciudades on oficina.IdDireccionFk equals ciudad.Id
                        select new
                        {
                            NombreCliente = cliente.Nombre,
                            NombreRepresentante = empleado.Nombre,
                            ApellidoRepresentante = empleado.Apellido,
                            CiudadOficinaRepresentante = ciudad.Nombre
                        };

            var clientesInfoRepresentanteYCiudad = query.ToList();

            return Ok(clientesInfoRepresentanteYCiudad);
        }

        [HttpGet("empleadosNoRepresentantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleadosNoRepresentantes()
        {
            var query = from empleado in _context.Empleados
                        where !(_context.Clientes.Any(cliente => cliente.IdEmpleadoRepresentanteVentasFk == empleado.Id))
                        join puesto in _context.Puestos on empleado.IdPuestoFk equals puesto.Id
                        join oficina in _context.Oficinas on empleado.IdOficinaFk equals oficina.Id
                        select new
                        {
                            Nombre = empleado.Nombre,
                            Apellidos = empleado.Apellido,
                            Puesto = puesto.Nombre,
                            TelefonoOficina = oficina.Telefono
                        };

            var empleadosNoRepresentantes = query.ToList();

            return Ok(empleadosNoRepresentantes);
        }
        [HttpGet("ciudadesConNumeroEmpleados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCiudadesConNumeroEmpleados()
        {
            var query = from ciudad in _context.Ciudades
                        join oficina in _context.Oficinas on ciudad.Id equals oficina.IdDireccionFk
                        join empleado in _context.Empleados on oficina.Id equals empleado.IdOficinaFk
                        group empleado by ciudad into empleadosPorCiudad
                        select new
                        {
                            Ciudad = empleadosPorCiudad.Key.Nombre,
                            NumeroEmpleados = empleadosPorCiudad.Count()
                        };

            var ciudadesConNumeroEmpleados = query.ToList();

            return Ok(ciudadesConNumeroEmpleados);
        }


        [HttpGet("clientesLimiteCreditoMayorPagos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesLimiteCreditoMayorPagos()
        {
            var query = from cliente in _context.Clientes
                        where cliente.LimiteCredito > _context.Pagos.Where(p => p.IdClienteFk == cliente.Id).Sum(p => p.Total)
                        select cliente;

            var clientesLimiteCreditoMayorPagos = query.ToList();

            return Ok(clientesLimiteCreditoMayorPagos);
        }
        [HttpGet("clientesSinPagos2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesSinPagos2()
        {
            var query = from cliente in _context.Clientes
                        where !_context.Pagos.Any(p => p.IdClienteFk == cliente.Id)
                        select cliente;

            var clientesSinPagos = query.ToList();

            return Ok(clientesSinPagos);
        }
        [HttpGet("clientesConPagos2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPagos2()
        {
            var query = from cliente in _context.Clientes
                        where _context.Pagos.Any(p => p.IdClienteFk == cliente.Id)
                        select cliente;

            var clientesConPagos = query.ToList();

            return Ok(clientesConPagos);
        }
        [HttpGet("clientesConPedidosSinPagos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetClientesConPedidosSinPagos()
        {
            var query = from cliente in _context.Clientes
                        where cliente.Pedidos.Any() && !_context.Pagos.Any(p => p.IdClienteFk == cliente.Id)
                        select cliente;

            var clientesConPedidosSinPagos = query.ToList();

            return Ok(clientesConPedidosSinPagos);
        }

        [HttpGet("clienteMaxCredito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteMaxCreditoDto>>> GetClienteMaxCredito()
        {
            var clienteMaxCredito = await _unitOfWork.Clientes.GetClienteMaxCredito();
            if (clienteMaxCredito.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteMaxCreditoDto>>(clienteMaxCredito);
        }

        [HttpGet("clienteSinPago3")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ClienteSinPagoDto>>> GetClienteSinPago()
        {
            var clienteMaxCredito = await _unitOfWork.Clientes.GetClienteSinPago();
            if (clienteMaxCredito.IsNullOrEmpty())
            {
                return NotFound();
            }
            return _mapper.Map<List<ClienteSinPagoDto>>(clienteMaxCredito);
        }

    }
}