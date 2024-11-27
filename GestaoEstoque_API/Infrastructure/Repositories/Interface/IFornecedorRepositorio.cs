using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Infrastructure.Repositories.Interface
{
    public interface IFornecedorRepositorio
    {
        List<FornecedorResponseDto> BuscarFornecedores();
        FornecedorResponseDto BuscarPorId(int fornecedorId);
        Task<FornecedorRequestDto> AdicionarAsync(FornecedorRequestDto fornecedorDto);
        Task<FornecedorRequestDto> AtualizarAsync(FornecedorRequestDto fornecedorDto, int fornecedorId);
        string Apagar(int fornecedorId);
    }
}
