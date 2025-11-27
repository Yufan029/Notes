using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly string ApiKey = "your-openai-api-key-here";
    private static readonly string ApiEndpoint = "https://api.openai.com/v1/chat/completions";
    private static readonly HttpClient client = new HttpClient();
    private static readonly string SystemRole = "你是一个有帮助的 AI 助手，用中文回答所有问题。";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== .NET AI 助手 ===");
        Console.WriteLine("输入你的问题 (输入 'exit' 退出):\n");

        while (true)
        {
            Console.Write("你: ");
            string question = Console.ReadLine();

            if (question.ToLower() == "exit")
                break;

            if (string.IsNullOrWhiteSpace(question))
                continue;

            var answer = await GetAIAnswer(question);
            Console.WriteLine($"AI: {answer}\n");
        }
    }

    static async Task<string> GetAIAnswer(string question)
    {
        try
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new object[]
                {
                    new { role = "system", content = SystemRole },
                    new { role = "user", content = question }
                },
                temperature = 0.7,
                max_tokens = 1000
            };

            string jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

            HttpResponseMessage response = await client.PostAsync(ApiEndpoint, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"错误: {response.StatusCode} - {responseContent}";
            }

            var jsonDoc = JsonDocument.Parse(responseContent);
            var answer = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return answer;
        }
        catch (Exception ex)
        {
            return $"异常: {ex.Message}";
        }
    }
}
