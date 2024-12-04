using MonitoraEstoqueServico.Dtos;
using MonitoraEstoqueServico.Models;
using MonitoraEstoqueServico.Services;
using System.Net.Mail;
using System.Net;
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


        private string MontarCorpoEmailOficial(List<ProdutoResponseDto> produtosBaixa, List<ProdutoResponseDto> produtosAlta)
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
            }

            return builder.ToString();
        }

        private string MontarCorpoEmail(List<ProdutoResponseDto> produtosBaixa, List<ProdutoResponseDto> produtosAlta)
        {
            var builder = new System.Text.StringBuilder();

            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<style>");
            builder.AppendLine("body { font-family: 'Arial', sans-serif; background-color: #f4f7fb; margin: 0; padding: 0; }");
            builder.AppendLine("h2 { color: #333366; text-align: center; margin-top: 30px; font-size: 24px; }");
            builder.AppendLine("h3 { color: #004d99; margin-top: 20px; font-size: 20px; }");
            builder.AppendLine("table { width: 100%; border-spacing: 0; margin-top: 20px; border-radius: 8px; overflow: hidden; }");
            builder.AppendLine("th, td { padding: 12px 15px; text-align: left; border: 1px solid #ddd; }");
            builder.AppendLine("th { background-color: #00796b; color: white; font-weight: bold; text-transform: uppercase; }");
            builder.AppendLine("tr:nth-child(even) { background-color: #fafafa; }");
            builder.AppendLine("tr:hover { background-color: #e0f2f1; }");
            builder.AppendLine("p { font-size: 14px; color: #555; margin-top: 20px; text-align: center; }");
            builder.AppendLine(".footer { text-align: center; font-size: 12px; color: #888; margin-top: 20px; padding: 10px; }");
            builder.AppendLine("</style>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");

            builder.AppendLine("<h2>Relatório de Estoque - Sistema de Monitoramento</h2>");

            if (produtosBaixa.Any())
            {
                builder.AppendLine("<h3>Produtos com Baixa Quantidade</h3>");
                builder.AppendLine("<table>");
                builder.AppendLine("<tr><th>Produto</th><th>Quantidade em Estoque</th></tr>");
                foreach (var produto in produtosBaixa)
                {
                    builder.AppendLine($"<tr><td>{produto.Nome}</td><td>{produto.QuantidadeEstoque}</td></tr>");
                }
                builder.AppendLine("</table>");
            }
            else
            {
                builder.AppendLine("<h3>Não há produtos com baixa quantidade no estoque.</h3>");
            }

            if (produtosAlta.Any())
            {
                builder.AppendLine("<h3>Produtos com Alta Quantidade</h3>");
                builder.AppendLine("<table>");
                builder.AppendLine("<tr><th>Produto</th><th>Quantidade em Estoque</th></tr>");
                foreach (var produto in produtosAlta)
                {
                    builder.AppendLine($"<tr><td>{produto.Nome}</td><td>{produto.QuantidadeEstoque}</td></tr>");
                }
                builder.AppendLine("</table>");
            }
            else
            {
                builder.AppendLine("<h3>Não há produtos com alta quantidade no estoque.</h3>");
            }

            builder.AppendLine("<p>Este relatório foi gerado automaticamente pelo sistema de monitoramento de estoque.</p>");
            builder.AppendLine("<p class='footer'>Atenciosamente, <br>Equipe de Gestão de Estoque</p>");

            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }




        public async Task EnviarEmailAsync(string assunto, string mensagem, string destinatarioEmail)
        {
            try
            {
                string servidorSMTP = "smtp.gmail.com";
                int portaSMTP = 587;
                string emailDeEnvio = "";
                string senhaDeEnvio = "";
                SmtpClient clienteSMTP = new SmtpClient()
                {
                    Port = portaSMTP,
                    Host = servidorSMTP,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailDeEnvio, senhaDeEnvio),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network

                };

                MailMessage mensagemEmail = new MailMessage
                {
                    From = new MailAddress(emailDeEnvio),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };

                mensagemEmail.To.Add(destinatarioEmail);
                await clienteSMTP.SendMailAsync(mensagemEmail);

                RegistrarLog($"Email enviado com sucesso com '{assunto} para o destinatário: '{destinatarioEmail}'");
            }
            catch (Exception ex)
            {
                RegistrarLog($"Falha ao enviar e-mail. Assunto: '{assunto}' | Destinatário: '{destinatarioEmail}' | Erro: {ex.Message}");
            }
        }

        private void RegistrarQuantidadeTotalProdutos(List<ProdutoResponseDto> produtos)
        {
            var totalProdutos = produtos.Count;
            RegistrarLog($"Total de produtos no estoque: {totalProdutos}");
        }
        private async Task VerificarBaixaAltaQuantidade(List<ProdutoResponseDto> produtos)
        {
            var configuracoesBaseEstoque = CarregarConfiguracoes();
            var produtosBaixaQuantidade = produtos.Where(p => p.QuantidadeEstoque < configuracoesBaseEstoque.QuantidadeEstoqueBaixo).ToList();
            var produtosAltaQuantidade = produtos.Where(p => p.QuantidadeEstoque > configuracoesBaseEstoque.QuantidadeEstoqueAlto).ToList();

            RegistrarLog($"Verificação de Estoque: {produtosBaixaQuantidade.Count} produtos com estoque baixo e {produtosAltaQuantidade.Count} produtos com estoque alto.");

            var corpoEmail = MontarCorpoEmail(produtosBaixaQuantidade, produtosAltaQuantidade);

            if (!string.IsNullOrEmpty(corpoEmail))
                await EnviarEmailAsync("Relatório de Quantidades no Estoque", corpoEmail, configuracoesBaseEstoque.EmailNotificacao);
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
