using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.Entities
{
    public class Pedido
    {
        public Pedido(string nomeProduto, string tamanhoProduto, string corProduto, string nomeCliente, string telCliente, string enderecoCliente, string metodoPagamento, float valorFinal)
        {
            NomeProduto = nomeProduto;
            TamanhoProduto = tamanhoProduto;
            CorProduto = corProduto;
            NomeCliente = nomeCliente;
            TelCliente = telCliente;
            EnderecoCliente = enderecoCliente;
            MetodoPagamento = metodoPagamento;
            ValorFinal = valorFinal;
        }

        public int Id { get; private set; }
        public string NomeProduto { get; private set; }
        public string TamanhoProduto { get; private set; }
        public string CorProduto { get; private set; }
        public string NomeCliente { get; private set; }
        public string TelCliente { get; private set; }
        public string EnderecoCliente { get; private set; }
        public string MetodoPagamento { get; private set; }
        public float ValorFinal { get; private set; }
        public int VendedorForeignKey { get; private set; }
        public int ProdutoForeignKey { get; private set; }
    }
}
