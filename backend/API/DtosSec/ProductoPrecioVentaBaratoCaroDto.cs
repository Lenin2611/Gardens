using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class ProductoPrecioVentaBaratoCaroDto
    {
        public string NombreCaro { get; set; }
        public decimal Caro { get; set; }
        public string NombreBarato { get; set; }
        public decimal Barato { get; set; }
    }
}