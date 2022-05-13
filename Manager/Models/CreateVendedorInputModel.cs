using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Models
{
    public class CreateVendedorInputModel
    {
        public string Nome { get; set; }
        public string Tel { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }

    }
}
