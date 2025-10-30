using System.Text.Json;
using System.Text.Json.Nodes;

namespace CalculateDuration
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            using var stream = File.OpenRead("time.json");
            var list = await JsonSerializer.DeserializeAsync<IEnumerable<VideoInfo>>(stream);

            var duration = 0;
            foreach (var info in list)
            {
                if (info.Cid < 359478081)
                {

                    duration += info.Duration;
                }
            }

            Console.WriteLine(duration / 3600.0);
        }
    }
}
