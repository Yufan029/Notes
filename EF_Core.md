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