using AutoMapper;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Domain.Entities;
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
            var categorias = _dbContext.Categorias
                .Include(c => c.Produtos) 
                .ToList();

            return _mapper.Map<List<CategoriaResponseDto>>(categorias);
        }

        public CategoriaResponseDto BuscarPorId(int categoriaId)
        {
            var categoria = _dbContext.Categorias
                .Include(c => c.Produtos) 
                .FirstOrDefault(c => c.CategoriaId == categoriaId);

            if (categoria == null)
                return null;

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        public async Task<RequestCategoriaDto> Adicionar(RequestCategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            await _dbContext.Categorias.AddAsync(categoria);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RequestCategoriaDto>(categoria);
        }

        public async Task<RequestCategoriaDto> Atualizar(RequestCategoriaDto categoriaDto, int categoriaId)
        {
            var categoriaExistente = await _dbContext.Categorias.FindAsync(categoriaId);

            if (categoriaExistente == null)
                throw new Exception($"Categoria para o ID: {categoriaId} não foi encontrada no banco de dados, atualização não realizada.");

            _mapper.Map(categoriaDto, categoriaExistente);

            _dbContext.Categorias.Update(categoriaExistente);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RequestCategoriaDto>(categoriaExistente);
        }

        public bool Apagar(int categoriaId)
        {
            var categoriaExistente = _dbContext.Categorias.Find(categoriaId);

            if (categoriaExistente == null)
                throw new Exception($"Categoria para o ID: {categoriaId} não foi encontrada no banco de dados.");

            _dbContext.Categorias.Remove(categoriaExistente);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
