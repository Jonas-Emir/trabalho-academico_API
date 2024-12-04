using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos;

namespace API_SistemaDeAtividades.Controllers
{
    [Route("api/categoria/")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        [HttpGet("listar/")]
        public async Task<ActionResult<List<CategoriaResponseDto>>> BuscarTodasCategorias()
        {
            try
            {
                var categorias = _categoriaRepositorio.BuscarCategorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar categorias: {ex.Message}");
            }
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> BuscarPorId(int id)
        {
            try
            {
                var categoria = _categoriaRepositorio.BuscarPorId(id);

                if (categoria == null)
                    return NotFound($"Categoria com ID {id} não encontrada.");

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar categoria: {ex.Message}");
            }
        }

        [HttpPost("inserir/")]
        public async Task<ActionResult<CategoriaRequestDto>> Cadastrar([FromBody] CategoriaRequestDto categoriaDto)
        {
            try
            {
                var categoriaCadastrada = await _categoriaRepositorio.Adicionar(categoriaDto);
                return Ok(categoriaCadastrada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar categoria: {ex.Message}");
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<ActionResult<CategoriaRequestDto>> Atualizar([FromBody] CategoriaRequestDto categoriaDto, int id)
        {
            try
            {
                var categoriaExistente = _categoriaRepositorio.BuscarPorId(id);
                if (categoriaExistente == null)
                    return NotFound($"Categoria com ID {id} não encontrada.");

                var categoriaAtualizada = await _categoriaRepositorio.Atualizar(categoriaDto, id);
                return Ok(categoriaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar categoria: {ex.Message}");
            }
        }

        [HttpDelete("apagar/{id}")]
        public async Task<ActionResult<string>> Apagar(int id)
        {
            try
            {
                var resposta = _categoriaRepositorio.Apagar(id);
                if (string.IsNullOrEmpty(resposta))
                    return NotFound($"Categoria com ID {id} não encontrada.");

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar categoria: {ex.Message}");
            }
        }
    }
}
