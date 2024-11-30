using MonitoraEstoqueServico.Dtos;
using System.Text.Json;

namespace ServiceMonitor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7108");
        }

        public async Task<List<ProdutoResponseDto>> ListarProdutosAsync()
        {
            var response = await _httpClient.GetAsync("/api/produto/listarprodutos");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProdutoResponseDto>>(json);
            }
            else
            {
                throw new Exception($"Erro ao listar produtos: {response.ReasonPhrase}");
            }
        }
    }
}
