using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class PythonExecutor
{
    private readonly HttpClient client;
    private readonly string apiUrl = "http://10.239.0.182:8000/converter/pdf";

    public PythonExecutor()
    {
        client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(5);
    }

    public bool VerificarPython()
    {
        try
        {
            var response = client.GetAsync("http://10.239.0.182:8000/").Result;
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar API Python: {ex.Message}");
            return false;
        }
    }

    // ✅ MÉTODO PRINCIPAL: Agora apenas obtém o file_id sem salvar novamente
    public async Task<string> ConverterParaPdfAsync(string caminhoArquivo, string pastaDestino)
    {
        try
        {
            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo não encontrado.", caminhoArquivo);

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

                    // ✅ AGORA: Apenas retornar o file_id para uso no banco de dados
                    // A API já salvou o arquivo com nome {fileId}.pdf na pasta BASE_DIR
                    // Não precisamos salvar novamente!

                    Console.WriteLine($"Arquivo processado pela API com ID: {fileId}");
                    return fileId + ".pdf"; // ✅ Retorna o nome do arquivo que a API já salvou
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