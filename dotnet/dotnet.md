### Project Code ###
  - https://github.com/yangzhongke/NETBookMaterials

### Check the location of FileStream in different Assembly (.net framework, .net core, .net standard) ####
  - typeof(FileStream).Assemble.Location

### 反编译工具 ### 

- ILSpy
- 反编译后，知道 async await 是被编译器拆分成一个个小部分，在 switch 语句中调用。

ThreadPool.QueueUserWorkItem 在线程池执行

查看异步运行前后的线程 Id, 如果异步程序运行时长，线程很有可能切换。之前的线程返回线程池，异步完成后，分配新的线程 from threadpool。

### Occam's Razor ###
- 如无必要，勿增实体

Self-defined ```IConfigReader``` contain ```IEnumerable<IConfigService> configServices```
- Based on the register order to get the configured values, like environment first, config file second.
- If there's no enviorment variable then getting into the config file to get the value.
- If both of them don't have, then reture null.

### Configuration reading: ###
1. Configuration settings, required namespaces 
    - Microsoft.Extensions.Configuratin
    - Microsoft.Extensions.Configuration.Json
```c#
    var configBuilder = new ConfigurationBuilder();
    configBuilder.AddJson(...)

    var config = configBuilder.Build();
    config["name"]
    config.Get<T>()
```

2. Option pattern for configuration, required namespaces
    - Microsoft.Extensions.Configuration
    - Microsoft.Extensions.Configuration.Json
    - Microsoft.Extensions.Options
    - Microsoft.Extensions.Configuration.Binder - 把配置项绑定成类
  
3. How to use with DI.
    - IOptions<T>,
    - IOptionsMonitor<T>,
    - IOptionsSnapshot<T> （同一范围内保持一致，例如 asp.net 的一个 request 里）， Recommended

4. How to 自己写 `confinguration provider`.
   1. create 自己的 provider, 继承 `IConfigurationProvider` OR `FileConfigurationProvider`, 实现 `Load` 方法，从 XML 文件/数据库/JSON 里读取 config file value, 然后将*扁平化*处理 *XXX：YYY:ids:1* 的数据存贮在 Dictionary<string, string> 里，然后赋值 provider 的 Data property.
   2. create 自己的 source, 继承 `IConfigurationSource` OR `FileConfigurationSource`, 实现 `Build` 方法，确定调用 `EnsureDefault(builder)`, 然后 return new SelfCreatedConfigProvider(this) // this 就是 source 自身.
   3. 用的时候把配置文件路径传进去就可以了。
   4. 可以用 `IOptionsSnapshot<XXXConfig>` 来验证。 XXXConfig 就是自定应的类，映射配置文件的格式。

### Logging ###
1. console log, event viewer, Nlog,
2. 集群结构化 log 信息， .net 给 NLog --> Serilog --> Exceptionless.
3. ELK (Elastic Search, Logging, Kibana)
- 普通项目，没有集群, NLog 搞定
- Serilog(机构化日志) + 集群部署(exceptionless)

### 要清楚每个东西的优缺点，如何选择技术方案。 ###
>- 重要的是设计解决方案的能力, 在各种各样的产品中，根据项目的需求，选择一套解决方案。
>- 要明白各个产品的应用场景，明白产品优缺点，明白公司现状，从而在这些纷杂（乱七八糟）的产品中选择一套解决方案，适合公司，适合开发，成本又低，这才是程序员努力方向，才能做到越老越值钱。

### EFCore ###

- 什么是唯一索引，什么是聚合索引， IsUnique(), IsClustered()

- 聚合索引的特点
  - 顺序存贮

- GUID 做主键
  - 不能设置成聚集索引（cluster index）， 因为不连续，
  - 如果设置成聚集索引，那每次插入的时候，它都要重新排序（按照 GUID），效率非常低。

- SQLServer NewSequentialId()是可以生成连续的 GUID
  - 但也不能解决问题，存到数据库里不一定是连续的。
  - 所以，在插入的时候很可能要重新排序（如果设置了 cluster index），所以效率低

- Cluster Index: 
  - 保证主键有序，范围查询。
  - 二分查询，提高索引效率。
  - 所以要一直保持索引有序。

