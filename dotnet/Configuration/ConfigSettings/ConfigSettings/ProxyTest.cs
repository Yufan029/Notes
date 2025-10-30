using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSettings
{
    public class ProxyTest
    {
        private readonly IOptionsSnapshot<Proxy> configProxy;

        public ProxyTest(IOptionsSnapshot<Proxy> configProxy)
        {
            this.configProxy = configProxy;
        }

        public void Test()
        {
            Console.WriteLine($"Inside proxy config, address={this.configProxy.Value.Address}, " +
                $"port={this.configProxy.Value.Port}, ids=[{string.Join(", ", this.configProxy.Value.Ids)}]");
        }
    }
}
