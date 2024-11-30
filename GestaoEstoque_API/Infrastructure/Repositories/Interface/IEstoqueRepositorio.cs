using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IEstoqueRepositorio
    {
        Task<List<EstoqueResponseDto>> BuscarEstoques();
        Task<EstoqueResponseDto> BuscarPorId(int estoqueId);
        Task<ProdutoEstoqueResponseDto> BuscarQuantidadeEstoquePorProduto(int produtoId);
        Task<Dictionary<string, int>> BuscarQuantidadePorTipoMovimento(int produtoId);
        Task<EstoqueResponseDto> VerificarSeProdutoExisteAsync(int produtoId);
        Task<EstoqueRequestDto> Adicionar(EstoqueRequestDto estoqueDto);
        Task<EstoqueRequestDto> Atualizar(EstoqueRequestDto produtoDto, int estoqueId);
        Task<bool> Apagar(int estoqueId);
    }
}
