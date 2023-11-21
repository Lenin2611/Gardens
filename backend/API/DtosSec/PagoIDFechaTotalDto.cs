using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class PagoIDFechaTotalDto
    {
        public string Id { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal Total { get; set; }
        public string FormaPago { get; set; }
    }
}