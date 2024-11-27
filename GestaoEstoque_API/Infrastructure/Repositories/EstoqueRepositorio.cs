using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
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
            var estoque = _mapper.Map<Estoque>(estoqueDto);

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
        public async Task<EstoqueResponseDto> BuscarPorProduto(int produtoId)
        {
            var estoque = await _dbContext.Estoque
                .Include(x => x.Produto)
                .Include(x => x.Id_Tipo_Movimento)
                .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);

            if (estoque == null)
                return null;

            return _mapper.Map<EstoqueResponseDto>(estoque);
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
