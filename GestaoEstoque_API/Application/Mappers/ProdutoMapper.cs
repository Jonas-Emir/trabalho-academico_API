using AutoMapper;
using GestaoEstoque_API.Application.Dtos.Produto;
using GestaoEstoque_API.Domain.Entities;

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
