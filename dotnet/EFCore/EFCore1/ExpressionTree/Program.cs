using ExpressionTreeToString;
using OptimisticConcurrency;
using System.Linq.Expressions;

namespace ExpressionTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<Book, bool>> e1 = book => book.Price > 5;
            Expression<Func<Book, Book, double>> e2 = (book1, book2) => book1.Price + book2.Price;

            Func<Book, bool> f1 = book => book.Price > 5;
            Func<Book, Book, double> f2 = (book1, book2) => book1.Price + book2.Price;

            // Using expression tree ToString extension library.
            Console.WriteLine(e1.ToString("Object notation", "C#"));

            //using (var ctx = new MyDbContext())
            //{
            //    var books = ctx.Books.Where(e1).ToArray();
            //}
        }
    }
}
