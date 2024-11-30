using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Application.Enums;
using GestaoEstoque_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class EstoqueRepositorioTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly EstoqueRepositorio _estoqueRepositorio;

        public EstoqueRepositorioTests()
        {
            _mapperMock = new Mock<IMapper>();
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _estoqueRepositorio = new EstoqueRepositorio(_dbContext, _mapperMock.Object);
        }

        private void SeedDatabase()
        {
            var produtos = new List<Produto>
            {
             new Produto
             {
                 ProdutoId = 1,
                 Nome = "Notebook",
                 Preco = 3500.00m,
                 Ativo = true,
                 CategoriaId = 1,
                 FornecedorId = 1,
                 DataCriacao = DateTime.Now,
             },
             new Produto
             {
                 ProdutoId = 2,
                 Nome = "Camiseta",
                 Preco = 50.00m,
                 Ativo = true,
                 CategoriaId = 2,
                 FornecedorId = 2,
                 DataCriacao = DateTime.Now,
             }
              };

            var estoques = new List<Estoque>
               {
             new Estoque
             {
                 EstoqueId = 1,
                 ProdutoId = 1,
                 Produto = produtos.First(p => p.ProdutoId == 1),
                 Quantidade = 20,
                 Id_Tipo_Movimento = TipoMovimento.Entrada
             },
             new Estoque
             {
                 EstoqueId = 2,
                 ProdutoId = 2,
                 Produto = produtos.First(p => p.ProdutoId == 2),
                 Quantidade = 10,
                 Id_Tipo_Movimento = TipoMovimento.Entrada
             },
             new Estoque
             {
                 EstoqueId = 3,
                 ProdutoId = 1,
                 Produto = produtos.First(p => p.ProdutoId == 1),
                 Quantidade = -5,
                 Id_Tipo_Movimento = TipoMovimento.Saida
             },
             new Estoque
             {
                 EstoqueId = 4,
                 ProdutoId = 2,
                 Produto = produtos.First(p => p.ProdutoId == 2),
                 Quantidade = -3,
                 Id_Tipo_Movimento = TipoMovimento.Saida
             }
            };

            _dbContext.Produto.AddRange(produtos);
            _dbContext.Estoque.AddRange(estoques);
            _dbContext.SaveChanges();
        }


        [Fact]
        public async Task Adicionar_DeveLancarExcecao_SeProdutoNaoExistir()
        {
            SeedDatabase();

            var estoqueDto = new EstoqueRequestDto
            {
                ProdutoId = 999,
                Quantidade = 10,
                TipoMovimentoId = (Application.Enums.TipoMovimento)1
            };

            await Assert.ThrowsAsync<Exception>(() => _estoqueRepositorio.Adicionar(estoqueDto));
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarProdutoNoEstoque()
        {
            SeedDatabase();

            var estoqueDto = new EstoqueRequestDto
            {
                ProdutoId = 1,
                Quantidade = 10,
                TipoMovimentoId = (Application.Enums.TipoMovimento)1
            };

            _mapperMock.Setup(m => m.Map<Estoque>(It.IsAny<EstoqueRequestDto>()))
                .Returns(new Estoque
                {
                    ProdutoId = 1,
                    Quantidade = 10,
                });

            _mapperMock.Setup(m => m.Map<EstoqueRequestDto>(It.IsAny<Estoque>()))
                .Returns(estoqueDto);

            var estoqueAdicionado = await _estoqueRepositorio.Adicionar(estoqueDto);

            Assert.NotNull(estoqueAdicionado);
            Assert.Equal(1, estoqueAdicionado.ProdutoId);
            Assert.Equal(10, estoqueAdicionado.Quantidade);
        }

        [Fact]
        public async Task BuscarEstoque_DeveRetornarEstoquePorProduto()
        {
            SeedDatabase();

            var estoque = new Estoque
            {
                ProdutoId = 1,
                Quantidade = 10,
            };

            _dbContext.Estoque.Add(estoque);
            await _dbContext.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<EstoqueResponseDto>(It.IsAny<Estoque>()))
                .Returns(new EstoqueResponseDto
                {
                    ProdutoId = 1,
                    Quantidade = 10,
                });

            var estoqueDto = await _estoqueRepositorio.BuscarPorId(1);

            Assert.NotNull(estoqueDto);
            Assert.Equal(10, estoqueDto.Quantidade);
        }

        [Fact]
        public async Task AtualizarEstoque_DeveAtualizarEstoqueExistente()
        {
            SeedDatabase();

            var estoqueDto = new EstoqueRequestDto
            {
                ProdutoId = 1,
                Quantidade = 20,
                TipoMovimentoId = (Application.Enums.TipoMovimento)1
            };

            _mapperMock.Setup(m => m.Map<Estoque>(It.IsAny<EstoqueRequestDto>()))
                .Returns(new Estoque
                {
                    ProdutoId = 1,
                    Quantidade = 20,
                    Id_Tipo_Movimento = TipoMovimento.Entrada
                });

            _mapperMock.Setup(m => m.Map<EstoqueRequestDto>(It.IsAny<Estoque>()))
                .Returns(estoqueDto);

            var estoqueAtualizado = await _estoqueRepositorio.Atualizar(estoqueDto, 1);

            Assert.NotNull(estoqueAtualizado);
            Assert.Equal(20, estoqueAtualizado.Quantidade);
        }

        [Fact]
        public async Task ApagarEstoque_DeveRemoverEstoque()
        {
            SeedDatabase();

            var estoque = new Estoque
            {
                ProdutoId = 1,
                Quantidade = 10,
            };

            _dbContext.Estoque.Add(estoque);
            await _dbContext.SaveChangesAsync();

            var resultado = await _estoqueRepositorio.Apagar(1);

            Assert.True(resultado);
            Assert.Null(await _dbContext.Estoque.FindAsync(1));
        }
    }
}
