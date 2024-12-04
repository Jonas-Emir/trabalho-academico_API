using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos;

namespace API_SistemaDeAtividades.Controllers
{
    [Route("api/produto/")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<ProdutoResponseDto>>> BuscarTodosProdutos()
        {
            try
            {
                var produtos = await _produtoRepositorio.BuscarProdutos();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar produtos: {ex.Message}");
            }
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> BuscarPorId(int id)
        {
            try
            {
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                if (produto == null)
                    return NotFound($"Produto com ID {id} não encontrado.");

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produto: {ex.Message}");
            }
        }

        [HttpPost("inserir")]
        public async Task<ActionResult<RequestProdutoDto>> Cadastrar([FromBody] RequestProdutoDto produto)
        {
            try
            {
                var produtoCadastrado = await _produtoRepositorio.Adicionar(produto);
                return Ok(produtoCadastrado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar produto: {ex.Message}");
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<ActionResult<RequestProdutoDto>> Atualizar([FromBody] RequestProdutoDto produtoDto, int id)
        {
            try
            {
                var produtoExistente = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                if (produtoExistente == null)
                    return NotFound($"Produto com ID {id} não encontrado.");

                var produtoAtualizado = await _produtoRepositorio.Atualizar(produtoDto, id);
                return Ok(produtoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar produto: {ex.Message}");
            }
        }

        [HttpDelete("apagar/{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            try
            {
                bool apagado = await _produtoRepositorio.Apagar(id);
                if (!apagado)
                    return NotFound($"Produto com ID {id} não encontrado.");

                return Ok(apagado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar produto: {ex.Message}");
            }
        }
    }
}
