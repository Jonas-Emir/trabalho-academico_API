using Moq;
using AutoMapper;
using GestaoEstoque_API.Application.Dtos.Produto;
using GestaoEstoque_API.Domain.Entities;
using GestaoEstoque_API.Infrastructure.Repositories;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;


namespace GestaoEstoque_API.Tests.Repositories
{
    public class ProdutoRepositorioTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProdutoRepositorio _repositorio;
        private readonly Mock<DbSet<Produto>> _mockDbSet;

        public ProdutoRepositorioTests()
        {
            _mockDbContext = new Mock<AppDbContext>();
            _mockMapper = new Mock<IMapper>();
            _repositorio = new ProdutoRepositorio(_mockDbContext.Object, _mockMapper.Object);
            _mockDbSet = new Mock<DbSet<Produto>>();
        }

        [Fact]
        public async Task BuscarProdutos_RetornaListaDeProdutos()
        {
            var produtos = new List<Produto> {
            new Produto
            {
                ProdutoId = 1,
                Nome = "Camiseta Polo Masculina",
                Preco = 89.99m,
                QuantidadeEstoque = 200,
                Ativo = true,
                CategoriaId = 1,
                FornecedorId = 1,
                Categoria = new Categoria { CategoriaId = 1, Nome = "Roupas Masculinas" },
                Fornecedor = new Fornecedor { FornecedorId = 1, Nome = "Fashion Trends" },
                DataCriacao = DateTime.UtcNow.AddMonths(-3),
                MovimentacoesEstoque = new List<MovimentacaoEstoque>
                {
                    new MovimentacaoEstoque { Quantidade = 30, DataMovimento = DateTime.UtcNow.AddDays(-10) },
                    new MovimentacaoEstoque { Quantidade = 50, DataMovimento = DateTime.UtcNow.AddDays(-5) }
                }
            },
            new Produto
            {
                ProdutoId = 2,
                Nome = "Tênis Running Feminino",
                Preco = 199.90m,
                QuantidadeEstoque = 75,
                Ativo = true,
                CategoriaId = 2,
                FornecedorId = 2,
                Categoria = new Categoria { CategoriaId = 2, Nome = "Calçados Femininos" },
                Fornecedor = new Fornecedor { FornecedorId = 2, Nome = "RunStyle" },
                DataCriacao = DateTime.UtcNow.AddMonths(-1),
                MovimentacoesEstoque = new List<MovimentacaoEstoque>
                {
                    new MovimentacaoEstoque { Quantidade = 10, DataMovimento = DateTime.UtcNow.AddDays(-3) },
                    new MovimentacaoEstoque { Quantidade = 15, DataMovimento = DateTime.UtcNow.AddDays(-2) }
                }
            },
            new Produto {
                ProdutoId = 3,
                Nome = "Relógio de Pulso Masculino",
                Preco = 499.99m,
                QuantidadeEstoque = 50,
                Ativo = false,
                CategoriaId = 3,
                FornecedorId = 3,
                Categoria = new Categoria { CategoriaId = 3, Nome = "Acessórios Masculinos" },
                Fornecedor = new Fornecedor { FornecedorId = 3, Nome = "TimeStyle" },
                DataCriacao = DateTime.UtcNow.AddMonths(-6),
                MovimentacoesEstoque = new List<MovimentacaoEstoque>
                {
                    new MovimentacaoEstoque { Quantidade = 5, DataMovimento = DateTime.UtcNow.AddMonths(-2) },
                    new MovimentacaoEstoque { Quantidade = 10, DataMovimento = DateTime.UtcNow.AddMonths(-4) }
                }
            }
           };

            var mockProdutoDbSet = produtos.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(db => db.Produtos).Returns(mockProdutoDbSet.Object);

            var produtoDtos = new List<ProdutoResponseDto> {
           new ProdutoResponseDto { ProdutoId = 1, Nome = "Camiseta Polo Masculina", CategoriaNome = "Roupas Masculinas", FornecedorNome = "Fashion Trends" },
           new ProdutoResponseDto { ProdutoId = 2, Nome = "Tênis Running Feminino", CategoriaNome = "Calçados Femininos", FornecedorNome = "RunStyle" },
           new ProdutoResponseDto { ProdutoId = 3, Nome = "Relógio de Pulso Masculino", CategoriaNome = "Acessórios Masculinos", FornecedorNome = "TimeStyle" }};

            _mockMapper.Setup(m => m.Map<List<ProdutoResponseDto>>(It.IsAny<List<Produto>>()))
                       .Returns(produtoDtos);

            var resultado = await _repositorio.BuscarProdutos();

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count);
            Assert.Equal("Camiseta Polo Masculina", resultado[0].Nome);
            Assert.Equal("Tênis Running Feminino", resultado[1].Nome);
            Assert.Equal("Relógio de Pulso Masculino", resultado[2].Nome);
            Assert.Equal("Roupas Masculinas", resultado[0].CategoriaNome);
            Assert.Equal("Fashion Trends", resultado[0].FornecedorNome);
            Assert.Equal("Calçados Femininos", resultado[1].CategoriaNome);
            Assert.Equal("RunStyle", resultado[1].FornecedorNome);
            Assert.Equal("Acessórios Masculinos", resultado[2].CategoriaNome);
            Assert.Equal("TimeStyle", resultado[2].FornecedorNome);
        }

        [Fact]
        public void BuscarProdutoPorId_LancaExcecaoQuandoNaoExistente()
        {
            var produtoId = 999; 
            var produtos = new List<Produto>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Produto>>();

            mockDbSet.As<IQueryable<Produto>>()
                     .Setup(m => m.Provider).Returns(produtos.Provider);
            mockDbSet.As<IQueryable<Produto>>()
                     .Setup(m => m.Expression).Returns(produtos.Expression);
            mockDbSet.As<IQueryable<Produto>>()
                     .Setup(m => m.ElementType).Returns(produtos.ElementType);
            mockDbSet.As<IQueryable<Produto>>()
                     .Setup(m => m.GetEnumerator()).Returns(produtos.GetEnumerator());

            _mockDbContext.Setup(db => db.Produtos).Returns(mockDbSet.Object);

            var exception = Assert.Throws<KeyNotFoundException>(() => _repositorio.BuscarProdutoPorId(produtoId));
            Assert.Equal($"Produto com ID {produtoId} não encontrado.", exception.Message);
        }

        [Fact]
        public async Task Adicionar_ProdutoComCategoriaInexistente_LancaExcecao()
        {
            var produtoDto = new RequestProdutoDto
            {
                Nome = "Jaqueta de Couro Masculina",
                Preco = 349.99m,
                QuantidadeEstoque = 120,
                CategoriaId = 999,
                FornecedorId = 2,
                Ativo = true,
            };

            _mockDbContext.Setup(db => db.Categorias.FindAsync(produtoDto.CategoriaId))
                          .ReturnsAsync((Categoria)null);

            var exception = await Assert.ThrowsAsync<Exception>(() => _repositorio.Adicionar(produtoDto));
            Assert.Equal($"A categoria com ID {produtoDto.CategoriaId} não existe. Por favor vincule a uma categoria e um fornecedor existente.", exception.Message);
        }

        [Fact]
        public async Task Atualizar_ProdutoExistente_AtualizaComSucesso()
        {
            var produtoId = 1;
            var produtoExistente = new Produto
            {
                ProdutoId = produtoId,
                Nome = "Tênis Esportivo Masculino",
                Preco = 199.90m,
                QuantidadeEstoque = 150,
                CategoriaId = 1,
                FornecedorId = 1,
                DataCriacao = DateTime.UtcNow.AddMonths(-2),
                DataAtualizacao = DateTime.MinValue
            };

            var produtoDto = new RequestProdutoDto
            {
                Nome = "Tênis Esportivo Masculino - Atualizado",
                Preco = 179.90m,
                QuantidadeEstoque = 100,
                CategoriaId = 2,
                FornecedorId = 2
            };

            _mockDbContext.Setup(db => db.Produtos.FindAsync(produtoId))
                          .ReturnsAsync(produtoExistente);

            _mockMapper.Setup(m => m.Map(produtoDto, produtoExistente))
                       .Callback<RequestProdutoDto, Produto>((src, dest) =>
                       {
                           dest.Nome = src.Nome;
                           dest.Preco = src.Preco;
                           dest.QuantidadeEstoque = src.QuantidadeEstoque;
                           dest.CategoriaId = src.CategoriaId;
                           dest.FornecedorId = src.FornecedorId;
                           dest.DataAtualizacao = DateTime.UtcNow;
                       });

            _mockMapper.Setup(m => m.Map<RequestProdutoDto>(produtoExistente))
                       .Returns(new RequestProdutoDto
                       {
                           Nome = "Tênis Esportivo Masculino - Atualizado",
                           Preco = 179.90m,
                           QuantidadeEstoque = 100,
                           CategoriaId = 2,
                           FornecedorId = 2
                       });

            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);

            _mockDbContext.Setup(db => db.Produtos.Update(It.IsAny<Produto>()))
                          .Verifiable();

            var resultado = await _repositorio.Atualizar(produtoDto, produtoId);

            Assert.NotNull(resultado);
            Assert.Equal("Tênis Esportivo Masculino - Atualizado", resultado.Nome);
            Assert.Equal(2, resultado.CategoriaId);
            Assert.Equal(2, resultado.FornecedorId);
            Assert.Equal(179.90m, resultado.Preco);
            Assert.Equal(100, resultado.QuantidadeEstoque);

            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Apagar_ProdutoExistente_RetornaTrue()
        {
            var produtoExistente = new Produto
            {
                ProdutoId = 1,
                Nome = "Camiseta Masculina",
                Preco = 99.90m,
                QuantidadeEstoque = 50,
                CategoriaId = 1,
                FornecedorId = 1,
                Categoria = new Categoria { CategoriaId = 1, Nome = "Roupas Masculinas" },
                Fornecedor = new Fornecedor { FornecedorId = 1, Nome = "Fashion Trends" },
                DataCriacao = DateTime.UtcNow.AddMonths(-3),
                MovimentacoesEstoque = new List<MovimentacaoEstoque> {
                  new MovimentacaoEstoque { Quantidade = 20, DataMovimento = DateTime.UtcNow.AddDays(-10) },
                  new MovimentacaoEstoque { Quantidade = 30, DataMovimento = DateTime.UtcNow.AddDays(-5) }
                  }
            };

            _mockDbContext.Setup(db => db.Produtos.FindAsync(1)).ReturnsAsync(produtoExistente);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var resultado = await _repositorio.Apagar(1);
            Assert.True(resultado);

            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(db => db.Produtos.Remove(produtoExistente), Times.Once);
        }

        [Fact]
        public async Task Apagar_ProdutoNaoExistente_LancaException()
        {
            _mockDbContext.Setup(db => db.Produtos.FindAsync(1)).ReturnsAsync((Produto)null);
            await Assert.ThrowsAsync<Exception>(() => _repositorio.Apagar(1));
        }
    }
}
