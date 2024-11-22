using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos;

namespace API_SistemaDeAtividades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        [HttpGet("ListarCategorias")]
        public async Task<ActionResult<List<CategoriaResponseDto>>> BuscarTodasCategorias()
        {
            var categorias = _categoriaRepositorio.BuscarCategorias();
            return Ok(categorias);
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> BuscarPorId(int id)
        {
            var categoria = _categoriaRepositorio.BuscarPorId(id);

            if (categoria == null)
                return NotFound($"Categoria com ID {id} não encontrada.");

            return Ok(categoria);
        }

        [HttpPost("InserirCategoria")]
        public async Task<ActionResult<RequestCategoriaDto>> Cadastrar([FromBody] RequestCategoriaDto categoriaDto)
        {
            var categoriaCriada = await _categoriaRepositorio.Adicionar(categoriaDto);

            return Ok(categoriaCriada);
        }

        [HttpPut("AtualizarCategoria/{id}")]
        public async Task<ActionResult<RequestCategoriaDto>> Atualizar([FromBody] RequestCategoriaDto categoriaDto, int id)
        {
            var categoriaExistente = _categoriaRepositorio.BuscarPorId(id);

            if (categoriaExistente == null)
                return NotFound($"Categoria com ID {id} não encontrada.");

            var categoriaAtualizada = await _categoriaRepositorio.Atualizar(categoriaDto, id);
            return Ok(categoriaAtualizada);
        }

        [HttpDelete("ApagarCategoria/{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            bool apagado = _categoriaRepositorio.Apagar(id);
            if (!apagado)
                return NotFound($"Categoria com ID {id} não encontrada.");

            return Ok(apagado);
        }
    }
}
