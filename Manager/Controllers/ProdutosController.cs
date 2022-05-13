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
    public class ProdutosController : ControllerBase
    {
        private readonly ManagerDBContext _dbContext;
        public ProdutosController(ManagerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost()]
        public IActionResult Post([FromBody] CreateProdutoInputModel produtoModel)
        {
            var vendedor = _dbContext.Vendedores.Where(v => v.Id == produtoModel.VendedorForeignKey);

            if (vendedor.Count() == 0)
            {
                return StatusCode(409);
            }

            var produto = new Produto(produtoModel.Nome, produtoModel.Tamanho, produtoModel.Cor, produtoModel.Descricao, produtoModel.Tipo, produtoModel.VendedorForeignKey);
            _dbContext.Produtos.Add(produto);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = produtoModel.VendedorForeignKey }, produtoModel);

        }

        [HttpGet("{vendedorId}")]
        public IActionResult GetAll(int vendedorId)
        {
            var produtos = _dbContext.Produtos.Where(p => p.VendedorForeignKey == vendedorId && p.Active);

            if (produtos.Count() == 0)
            {
                return NotFound();
            }

            return Ok(produtos);

        }

        [HttpGet("{vendedorId}/{produtoId}")]
        public IActionResult Get(int vendedorId, int produtoId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);
            var produto = _dbContext.Produtos.SingleOrDefault(p => p.VendedorForeignKey == vendedorId && p.Id == produtoId && p.Active);

            if (vendedor == null || produto == null)
            {
                return NotFound();
            }

            return Ok(produto);

        }

        [HttpDelete("{vendedorId}/{produtoId}")]
        public IActionResult Delete(int vendedorId, int produtoId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);
            var produto = _dbContext.Produtos.SingleOrDefault(p => p.Id == produtoId && p.VendedorForeignKey == vendedorId && p.Active);

            if (vendedor == null || produto == null)
            {
                return NotFound();
            }


            produto.Deactivate();

            return NoContent();
        }

        [HttpPut()]
        public IActionResult Put([FromBody] UpdateProdutoInputModel produto)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == produto.VendedorForeignKey && v.Active);
            var produtoSearch = _dbContext.Produtos.SingleOrDefault(p => p.Id == produto.Id && p.VendedorForeignKey == produto.VendedorForeignKey && p.Active);

            if (vendedor == null || produtoSearch == null)
            {
                return NotFound();
            }

            produtoSearch.Update(produto.Nome, produto.Tamanho, produto.Cor, produto.Descricao, produto.Tipo);
            _dbContext.Produtos.Update(produtoSearch);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
