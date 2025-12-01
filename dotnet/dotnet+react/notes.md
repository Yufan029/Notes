## Section 2 - Building a walking skeleton Part 1 - .Net API ##

### Lesson 1 - Big Picture ###
- Domain is the center of the application and has no dependency on other parts.
![alt text](image.png)![alt text](image-1.png) 

### Lesson 2 - CLI ###
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
  - Drop database
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

## Section 3 Read & execute Walking Skeleton Part 2 - Client ##
### Lesson 1 ###
- Vite, React project files, React concepts, React Dev tools
- Typescript
- Axios
- Material UI

### Lesson 2 ###
- npm create vite@latest
  - Check the command from the vite doc.
  - npm first complain about it, update it from 11.6.2 -> 11.6.4, fix it.
  - Ok to proceed? (y)
  - Project name: `client`
  - Select a framework: `React`
  - Select a variant: `TypeScript + SWC`

- Create the `client` project in the `Reactivities` sln

### Lesson 3 ###
- Change port
  - In vite.config.ts
    ```ts
        server: {
          port: 3000
        }
    ``` 
- ES7+React/Redux/React-Native snippets
- ESLint

### Lesson 4, 5 Fetching Data From Server ###
- useState, useEffect
- builder.Services.AddCors()

### Lesson 6, 7 typescript, react dev tools ###
- create a file in src/lib/types/index.d.ts (typescript definition file)
- copy the json we got from server then find a converter in internet, convert it from json to ts
- short-cut key for format shift + alt + F in windows, you can check that in right click menu
- Adding the definition at the highest level as you can.
  - Say in useState

### Lesson 8 material ui ###
- material ui doc
  - install material ui etc.
  - install font (Roboto)
  - install icon
  - vs code -> setting -> linked editing (change the pair tag at the same time)

- material ui we use
  - Typography variant='h3'
  - List, ListItem, ListItemText

### Lesson 9 mkcert (Certificate Authorization) ###
- Get the dev env as samiliar as production env.
  - use https
  - have certificates
- mkcert
  - Since the address we use is not secure, we'll use mkcert to create local trust dev certificates.
  - use via vite
    - npm install -D vite-plugin-mkcert 
    - after the installation, import in `vite.config.ts`
      - import mkcert from 'vite-plugin-mkcert';
      - `plugins: [react(), mkcert()],`

### Lesson 10 Axios ###
- npm install axios