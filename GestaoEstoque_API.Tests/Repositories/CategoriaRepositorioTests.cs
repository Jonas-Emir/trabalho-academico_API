using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class CategoriaRepositorioTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly CategoriaRepositorio _categoriaRepositorio;

        public CategoriaRepositorioTests()
        {
            _mapperMock = new Mock<IMapper>();
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco de dados em memória
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _categoriaRepositorio = new CategoriaRepositorio(_dbContext, _mapperMock.Object);
        }

        private void SeedDatabase()
        {
            _dbContext.Categoria.AddRange(new List<Categoria>  {
                new Categoria
                {
                    CategoriaId = 1,
                    Nome = "Eletrônicos",
                    Descricao = "Produtos como celulares, computadores e acessórios eletrônicos."
                },
                new Categoria
                {
                    CategoriaId = 2,
                    Nome = "Roupas",
                    Descricao = "Artigos de vestuário, moda e acessórios."
                },
                new Categoria
                {
                    CategoriaId = 3,
                    Nome = "Alimentos",
                    Descricao = "Produtos alimentícios não perecíveis e bebidas."
                }
             });

            _dbContext.SaveChanges();
        }

        [Fact]
        public void BuscarCategorias_DeveRetornarListaDeCategorias()
        {
            SeedDatabase();
            _mapperMock.Setup(m => m.Map<List<CategoriaResponseDto>>(It.IsAny<List<Categoria>>()))
                       .Returns(new List<CategoriaResponseDto>
                       {
                   new CategoriaResponseDto { CategoriaId = 1, Nome = "Eletrônicos" },
                   new CategoriaResponseDto { CategoriaId = 2, Nome = "Roupas" }
                       });

            var categorias = _categoriaRepositorio.BuscarCategorias();

            Assert.NotNull(categorias);
            Assert.Equal(2, categorias.Count);
        }

        [Fact]
        public void BuscarPorId_DeveRetornarCategoria_QuandoIdExistente()
        {
            SeedDatabase();
            var categoriaId = 1;

            var categoriaResponseDto = new CategoriaResponseDto
            {
                Nome = "Eletrônicos",
                Descricao = "Produtos eletrônicos"
            };
            _mapperMock.Setup(m => m.Map<CategoriaResponseDto>(It.IsAny<Categoria>())).Returns(categoriaResponseDto);

            CategoriaResponseDto resultado = _categoriaRepositorio.BuscarPorId(categoriaId);

            Assert.NotNull(resultado);
            Assert.Equal("Eletrônicos", resultado.Nome);
            Assert.Equal("Produtos eletrônicos", resultado.Descricao);
        }
        [Fact]
        public async Task Adicionar_DeveAdicionarCategoria()
        {
            CategoriaRequestDto categoriaRequest = new CategoriaRequestDto
            {
                Nome = "Eletrônicos",
                Descricao = "Produtos eletrônicos"
            };

            _mapperMock.Setup(m => m.Map<CategoriaRequestDto>(It.IsAny<Categoria>())).Returns(categoriaRequest);
            CategoriaRequestDto resultado = await _categoriaRepositorio.Adicionar(categoriaRequest);

            Assert.NotNull(resultado);
            Assert.Equal("Eletrônicos", resultado.Nome);
            Assert.Equal("Produtos eletrônicos", resultado.Descricao);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarCategoria()
        {
            var categoriaRequest = new CategoriaRequestDto
            {
                Nome = "Eletrônicos Atualizados",
                Descricao = "Produtos eletrônicos novos"
            };

            var categoria = new Categoria
            {
                CategoriaId = 1,
                Nome = "Eletrônicos",
                Descricao = "Produtos eletrônicos"
            };

            _dbContext.Categoria.Add(categoria);
            await _dbContext.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<CategoriaRequestDto>(It.IsAny<Categoria>())).Returns(categoriaRequest);
            var resultado = await _categoriaRepositorio.Atualizar(categoriaRequest, 1);

            Assert.NotNull(resultado);
            Assert.Equal("Eletrônicos Atualizados", resultado.Nome);
        }

        [Fact]
        public async Task Apagar_DeveExcluirCategoria()
        {
            var categoria = new Categoria
            {
                CategoriaId = 1,
                Nome = "Eletrônicos",
                Descricao = "Produtos eletrônicos"
            };

            _dbContext.Categoria.Add(categoria);
            await _dbContext.SaveChangesAsync();
            var resultado = _categoriaRepositorio.Apagar(1);

            Assert.NotNull(resultado);
            Assert.Contains("apagada com sucesso", resultado);

            var categoriaRemovida = await _dbContext.Categoria.FirstOrDefaultAsync(c => c.CategoriaId == 1);
            Assert.Null(categoriaRemovida);
        }
    }
}