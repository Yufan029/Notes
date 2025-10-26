using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigSettings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 第一种方式，直接取
            /*
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("config.json", optional:true, reloadOnChange:true);

            IConfigurationRoot configRoot = configBuilder.Build();
            var name = configRoot["name"];
            Console.WriteLine(name);

            string addr = configRoot.GetSection("proxy:address").Value;
            Console.WriteLine(addr);

            var proxy = configRoot.GetSection("proxy").Get<Proxy>();
            Console.WriteLine($"proxy, address= {proxy.Address}, port={proxy.Port}");

            var config = configRoot.Get<Config>();
            Console.WriteLine($"config name = {config.Name}, age = {config.Age}, address = {config.Proxy.Address}");
            */

            // 第二种方式，用options，在DI container 里注册options
            /*
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json", optional: true, reloadOnChange: true);
            var configRoot = builder.Build();

            ServiceCollection services = new ServiceCollection();

            // 先注册DI 对 Options 的支持
            // 把 config 对象，绑定到 configRoot, 
            // binding proxy section in configRoot to proxy
            services.AddOptions()
                .Configure<Config>(config => configRoot.Bind(config))
                .Configure<Proxy>(proxy => configRoot.GetSection("proxy").Bind(proxy));

            services.AddScoped<TestController>();
            services.AddScoped<ProxyTest>();

            using (var sp = services.BuildServiceProvider())
            {
                while (true)
                {
                    using (var scope = sp.CreateScope())
                    {
                        var testController = scope.ServiceProvider.GetRequiredService<TestController>();
                        var proxyTester = scope.ServiceProvider.GetRequiredService<ProxyTest>();

                        testController.Test();
                        Console.WriteLine("Let's change the age to identify the config file will refresh at next scope, since IOptionsSnapshot");

                        // read the key to continue, give enough time to change the age in the config file.
                        Console.ReadKey();

                        // Check the age after change the config file, should be the same, will change in the next scope.
                        testController.Test();
                        proxyTester.Test();

                        Console.WriteLine("press any key to get into next SCOPE");
                        Console.ReadKey();
                    }
                }
            }
            */

            // 第三种方式，commandline 传参数，参数扁平化处理
            // ConfigSettings.exe age=18 proxy:address="kasjlkjf" proxy:Port=23 proxy:ids:0=234 proxy:ids:2=54 proxy:ids:1=99
            // the parameter can be passed in visual studio project properties debug tab as well.
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);

            // or get all the values from environment variables
            // Install-Package Microsoft.Extensions.Configuration.EnvironmentVariables
            // Also can pass a prefix in order to avoid conflicts with other applications.
            builder.AddEnvironmentVariables("ConfigSetting_");

            var configRoot = builder.Build();

            ServiceCollection services = new ServiceCollection();

            services.AddOptions()
                .Configure<Config>(config => configRoot.Bind(config))
                .Configure<Proxy>(proxy => configRoot.GetSection("proxy").Bind(proxy));

            services.AddScoped<TestController>();
            services.AddScoped<ProxyTest>();

            var sp = services.BuildServiceProvider();
            var testController = sp.GetRequiredService<TestController>();
            var proxyTester = sp.GetRequiredService<ProxyTest>();

            testController.Test();
            proxyTester.Test();
        }

    }
}
