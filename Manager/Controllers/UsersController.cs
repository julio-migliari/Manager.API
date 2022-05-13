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
    //[Authorize(Policy = "user")]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ManagerDBContext _dbContext;
        public UsersController(ManagerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpPost(), Route("produtos")]
        //public IActionResult Post([FromBody] CreateProdutoInputModel produto)
        //{
        //    var vendedor = _dbContext.Vendedores.Where(v => v.Id == produto.VendedorForeignKey);

        //    if (vendedor.Count() == 0)
        //    {
        //        return StatusCode(409);
        //    }

        //    var newProduto = new Produto(produto.Nome, produto.Tamanho, produto.Cor, produto.Descricao, produto.Tipo, produto.VendedorForeignKey);
        //    _dbContext.Produtos.Add(newProduto);
        //    _dbContext.SaveChanges();

        //    return CreatedAtAction(nameof(Get), new { id = produto.VendedorForeignKey }, produto);

        //}

        // api/users/45
        [HttpGet("vendedores/{id}")]
        public IActionResult Get(int id)
        {
            var user = _dbContext.Vendedores.SingleOrDefault(v => v.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
