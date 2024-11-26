using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface ICategoriaRepositorio
    {
        List<CategoriaResponseDto> BuscarCategorias();
        CategoriaResponseDto BuscarPorId(int categoriaId);
        Task<CategoriaRequestDto> Adicionar(CategoriaRequestDto categoriaDto);
        Task<CategoriaRequestDto> Atualizar(CategoriaRequestDto categoriaDto, int categoriaId);
        string Apagar(int categoriaId);
    }
}
