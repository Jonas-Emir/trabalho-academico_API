using MonitoraEstoqueServico.Dtos;
using System.Text.Json;

namespace ServiceMonitor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly MonitoramentoService _monitoramentoService;
        private HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7108");
        }

        public async Task<List<ProdutoResponseDto>> ListarQuantidadeTotalPorProduto()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/estoque/buscar-quantidade-estoque-total");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<ProdutoResponseDto>>(json);
                }
                else
                {
                    var errorMessage = $"Erro ao listar produtos. Status: {response.StatusCode}, Motivo: {response.ReasonPhrase}";
                    _monitoramentoService.RegistrarLog(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                MonitoramentoService monitoramentoService = new MonitoramentoService();
                var errorMessage = $"Conexão com a API -> {ex.Message}";
                throw new Exception(errorMessage, ex);
            }
        }
    }
}
