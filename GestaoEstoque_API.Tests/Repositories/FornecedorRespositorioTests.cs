using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class FornecedorRepositorioTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly FornecedorRepositorio _fornecedorRepositorio;

        public FornecedorRepositorioTests()
        {
            _mapperMock = new Mock<IMapper>();
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco de dados em memória
                .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _fornecedorRepositorio = new FornecedorRepositorio(_dbContext, _mapperMock.Object);
        }

        private void SeedDatabase()
        {
            _dbContext.Fornecedor.AddRange(new List<Fornecedor>
    {
        new Fornecedor
        {
            FornecedorId = 1,
            Nome = "Fornecedor Eletrônicos",
            CNPJ = "12345678000199",
            Telefone = "(11) 98765-4321",
            Email = "eletronicos@empresa.com",
            Endereco = "Rua dos Eletrônicos, 123",
            DataCriacao = DateTime.Now,
        },
        new Fornecedor
        {
            FornecedorId = 2,
            Nome = "Fornecedor Vestuário",
            CNPJ = "98765432000188",
            Telefone = "(21) 99876-5432",
            Email = "vestuario@empresa.com",
            Endereco = "Avenida das Roupas, 456",
            DataCriacao = DateTime.Now,
        }
         });

            _dbContext.SaveChanges();
        }

        [Fact]
        public void BuscarFornecedores_DeveRetornarListaDeFornecedores()
        {
            SeedDatabase();
            _mapperMock.Setup(m => m.Map<List<FornecedorResponseDto>>(It.IsAny<List<Fornecedor>>()))
                       .Returns(new List<FornecedorResponseDto>
                       {
                           new FornecedorResponseDto { FornecedorId = 1, Nome = "Fictício Comércio LTDA" },
                           new FornecedorResponseDto { FornecedorId = 2, Nome = "Distribuidora XYZ" }
                       });

            var fornecedores = _fornecedorRepositorio.BuscarFornecedores();

            Assert.NotNull(fornecedores);
            Assert.Equal(2, fornecedores.Count);
        }

        [Fact]
        public void BuscarPorId_DeveRetornarFornecedor_QuandoIdExistente()
        {
            SeedDatabase();
            var fornecedorId = 1;

            var fornecedorResponseDto = new FornecedorResponseDto
            {
                Nome = "Fictício Comércio LTDA",
                CNPJ = "11111111000111",
            };
            _mapperMock.Setup(m => m.Map<FornecedorResponseDto>(It.IsAny<Fornecedor>())).Returns(fornecedorResponseDto);

            FornecedorResponseDto resultado = _fornecedorRepositorio.BuscarPorId(fornecedorId);

            Assert.NotNull(resultado);
            Assert.Equal("Fictício Comércio LTDA", resultado.Nome);
            Assert.Equal("11111111000111", resultado.CNPJ);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarFornecedor()
        {
            FornecedorRequestDto fornecedorRequest = new FornecedorRequestDto
            {
                Nome = "Nova Fábrica ABC",
                CNPJ = "33333333000333",
                Telefone = "(21) 99876-5432",
                Email = "vestuario@empresa.com",
                Endereco = "Avenida das Roupas, 456"
            };

            _mapperMock.Setup(m => m.Map<FornecedorRequestDto>(It.IsAny<Fornecedor>())).Returns(fornecedorRequest);

            FornecedorRequestDto resultado = await _fornecedorRepositorio.AdicionarAsync(fornecedorRequest);

            Assert.NotNull(resultado);
            Assert.Equal("Nova Fábrica ABC", resultado.Nome);
            Assert.Equal("33333333000333", resultado.CNPJ);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarFornecedor()
        {
            SeedDatabase();
            var fornecedorRequest = new FornecedorRequestDto
            {
                Nome = "Fornecedor Alterado",
                CNPJ = "11111111000111",
                Telefone = "(21) 99876-5432",
                Email = "vestuario@empresa.com",
                Endereco = "Avenida das Roupas, 456",
            };

            _mapperMock.Setup(m => m.Map<FornecedorRequestDto>(It.IsAny<Fornecedor>())).Returns(fornecedorRequest);

            var resultado = await _fornecedorRepositorio.AtualizarAsync(fornecedorRequest, 1);

            Assert.NotNull(resultado);
            Assert.Equal("Fornecedor Alterado", resultado.Nome);
        }

        [Fact]
        public void Apagar_DeveExcluirFornecedor()
        {
            SeedDatabase();

            var resultado = _fornecedorRepositorio.Apagar(1);

            Assert.NotNull(resultado);
            Assert.Contains("apagado com sucesso", resultado);

            var fornecedorRemovido = _dbContext.Fornecedor.FirstOrDefault(f => f.FornecedorId == 1);
            Assert.Null(fornecedorRemovido);
        }

        [Fact]
        public void Apagar_DeveRetornarErro_QuandoExistemProdutosVinculados()
        {
            SeedDatabase();
            _dbContext.Produto.Add(new Produto { ProdutoId = 1, Nome = "Produto Exemplo", FornecedorId = 1 });
            _dbContext.SaveChanges();

            var resultado = _fornecedorRepositorio.Apagar(1);

            Assert.Contains("Não é possível excluir o fornecedor", resultado);
        }
    }
}
