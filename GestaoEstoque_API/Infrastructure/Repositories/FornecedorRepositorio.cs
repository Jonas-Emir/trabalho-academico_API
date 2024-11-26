using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoEstoque_API.Infrastructure.Repositories
{
    public class FornecedorRepositorio : IFornecedorRepositorio
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public FornecedorRepositorio(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<FornecedorResponseDto> BuscarFornecedores()
        {
            var fornecedores = _dbContext.Fornecedor.ToList();
            return _mapper.Map<List<FornecedorResponseDto>>(fornecedores);
        }

        public FornecedorResponseDto BuscarPorId(int fornecedorId)
        {
            var fornecedor = _dbContext.Fornecedor.Find(fornecedorId);
            if (fornecedor == null)
                return null;

            return _mapper.Map<FornecedorResponseDto>(fornecedor);
        }

        public async Task<FornecedorRequestDto> AdicionarAsync(FornecedorRequestDto fornecedorDto)
        {
            var fornecedorExistente = await VerificarFornecedorExistente(fornecedorDto.CNPJ);
            if (fornecedorExistente)
                throw new Exception($"Já existe um fornecedor cadastrado com o CNPJ '{fornecedorDto.CNPJ}'.");

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            await _dbContext.Fornecedor.AddAsync(fornecedor);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<FornecedorRequestDto>(fornecedor);
        }

        public async Task<FornecedorRequestDto> AtualizarAsync(FornecedorRequestDto fornecedorDto, int fornecedorId)
        {
            var fornecedorExistente = await _dbContext.Fornecedor.FindAsync(fornecedorId);
            if (fornecedorExistente == null)
                throw new Exception($"Fornecedor com ID {fornecedorId} não encontrado.");

            var fornecedorComMesmoCNPJ = await VerificarFornecedorExistente(fornecedorDto.CNPJ, fornecedorId);
            if (fornecedorComMesmoCNPJ)
                throw new Exception($"Já existe um fornecedor cadastrado com o CNPJ '{fornecedorDto.CNPJ}'.");

            _mapper.Map(fornecedorDto, fornecedorExistente);
            _dbContext.Fornecedor.Update(fornecedorExistente);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<FornecedorRequestDto>(fornecedorExistente);
        }

        public string Apagar(int fornecedorId)
        {
            var fornecedorExistente = _dbContext.Fornecedor.Find(fornecedorId);
            if (fornecedorExistente == null)
                return $"Fornecedor com ID {fornecedorId} não foi encontrado.";

            var produtosVinculados = VerificarProdutosVinculadosAoFornecedor(fornecedorId);
            if (produtosVinculados.Any())
            {
                var produtos = string.Join(", ", produtosVinculados);
                return $"Não é possível excluir o fornecedor com ID {fornecedorId}. Produtos vinculados: {produtos}.";
            }

            _dbContext.Fornecedor.Remove(fornecedorExistente);
            _dbContext.SaveChanges();

            return $"Fornecedor com o ID {fornecedorId} foi apagado com sucesso.";
        }


        #region Métodos auxiliares
        private async Task<bool> VerificarFornecedorExistente(string cnpj, int? fornecedorId = null)
        {
            return await _dbContext.Fornecedor
                .Where(f => f.CNPJ == cnpj && (fornecedorId == null || f.FornecedorId != fornecedorId))
                .AnyAsync();
        }

        private List<string> VerificarProdutosVinculadosAoFornecedor(int fornecedorId)
        {
            return _dbContext.Produto
                .Where(p => p.FornecedorId == fornecedorId)
                .Select(p => p.Nome)
                .ToList();
        }

        #endregion
    }
}
