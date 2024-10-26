using System.ComponentModel.DataAnnotations;

namespace GestaoEstoque_API.Application.Dtos.Produto
{
    public class RequestProdutoDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public bool Ativo { get; set; }
        public int CategoriaId { get; set; } 
        public int FornecedorId { get; set; }
    }

    public class ProdutoResponseDto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public bool Ativo { get; set; }
        public string CategoriaNome { get; set; }
        public string FornecedorNome { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