- GUID
  - 优点： 高并发插入数据，不重复。
  - 缺点： 占空间，不连续，cluster index 效率低。

- 自增
  - 缺点： 并发插入效率低。

所以 =>

- 在 SQLServer 等数据库中，不要把 GUID 主键设置为 cluster index

- 在 MySQL 中，插入频繁的表不要用 GUID 做主键。

- 另一种解决方案：混合自增和 GUID，自增作为物理主键，GUID 作为逻辑主键

- Hi/Lo 算法

### Migration Command ###
- Update-Database XXX
- Remove-Migration
- Script-Migration: 
  - 用于生产环境，可以指定生成从一个 migration 到另一个 migration 的脚本，数据库可以直接运行。
  - script-migration FROM TO

### 自引用的组织结构树 ###
一对多
  - 最简单
  - 一端的 entity 包含 多端的 List, 多端的 entity 包含一端的 entity
  - 外键可以根据需要来加，也可以不加。

一对一
  - 需要一端方加一个外键，这样 EFCore 才能推断出。

多对多
  - 两边加 List, configuration 其中一方，
  - ```HasMany<Teacher>(s => s.Teachers).WithMany(t => t.Students).UsingEntity(j => j.ToTable("Students_Teachers"))```

总结：
  - HasXXX().WithXXX()
  - HasOne().WithMany()
  - HasMany().WithOne()
  - HasMany().WithMany()
  
### IQueryable VS IEnumerable ###
  - IQueryable 生成的 SQL 语句会apply filter, substring etc., 会在服务器端传递数据之前，执行的 SQL 语句, 已经过滤调了不需要的 items.
  - IEnumerable 是把所有的数据拉回到程序内存里在做 filter, substring, etc.
  - Where, Select, etc. 等 Linq 语法，都有对应的 IQueryable 和 IEnumerable 两个版本
  - 可以强制转换成 IEnumerable，把数据先拉回程序中，再进行一些复杂操作。（如果用 IQueryable 生成的 SQL 太复杂反而性能降低）
  - 也可以利用 ToArray(), ToList(), 把 IQueryable 的结果一次加载到内存中，就行了。

### ADO.NET两种读取数据方式 ###
  1. DataReader - 从数据库分批读取，内存占用小，DB 链接占用时间长
  2. DataTable - 一次性读取数据，加载到内存，内存占用大，节省 DB 链接

### SQLServer connection string, `MultipleActiveResultSet`=true ###
  - 设置数据库支持多个 DataReader 同时读取。
  - EFCore 本质上就是调用 ADO.NET，IQueryable 就是调用 DataReader, 原则上是不支持多个 DataReader 同时执行.
  - 但是在 SQLServer connection string 中加上这个设置就可以了。

### Interpolation ###
  - 赋值给 string， 字符串拼接， "insert into " + "T_Articles" + .....
  - 赋值给 FormattableString， 编译器构造 FormattableString 对象，避免 SQL 注入
```sql
FormattableString sql =
@"Insert into T_Articles(Title, Message, Price)
  select Title, {name}, Price
  from T_Articles
  where Price > 10"
```

### 解决 SQL 注入最好的方法就是参数化查询 ###

### 直接执行 sql 语句，非查询语句 ###
  - `ctx.Database.ExecuteSqlInterpolatedAsync`
  - 之前用 `ExecuteSqlRaw()`， 需要自己设置参数，有注入风险，不推荐了

### 与实体（entity）相关 ###
  - `FromSqlInterpolated` 只支持单表查询，不可以 join， 包含所有的列
  - `ctx.Books.FromSqlInterpolated(@$"select * from T_Books where DataPart(year, PubTime) > {year} order by newid()")`

### 执行任意 SQL ###
```C#
ctx.Database.GetDbConnection()获得 ADO.NET core 的数据库链接对象。
DbConnection conn = ctx.Database.GetDbConnection();
if (conn.State != ConnectionState.Open)
{
  conn.Open();
}

using (var cmd = conn.CreateCommand())
{
  cmd.CommandText = @"xxxx e.g.: select Price, Count(*) from T_Articles group by Price";
  var pl = cmd.CreateParameter();
  pl.ParameterName = "@year";
  pl.Value = year;
  cmd.Parameters.Add(pl);
  using（var reader = cmd.ExecuteReader())
}
```

