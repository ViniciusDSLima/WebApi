using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.services;
using WebApplication1.services.implementation;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //categorias
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext? _context;

        private readonly IConfiguration _configuration;

        public CategoriaController(AppDbContext? context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost] //categorias
        public ActionResult AdicionarCategoria(Categoria categoria)
        {
            if (categoria is null) 
            {
                return BadRequest("Adicione uma categoria!");
            }
            _context.categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new {id = categoria.CategoriaId}, categoria);
        }

        [HttpPut("{id:int}")] //categorias
        public ActionResult AtualizarCategoria(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest("Categoria nao encontrada.");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok("Categoria atualizada com sucesso!" + categoria);
        }

        [HttpGet] //categorias
        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategorias(int id)
        {
            var categorias = await _context.categorias.AsNoTracking().ToListAsync();

            if(categorias == null)
            {
                return NotFound("Nao contem categorias disponiveis");
            }

            return categorias;
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")] //categorias/id
        public async Task<ActionResult<Categoria>> BuscarCategoriasPorId(int id)
        {
            var categoria = await _context.categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId  == id);
            if(categoria == null)
            {
                return NotFound("Categoria nao encontrada");
            }
            return categoria;
        }

        [HttpGet("produtos")] //categorias/produtos
        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriaProduto()
        {
            return await _context.categorias.AsNoTracking().Include(p => p.Produtos).ToListAsync();
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> BuscarSaudacao([FromServices] IMeuServico meuServico,
            string nome)
        {
            return meuServico.saudacao(nome);
        }
    }
}
