using MailService;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        public async static Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLoggerService();
            serviceCollection.AddConfigService();
            serviceCollection.AddMailService();
            using (var sp = serviceCollection.BuildServiceProvider())
            {
                var mailService = sp.GetRequiredService<IMailService>();

                mailService.Send("this is email content");
            }

                
        }

        public static async Task<int> GetUrlLengthAsync()
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine("Running " + Thread.CurrentThread.ManagedThreadId);
                var html = await client.GetStringAsync("http://www.google.com");
                var sb = new StringBuilder();
                for (int i = 0; i < 500; i++)
                {
                    sb.Append("xxx");
                    await File.WriteAllTextAsync(@"c:\yufan\temp\1.txt", sb.ToString());
                }
                return html.Length;
            }
        }
    }
}
