using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoraEstoqueServico.Services
{
    public class MonitoramentoService
    {
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
