
using MonitoraEstoqueServico.Dtos;
using System.Timers;

namespace ServiceMonitor.Services
{
    public class MonitoramentoService
    {
        private readonly ApiService _apiService;

        public MonitoramentoService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async void ExecutarServico(object sender, ElapsedEventArgs e)
        {

            RegistrarLog("--- Serviço iniciado ---");

            try
            {
                var apiService = new ApiService(new HttpClient());

                var produtos = await apiService.ListarProdutosAsync();

                RegistrarQuantidadeTotalProdutos(produtos);

                await VerificarBaixaQuantidade(produtos);
                //await VerificarProdutosProximosDaValidade(produtos);
                RegistrarLog("Verificação concluida com sucesso!");
                RegistrarLog("");

            }
            catch (Exception ex)
            {
                RegistrarLog($"Erro ao executar serviço: {ex.Message}");
            }
        }
        private void RegistrarQuantidadeTotalProdutos(List<ProdutoResponseDto> produtos)
        {
            var totalProdutos = produtos.Count;
            RegistrarLog($"Total de produtos no estoque: {totalProdutos}");
        }

        private async Task VerificarBaixaQuantidade(List<ProdutoResponseDto> produtos)
        {
            var produtosBaixaQuantidade = produtos.Where(p => p.QuantidadeEstoque < 5).ToList();
            if (produtosBaixaQuantidade.Any())
            {
                foreach (var produto in produtosBaixaQuantidade)
                {
                    RegistrarLog($"Atenção: Produto '{produto.Nome}' está com baixa quantidade: {produto.QuantidadeEstoque}");
                    await EnviarEmailAsync("Alerta de Baixa Quantidade", $"Produto '{produto.Nome}' está com apenas {produto.QuantidadeEstoque} unidades no estoque.");
                }
            }
        }

        //private async Task VerificarProdutosProximosDaValidade(List<ProdutoResponseDto> produtos)
        //{
        //    var produtosProximosValidade = produtos.Where(p => p.DataValidade <= DateTime.Now.AddDays(30)).ToList();
        //    if (produtosProximosValidade.Any())
        //    {
        //        foreach (var produto in produtosProximosValidade)
        //        {
        //            RegistrarLog($"Atenção: Produto '{produto.Nome}' está próximo da validade: {produto.DataValidade.ToShortDateString()}");
        //            // Enviar e-mail de alerta sobre validade
        //            await EnviarEmailAsync("Alerta de Validade", $"Produto '{produto.Nome}' está próximo da validade em {produto.DataValidade.ToShortDateString()}.");
        //        }
        //    }
        //}

        private async Task EnviarEmailAsync(string assunto, string corpo)
        {
            await Task.Run(() =>
            {
                RegistrarLog($"E-mail enviado: {assunto} - {corpo}");
            });
        }

        public string CriarPastaLogs()
        {
            string nomePastaLogs = "Logs";
            string caminhoDiretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoLogs = Path.Combine(caminhoDiretorio, nomePastaLogs);

            if (!Directory.Exists(caminhoLogs))
            {
                Directory.CreateDirectory(caminhoLogs);
                MessageBox.Show("Pasta de log não encontrada. Nova pasta criada com sucesso!");
                return caminhoLogs;
            }

            return caminhoLogs;
        }

        public void RegistrarLog(string mensagem)
        {
            try
            {
                string nomePastaLogs = "Logs";
                string caminhoDiretorio = AppDomain.CurrentDomain.BaseDirectory;
                string caminhoPastaLogs = Path.Combine(caminhoDiretorio, nomePastaLogs);

                if (!Directory.Exists(caminhoPastaLogs))
                    CriarPastaLogs();

                string nomeArquivoLog = $"Log_Monitoramento_{DateTime.Now:dd-MM-yyyy}.txt";
                string caminhoArquivoLog = Path.Combine(caminhoPastaLogs, nomeArquivoLog);
                string logEntrada = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} - {mensagem}{Environment.NewLine}";

                File.AppendAllText(caminhoArquivoLog, logEntrada);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar log: {ex.Message}");
            }
        }
    }
}
