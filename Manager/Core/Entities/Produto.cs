using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.Entities
{
    public class Produto
    {
        public Produto(string nome, string tamanho, string cor, string descricao, string tipo, int vendedorForeignKey)
        {
            Nome = nome;
            Tamanho = tamanho;
            Cor = cor;
            Descricao = descricao;
            Tipo = tipo;
            VendedorForeignKey = vendedorForeignKey;
            Active = true;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Tamanho { get; private set; }
        public string Cor { get; private set; }
        public string Descricao { get; private set; }
        public string Tipo { get; private set; }
        public bool Active { get; private set; }

        public int VendedorForeignKey { get; private set; }
        public IEnumerable<Pedido>Pedidos { get; private set; }

        public void Deactivate()
        {
            Active = false;
        }

        public void Update(string nome, string tamanho, string cor, string descricao, string tipo)
        {
            Nome = nome;
            Tamanho = tamanho;
            Cor = cor;
            Descricao = descricao;
            Tipo = tipo;
        }

    }
}
