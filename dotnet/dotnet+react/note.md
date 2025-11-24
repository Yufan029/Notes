## Section 2 - Building a walking skeleton Part 1 - .Net API ##
- Domain is the center of the application and has no dependency on other parts.
![alt text](image.png)![alt text](image-1.png) 

- dotnet CLI
  - `dotnet --info`
  - `dotnet new list`
    - `dotnet new sln`
      - Will create the sln container based on the directory name we're now at.
    - `dotnet new webapi -n NAME -controllers`
    - `dotnet new classlib -n DOMAIN`
    - `dotnet sln add DOMAIN`

  - `dotnet watch`
    - hot reload the changes, but not always applied
  
  - launchSettings.json
    - decide which port to launch the project.
    - we can remove the http one, since we gonna use the https in production.
  
  - When chrome doesn't trust the localhost development certificates. ***Not Secure***
    - `dotnet dev-certs https --check`
      - see if there's a valid certificate
    - `dotnet dev-certs https --help`
    - `dotnet dev-certs https --clean`
      - to clean the non-valid certificate
    - `dotnet dev-certs https --trust`
      - trust the existing certificate
    - restart the browser to validate it.

- Create migration
  - services.AddDbContext<AppDbContext>(opt => {})
    - Needs Microsoft.EntityFrameworkCore.Design
  - Migrations
    - Need `dotnet ef` command, which needs to install from the nuget. keyword: "dotnet ef nuget"
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
  1. create a scope
  2. scope get service
  3. service get DbContext
  4. context.Database.MigrateAsync
  5. Initilizer.SeedData(context)
  6. Log if there's any exception. Logger is DI resolved as well.
  7. Wrap 4, 5 in try, 6 in catch