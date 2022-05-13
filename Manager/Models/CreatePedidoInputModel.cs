using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Models
{
    public class CreatePedidoInputModel
    {
        public int VendedorForeignKey { get; private set; }
        public int ProdutoForeignKey { get; private set; }
        public string NomeProduto { get; private set; }
        public string TamanhoProduto { get; private set; }
        public string CorProduto { get; private set; }
        public string NomeCliente { get; private set; }
        public string TelCliente { get; private set; }
        public string EnderecoCliente { get; private set; }
        public string MetodoPagamento { get; private set; }
        public float ValorFinal { get; private set; }
    }
}
