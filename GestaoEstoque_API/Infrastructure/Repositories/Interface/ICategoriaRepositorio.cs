using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Domain.Entities;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface ICategoriaRepositorio
    {
        List<CategoriaResponseDto> BuscarCategorias();
        CategoriaResponseDto BuscarPorId(int categoriaId);
        Task<RequestCategoriaDto> Adicionar(RequestCategoriaDto categoriaDto);
        Task<RequestCategoriaDto> Atualizar(RequestCategoriaDto categoriaDto, int categoriaId);
        bool Apagar(int categoriaId);
    }
}
