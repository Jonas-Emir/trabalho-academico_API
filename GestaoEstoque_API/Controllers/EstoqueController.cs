using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueRepositorio _estoqueRepositorio;

        public EstoqueController(IEstoqueRepositorio estoqueRepositorio)
        {
            _estoqueRepositorio = estoqueRepositorio;
        }

        [HttpGet("ListarEstoques")]
        public async Task<ActionResult<List<EstoqueDto>>> BuscarTodosEstoques()
        {
            var estoques = await _estoqueRepositorio.BuscarEstoques();
            return Ok(estoques);
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<EstoqueDto>> BuscarPorId(int id)
        {
            var estoque = await _estoqueRepositorio.BuscarPorId(id);

            if (estoque == null)
                return NotFound($"Estoque com ID {id} não encontrado.");

            return Ok(estoque);
        }

        [HttpPost("InserirEstoque")]
        public async Task<ActionResult<EstoqueDto>> Cadastrar([FromBody] EstoqueDto estoque)
        {
            var estoqueCadastrado = await _estoqueRepositorio.Adicionar(estoque);

            return Ok(estoqueCadastrado);
        }

        [HttpPut("AtualizarEstoque/{id}")]
        public async Task<ActionResult<EstoqueDto>> Atualizar([FromBody] EstoqueDto estoqueDto, int id)
        {
            var estoqueExistente = await _estoqueRepositorio.BuscarPorId(id);

            if (estoqueExistente == null)
                return NotFound($"Estoque com ID {id} não encontrado.");

            var estoqueAtualizado = await _estoqueRepositorio.Atualizar(estoqueDto, id);
            return Ok(estoqueAtualizado);
        }

        [HttpDelete("ApagarEstoque/{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            bool apagado = await _estoqueRepositorio.Apagar(id);
            if (!apagado)
                return NotFound($"Estoque com ID {id} não encontrado.");

            return Ok(apagado);
        }
    }
}
