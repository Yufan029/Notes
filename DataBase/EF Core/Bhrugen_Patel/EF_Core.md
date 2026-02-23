- ORM
- Nov 2022, EF Core 7

- question:
    - when you want to verify if some issue exist old version, but already fixed in the new version, you can roll back the database to that stage.

- Rollback database to a specific migration version
    - `update-database [here you specify the migration version]`, will go back to a specific migration version of the database status
    - after testing issue in the old database.
    - run `update-database`, will bring the db to the latest status.

- Seed Data can be done in `OnModelCreating` in AppDbContext
```c#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = 1, Title = "Spider without Duty", ISBN = "123b32", Price = 10.99m }
            new Book { BookId = 2, Title = "Fortune of time", ISBN = "123ASKJLBSD3242", Price = 10.99m }
        )
    }

```

- Create new database diagrams in SQL Server

- In Visual Studio, there's a tool called `EF Core Power Tools`

- Right click the DataAccess project, there's an option in the menu: `EF Core Power Tools`

### Lesson 33 ###
- One to one relation
    - Two tables, `Book` & `BookDetails`
    - if only add the `BookDetials_Id` into `Book` Model, the relationship will not formed
    - only if add the **navigation property** `BookDetials` into `Book`, then the relationship from `Book` to `BookDetails` are established.
    - At the moment, the relationship is 1 (Book) to * (BookDetals)
    - After adding the `Book_Id` into `BookDetails` Model, the 1 to 1 relationship is established. 
    - Don't forget the annotation (<ForeignKey(NavigationPropertyName)>) for `BookDetails_Id` in `Book` Model

    - After finish upper procedure, now `Book` class has a foreign key of `BookDetails`,
    - **BUT**
    - `Book` shouldn't have a foreign key to `BookDetails`, feels like `Book` **is dependent on** `BookDetails`.
    - Shoule be the other way round.
    - `BookDetails` should have a foreign key of `Book`, so the upper procedure should change reversly.

- So in this case, `Book` and `BookDetails`
    - `BookDetails` should depend on `Book` => `BookDetails` should have the foreign key.
    - Add navigation property `public Book Book` to `BookDetails` first.
    - Add foreign key `public int Book_Id` and annotate with <ForeignKey("Book")>
    - Add `public BookDetails BookDetails` in `Book`
    - A 依赖 B, A 里面就有 B 的 <ForeignKey>, 有 B 的 Navigation Property

```c#
    public class Book
    {
        public int BookId { get; set; }
        public BookDetails BookDetails { get; set; }
    }

    public class BookDetails
    {
        public int BookDetailsId { get; set; }

        [ForeignKey("Book111")]
        public int Book_Id { get; set; }
        public Book Book111 { get; set; }      // Navigation Property
    }


### Lesson 35 ###
- One to Many
    - `Book`(*) Vs `Publisher`(1)
    ```c#
    class Books
    {
        public int BookId { get; set; }
        
        [ForeignKey("Publisher")]
        public int Publisher_Id { get; set; }
        public Publisher Publisher { get; set; }  // Navigation Property
    }

    class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public List<Book> Books { get; set; }   // Navigation Property
    }
    ```

### Lesson 38 ###endregion
- Many to Many
```c#
    public class Book {
        public int BookId { get; set; }
        public List<Author> Authors { get; set; }
    }

    public class Author {
        public int AuthorId { get; set; }
        public List<Book> Books { get; set; }
    }
```
- Start from dotnet core 5, EF Core will create the middle table for us, before dotnet 5, you need to create the middle table yourself
- Also if you want to add extra columns, you still need to add the middle table yourself

- Customise the middle mapping table, 其实就是中间表是 1，另两个表是多，两个 1 对 多的关系。
```c#
    public class BookAuthorMap
    {
        [ForeignKey("Book")]
        public int Book_Id { get; set; }
        [ForeignKey("Author")]
        public int Author_Id { get; set; }

        public string CustomisedColumn { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }

    public class Book 
    {
        public int BookId { get; set; }
        public List<BookAuthorMap> BookAuthorMap { get; set; }
    }

    public class Author
    {
        public int AuthorId { get; set; }
        public List<BookAuthorMap> BookAuthorMap { get; set; }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthorMap>().HasKey(u => new { u.Book_Id, u.Author_Id });
    }
```

### Lesson 41 Fluent API ###
- Code first will do Fluent API --> Data Annotation --> Default Convention

```c#
    public DbSet<Fluent_BookDetail> BookDetai_fluent

    protected override void OnModelCreateing(ModelBuilder modelBuilder)
    {
        // Change the table name
        modelBuilder.Entity<Fluent_BookDetail>.ToTable("Fluent_BookDetails");

        // Change the column name
        modelBuilder.Entity<Fluent_BookDetail>().Property(u => u.NumberOfChapters).HasColumnName("NoOfChapters");

        // has key
        modelBuilder.Entity<Fluent_BookDetail>().HasKey(c => c.BookDetail_Id);

        // required
        modelBuilder.Entity<Fluent_BookDetail>().Property(u => u.NumberOfChapters).IsRequired();

        // Max Length
        modelBuilder.Entity<Fluent_Book>().Property(u => u.ISBN).HasMaxLength(50);

        // [NotMapped]
        modelBuilder.Entity<Fluent_Book>().Ignore(u => u.PriceRange);
    }
```

### Lesson 47 One to One in Fluent API ###
![alt text](image.png)
- Book has one BookDetails, BookDetails has one Book
- If you want to use Book as the `parent` class there, you need to add the **foreign key** to the BookDetails, and the navigation properties on both side.
- Fluent API version
```c#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fluent_BookDetail>().HasOne(b => b.Book).Withone(b => b.BookDetail).HasForeignKey<Fluent_BookDetail>(u => u.Book_Id);
    }
