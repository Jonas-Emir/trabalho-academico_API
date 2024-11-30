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
    }
}
