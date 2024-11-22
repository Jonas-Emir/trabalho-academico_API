using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<EstoqueDto>> BuscarEstoques()
        {
            var estoques = await _dbContext.Estoque
                .Include(x => x.Produto) 
                .Include(x => x.Id_Tipo_Movimento) 
                .ToListAsync();

            return _mapper.Map<List<EstoqueDto>>(estoques);
        }

        public async Task<EstoqueDto> BuscarPorId(int estoqueId)
        {
            var estoque = await _dbContext.Estoque
                .Include(x => x.Produto)  
                .Include(x => x.Id_Tipo_Movimento)  
                .FirstOrDefaultAsync(x => x.EstoqueId == estoqueId);

            if (estoque == null)
                return null;

            return _mapper.Map<EstoqueDto>(estoque);
        }

        public async Task<EstoqueDto> Adicionar(EstoqueDto estoqueDto)
        {
            var estoque = _mapper.Map<Estoque>(estoqueDto);

            await _dbContext.Estoque.AddAsync(estoque);
            await _dbContext.SaveChangesAsync(); 

            return _mapper.Map<EstoqueDto>(estoque);
        }

        public async Task<EstoqueDto> Atualizar(EstoqueDto estoqueDto, int estoqueId)
        {
            var estoqueColetado = await _dbContext.Estoque.FindAsync(estoqueId);

            if (estoqueColetado == null)
                throw new Exception($"Estoque com o ID: {estoqueId} não encontrado, atualização não realizada.");

            _mapper.Map(estoqueDto, estoqueColetado);

            _dbContext.Estoque.Update(estoqueColetado);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EstoqueDto>(estoqueColetado);
        }
        public async Task<EstoqueDto> BuscarPorProduto(int produtoId)
        {
            var estoque = await _dbContext.Estoque
                .Include(x => x.Produto)
                .Include(x => x.Id_Tipo_Movimento)
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);

            if (estoque == null)
                return null;

            return _mapper.Map<EstoqueDto>(estoque);
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
    }
}
