using System.ComponentModel.DataAnnotations;

namespace GestaoEstoque_API.Application.Domain.Entities
{
    public class FornecedorRequestDto
    {
        [Key]
        public int FornecedorId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Endereco { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

    public class FornecedorResponseDto
    {
        public int FornecedorId { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
