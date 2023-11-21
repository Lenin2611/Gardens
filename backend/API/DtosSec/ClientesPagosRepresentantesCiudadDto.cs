using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class ClientesPagosRepresentantesCiudadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreEmpleadoRepresentante { get; set; }
        public string Ciudad { get; set; }

    }
}