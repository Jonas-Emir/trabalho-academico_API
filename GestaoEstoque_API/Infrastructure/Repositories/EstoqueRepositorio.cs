using AutoMapper;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Domain.Entities;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class EstoqueRepositorio
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public EstoqueRepositorio(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<MovimentacaoEstoqueDto>> BuscarEstoques()
        {
            var estoques = await _dbContext.Estoque
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .ToListAsync();

            return _mapper.Map<List<MovimentacaoEstoqueDto>>(estoques);
        }

        public async Task<MovimentacaoEstoqueDto> BuscarPorId(int estoqueId)
        {
            var estoque = await _dbContext.Estoque
                .Include(x => x.Categoria)
                .Include(x => x.Fornecedor)
                .FirstOrDefaultAsync(x => x.EstoqueId == estoqueId);

            if (estoque == null)
                return null;

            return _mapper.Map<MovimentacaoEstoqueDto>(estoque);
        }

        public async Task<MovimentacaoEstoqueDto> Adicionar(MovimentacaoEstoqueDto estoqueDto)
        {
            var estoque = _mapper.Map<Estoque>(estoqueDto);

            await _dbContext.Estoque.AddAsync(estoque);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MovimentacaoEstoqueDto>(estoque);
        }

        public async Task<MovimentacaoEstoqueDto> Atualizar(MovimentacaoEstoqueDto estoqueDto, int estoqueId)
        {
            var estoqueColetado = await _dbContext.Estoque.FindAsync(estoqueId);

            if (estoqueColetado == null)
                throw new Exception($"Estoque para o ID: {estoqueId} não foi encontrado no banco de dados, atualização não realizada.");

            _mapper.Map(estoqueDto, estoqueColetado); 

            _dbContext.Estoque.Update(estoqueColetado);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MovimentacaoEstoqueDto>(estoqueColetado);
        }

        public async Task<bool> Apagar(int estoqueId)
        {
            var estoqueColetado = await _dbContext.Estoque.FindAsync(estoqueId);

            if (estoqueColetado == null)
                throw new Exception($"Estoque para o ID: {estoqueId} não foi encontrado no banco de dados.");

            _dbContext.Estoque.Remove(estoqueColetado);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
