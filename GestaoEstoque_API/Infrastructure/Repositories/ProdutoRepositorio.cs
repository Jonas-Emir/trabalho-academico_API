using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
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

        public async Task<ProdutoResponseDto> BuscarProdutoPorIdAsync(int produtoId)
        {
            var produto = await _dbContext.Produto
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);

            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado.");

            return _mapper.Map<ProdutoResponseDto>(produto);
        }

        public async Task<RequestProdutoDto> Adicionar(RequestProdutoDto produtoDto)
        {
            await VerificarExistenciaEntidadesCategoriaFornecedor(produtoDto.CategoriaId, produtoDto.FornecedorId);

            var produtoExistente = await VerificarProdutoExistente(produtoDto.Nome);
            if (produtoExistente)
                throw new Exception($"O produto com o nome '{produtoDto.Nome}' já está cadastrado.");

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

        #region Métodos auxiliares
        private async Task VerificarExistenciaEntidadesCategoriaFornecedor(int categoriaId, int fornecedorId)
        {
            var verificacoes = new (Func<Task<bool>> Verificacao, string Mensagem)[]
            {
                 (async () => !await _dbContext.Categoria.AnyAsync(c => c.CategoriaId == categoriaId), $"A categoria com ID {categoriaId} não existe."),
                 (async () => !await _dbContext.Fornecedor.AnyAsync(f => f.FornecedorId == fornecedorId), $"O fornecedor com ID {fornecedorId} não existe.")
            };

            var erros = new List<string>();

            foreach (var (verificacao, mensagem) in verificacoes)
            {
                if (await verificacao())
                    erros.Add(mensagem);
            }

            if (erros.Any())
                throw new Exception(string.Join(" ", erros));
        }

        private async Task<bool> VerificarProdutoExistente(string nomeProduto, int? produtoId = null)
        {
            return await _dbContext.Produto
                .Where(p => p.Nome == nomeProduto && (produtoId == null || p.ProdutoId != produtoId))
                .AnyAsync();
        }
        #endregion
    }
}

