using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestaoEstoque_API.Application.Enums;

namespace GestaoEstoque_API.Domain.Entities
{
    public class MovimentacaoEstoque
    {
        [Key]
        public int MovimentacaoEstoqueId { get; set; }

        [Required]
        public int ProdutoId { get; set; } 

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; } 

        [Required]
        public int Quantidade { get; set; } 

        [Required]
        public DateTime DataMovimento { get; set; }

        [Required]
        public TipoMovimento Tipo { get; set; }

        public string Observacao { get; set; }
    }
}
