using MonitoraEstoqueServico.Models;
using MonitoraEstoqueServico.Services;
using System.Text.Json;

namespace MonitoraEstoqueServico
{
    public partial class MainForm : Form
    {
        private ConfiguracaoModel _configuracao;
        private System.Timers.Timer _timer;
        private readonly ConfiguracaoService _configuracaoService = new ConfiguracaoService();
        private readonly MonitoramentoService _monitoramentoService = new MonitoramentoService();

        public MainForm()
        {
            InitializeComponent();
            CarregarConfiguracoes();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var configuracao = new ConfiguracaoModel
            {
                IntervaloExecucoes = (int)nrIntervalo.Value,
                StrFormatoIntervalo = strFormatoIntervalo.Text,
                EmailNotificacao = txtNotificaEmail.Text,
                QuantidadeEstoqueAlto = (int)nrEstoqueAlto.Value,
                QuantidadeEstoqueBaixo = (int)nrEstoqueBaixo.Value
            };

            string json = JsonSerializer.Serialize(configuracao, new JsonSerializerOptions { WriteIndented = true });

            string caminhoDiretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoArquivo = Path.Combine(caminhoDiretorio, "configuracao.json");
            File.WriteAllText(caminhoArquivo, json);

            MessageBox.Show("Configurações salvas com sucesso!");
        }

        private void lnkLogs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string caminhoLogs = _monitoramentoService.CriarPastaLogs();
            System.Diagnostics.Process.Start("explorer.exe", caminhoLogs);
        }

        private void CarregarConfiguracoes()
        {
            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracao.json");

            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText(caminhoArquivo);
                ConfiguracaoModel configuracao = JsonSerializer.Deserialize<ConfiguracaoModel>(json);

                nrIntervalo.Value = configuracao.IntervaloExecucoes;
                strFormatoIntervalo.Text = configuracao.StrFormatoIntervalo ?? string.Empty;
                txtNotificaEmail.Text = configuracao.EmailNotificacao;
                nrEstoqueAlto.Value = configuracao.QuantidadeEstoqueAlto;
                nrEstoqueBaixo.Value = configuracao.QuantidadeEstoqueBaixo;
            }
            else
                MessageBox.Show("Arquivo de configuração não encontrado. Salve as configurações para criar um novo arquivo.");
        }
    }
}
