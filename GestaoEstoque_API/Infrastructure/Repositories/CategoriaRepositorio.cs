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
            try
            {
                var categoriaExistente = await VerificarCategoriaExistente(categoriaDto.Nome);
                if (categoriaExistente)
                    throw new Exception($"A categoria com o nome '{categoriaDto.Nome}' já está cadastrada.");

                await _dbContext.Categoria.AddAsync(categoria);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
            return _mapper.Map<CategoriaRequestDto>(categoria);
        }

        public async Task<CategoriaRequestDto> Atualizar(CategoriaRequestDto categoriaDto, int categoriaId)
        {
            var categoriaExistente = await _dbContext.Categoria.FindAsync(categoriaId);
            var categoriaRepetida = await VerificarCategoriaExistente(categoriaDto.Nome);

            if (categoriaRepetida)
                throw new Exception($"A categoria com o nome '{categoriaDto.Nome}' já está cadastrada.");

            _mapper.Map(categoriaDto, categoriaExistente);

            _dbContext.Categoria.Update(categoriaExistente);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoriaRequestDto>(categoriaExistente);
        }

        public string Apagar(int categoriaId)
        {
            var categoriaExistente = _dbContext.Categoria.Find(categoriaId);
            if (categoriaExistente == null)
                return $"Categoria com o ID {categoriaId} não foi encontrada.";

            var produtosVinculados = VerificarProdutosVinculadosPorCategoria(categoriaId);

            if (produtosVinculados.Any())
            {
                var produtos = string.Join(", ", produtosVinculados);
                return $"Não é possível excluir a categoria com ID {categoriaId}. Produtos vinculados: {produtos}.";
            }

            _dbContext.Categoria.Remove(categoriaExistente);
            _dbContext.SaveChanges();

            return $"Categoria com o ID {categoriaId} foi apagada com sucesso.";
        }

        #region Métodos auxiliares
        private async Task<bool> VerificarCategoriaExistente(string nomeCategoria, int? categoriaId = null)
        {
            return await _dbContext.Categoria
                .Where(c => c.Nome == nomeCategoria && (categoriaId == null || c.CategoriaId != categoriaId))
                .AnyAsync();
        }

        private List<string> VerificarProdutosVinculadosPorCategoria(int categoriaId)
        {
            var produtosVinculados = _dbContext.Produto
                .Where(p => p.CategoriaId == categoriaId)
                .Select(p => p.Nome)
                .ToList();

            return produtosVinculados;
        }
        #endregion
    }
}
