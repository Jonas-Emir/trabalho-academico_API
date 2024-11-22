using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IEstoqueRepositorio
    {
        Task<List<EstoqueDto>> BuscarEstoques();
        Task<EstoqueDto> BuscarPorId(int produtoId);
        Task<EstoqueDto> Adicionar(EstoqueDto produtoDto);
        Task<EstoqueDto> Atualizar(EstoqueDto produtoDto, int produtoId);
        Task<bool> Apagar(int produtoId);
    }
}
