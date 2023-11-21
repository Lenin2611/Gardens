using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DtosSec
{
    public class ClienteMaxCreditoDto
    {
        public string Nombre { get; set; }
        public decimal  LimiteCredito { get; set; }
    }
}