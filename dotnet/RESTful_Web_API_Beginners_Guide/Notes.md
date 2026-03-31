- Add scalar
    - nuget package
    - project property -> debug -> launch browser & launch URL

- 2026/03/31
    - SeedData
        - Passing the `WebApplication` as parameter, then create the `scope`, then get service provider to resolve the ApplicationDbContext
        - Then perform Migration to seed the database.
        ```c#
            static async Task SeedDataAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await context.Database.MigrateAsync();
            }
        ```
    - automapper
