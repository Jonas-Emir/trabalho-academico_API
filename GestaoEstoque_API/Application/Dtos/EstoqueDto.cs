

using GestaoEstoque_API.Application.Enums;

namespace GestaoEstoque_API.Application.Dtos
{
    public class EstoqueRequestDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public TipoMovimento TipoMovimentoId { get; set; }
    }

    public class EstoqueResponseDto
    {
        public int EstoqueId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
    }

    public class ProdutoEstoqueResponseDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
    }

    public class QuantidadePorTipoMovimentoResponseDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public Dictionary<string, int> QuantidadePorTipoMovimento { get; set; }
    }

    public class ProdutoQuantidadeDto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}
