using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class PagoFechasClienteNombreApellidoDto
    {
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public DateOnly FechaPrimer { get; set; }
        public DateOnly FechaUltimo { get; set; }
    }
}