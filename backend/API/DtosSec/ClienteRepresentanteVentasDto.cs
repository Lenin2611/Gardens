using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class ClienteRepresentanteVentasDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreContacto { get; set; }
        public string ApellidoContacto { get; set; }
    }
}