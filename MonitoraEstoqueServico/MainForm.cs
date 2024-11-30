using MonitoraEstoqueServico.Models;
using MonitoraEstoqueServico.Services;
using ServiceMonitor.Services;
using System.Text.Json;
using System.Timers;

namespace MonitoraEstoqueServico
{
    public partial class MainForm : Form
    {
        private ConfiguracaoModel _configuracao;
        private System.Timers.Timer _timer;
        private readonly ConfiguracaoService _configuracaoService;
        private readonly MonitoramentoService _monitoramentoService;

        public MainForm(MonitoramentoService monitoramentoService, ConfiguracaoService configuracaoService)
        {
            _monitoramentoService = monitoramentoService;
            _configuracaoService = configuracaoService;

            InitializeComponent();
            CarregarConfiguracoes(true);
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            IniciarServicoMonitoramento();
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            PararTimer();
            StatusServico("Serviço parado", Color.Red);
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            StatusServico("Reiniciando serviço", Color.Orange);

            PararTimer();
            Task.Delay(1000);

            IniciarServicoMonitoramento();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string logMsg = "SALVANDO TESTE";
            _monitoramentoService.RegistrarLog(logMsg);

            TimeSpan intervaloExecucoes = _configuracaoService.ObterIntervalo((int)nrIntervalo.Value, strFormatoIntervalo.Text);

            var configuracao = new ConfiguracaoModel
            {
                IntervaloExecucoes = intervaloExecucoes,
                StrFormatoIntervalo = strFormatoIntervalo.Text,
                EmailNotificacao = txtNotificaEmail.Text,
                QuantidadeEstoqueAlto = (int)nrEstoqueAlto.Value,
                QuantidadeEstoqueBaixo = (int)nrEstoqueBaixo.Value
            };

            _configuracaoService.SalvarConfiguracao(configuracao);
            MessageBox.Show("Configurações salvas com sucesso!");
        }

        private void lnkLogs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string caminhoLogs = _monitoramentoService.CriarPastaLogs();
            System.Diagnostics.Process.Start("explorer.exe", caminhoLogs);
        }

        private void PararTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        private void ConfigurarTimer()
        {
            if (_configuracao != null)
            {
                int intervaloExecucao = _configuracaoService.ObterIntervaloConvertido((TimeSpan)_configuracao.IntervaloExecucoes, _configuracao.StrFormatoIntervalo);

                _timer = new System.Timers.Timer(intervaloExecucao);
            }
        }

        private void IniciarServicoMonitoramento()
        {
            _configuracao = CarregarConfiguracoes();

            if (_configuracao != null)
            {
                ConfigurarTimer();

                _timer.Elapsed += _monitoramentoService.ExecutarServico;
                _timer.AutoReset = true;
                _timer?.Start();

                _monitoramentoService.RegistrarLog($"Serviço iniciado com o intervalo configurado de {_configuracao.IntervaloExecucoes.TotalMinutes} minutos.");
                StatusServico("Serviço iniciado", Color.Green);
            }
            else
            {
                MessageBox.Show("Não foi possível carregar as configurações.");
            }
        }

        private void StatusServico(string message, Color color)
        {
            txtStatusServico.Text = message;
            txtStatusServico.ForeColor = color;
        }

        private ConfiguracaoModel CarregarConfiguracoes(bool preencherCampos = false)
        {
            try
            {
                string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracao.json");

                if (!File.Exists(caminhoArquivo))
                {
                    MessageBox.Show("Arquivo de configuração não encontrado. Salve as configurações para criar um novo arquivo.");
                    return null;
                }

                string json = File.ReadAllText(caminhoArquivo);
                ConfiguracaoModel configuracao = JsonSerializer.Deserialize<ConfiguracaoModel>(json);

                if (preencherCampos)
                {
                    nrIntervalo.Value = _configuracaoService.ConverterTimeSpanParaInt(configuracao.IntervaloExecucoes, configuracao.StrFormatoIntervalo);
                    strFormatoIntervalo.Text = configuracao.StrFormatoIntervalo ?? string.Empty;
                    txtNotificaEmail.Text = configuracao.EmailNotificacao;
                    nrEstoqueAlto.Value = configuracao.QuantidadeEstoqueAlto;
                    nrEstoqueBaixo.Value = configuracao.QuantidadeEstoqueBaixo;
                }

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
