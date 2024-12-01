using MonitoraEstoqueServico.Dtos.Enums;
using MonitoraEstoqueServico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonitoraEstoqueServico.Services
{
    public class ConfiguracaoService
    {
        public void SalvarConfiguracao(ConfiguracaoModel configuracao)
        {
            string json = JsonSerializer.Serialize(configuracao, new JsonSerializerOptions { WriteIndented = true });
            string caminhoDiretorio = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoArquivo = Path.Combine(caminhoDiretorio, "configuracao.json");
            File.WriteAllText(caminhoArquivo, json);
        }

        public TimeSpan ObterIntervalo(int valor, string tipoFormato)
        {
            if (Enum.TryParse<TipoIntervalo>(tipoFormato, out TipoIntervalo tipoIntervalo))
            {
                switch (tipoIntervalo)
                {
                    case TipoIntervalo.Segundos:
                        return TimeSpan.FromSeconds(valor);
                    case TipoIntervalo.Minutos:
                        return TimeSpan.FromMinutes(valor);
                    case TipoIntervalo.Horas:
                        return TimeSpan.FromHours(valor);
                    default:
                        return TimeSpan.Zero;
                }
            }
            else
            {
                throw new ArgumentException("Formato de intervalo inválido.", nameof(tipoFormato));
            }
        }

        public int ConverterTimeSpanParaInt(TimeSpan intervalo, string tipoFormato)
        {
            if (Enum.TryParse<TipoIntervalo>(tipoFormato, out TipoIntervalo tipoIntervalo))
            {
                switch (tipoIntervalo)
                {
                    case TipoIntervalo.Segundos:
                        return (int)intervalo.TotalSeconds;
                    case TipoIntervalo.Minutos:
                        return (int)intervalo.TotalMinutes;
                    case TipoIntervalo.Horas:
                        return (int)intervalo.TotalHours;
                    default:
                        return 0;
                }
            }
            else
            {
                throw new ArgumentException("Formato de intervalo inválido.", nameof(tipoFormato));
            }
        }

        public int ObterIntervaloConvertido(TimeSpan valor, string tipoFormato)
        {
            if (Enum.TryParse<TipoIntervalo>(tipoFormato, out TipoIntervalo tipoIntervalo))
            {
                return tipoIntervalo switch
                {
                    TipoIntervalo.Minutos => (int)(valor.TotalMinutes * 60 * 1000),
                    TipoIntervalo.Segundos => (int)(valor.TotalSeconds * 1000),
                    TipoIntervalo.Horas => (int)(valor.TotalHours * 60 * 60 * 1000),
                    _ => throw new ArgumentOutOfRangeException(nameof(tipoFormato), tipoFormato, null),
                };
            }
            else
            {
                throw new ArgumentException("Formato de intervalo inválido.", nameof(tipoFormato));
            }
        }
    }
}
