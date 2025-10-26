using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigService
{
    internal class FileConfigService : IConfigService
    {
        private readonly string filePath = "temp.ini";

        public string? GetValue(string key)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            var result = File
                .ReadAllLines(filePath)
                .Select(line => line.Split("="))
                .Select(str => new { Key = str[0], Value = str[1] })
                .Where(x => x.Key == key)
                .SingleOrDefault();

            return result?.Value;
        }
    }
}