```

- **谁拥有foreign key,就在谁里边定义**

- One to Many
![alt text](image-1.png)
```c#
    // with one to many, the foreign key always in the one side, no need to specify in the angle bracket <> of HasForeignKey, EF Core will know it.
    modelBuidler.Entity<Fluent_Book>().HasOne(b => b.Publisher).WithMany(p => p.Books).HasForeignKey(b => b.Publisher_Id);
```

- Many to Many, Authors to Books
    - if using the auto generated middle table, no need to set the fluent API, just add the *Navigation Properties*

- With the manual mapping table
![alt text](image-2.png)
```c#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fluent_BookAuthorMap>().HasKey(u => new { u.Book_Id, u.Author_Id });
        modelBuilder.Entity<Fluent_BookAuthorMap>().HasOne(b => b.Book).WithMany(b => b.BookAuthorMap).HasForeignKey(u => u.Book_Id);
        modelBuilder.Entity<Fluent_BookAuthorMap>().HasOne(b => b.Author).WithMany(b => b.BookAuthorMap).HasForeignKey(u => u.Author_Id);
    }
```

- EF Core will automatically create the mapping table in DB, while not adding DbSet<Fluent_BookAuthorMap> in DbContext, 
- But if user want to access the data in this manual mapping table, you need to explicitly adding DbSet<MappingTable> to DbContext

### Lesson 52 Organize Fluent API ###
- Create specific config file, like FluentBookDetailConfig, inherit from IEntityTypeConfiguration<Fluent_BookDetail>
```c#
    public class FluentBookDetailConfig : IEntityTypeConfiguration<Fluent_BookDetail>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookDetail> modelBuilder)
        {
            // then all the setting in the OnModelCreatiing can be move to here, no need to specify the Entity<Fluent_BookDetail>()
            modelBuilder.ToTable("Fluent_BookDetails");
            modelBuilder.Property(u => u.NumberOfChapters).HasColumnName("NoOfChapters");
            modelBuilder.HasKey(c => c.BookDetail_Id);
            modelBuilder.Property(u => u.NumberOfChapters).IsRequired();
        }
    }

    // then in the OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FluentBookDetailConfig());
    }
```

## Day 6 ##
### Lesson 53 Access DbContext from Console ###
```c#
    using(ApplicationDbContext context = new())
    {
        context.Database.EnsureCreated();
        if (context.Database.GetPendingMigration().Count() > 0)
        {
            context.Database.Migrate();
        }
    }
```

```c#
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("connectonString").LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }
```

- EF Core contain function itself 
```c#
    // all books ISBN starts with 12
    var books = context.Books.Where(b => EF.Functions.Like(b.ISBN, "12%"));
```

-- Pagination
    - Skip, Take 

### 8-86 Explicit loading ###
```c#
    // least efficient
    obj.Publisher = _db.Publishers.Find(obj.Publisher_Id);

    // more efficient (Explicit loading)
    _db.Entry(obj).Reference(u => u.Publisher).Load();

    // eager loading ❤️
    _db.BookDetail.Include(x => x.Book).FirstOrDefault(x => x.BookDetail_Id = Id);
```

### 92 IEnumerable vs IQueryable ###
- IEnumerable, filter data on client side.
```c#
    IEnumerable<Books> BookList = _db.Books;
    var FilteredBook = BookList.Where(b => b.Price > 50).ToList();

    // Generated Query
    SELECT [b].[BookId], [b].[Category_Id], [b].[BookISBN], [b].[Price], [b].[Publisher_Id], [b].[Title]
    FROM [Books] AS [b]

    // then Filter is applied in memory
```

- IQueryable, filter the data on database side. ✔️
```c#
    IQueryable<Books> BookList = _db.Books;
    var fitleredBook = BookList.Where(b => b.Price > 50).ToList();

    // Generated Query, fitler applied in place
    SELECT [b].[BookId], [b].[Category_Id], [b].[BookISBN], [b].[Price], [b].[Publisher_Id], [b].[Title]
    FROM [Books] AS [b]
    WHERE [b].[Price] > 50.0
```

- In visual studio watch window can lookup: "_db.ChangeTracker.Entries()"

- Adding a view in migration 
    ```sql
        migrationBuilder.Sql(@"CREATE OR ALTER VIEW dbo.GetMainBookDetails
            AS
            SELECT m.ISBN, m.Title, m.Price FROM dbo.Books m
        ");
    ```
- Create a corresponding class to include ISBN, Title, Price and no id.
- We still need to create a DbSet<MainBookDetails> for the class, but we don't want to create a new table in DB, so we need to let EF Core knows that we don't want to create a table for this view.
    ```sql
        -- ToView tells EF Core we donnot want to create a table for that, it is a view.
        -- And EF Core will not track this entity
        -- Anything retrieved frmo the view will read only, cannot be tracked.
        modelBuilder.Entity<MainBookDetails>.HasNoKey().ToView("GetMainBookDetails");
    ```
    ```c#
        // When using
        var viewList = _db.MainBookDetails.ToList();
        var viewList1 = _db.MainBookDetails.Where(x => x.Price > 30);
    ```

- Raw SQL 
    ```c#
        var bookRaw = _db.Books.FromSqlRaw("Select * from dbo.books").ToList();
        var bookRawInter = _db.Books.FromSqlInterpolated($"Select * from dbo.Books where bookId={id}").ToList();
    ```