using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Application.Enums;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class EstoqueRepositorio : IEstoqueRepositorio
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public EstoqueRepositorio(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<EstoqueResponseDto>> BuscarEstoques()
        {
            var estoques = await _dbContext.Estoque
                .Include(x => x.Produto)
                .ToListAsync();

            return _mapper.Map<List<EstoqueResponseDto>>(estoques);
        }

        public async Task<EstoqueResponseDto> BuscarPorId(int estoqueId)
        {
            var estoque = await _dbContext.Estoque
                .Include(x => x.Produto)
                .FirstOrDefaultAsync(x => x.EstoqueId == estoqueId);

            if (estoque == null)
                return null;

            return _mapper.Map<EstoqueResponseDto>(estoque);
        }

        public async Task<EstoqueRequestDto> Adicionar(EstoqueRequestDto estoqueDto)
        {
            var produto = await VerificarSeProdutoExisteAsync(estoqueDto.ProdutoId);
            var estoque = new Estoque
            {
                ProdutoId = estoqueDto.ProdutoId,
                Quantidade = estoqueDto.Quantidade,
                Id_Tipo_Movimento = estoqueDto.TipoMovimentoId
            };

            await _dbContext.Estoque.AddAsync(estoque);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EstoqueRequestDto>(estoque);
        }

        public async Task<EstoqueRequestDto> Atualizar(EstoqueRequestDto estoqueDto, int estoqueId)
        {
            var estoqueColetado = await _dbContext.Estoque.FindAsync(estoqueId);

            if (estoqueColetado == null)
                throw new Exception($"Estoque com o ID: {estoqueId} não encontrado, atualização não realizada.");

            _mapper.Map(estoqueDto, estoqueColetado);

            _dbContext.Estoque.Update(estoqueColetado);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EstoqueRequestDto>(estoqueColetado);
        }

        public async Task<bool> Apagar(int estoqueId)
        {
            var estoqueColetado = await _dbContext.Estoque.FindAsync(estoqueId);

            if (estoqueColetado == null)
                throw new Exception($"Estoque com o ID: {estoqueId} não encontrado.");

            _dbContext.Estoque.Remove(estoqueColetado);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        #region Métodos auxiliares
        public async Task<List<ProdutoQuantidadeDto>> BuscarQuantidadeTotalDeCadaProduto()
        {
            var produtos = await _dbContext.Estoque
                .Join(_dbContext.Produto,
                      estoque => estoque.ProdutoId,
                      produto => produto.ProdutoId,
                      (estoque, produto) => new { produto.ProdutoId, produto.Nome })
                .Distinct()
                .ToListAsync();

            var resultados = new List<ProdutoQuantidadeDto>();

            foreach (var produto in produtos)
            {
                var quantidadeTotal = await CalcularQuantidadeTotalEstoque(produto.ProdutoId);
                resultados.Add(new ProdutoQuantidadeDto
                {
                    ProdutoId = produto.ProdutoId,
                    Nome = produto.Nome,
                    QuantidadeTotal = quantidadeTotal
                });
            }

            return resultados;
        }

        public async Task<QuantidadePorTipoMovimentoResponseDto> ObterMovimentacoesPorProduto(int produtoId)
        {
            var produto = await _dbContext.Produto
                .Where(p => p.ProdutoId == produtoId)
                .Select(p => new { p.ProdutoId, p.Nome })
                .FirstOrDefaultAsync();

            if (produto == null)
                throw new Exception($"Produto com ID {produtoId} não foi encontrado.");

            var quantidadePorTipoMovimento = await BuscarQuantidadePorTipoMovimento(produtoId);

            return new QuantidadePorTipoMovimentoResponseDto
            {
                ProdutoId = produto.ProdutoId,
                ProdutoNome = produto.Nome,
                QuantidadePorTipoMovimento = quantidadePorTipoMovimento
            };
        }

        public async Task<Dictionary<string, int>> BuscarQuantidadePorTipoMovimento(int produtoId)
        {
            var movimentos = await _dbContext.Estoque
                .Where(e => e.ProdutoId == produtoId)
                .GroupBy(e => e.Id_Tipo_Movimento)
                .Select(g => new
                {
                    TipoMovimento = g.Key,
                    Quantidade = g.Sum(e => e.Quantidade)
                })
                .ToListAsync();

            var quantidadePorTipoMovimento = movimentos
                .ToDictionary(
                    m => m.TipoMovimento.ToString(),
                    m => m.Quantidade
                );

            return quantidadePorTipoMovimento;
        }

        public async Task<ProdutoEstoqueResponseDto> BuscarQuantidadeEstoquePorProduto(int produtoId)
        {
            var produto = await VerificarSeProdutoExisteAsync(produtoId);

            var quantidadeTotal = await CalcularQuantidadeTotalEstoque(produtoId);

            var estoqueResponse = new ProdutoEstoqueResponseDto
            {
                ProdutoId = produtoId,
                ProdutoNome = produto.ProdutoNome,
                Quantidade = quantidadeTotal
            };

            return estoqueResponse;
        }

        public async Task<int> CalcularQuantidadeTotalEstoque(int produtoId)
        {
            var quantidadeEntradas = await _dbContext.Estoque
                .Where(e => e.ProdutoId == produtoId && e.Id_Tipo_Movimento == TipoMovimento.Entrada)
                .SumAsync(e => e.Quantidade);

            var quantidadeQuebras = await _dbContext.Estoque
                .Where(e => e.ProdutoId == produtoId && e.Id_Tipo_Movimento == TipoMovimento.Quebra)
                .SumAsync(e => e.Quantidade);

            var quantidadeSaidas = await _dbContext.Estoque
                .Where(e => e.ProdutoId == produtoId && e.Id_Tipo_Movimento == TipoMovimento.Saida)
                .SumAsync(e => e.Quantidade);

            var quantidadeTotal = quantidadeEntradas - quantidadeQuebras - quantidadeSaidas;

            return quantidadeTotal;
        }


        public async Task<EstoqueResponseDto> VerificarSeProdutoExisteAsync(int produtoId)
        {
            var produto = await _dbContext.Produto
                .Where(p => p.ProdutoId == produtoId)
                .Select(p => new { p.ProdutoId, p.Nome })
                .FirstOrDefaultAsync();

            if (produto == null)
                throw new Exception($"Produto com ID {produtoId} não foi encontrado.");

            return new EstoqueResponseDto
            {
                ProdutoId = produto.ProdutoId,
                ProdutoNome = produto.Nome
            };
        }

        public async Task<int> ObterQuantidadeTotalEstoque(int produtoId)
        {
            var quantidadeTotal = await _dbContext.Estoque
                .Where(e => e.ProdutoId == produtoId)
                .SumAsync(e => e.Id_Tipo_Movimento == TipoMovimento.Entrada ? e.Quantidade : -e.Quantidade);

            return quantidadeTotal;
        }
        #endregion
    }
}
