using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class OficinaIdCiudadNombreDto
    {
        public string NombreCiudad { get; set; }
        public List<OficinaIdDto> Oficinas { get; set; }
    }
}