
namespace MonitoraEstoqueServico.Models
{
    public class ConfiguracaoModel
    {
        public TimeSpan IntervaloExecucoes { get; set; }
        public string? StrFormatoIntervalo { get; set; }
        public string? EmailNotificacao { get; set; }
        public int QuantidadeEstoqueAlto { get; set; }
        public int QuantidadeEstoqueBaixo { get; set; }
    }
}
