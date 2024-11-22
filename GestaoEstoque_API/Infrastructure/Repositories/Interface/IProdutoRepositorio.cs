using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IProdutoRepositorio
    {
        Task<List<ProdutoResponseDto>> BuscarProdutos();
        ProdutoResponseDto BuscarProdutoPorId(int produtoId);
        Task<RequestProdutoDto> Adicionar(RequestProdutoDto produtoDto);
        Task<RequestProdutoDto> Atualizar(RequestProdutoDto produtoDto, int produtoId);
        Task<bool> Apagar(int produtoId);
    }
}
