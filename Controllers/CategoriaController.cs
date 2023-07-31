using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext? context;

        public CategoriaController(AppDbContext? context)
        {
            this.context = context;
        }

        [HttpPost]
        public ActionResult AdicionarCategoria(Categoria categoria)
        {
            if (categoria is null) 
            {
                return BadRequest("Adicione uma categoria!");
            }
            context.categorias.Add(categoria);
            context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new {id = categoria.CategoriaId}, categoria);
        }

        [HttpPut("{id:int}")] 
        public ActionResult AtualizarCategoria(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest("Categoria nao encontrada.");
            }
            context.Entry(categoria).State = EntityState.Modified;
            context.SaveChanges();

            return Ok("Categoria atualizada com sucesso!" + categoria);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> BuscarCategorias(int id)
        {
            var categorias = context.categorias.AsNoTracking().ToList();

            if(categorias == null)
            {
                return NotFound("Nao contem categorias disponiveis");
            }

            return categorias;
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> BuscarCategoriasPorId(int id)
        {
            var categoria = context.categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId  == id);
            if(categoria == null)
            {
                return NotFound("Categoria nao encontrada");
            }
            return categoria;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> BuscarCategoriaProduto()
        {
            return context.categorias.AsNoTracking().Include(p => p.Produtos).ToList();
        }
    }
}
