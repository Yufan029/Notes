using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigService
{
    internal class LayeredConfigReader : ILayeredConfigReader
    {

        private readonly IEnumerable<IConfigService> configServices;

        public LayeredConfigReader(IEnumerable<IConfigService> configServices)
        {
            this.configServices = configServices;
        }

        public string? GetValue(string key)
        {
            var result = string.Empty;
            foreach (var configService in this.configServices)
            {
                if (!string.IsNullOrEmpty(configService.GetValue(key)))
                {
                    result = configService.GetValue(key);
                }
            }

            return result;
        }
    }
}
