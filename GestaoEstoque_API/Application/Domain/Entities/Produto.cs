using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestaoEstoque_API.Application.Domain.Entities
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public bool Ativo { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public int FornecedorId { get; set; }
        [ForeignKey("FornecedorId")]
        public Fornecedor Fornecedor { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public List<Estoque> Estoque { get; set; }
    }
}
