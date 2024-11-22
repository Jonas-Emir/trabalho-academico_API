using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoriaRepositorio(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<CategoriaResponseDto> BuscarCategorias()
        {
            var categorias = _dbContext.Categoria
                .Include(c => c.Produtos) 
                .ToList();

            return _mapper.Map<List<CategoriaResponseDto>>(categorias);
        }

        public CategoriaResponseDto BuscarPorId(int categoriaId)
        {
            var categoria = _dbContext.Categoria
                .Include(c => c.Produtos) 
                .FirstOrDefault(c => c.CategoriaId == categoriaId);

            if (categoria == null)
                return null;

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        public async Task<CategoriaRequestDto> Adicionar(CategoriaRequestDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            await _dbContext.Categoria.AddAsync(categoria);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaRequestDto>(categoria);
        }

        public async Task<CategoriaRequestDto> Atualizar(CategoriaRequestDto categoriaDto, int categoriaId)
        {
            var categoriaExistente = await _dbContext.Categoria.FindAsync(categoriaId);

            if (categoriaExistente == null)
                throw new Exception($"Categoria para o ID: {categoriaId} não foi encontrada no banco de dados, atualização não realizada.");

            _mapper.Map(categoriaDto, categoriaExistente);

            _dbContext.Categoria.Update(categoriaExistente);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaRequestDto>(categoriaExistente);
        }

        public bool Apagar(int categoriaId)
        {
            var categoriaExistente = _dbContext.Categoria.Find(categoriaId);

            if (categoriaExistente == null)
                throw new Exception($"Categoria para o ID: {categoriaId} não foi encontrada no banco de dados.");

            _dbContext.Categoria.Remove(categoriaExistente);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
