using GestaoEstoque_API.Application.Enums;

namespace GestaoEstoque_API.Application.Dtos
{
    public class EstoqueDto
    {
        public int EstoqueId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public TipoMovimento TipoMovimento { get; set; }

        public string TipoMovimentoDescricao => TipoMovimento.ToString();
    }
}
