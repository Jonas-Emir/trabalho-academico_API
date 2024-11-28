using Moq;
using AutoMapper;
using GestaoEstoque_API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using GestaoEstoque_API.Application.Enums;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoEstoque_API.Tests.Repositories
{
    public class ProdutoRepositorioTests
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProdutoRepositorio _repositorio;

        public ProdutoRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseInMemoryDatabase("ProdutoTeste")
                          .Options;

            _dbContext = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _repositorio = new ProdutoRepositorio(_dbContext, _mockMapper.Object);

            _dbContext.Database.EnsureDeleted(); 
            _dbContext.Database.EnsureCreated();
        }


        private async Task AdicionarDadosTeste()
        {
            // Adiciona Fornecedores
            var fornecedores = new List<Fornecedor>
            {
                new Fornecedor
                {
                    FornecedorId = 1,
                    Nome = "Fashion Trends",
                    CNPJ = "12345678000195",
                    Telefone = "123456789",
                    Email = "contato@fashiontrends.com",
                    Endereco = "Rua Moda, 123",
                    DataCriacao = DateTime.Now
                },
                new Fornecedor
                {
                    FornecedorId = 2,
                    Nome = "RunStyle",
                    CNPJ = "98765432000123",
                    Telefone = "987654321",
                    Email = "contato@runstyle.com",
                    Endereco = "Avenida Esportiva, 456",
                    DataCriacao = DateTime.Now
                }
            };

            _dbContext.Fornecedor.AddRange(fornecedores);
            await _dbContext.SaveChangesAsync();

            // Adiciona Categorias
            var categorias = new List<Categoria>
            {
                new Categoria { CategoriaId = 1, Nome = "Roupas Masculinas", Descricao = "Roupa masculina" },
                new Categoria { CategoriaId = 2, Nome = "Calçados Femininos", Descricao = "Calçados femininos" },
                new Categoria { CategoriaId = 3, Nome = "Acessórios Masculinos", Descricao = "Acessórios masculinos" }
            };

            _dbContext.Categoria.AddRange(categorias);
            await _dbContext.SaveChangesAsync();

            // Adiciona Produtos
            var produtos = new List<Produto>
            {
                new Produto
                {
                    ProdutoId = 1,
                    Nome = "Camiseta Polo Masculina",
                    Preco = 79.99m,
                    Ativo = true,
                    CategoriaId = 1,
                    FornecedorId = 1,
                    DataCriacao = DateTime.Now
                },
                new Produto
                {
                    ProdutoId = 2,
                    Nome = "Tênis Running Feminino",
                    Preco = 199.99m,
                    Ativo = true,
                    CategoriaId = 2,
                    FornecedorId = 2,
                    DataCriacao = DateTime.Now
                },
                new Produto
                {
                    ProdutoId = 3,
                    Nome = "Relógio de Pulso Masculino",
                    Preco = 129.99m,
                    Ativo = true,
                    CategoriaId = 3,
                    FornecedorId = 1,
                    DataCriacao = DateTime.Now
                }
            };

            _dbContext.Produto.AddRange(produtos);
            await _dbContext.SaveChangesAsync();

            // Adiciona Estoque
            var estoque = new List<Estoque>
            {
                new Estoque { ProdutoId = 1, Quantidade = 100, Id_Tipo_Movimento = TipoMovimento.Entrada },
                new Estoque { ProdutoId = 2, Quantidade = 50, Id_Tipo_Movimento = TipoMovimento.Saida },
                new Estoque { ProdutoId = 3, Quantidade = 75, Id_Tipo_Movimento = TipoMovimento.Entrada }
            };

            _dbContext.Estoque.AddRange(estoque);
            await _dbContext.SaveChangesAsync();
        }

        // Teste: BuscarProdutos_RetornaListaDeProdutos
        [Fact]
        public async Task BuscarProdutos_RetornaListaDeProdutos()
        {
            // Adiciona os dados de teste ao banco de dados em memória
            await AdicionarDadosTeste();

            // Dados esperados para o mapeamento do Mapper
            var produtoDtos = new List<ProdutoResponseDto>
            {
                new ProdutoResponseDto { ProdutoId = 1, Nome = "Camiseta Polo Masculina", CategoriaNome = "Roupas Masculinas", FornecedorNome = "Fashion Trends" },
                new ProdutoResponseDto { ProdutoId = 2, Nome = "Tênis Running Feminino", CategoriaNome = "Calçados Femininos", FornecedorNome = "RunStyle" },
                new ProdutoResponseDto { ProdutoId = 3, Nome = "Relógio de Pulso Masculino", CategoriaNome = "Acessórios Masculinos", FornecedorNome = "Fashion Trends" }
            };

            // Configura o Mock do Mapper
            _mockMapper.Setup(m => m.Map<List<ProdutoResponseDto>>(It.IsAny<List<Produto>>()))
                      .Returns(produtoDtos);

            // Chama o método de busca
            var resultado = await _repositorio.BuscarProdutos();

            // Verifica o resultado
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count);
            Assert.Contains(resultado, p => p.Nome == "Camiseta Polo Masculina");
            Assert.Contains(resultado, p => p.Nome == "Tênis Running Feminino");
            Assert.Contains(resultado, p => p.Nome == "Relógio de Pulso Masculino");
        }

        [Fact]
        public async Task BuscarProdutoPorId_RetornaProdutoQuandoExistente()
        {
            // Adiciona os dados de teste ao banco de dados em memória
            await AdicionarDadosTeste();

            int produtoId = 1;

            // Chama o método de forma assíncrona
            var resultado = await _repositorio.BuscarProdutoPorIdAsync(produtoId);  // Agora é assíncrono

            Assert.NotNull(resultado);
            Assert.Equal(produtoId, resultado.ProdutoId);
            Assert.Equal("Camiseta Polo Masculina", resultado.Nome);
        }

    }
}
