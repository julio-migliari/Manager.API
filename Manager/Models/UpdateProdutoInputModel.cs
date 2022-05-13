using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Models
{
    public class UpdateProdutoInputModel
    {
        public int Id { get; private set; }
        public string Nome { get; set; }
        public string Tamanho { get; set; }
        public string Cor { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int VendedorForeignKey { get; set; }
    }
}
