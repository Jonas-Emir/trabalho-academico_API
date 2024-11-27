using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GestaoEstoque_API.Application.Enums;

namespace GestaoEstoque_API.Application.Domain.Entities
{
    public class Estoque
    {
        [Key]
        public int EstoqueId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public TipoMovimento Id_Tipo_Movimento { get; set; }

    }
}
