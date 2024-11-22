using GestaoEstoque_API.Application.Dtos.Movimentacao;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IEstoqueRepositorio
    {
        Task<List<MovimentacaoEstoqueDto>> BuscarEstoques();
        Task<MovimentacaoEstoqueDto> BuscarPorId(int produtoId);
        Task<MovimentacaoEstoqueDto> Adicionar(MovimentacaoEstoqueDto produtoDto);
        Task<MovimentacaoEstoqueDto> Atualizar(MovimentacaoEstoqueDto produtoDto, int produtoId);
        Task<bool> Apagar(int produtoId);
    }
}
