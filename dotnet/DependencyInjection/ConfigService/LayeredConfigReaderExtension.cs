using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigService
{
    public static class LayeredConfigReaderExtension
    {
        public static void AddLayeredConfigReader(this ServiceCollection services)
        {
            services.AddScoped<ILayeredConfigReader, LayeredConfigReader>();
        }
    }
}