没有用 `FromSqlInterpolated`，是因为不想创建过多的实体（entity）类，并不需要在数据库中存储数据，可能只需要一个映射的类，但是导致 dbSet 膨胀。
可以用 dapper 来操作，自动映射 raw sql 查询结果到类,并不是实体类，不用放入 dbContext

DbContext Entry(object) 得到 `EntityEntry`, EFCore 靠它跟踪对象。 `EntityEntry.State` 可以得到 entity 的状态， DebugView.LongView 可以看到实体变化信息。

### 全局筛选器 ###
  - 在 config entity 的时候加入条件，e.g. In `ArticleConfig : IEntityTypeConfiguration<Article>` 中的 `Configure` 方法中，`builder.HasQueryFilter(a => a.IsDeleted == false)`
  - 如果想 ignore 全局筛选器， 在做 filter 的时候需要显示表明 IgnoreQueryFilters()
```c#
   foreach (var a in ctx.Articles.IgnoreQueryFilters().Where(a => a.IdDeleted))
   {
   }
```

### 应用的地方 ###

   - 软删除
   - 多租户

有可能的问题
  - 性能下降，例如索引失效。

### 并发控制 ###
  - 避免多个用户同时操作资源造成并发冲突，例如两个用户同时点赞，有可能点赞丢失。
  - 问题：
      - 脏写：一个事务覆盖了另一个还没提交的修改；
      - 丢失更新：后提交的事务把先提交的更新覆盖掉。

  - 解决:
      - 数据库层面，两种策略：
          - 悲观（Pessimistic Locking） - 总认为会有冲突，加锁。 需要开发人员便携原生SQL语句，不同数据库语法不同。行锁，表锁
          ```SQL:
            select * from T_Houses where Id = 1 for update  // 这个for update就是行锁
          ```
            需要包含在transaction中，
          ```sql
            BeginTransactionAsync()
              // 执行MySQL的raw sql，需要调用来 update operation
              ctx.Houses.FromSqlInterpolated($"select * from T_Houses where Id=1 for update").SingleAsync();
            CommitAsync()
          ```

          - 优点：
            - 简单
          - 缺点：
            - 锁是独占的，排他的，如果并发量大，会严重影响性能，如果使用不当，导致死锁。
            - 不同数据库，语法不一样。

          - 使用场景：
            - 简单系统，访问量不大，就可以用这个悲观并发控制

        - 乐观（Optimistic Locking）- 并发令牌， EFCore提供, recommended.
          - 一个列的情况：
            - 原理：
              - update语句不仅有check id, 还要加上要修改的列(tom 执行)：
                ```sql
                 update T_Houses set Owner='tom' where Id=1 and Owner=""
                ```

              - (jerry执行)：
                ```sql
                  update T_houses set Owner='jerry' where Id=1 and Owner=""
                ```
              
              - Tom 的语句影响的行数为1，因为先执行的。
              - Jerry 影响的行数为0，因为执行的时候Owner已经不为空了，所以执行失败，影响行数为0，EFCore抛出异常 `DbUpdateConcurrencyException`
                ```c#
                  var owner = ex.Entries.First().GetDatabaseValues().GetValue<string>(nameof(House.Owner));
                  Console.WriteLine($"并发冲突，房子已经被{owner}提前抢走了");
                ```

            - 实现：
              - 在table configuration 时表明哪一列为并发toke： 
                ```c#
                  builder.Property(h => h.Owner).IsConcurrencyToken();
                ```
            
            - 问题（缺点）：
              - ABA问题
                - 我拿到 A, 别人改成 B, 另一个人又改成 A, 我就认为这个值没有改变。 
                - 如果本来需求如此，改了之后又改回来就认为没有改，而且只有一列作为并发token，可以考虑用这个IsConcurrencyToken
                - 除此之外，用下面的*RowVersion*应该是更好是选择。

          - 多个列的情况：
            - SQL Server
            ```SQL
              Add a byte[] RowVersion for the entity, then in table configuration, builder.Property(x => x.RowVersion).IsRowVersion();
            ```

            - 其他数据库：
              - 目前还没有支持这种Row Version, MySql 用 timestamp，但是低版本精度不够，不能保证两次操作一定能 update timestamp.
            
            - 解决方法：
                - 手动加一个GUID列，用这个GUID列作为并发token, 但是每次修改其他值的同时，需要手动 update 这个 GUID 
                ```c#
                  h1.RowVer = Guid.NewGuid();
                ```

          - 优点
            - 没有锁，更不会死锁了

