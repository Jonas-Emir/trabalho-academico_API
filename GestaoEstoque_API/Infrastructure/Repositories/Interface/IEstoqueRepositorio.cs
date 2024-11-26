using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IEstoqueRepositorio
    {
        Task<List<EstoqueResponseDto>> BuscarEstoques();
        Task<EstoqueResponseDto> BuscarPorId(int estoqueId);
        Task<EstoqueResponseDto> BuscarPorProduto(int produtoId);
        Task<EstoqueRequestDto> Adicionar(EstoqueRequestDto estoqueDto);
        Task<EstoqueRequestDto> Atualizar(EstoqueRequestDto produtoDto, int estoqueId);
        Task<bool> Apagar(int estoqueId);
    }
}
