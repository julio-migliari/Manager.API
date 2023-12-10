using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Manager.Core.Entities;
using Manager.Models;
using Manager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;

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

        [HttpPost]
        public IActionResult Post(IFormCollection formData)
        {
            if (formData == null || !formData.Files.Any() || !formData.Keys.Any())
            {
                return NoContent();
            }

            Microsoft.Extensions.Primitives.StringValues json;
            CreateProdutoInputModel produtoModel = null;
            if(formData.TryGetValue("json", out json))
            {
                produtoModel = JsonConvert.DeserializeObject<CreateProdutoInputModel>(json);
            }

            if(produtoModel != null)
            {
                var vendedor = _dbContext.Vendedores.Where(v => v.Id == produtoModel.VendedorForeignKey);
                if (vendedor == null)
                {
                    return StatusCode(409);
                }

                var produto = new Produto(produtoModel);
                try
                {
                    produto.AddFile(formData.Files[0]);
                    _dbContext.Produtos.Add(produto);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    StatusCode(400);
                }

                return CreatedAtAction(nameof(Get), new { vendedorId = produtoModel.VendedorForeignKey, produtoid = produto.Id }, produto);
            }

            return BadRequest();

        }

        [AllowAnonymous]
        [HttpGet("upload/{vendedorId}/{produtoId}")]
        public IActionResult GetImage(int vendedorId, int produtoId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);
            var produto = _dbContext.Produtos.SingleOrDefault(p => p.VendedorForeignKey == vendedorId && p.Id == produtoId && p.Active);

            if (vendedor == null)
            {
                return StatusCode(409);
            }

            if (produto == null)
            {
                return NotFound();
            }

            //CRIAÇÃO DA IMAGEM A PARTIR DO BYTEARRAY
            var path = "Files/" + produto.Id + produto.Nome + ".jpeg";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            if(produto.Imagem != null)
            {
                try
                {
                    fs.Write(produto.Imagem, 0, produto.Imagem.Length);
                }
                catch
                {
                    fs.Close();
                }
            }
            fs.Close();

            //FIM DA CRIAÇÃO DA IMAGEM


            // BUSCA DA IMAGEM PELO CAMINHO DO DIRETORIO
            byte[] b = System.IO.File.ReadAllBytes(path);        
            if (b == null)
            {
                return NotFound();
            }
            
            //RETORNO DA IMAGEM
            return File(b, "image/jpeg");

        }

        [AllowAnonymous]
        [HttpGet("{vendedorId}")]
        public IActionResult GetAll(int vendedorId)
        {
            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == vendedorId && v.Active);
            var produtos = _dbContext.Produtos.Where(p => p.VendedorForeignKey == vendedorId && p.Active).ToList();

            if (vendedor == null)
            {
                return StatusCode(409);
            }

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

            if (vendedor == null)
            {
                return StatusCode(409);
            }

            if(produto == null)
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

            if (vendedor == null)
            {
                return StatusCode(409);
            }

            if (produto == null)
            {
                return NotFound();
            }


            produto.Deactivate();
            _dbContext.Produtos.Update(produto);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut()]
        public IActionResult Put([FromBody] UpdateProdutoInputModel produto)
        {
            if(produto == null)
            {
                return BadRequest();
            }

            var vendedor = _dbContext.Vendedores.SingleOrDefault(v => v.Id == produto.VendedorForeignKey && v.Active);
            var produtoSearch = _dbContext.Produtos.SingleOrDefault(p => p.Id == produto.Id && p.VendedorForeignKey == produto.VendedorForeignKey && p.Active);

            if (vendedor == null)
            {
                return Conflict();
            }

            if (produtoSearch == null)
            {
                return NotFound();
            }

            produtoSearch.Update(produto.Nome, produto.Tamanho, produto.Cor, produto.Descricao, produto.Tipo, produto.Valor);
            _dbContext.Produtos.Update(produtoSearch);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
