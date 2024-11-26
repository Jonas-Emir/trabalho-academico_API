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
        public async Task<ActionResult<CategoriaRequestDto>> Cadastrar([FromBody] CategoriaRequestDto categoriaDto)
        {
            var categoriaCadastrada = await _categoriaRepositorio.Adicionar(categoriaDto);
            return Ok(categoriaCadastrada);
        }

        [HttpPut("AtualizarCategoria/{id}")]
        public async Task<ActionResult<CategoriaRequestDto>> Atualizar([FromBody] CategoriaRequestDto categoriaDto, int id)
        {
            var categoriaExistente = _categoriaRepositorio.BuscarPorId(id);
            if (categoriaExistente == null)
                return NotFound($"Categoria com ID {id} não encontrada.");

            var categoriaAtualizada = await _categoriaRepositorio.Atualizar(categoriaDto, id);
            return Ok(categoriaAtualizada);
        }

        [HttpDelete("ApagarCategoria/{id}")]
        public async Task<ActionResult<string>> Apagar(int id)
        {
            var resposta = _categoriaRepositorio.Apagar(id);
            if (string.IsNullOrEmpty(resposta))
                return NotFound($"Categoria com ID {id} não encontrada.");

            return Ok(resposta);
        }
    }
}
