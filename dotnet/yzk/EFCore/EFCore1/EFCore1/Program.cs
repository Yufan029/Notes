namespace EFCore1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var ctx = new MyDbContext())
            {
                /*
                var books = new List<Book>
                {
                    new Book { Title = "The Silent River", PubTime = new DateTime(2018, 5, 12), Price = 19.99, AuthorName = "Alice Green" },
                    new Book { Title = "C# in Depth", PubTime = new DateTime(2021, 3, 8), Price = 45.50, AuthorName = "Jon Skeet" },
                    new Book { Title = "Journey to the West", PubTime = new DateTime(1999, 11, 3), Price = 25.00, AuthorName = "Wu Cheng'en" },
                    new Book { Title = "Learning ASP.NET Core", PubTime = new DateTime(2023, 2, 18), Price = 39.90, AuthorName = "Adam Freeman" },
                    new Book { Title = "The Ocean's Whisper", PubTime = new DateTime(2017, 7, 25), Price = 15.75, AuthorName = "Lily Hart" },
                    new Book { Title = "Data Structures Unlocked", PubTime = new DateTime(2020, 10, 5), Price = 29.95, AuthorName = "Mark Allen" },
                    new Book { Title = "The Future of AI", PubTime = new DateTime(2024, 6, 10), Price = 49.99, AuthorName = "Andrew Ng" },
                    new Book { Title = "Design Patterns Explained", PubTime = new DateTime(2019, 9, 14), Price = 34.80, AuthorName = "Erich Gamma" },
                    new Book { Title = "The Art of Clean Code", PubTime = new DateTime(2022, 12, 1), Price = 42.00, AuthorName = "Robert C. Martin" },
                    new Book { Title = "A Brief History of Time", PubTime = new DateTime(1988, 4, 1), Price = 18.00, AuthorName = "Stephen Hawking" }
                };

                ctx.Books.AddRange(books);
                await ctx.SaveChangesAsync();
                */
                foreach (var grouping in ctx.Books.GroupBy(book => book.AuthorName))
                {
                    Console.WriteLine($"{grouping.Key}: {grouping.Count()}");
                }
            }
        }
    }
}
