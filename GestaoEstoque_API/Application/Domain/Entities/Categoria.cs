using System.ComponentModel.DataAnnotations;

namespace GestaoEstoque_API.Application.Domain.Entities
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [MaxLength(250)]
        public string Descricao { get; set; }

        public List<Produto> Produtos { get; set; }
    }
}
