using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProdutoController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> BuscarProdutos(int id) 
        {
            var produtos = context.Produto.AsNoTracking().ToList();
            if(produtos is null)
            {
                return NotFound("Produtos nao encontrados");
            }
            return produtos;
        }


        [HttpGet("{id:int} ", Name = "ObterProduto" )]
        public ActionResult<Produto> BuscarProdutoPeloId(int id)
        {
            var produto = context.Produto.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto nao encontrado");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult AdicionarProduto( Produto produto) 
        {
            if(produto is null)
            {
                return BadRequest("Cadastre um produto.");
            }
            context.Produto.Add(produto);
            context.SaveChanges();


            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult AtualizarProduto(int id, Produto produto)
        { 
            if(id != produto.ProdutoId) 
            {
                return BadRequest();
            }

            context.Entry(produto).State = EntityState.Modified;
            context.SaveChanges();

            return Ok("produto atualizado " + produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult ApagarProduto(int id) 
        {
            var produto = context.Produto.FirstOrDefault(P => P.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto nao localizado");
            }
            context.Produto.Remove(produto);
            context.SaveChanges();

            return Ok("produto excluido " + produto);
        }
    }
}