### 表达式树 （Exprssion Tree）###
  - 官方定义： 树形数据结构表示代码，可以在运行时访问逻辑运算结构
  - 把 lambda 表达式用表达式树来表示，编译器可以把表达式树（expression tree）翻译成 AST （Abstract Syntax Tree），从而进一步生成更优化的SQL语句。
  - 可以通过debug查看expression tree的结构，left tree, right tree, root etc.

  - `book => book.Price > 5;` 编译器会把这个表达式翻译成下面的expression tree.
  ```
                >
              /  \
             /    \
            .      5
           / \
          /   \
        book  Price     
  ```
  e.g.
  ```c#
      Expression<Func<Book, bool>> e1 = book => book.Price > 5;
      Expression<Func<Book, Book, double>> e2 = (book1, book2) => book1.Price + book2.Price;

      Func<Book, bool> f1 = book => book.Price > 5;
      Func<Book, Book, double> f2 = (book1, book2) => book1.Price + book2.Price;

      using (var ctx = new MyDbContext())
      {
          var books = ctx.Books.Where(e1).ToArray();
      }
  ```
 - Generated SQL statement:
    - when using e1, below is generated SQL:
    ```sql  
        SELECT [t].[Id], [t].[Author], [t].[Price], [t].[Title] FROM [T_Books] AS [t] WHERE [t].[Price] > 5.0E0
    ```
    - when using f1
    ```sql
        SELECT [t].[Id], [t].[Author], [t].[Price], [t].[Title] FROM [T_Books] AS [t]
    ```

### How to check generated expression tree in code: ###
  - Install-Package ExpressionTreeToString
    ```c#
      e1.ToString("Object notation", "C#"); // case-sensitive
    ```

### Asp.Net Core REST 落地 ###
1. RPC 风格： Users/AddNew, Users/GetAll, Users/DeleteById
2. 可以cache的操作，GET。 Idempotent update 使用 PUT。Idempotent Delete => DELETE. 其他操作 POST.
3. POST相对来说比较安全，因为 not idempotent 网关不会重试，保守一些。
3. 业务错误，返回合适的4XX, 报文体返回详细信息

### 实现方式 （简单，快速，好用）： ###
1. Add [action] annotation for Controller class, 优先根据路径匹配, RPC 风格
```c#
  [Route("[controller]/[action]")]
  public class WeatherForecastController : ControllerBase {}
```
2. 不同操作，不同方法名
3. 加[HttpGet], [HttpPost], [HttpDelete], [HttpPut]到对应的方法上

### Action 方法 ###
1. 可以同步也可异步
2. 异步Action方法一般不需要Async结尾，平台调用
3. Action方法返回类型为普通类型 （例如自定义的Person类），返回结果默认为serialized Json
4. 也可以返回 ActionResult<T>, 支持implicit conversion, 可以返回int, 也可以返回NotFound("id不存在");

### 前端 Vue ###
1. 安装Node.js
2. 国内的话，需要npm镜像
3. 安装yarn: npm install -g yarn
4. 创建Vue项目: yarn create @vitejs/app ProjectName

### 注册一个比较耗资源（耗时间）的服务，可以用`builder.Services`注册，然后在对应的方法中调用，不需要构造函数注册。
  - 否则在使用其他方法时，DI也会调用并初始化需要使用的服务，从而浪费资源，我们可以在使用的方法中加入参数名的annotation
```c#
  public int Test([FromServices]TestService testService, int x)
  {
    return testService.Count; // 扫描一个目录，显示文件个数
  }
```

>### drum up ###
> *drum up (something)* also *drum (something) up* 
> - to get or create (support, business, etc.) through hard work and a lot of effort
> - to try hard to get support or business
> - 竭力争取（支持）； 兜揽（生意）
> - They sent out flyers to drum up support for their candidate.
> - He had flown to the north of the country to drum up support for the campaign.


