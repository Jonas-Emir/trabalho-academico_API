namespace GestaoEstoque_API.Application.Dtos.Movimentacao
{
    public class MovimentacaoEstoqueDto
    {
        public int MovimentacaoEstoqueId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataMovimento { get; set; }
        public string TipoMovimentacao { get; set; }
        public string Observacao { get; set; }
    }
}
