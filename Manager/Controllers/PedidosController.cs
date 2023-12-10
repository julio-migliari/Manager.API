using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Manager.Core.Entities;
using Manager.Models;
using Manager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ManagerDBContext _dbContext;
        public PedidosController(ManagerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] CreatePedidoProdutoInputModel listPedidoProdutoModel)
        {
            if(listPedidoProdutoModel == null)
            {
                return BadRequest();
            }

            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == listPedidoProdutoModel.VendedorForeignKey && v.Active);
            var numeroPedido = _dbContext.Pedidos.Select(p => p.NumeroPedido).Count() + 1;

            var pedidoListView = new List<PedidoProduto>();
            var verificaFaltaProduto = false;
            float valorFinal;

            if(vendedor == null)
            {
                return Conflict();
            }

            listPedidoProdutoModel.ProdutoForeignKey.ForEach((produtoForeignKey) =>
            {

                var produto = _dbContext.Produtos.SingleOrDefault(p => p.Id == produtoForeignKey && p.Active);

                if(produto == null)
                {
                    verificaFaltaProduto = true;
                    return;
                }

                var pedido = new PedidoProduto(numeroPedido, produto.Nome, produto.Tamanho, produto.Cor, produto.Descricao, listPedidoProdutoModel.NomeCliente, listPedidoProdutoModel.TelCliente, listPedidoProdutoModel.EnderecoCliente, listPedidoProdutoModel.MetodoPagamento, produto.Valor, vendedor.Id, produto.Id);
                pedidoListView.Add(pedido);
                _dbContext.PedidosProdutos.Add(pedido);

            });

            if (verificaFaltaProduto)
            {
                return Conflict();
            }


            valorFinal = pedidoListView.Sum(p => p.ValorProduto);
            var pedido = new Pedidos(numeroPedido, valorFinal);

            _dbContext.Pedidos.Add(pedido);

            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { vendedorId = vendedor.Id ,numeroPedido = pedido.NumeroPedido }, pedidoListView);
        }

        [HttpGet("{vendedorId}")]
        public IActionResult GetAll(int vendedorId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId);

            if(vendedor == null)
            {
                return NotFound();
            }

            var pedidos = _dbContext.PedidosProdutos.Where(p => p.VendedorForeignKey == vendedor.Id).ToList();

            if (pedidos == null)
            {
                return NotFound();
            }

            var numerosPedidos = pedidos.Select(p => p.NumeroPedido).Distinct().ToList();

            List<PedidoProdutoViewModel> pedidoProdutoViewList = new List<PedidoProdutoViewModel>();

            numerosPedidos.ForEach(numeroPedido =>
            {
                List<DadosProdutoViewModel> dadosProdutoViewList = new List<DadosProdutoViewModel>();
                var dadosCliente = (from p in pedidos 
                                    where p.NumeroPedido == numeroPedido
                                    select new { p.NomeCliente, p.TelCliente, p.EnderecoCliente, p.MetodoPagamento}
                                    ).FirstOrDefault();

                var produtosCliente = (from p in pedidos 
                                       where p.NumeroPedido == numeroPedido
                                       select new { p.NomeProduto, p.TamanhoProduto, p.CorProduto, p.DescricaoProduto }
                                       ).ToList();
                var dadosPedido = _dbContext.Pedidos.Where(p => p.NumeroPedido == numeroPedido).SingleOrDefault();
                if(dadosPedido != null)
                {
                    produtosCliente.ForEach(produto =>
                    {
                        DadosProdutoViewModel dadosProduto = new DadosProdutoViewModel(produto.NomeProduto, produto.TamanhoProduto, produto.CorProduto, produto.DescricaoProduto);
                        dadosProdutoViewList.Add(dadosProduto);
                    });

                    var pedidoProdutoView = new PedidoProdutoViewModel(dadosPedido.Id, numeroPedido, dadosCliente.NomeCliente, dadosCliente.TelCliente, dadosCliente.EnderecoCliente, dadosCliente.MetodoPagamento, dadosPedido.ValorFinal, dadosPedido.Status, dadosProdutoViewList);
                    pedidoProdutoViewList.Add(pedidoProdutoView);

                }
            });

            return Ok(pedidoProdutoViewList);
        }

        [HttpGet("{vendedorId}/{numeroPedido}")]
        public IActionResult Get(int vendedorId, int numeroPedido)
        {

            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId);
            var pedido = _dbContext.PedidosProdutos.Where(p => p.NumeroPedido == numeroPedido);

            if (vendedor == null || pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPut("{vendedorId}/{numeroPedido}")]
        public IActionResult Put(int vendedorId, int numeroPedido)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);

            if(vendedor == null)
            {
                return StatusCode(409);
            }

            var pedido = _dbContext.Pedidos.SingleOrDefault(p => p.NumeroPedido == numeroPedido);
            pedido.Concluir();

            _dbContext.Pedidos.Update(pedido);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{vendedorId}/{numeroPedido}")]
        public IActionResult Delete(int vendedorId, int numeroPedido)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);

            if (vendedor == null)
            {
                return StatusCode(409);
            }

            var pedido = _dbContext.Pedidos.SingleOrDefault(p => p.NumeroPedido == numeroPedido);
            pedido.CancelarPedido();

            _dbContext.Pedidos.Update(pedido);
            _dbContext.SaveChanges();

            return NoContent();
        }

    }
}
