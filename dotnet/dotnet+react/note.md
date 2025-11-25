## Section 2 - Building a walking skeleton Part 1 - .Net API ##

### Lesson 1 - Big Picture ###
- Domain is the center of the application and has no dependency on other parts.
![alt text](image.png)![alt text](image-1.png) 

### Lesson 2 - DLI ###
- dotnet CLI
  - `dotnet --info`
  - `dotnet new list`
    - `dotnet new sln`
      - Will create the sln container based on the directory name we're now at.
    - `dotnet new webapi -n ProjectName -controllers`
    - `dotnet new classlib -n ProjectName`
    - `dotnet sln add ProjectName`

  - `dotnet watch`
    - hot reload the changes, but not always applied
  
  - launchSettings.json
    - decide which port to launch the project.
    - we can remove the http one, since we gonna use the https in production.
  
  - When chrome doesn't trust the localhost development certificates. ***Not Secure*** in the browser address bar.
    - `dotnet dev-certs https --check`
      - see if there's a valid certificate
    - `dotnet dev-certs https --help`
    - `dotnet dev-certs https --clean`
      - to clean all the certificates on the machine
    - `dotnet dev-certs https --trust`
      - trust the existing certificate
    - restart the browser to validate it.

### Lesson 6 Migration ###
- Create migration
  - In Program.cs file, `services.AddDbContext<AppDbContext>(opt => {})`
    - Needs Microsoft.EntityFrameworkCore.Design
  - Migrations
    - Need `dotnet ef` command, which needs to install from the nuget. keyword: *dotnet ef nuget*
  -Migration command
    - `dotnet ef migrations add Init -p Persistence -s API`
      - `-p` is the project where dbContext located.
      - `-s` is the start project
  - Update database
    - `dotnet ef database update -p Persistence -s API`
  - Dropp database
    - `dotnet ef database drop -p Persistence -s API`

### Lesson 7 - Seeding Data ###
- create static function named SeedData(AppDbContext context), async version
- In program, 
  1. create a scope - `app.Services.CreateScope()`
  2. scope get service - `scope.ServiceProvider`
  3. service get DbContext `services.GetRequiredService<AppDbContext>()`
  4. `await context.Database.MigrateAsync`
  5. `Initilizer.SeedData(context)`
  6. Log if there's any exception. Logger is DI resolved as well.
  7. Wrap 4, 5 in try, 6 in catch

### Lesson 8 - Create Controller ###
1. Create a base controller inherit from ControllerBase
2. Create ActivitiesController and use Primary Constructor syntax to inject the dbContext
- 503 server too busy
- ***Restart the server*** as the first step to fix any issue
- The request route is https://localhost:5001/api/activities

### Lesson 9 - Postman ###
- Create Workspace, like 'ReactivitiesCourse'
- Inside the workspace, you can create a collection, like 'Reactivities'
- And in each collection, you can create folder to organise your requests.