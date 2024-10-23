using GestaoEstoque_API.Application.Dtos.Produto;
using GestaoEstoque_API.Domain.Entities;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly AppDbContext _dbContext;

        public ProdutoRepositorio(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<List<ProdutoResponseDto>> BuscarProdutos()
        {
            var produtos = await _dbContext.Produtos
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .ToListAsync();

            return produtos.Select(produto => new ProdutoResponseDto
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Ativo = produto.Ativo,
            }).ToList();
        }

        public async Task<ProdutoResponseDto> BuscarPorId(int produtoId)
        {
            var produto = await _dbContext.Produtos
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);

            if (produto == null)
                return null;

            return new ProdutoResponseDto
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Ativo = produto.Ativo,
            };
        }

        public async Task<RequestProdutoDto> Adicionar(RequestProdutoDto produtoDto)
        {
            var categoriaExistente = await _dbContext.Categorias.FindAsync(produtoDto.CategoriaId);
            if (categoriaExistente == null)
            {
                throw new Exception($"A categoria com ID {produtoDto.CategoriaId} não existe.");
            }

            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                QuantidadeEstoque = produtoDto.QuantidadeEstoque,
                Ativo = produtoDto.Ativo,
                CategoriaId = produtoDto.CategoriaId,
                FornecedorId = produtoDto.FornecedorId,
                DataCriacao = DateTime.Now
            };

            await _dbContext.Produtos.AddAsync(produto);
            await _dbContext.SaveChangesAsync();

            return new RequestProdutoDto
            {
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Ativo = produto.Ativo,
                CategoriaId = produto.CategoriaId,
                FornecedorId = produto.FornecedorId,
            };
        }

        public async Task<RequestProdutoDto> Atualizar(RequestProdutoDto produtoDto, int produtoId)
        {
            var produtoColetado = await _dbContext.Produtos.FindAsync(produtoId);

            if (produtoColetado == null)
                throw new Exception($"Produto para o ID: {produtoId} não foi encontrado no banco de dados, atualização não realizada.");

            produtoColetado.Nome = produtoDto.Nome;
            produtoColetado.Preco = produtoDto.Preco;
            produtoColetado.QuantidadeEstoque = produtoDto.QuantidadeEstoque;
            produtoColetado.Ativo = produtoDto.Ativo;
            produtoColetado.CategoriaId = produtoDto.CategoriaId;
            produtoColetado.FornecedorId = produtoDto.FornecedorId;
            produtoColetado.DataAtualizacao = DateTime.Now;

            _dbContext.Produtos.Update(produtoColetado);
            await _dbContext.SaveChangesAsync();

            return new RequestProdutoDto
            {
                Nome = produtoColetado.Nome,
                Preco = produtoColetado.Preco,
                QuantidadeEstoque = produtoColetado.QuantidadeEstoque,
                Ativo = produtoColetado.Ativo,
                CategoriaId = produtoColetado.CategoriaId,
                FornecedorId = produtoColetado.FornecedorId,
            };
        }

        public async Task<bool> Apagar(int produtoId)
        {
            var produtoColetado = await _dbContext.Produtos.FindAsync(produtoId);

            if (produtoColetado == null)
                throw new Exception($"Produto para o ID: {produtoId} não foi encontrado no banco de dados.");

            _dbContext.Produtos.Remove(produtoColetado);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

