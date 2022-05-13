using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.Entities
{
    public class Vendedor : User
    {
        public Vendedor(string nome, string tel, string cpf, string endereco) : base()
        {
            Nome = nome;
            Tel = tel;
            Cpf = cpf;
            Endereco = endereco;
            Produtos = new List<Produto>();
            Pedidos = new List<Pedido>();

            //Produtos.Add(new Produto("a", "b", "c0", "d"));
        }

        public string Nome { get; private set; }
        public string Tel { get; private set; }
        public string Cpf { get; private set; }
        public string Endereco { get; private set; }
        public IEnumerable<Produto> Produtos { get; private set; }
        public IEnumerable<Pedido> Pedidos{ get; private set; }

        //public List<Produto> Produtos { get; private set; }

    }
}
