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

        [HttpPost()]
        public IActionResult Post([FromBody] CreatePedidoInputModel pedidoModel)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == pedidoModel.VendedorForeignKey && v.Active);
            var produto = _dbContext.Produtos.SingleOrDefault(p => p.Id == pedidoModel.ProdutoForeignKey && p.Active);

            if(vendedor == null || produto == null)
            {
                return StatusCode(409);
            }

            var pedido = new Pedido(pedidoModel.NomeProduto, pedidoModel.TamanhoProduto, pedidoModel.CorProduto, pedidoModel.NomeCliente, pedidoModel.TelCliente, pedidoModel.EnderecoCliente, pedidoModel.MetodoPagamento, pedidoModel.ValorFinal);
            _dbContext.Pedidos.Add(pedido);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = produto.VendedorForeignKey }, produto);
        }

        [HttpGet("{vendedorId}")]
        public IActionResult GetAll(int vendedorId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId);

            if(vendedor == null)
            {
                return NotFound();
            }

            var pedidos = _dbContext.Pedidos.Where(p => p.VendedorForeignKey == vendedor.Id);
            return Ok(pedidos);
        }

        [HttpGet("{vendedorId}/{pedidoId}")]
        public IActionResult Get(int vendedorId, int pedidoId)
        {

            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId);
            var pedido = _dbContext.Pedidos.SingleOrDefault(p => p.Id == pedidoId);

            if (vendedor == null || pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

    }
}
