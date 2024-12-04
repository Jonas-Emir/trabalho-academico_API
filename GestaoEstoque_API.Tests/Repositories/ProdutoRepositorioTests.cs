using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class ProdutoRepositorioTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoRepositorioTests()
        {
            _mapperMock = new Mock<IMapper>();
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _produtoRepositorio = new ProdutoRepositorio(_dbContext, _mapperMock.Object);
        }

        private void SeedDatabase()
        {
            var fornecedores = new List<Fornecedor>
    {
        new Fornecedor { FornecedorId = 1, Nome = "Fornecedor A", CNPJ = "11111111000191", Telefone = "11111111", Email = "fornecedorA@teste.com", Endereco = "Rua A", DataCriacao = DateTime.Now },
        new Fornecedor { FornecedorId = 2, Nome = "Fornecedor B", CNPJ = "22222222000192", Telefone = "22222222", Email = "fornecedorB@teste.com", Endereco = "Rua B", DataCriacao = DateTime.Now }
    };

            var categorias = new List<Categoria>
    {
        new Categoria { CategoriaId = 1, Nome = "Tecnologia", Descricao = "Produtos como celulares, computadores e acessórios eletrônicos." },
        new Categoria { CategoriaId = 2, Nome = "Roupas",  Descricao = "Artigos de vestuário, moda e acessórios." }
    };

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

            _dbContext.Fornecedor.AddRange(fornecedores);
            _dbContext.Categoria.AddRange(categorias);
            _dbContext.Produto.AddRange(produtos);
            _dbContext.SaveChanges();
        }


        [Fact]
        public async Task BuscarProdutos_DeveRetornarListaDeProdutos()
        {
            SeedDatabase();

            _mapperMock.Setup(m => m.Map<List<ProdutoResponseDto>>(It.IsAny<List<Produto>>()))
                .Returns(new List<ProdutoResponseDto>
                {
                   new ProdutoResponseDto { ProdutoId = 1, Nome = "Celular" },
                   new ProdutoResponseDto { ProdutoId = 2, Nome = "Camiseta" }
                });

            var produtos = await _produtoRepositorio.BuscarProdutos();

            Assert.NotNull(produtos);
            Assert.Equal(2, produtos.Count);
            Assert.Equal("Celular", produtos[0].Nome);
            Assert.Equal("Camiseta", produtos[1].Nome);
        }

        [Fact]
        public async Task BuscarProdutoPorId_DeveRetornarProdutoCorreto()
        {
            SeedDatabase();

            var categoriaResponseDto = new ProdutoResponseDto
            {
                Nome = "Celular",
                Preco = 5,
                Ativo = true

            };

            _mapperMock.Setup(m => m.Map<ProdutoResponseDto>(It.IsAny<Produto>())).Returns(categoriaResponseDto);

            ProdutoResponseDto produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(1);

            Assert.NotNull(produto);
            Assert.Equal("Celular", produto.Nome);
            Assert.Equal(5, produto.Preco);
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarProduto()
        {
            SeedDatabase();
            RequestProdutoDto novoProduto = new RequestProdutoDto
            {
                Nome = "Monitor",
                CategoriaId = 1,
                FornecedorId = 1,
                Preco = 1000.00m,
                Ativo = true,
            };
            Produto produtoMock = new Produto
            {
                ProdutoId = 1,
                Nome = "Monitor",
                CategoriaId = 1,
                FornecedorId = 1,
                Preco = 1000.00m,
                Ativo = true,
                DataCriacao = DateTime.Now 
            };

            _mapperMock.Setup(m => m.Map<Produto>(It.IsAny<RequestProdutoDto>()))
                      .Returns(new Produto
                      {
                          Nome = "Monitor",
                          CategoriaId = 1,
                          FornecedorId = 1,
                          Preco = 1000.00m,
                          Ativo = true,
                          DataCriacao = DateTime.Now
                      });

            _mapperMock.Setup(m => m.Map<RequestProdutoDto>(It.IsAny<Produto>())).Returns(novoProduto);

            RequestProdutoDto produtoAdicionado = await _produtoRepositorio.Adicionar(novoProduto);

            Assert.NotNull(produtoAdicionado);
            Assert.Equal("Monitor", produtoAdicionado.Nome);
            Assert.Equal(1000.00m, produtoAdicionado.Preco);
            Assert.True(produtoAdicionado.Ativo);
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarProduto()
        {
            SeedDatabase();
            var produtoAtualizado = new RequestProdutoDto
            {
                Nome = "Celular Atualizado",
                CategoriaId = 1,
                FornecedorId = 1,
                Preco = 2000.00m,
                Ativo = false
            };
         
            _mapperMock.Setup(m => m.Map<RequestProdutoDto>(It.IsAny<Produto>())).Returns(produtoAtualizado);
            var resultado = await _produtoRepositorio.Atualizar(produtoAtualizado, 1);

            Assert.NotNull(resultado);
            Assert.Equal("Celular Atualizado", resultado.Nome);
            Assert.Equal(2000.00m, resultado.Preco);
            Assert.False(resultado.Ativo);
        }

        [Fact]
        public async Task Apagar_DeveLancarExcecao_SeHouverVinculo()
        {
            SeedDatabase();

            var produtoId = 1; 
            var exception = await Assert.ThrowsAsync<Exception>(() => _produtoRepositorio.Apagar(produtoId));

            Assert.Equal("O produto com ID: 1 possui vínculos com uma categoria ou fornecedor e não pode ser excluído.", exception.Message);

            var produtoPersistente = await _dbContext.Produto.FindAsync(produtoId);
            Assert.NotNull(produtoPersistente);
        }
    }
}