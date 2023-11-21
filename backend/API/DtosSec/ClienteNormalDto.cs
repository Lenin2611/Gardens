using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class ClienteNormalDto
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; } = null!;

        public string NombreContacto { get; set; }

        public string ApellidoContacto { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }
    }
}