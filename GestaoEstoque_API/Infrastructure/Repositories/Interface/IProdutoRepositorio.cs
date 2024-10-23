using GestaoEstoque_API.Application.Dtos.Produto;
using GestaoEstoque_API.Domain.Entities;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IProdutoRepositorio
    {
        Task<List<ProdutoResponseDto>> BuscarProdutos();
        Task<ProdutoResponseDto> BuscarPorId(int produtoId);
        Task<RequestProdutoDto> Adicionar(RequestProdutoDto produtoDto);
        Task<RequestProdutoDto> Atualizar(RequestProdutoDto produtoDto, int produtoId);
        Task<bool> Apagar(int produtoId);
    }
}
