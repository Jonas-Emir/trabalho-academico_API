using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonitoraEstoqueServico.Dtos
{
    public class ProdutoResponseDto
    {
        [JsonPropertyName("produtoId")]
        public int ProdutoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("quantidadeEstoque")]
        public decimal QuantidadeEstoque { get; set; }

    }
}
