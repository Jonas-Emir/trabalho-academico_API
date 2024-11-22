using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos.Fornecedor;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestaoEstoque_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;

        public FornecedorController(IFornecedorRepositorio fornecedorRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<FornecedorResponseDto>>> ListarFornecedores()
        {
            var fornecedores = await _fornecedorRepositorio.BuscarFornecedores();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorResponseDto>> BuscarPorId(int id)
        {
            var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

            if (fornecedor == null)
                return NotFound($"Fornecedor com ID {id} não encontrado.");

            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorRequestDto>> InserirFornecedor([FromBody] FornecedorRequestDto fornecedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fornecedorCadastrado = await _fornecedorRepositorio.Adicionar(fornecedor);
            return CreatedAtAction(nameof(BuscarPorId), new { id = fornecedorCadastrado.FornecedorId }, fornecedorCadastrado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FornecedorRequestDto>> AtualizarFornecedor([FromBody] FornecedorRequestDto fornecedorDto, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fornecedorExistente = await _fornecedorRepositorio.BuscarPorId(id);
            if (fornecedorExistente == null)
                return NotFound($"Fornecedor com ID {id} não encontrado.");

            var fornecedorAtualizado = await _fornecedorRepositorio.Atualizar(fornecedorDto, id);
            return Ok(fornecedorAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> ApagarFornecedor(int id)
        {
            bool apagado = await _fornecedorRepositorio.Apagar(id);
            if (!apagado)
                return NotFound($"Fornecedor com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
