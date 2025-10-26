typeof(FileStream).Assemble.Location

- check the location of FileStream in different Assembly (.net framework, .net core, .net standard)

反编译

- ILSpy

ThreadPool.QueueUserWorkItem 在线程池执行

反编译后，知道 async await 是被编译器拆分成一个个小部分，在 switch 语句中调用。

查看异步运行前后的线程 Id, 如果异步程序运行时长，线程很有可能切换。之前的线程返回线程池，异步完成后，分配新的线程 from threadpool。

Occam's Razor

- 如无必要，勿增实体

IConfigReader contain IEnumerable<IConfigService> configServices, then based on the register order to get the configured values, like environment first, config file second, if there's no enviorment variable then getting into the config file to get the value, if both of them don't have, then reture null.

Configuration reading:

1. Configuration settings

- Microsoft.Extensions.Configuratin
- Microsoft.Extensions.Configuration.Json

var configBuilder = new ConfigurationBuilder();
configBuilder.AddJson(...)

var config = configBuilder.Build();
config["name"]
config.Get<T>()

2.

- Microsoft.Extensions.Configuratin
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Options
- Microsoft.Extensions.Configuration.Binder - 把配置项绑定成类
  Use with DI.

  IOptions<T>,
  IOptionsMonitor<T>,
  IOptionsSnapshot<T> （同一范围内保持一致，例如 asp.net 的一个 request 里)， Recommend
