using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Application.Mappers
{
    public class FornecedorMapper : Profile
    {
            public FornecedorMapper()
            {
                CreateMap<Fornecedor, FornecedorResponseDto>();
                CreateMap<FornecedorResponseDto, Fornecedor>();
                CreateMap<FornecedorRequestDto, Fornecedor>();
                CreateMap<Fornecedor, FornecedorRequestDto>();
            }
    }
}