### Part4-22 各项目自己注册DI ###
- 问题：
  - 如果需要 DI 注册， 那其他项目里的服务就要全部被 `program.cs` 所在的项目引用，然后调用 `builder.Services.AddScope<Class123>();`

- 解决：
  - 利用作者这个包可以每个项目自己注册需要的服务。
  - https://github.com/yangzhongke/NETBookMaterials/tree/main/%E6%9C%80%E5%90%8E%E5%A4%A7%E9%A1%B9%E7%9B%AE%E4%BB%A3%E7%A0%81/YouZack-VNext/Zack.Commons

- 要能做到会用，而且懂原理，如何实现
  - 反射 reflection
  - Extension method

### Part4-23~31 缓存 ###
- 概念
  - 缓存命中, hit
  - 缓存命中率, ratio
  - 缓存数据不一致, invalid
  - 多级缓存

- 客户端响应缓存 (app / browser)
  - RFC7324 http 协议
  - cache-control
    - 服务器如果返回 cache-control:max-age=60,
      - 表示服务器指示客户端 **可以** 缓存这个相应60秒。客户端**可以**选择不
  
  - 实现
  ```c#
    public class TestController : ControllerBase
    {
      [ResponseCache(Duration=20)]    // This is the attribute to use for setting the cache
      [HttpGet]
      pubic DateTime Now()
      {
        return DateTime.Now;
      }
    }
  ```

- 服务器端响应缓存，缓存中间件 （鸡肋，不推荐用）
  - 优点：
    - 多个客户端可以利用同一个缓存
  
  - 缺点：
    - 响应状态码为200的GET或者HEAD请求才能被缓存
    - 报文头中不能含有Authorization, Set-Cookie等。
    - 客户端可以通过设置`cache-control: no-cache`来强制服务器不缓存，恶意攻击。

  - 解决：
    - 内存缓存
    - 分布式缓存

  - 实现：
    ```c#
      app.UseCors();

      // 启用服务器端缓存
      // 要在 MapController 之前， UseCors 之后。
      app.UseResponseCaching();  

      app.MapController();
    ```

  -- 测试：
    - 打开两个浏览器，分别get DataTime.Now.
    - 他们拿到的时间时一致的，如果服务器缓存开启。
  

### 当浏览器端设置 ###
  - `cache-control: no-cache`
  - 服务器端无论设置，客户端还是服务器端缓存，均不起作用了。

### 内存缓存 (In-Memory cache) ###
  - 键值对，dictionary
  - 用法：
    1. 启用： `builder.Services.AddMemoryCache()`
    2. 注入 `IMemoryCache` 接口，使用方法：`TryGetValue()`, `Remove()`, `Set()`, `GetOrCreate()`, *`GetOrCreateAsync()`* (highly recommended)

### 缓存的过期时间策略 ###
  - 数据改变的时候调用 `Remove` 或者 `Set` 来删除或者修改缓存。（优点：及时。 缺点：写起来稍微麻烦点，需要在数据都被改动的地方 Remove / Set）
  - 设置过期时间
    - 绝对过期时间
    ```c#
      Book? b = await memoryCache.GetOrCreateAsync("Book" + id, async (e) => {
        Console.WriteLine($"缓存里没找到，到数据库中查一查， id={id}");
        e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSecond(10);    // 设置缓存有效期10秒

        return await MyDbContext.GetByIdAsync(id);
      });

    ```
    - 滑动过期时间 (单独用的情况比较少见)
    ```c#
      // 在指定的时间段内，如果访问了，那缓存被续命了⨀_⨀。
      // 从访问的时间开始，再加指定的时间。
      // 如果没访问，在指定的时间之后，缓存过期。
      e.SlidingExpiration = TimeSpan.FromSeconds(10);
    ``` 

    - 同时设置绝对和滑动
      ```c#
        e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSecond(30);
        e.SlidingExpiration = TimeSpan.FromSeconds(10);
      ```
      - 绝对时间 > 滑动时间
        - 这样缓存会在绝对时间过期之前，随着访问被滑动续期，但是一旦超过绝对时间，缓存项就被删除了。