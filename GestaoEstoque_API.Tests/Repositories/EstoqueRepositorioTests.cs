using AutoMapper;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories;
using Moq;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class EstoqueRepositorioTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EstoqueRepositorio _repositorio;

        public EstoqueRepositorioTests()
        {
            _mockDbContext = new Mock<AppDbContext>();
            _mockMapper = new Mock<IMapper>();
            _repositorio = new EstoqueRepositorio(_mockDbContext.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Adicionar_DeveLancarExcecao_SeProdutoNaoExistir()
        {
            var estoqueDto = new EstoqueRequestDto
            {
                ProdutoId = 999, 
                Quantidade = 10,
                TipoMovimentoId = (Application.Enums.TipoMovimento)1
            };

            await Assert.ThrowsAsync<Exception>(() => _repositorio.Adicionar(estoqueDto));
        }
    }
}
