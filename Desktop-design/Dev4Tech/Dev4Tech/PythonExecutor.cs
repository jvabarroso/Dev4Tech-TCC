using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PythonExecutor
{
    private readonly HttpClient client;
    private readonly string apiUrl = "http://127.0.0.1:8000/converter/pdf";

    public PythonExecutor()
    {
        client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(5);
    }

    public bool VerificarPython()
    {
        try
        {
            var response = client.GetAsync("http://127.0.0.1:8000/").Result;
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar API Python: {ex.Message}");
            return false;
        }
    }

    // ✅ MÉTODO PRINCIPAL: Sempre gera hash para o nome do arquivo
    public async Task<string> ConverterParaPdfAsync(string caminhoArquivo, string pastaDestino)
    {
        try
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

            // ✅ SEMPRE gerar hash aleatório para o nome do arquivo PDF
            string hashArquivo = Guid.NewGuid().ToString() + ".pdf";
            string caminhoDestinoFinal = Path.Combine(pastaDestino, hashArquivo);

            using (var form = new MultipartFormDataContent())
            {
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
                {
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                    form.Add(fileContent, "file", Path.GetFileName(caminhoArquivo));

                    Console.WriteLine($"Enviando arquivo para conversão: {caminhoArquivo}");

                    var response = await client.PostAsync(apiUrl, form);
                    var responseString = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Resposta da API: {responseString}");

                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"Erro na conversão: {responseString}");

                    var json = JObject.Parse(responseString);

                    bool sucesso = json["sucesso"]?.ToObject<bool>() ?? false;
                    if (!sucesso)
                    {
                        string mensagemErro = json["mensagem"]?.ToString() ?? "Erro desconhecido";
                        throw new Exception($"Conversão falhou: {mensagemErro}");
                    }

                    string fileId = json["arquivo_id"]?.ToString();
                    if (string.IsNullOrEmpty(fileId))
                        throw new Exception("ID do arquivo não retornado pela API");

                    // Fazer download do arquivo convertido
                    string downloadUrl = $"http://127.0.0.1:8000/download/{fileId}";
                    Console.WriteLine($"Fazendo download do arquivo: {downloadUrl}");

                    var downloadResponse = await client.GetAsync(downloadUrl);

                    if (!downloadResponse.IsSuccessStatusCode)
                        throw new Exception($"Erro ao baixar arquivo convertido: {downloadResponse.StatusCode}");

                    // ✅ Salvar com o nome do hash
                    using (var pdfStream = await downloadResponse.Content.ReadAsStreamAsync())
                    using (var fileDestino = new FileStream(caminhoDestinoFinal, FileMode.Create, FileAccess.Write))
                    {
                        await pdfStream.CopyToAsync(fileDestino);
                    }

                    Console.WriteLine($"Arquivo salvo com hash: {hashArquivo}");
                    return hashArquivo; // ✅ Retorna apenas o nome com hash
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro detalhado na conversão: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }

            throw new Exception($"Erro ao converter arquivo: {ex.Message}");
        }
    }
}