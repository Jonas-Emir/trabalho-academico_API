using AutoMapper;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Domain.Entities;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProdutoRepositorio(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<ProdutoResponseDto>> BuscarProdutos()
        {
            var produtos = await _dbContext.Produto
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .ToListAsync();

            return _mapper.Map<List<ProdutoResponseDto>>(produtos);
        }

        public async Task<ProdutoResponseDto> BuscarPorId(int produtoId)
        {
            var produto = await _dbContext.Produto
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);

            if (produto == null)
                return null;

            return _mapper.Map<ProdutoResponseDto>(produto);
        }

        public async Task<RequestProdutoDto> Adicionar(RequestProdutoDto produtoDto)
        {
            var categoriaExistente = await _dbContext.Categoria.FindAsync(produtoDto.CategoriaId);
            if (categoriaExistente == null)
            {
                throw new Exception($"A categoria com ID {produtoDto.CategoriaId} não existe. Por favor vincule a uma categoria e um fornecedor existente.");
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            produto.DataCriacao = DateTime.Now;

            await _dbContext.Produto.AddAsync(produto);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RequestProdutoDto>(produto);
        }

        public async Task<RequestProdutoDto> Atualizar(RequestProdutoDto produtoDto, int produtoId)
        {
            var produtoColetado = await _dbContext.Produto.FindAsync(produtoId);

            if (produtoColetado == null)
                throw new Exception($"Produto para o ID: {produtoId} não foi encontrado no banco de dados, atualização não realizada.");

            _mapper.Map(produtoDto, produtoColetado); 
            produtoColetado.DataAtualizacao = DateTime.Now;

            _dbContext.Produto.Update(produtoColetado);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RequestProdutoDto>(produtoColetado);
        }

        public async Task<bool> Apagar(int produtoId)
        {
            var produtoColetado = await _dbContext.Produto.FindAsync(produtoId);

            if (produtoColetado == null)
                throw new Exception($"Produto para o ID: {produtoId} não foi encontrado no banco de dados.");

            _dbContext.Produto.Remove(produtoColetado);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

