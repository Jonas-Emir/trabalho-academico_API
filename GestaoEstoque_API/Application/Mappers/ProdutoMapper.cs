using AutoMapper;
using GestaoEstoque_API.Application.Domain.Entities;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Application.Mappers
{
    public class ProdutoMapper : Profile
    {
        public ProdutoMapper()
        {
            CreateMap<Produto, ProdutoResponseDto>();
            CreateMap<RequestProdutoDto, Produto>();
            CreateMap<Produto, RequestProdutoDto>();
        }
    }
}
