using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos;

namespace API_SistemaDeAtividades.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpGet("ListarProdutos")]
        public async Task<ActionResult<List<ProdutoResponseDto>>> BuscarTodosProdutos()
        {
            var produtos = await _produtoRepositorio.BuscarProdutos();
            return Ok(produtos);
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> BuscarPorId(int id)
        {
            var produto = await _produtoRepositorio.BuscarPorId(id);

            if (produto == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            return Ok(produto);
        }

        [HttpPost("InserirProduto")]
        public async Task<ActionResult<RequestProdutoDto>> Cadastrar([FromBody] RequestProdutoDto produto)
        {
            var produtoCadastrado = await _produtoRepositorio.Adicionar(produto);

            return Ok(produtoCadastrado);
        }

        [HttpPut("AtualizarProduto/{id}")]
        public async Task<ActionResult<RequestProdutoDto>> Atualizar([FromBody] RequestProdutoDto produtoDto, int id)
        {
            var produtoExistente = await _produtoRepositorio.BuscarPorId(id);

            if (produtoExistente == null)
                return NotFound($"Produto com ID {id} não encontrado.");     

            var produtoAtualizado = await _produtoRepositorio.Atualizar(produtoDto, id);
            return Ok(produtoAtualizado);
        }

        [HttpDelete("ApagarProduto/{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            bool apagado = await _produtoRepositorio.Apagar(id);
            if (!apagado)
                return NotFound($"Produto com ID {id} não encontrado.");

            return Ok(apagado);
        }
    }
}
