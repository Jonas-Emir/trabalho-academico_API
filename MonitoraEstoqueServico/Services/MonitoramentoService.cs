
using MonitoraEstoqueServico.Dtos;
using MonitoraEstoqueServico.Models;
using MonitoraEstoqueServico.Services;
using System.Text.Json;
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

        public MonitoramentoService()
        {
        }

        public async void ExecutarServico(object sender, ElapsedEventArgs e)
        {
            try
            {
                var produtos = await _apiService.ListarQuantidadeTotalPorProduto();
                RegistrarQuantidadeTotalProdutos(produtos);

                await VerificarBaixaAltaQuantidade(produtos);
                RegistrarLog("Verificação e notificação concluída com sucesso!");
                RegistrarLog("");
            }
            catch (Exception ex)
            {
                RegistrarLog($"Erro ao executar serviço: {ex.Message}");
            }
        }

        private async Task VerificarBaixaAltaQuantidade(List<ProdutoResponseDto> produtos)
        {
            var configuracoesBaseEstoque = CarregarConfiguracoes();
            var produtosBaixaQuantidade = produtos.Where(p => p.QuantidadeEstoque < configuracoesBaseEstoque.QuantidadeEstoqueBaixo).ToList();
            var produtosAltaQuantidade = produtos.Where(p => p.QuantidadeEstoque > configuracoesBaseEstoque.QuantidadeEstoqueAlto).ToList();
            var corpoEmail = MontarCorpoEmail(produtosBaixaQuantidade, produtosAltaQuantidade);

            if (!string.IsNullOrEmpty(corpoEmail))
                await EnviarEmailAsync("Relatório de Quantidades no Estoque", corpoEmail);
        }

        private string MontarCorpoEmail(List<ProdutoResponseDto> produtosBaixa, List<ProdutoResponseDto> produtosAlta)
        {
            var builder = new System.Text.StringBuilder();

            if (produtosBaixa.Any())
            {
                builder.AppendLine("### Produtos com Baixa Quantidade ###");
                foreach (var produto in produtosBaixa)
                {
                    builder.AppendLine($"- {produto.Nome}: {produto.QuantidadeEstoque} unidades");
                }
                builder.AppendLine();
            }

            if (produtosAlta.Any())
            {
                builder.AppendLine("### Produtos com Alta Quantidade ###");
                foreach (var produto in produtosAlta)
                {
                    builder.AppendLine($"- {produto.Nome}: {produto.QuantidadeEstoque} unidades");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private async Task EnviarEmailAsync(string assunto, string mensagem)
        {
            try
            {
                // Implementação do envio de email faltante, falta realizar o método para fazer o envio de email passando o assunto e a mensagem

                string mensagemLog = mensagem.Length > 1000 ? mensagem.Substring(0, 1000) + "..." : mensagem;
                RegistrarLog($"Email enviado com sucesso: Assunto: '{assunto}', Mensagem: '{mensagemLog}'");
            }
            catch (Exception ex)
            {
                RegistrarLog($"Erro ao enviar email: {ex.Message}");
            }
        }

        private void RegistrarQuantidadeTotalProdutos(List<ProdutoResponseDto> produtos)
        {
            var totalProdutos = produtos.Count;
            RegistrarLog($"Total de produtos no estoque: {totalProdutos}");
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

        private ConfiguracaoModel CarregarConfiguracoes(bool preencherCampos = false)
        {
            try
            {
                string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracao.json");

                if (!File.Exists(caminhoArquivo))
                {
                    MessageBox.Show("Arquivo de configuração não encontrado.");
                    return null;
                }

                string json = File.ReadAllText(caminhoArquivo);
                ConfiguracaoModel configuracao = JsonSerializer.Deserialize<ConfiguracaoModel>(json);

                return configuracao;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o arquivo de configuração: " + ex.Message);
                return null;
            }
        }
    }
}
