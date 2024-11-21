using AutoMapper;
using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Domain.Entities;

namespace GestaoEstoque_API.Application.Mappers
{
    public class CategoriaMapper : Profile
    {
        public CategoriaMapper()
        {
            CreateMap<Produto, ProdutoResponseDto>();
            CreateMap<CategoriaResponseDto, Categoria>();
            CreateMap<ProdutoResponseDto, Produto>();
        }
    }
}
