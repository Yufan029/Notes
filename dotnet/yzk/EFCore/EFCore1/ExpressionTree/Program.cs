using ExpressionTreeToString;
using OptimisticConcurrency;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace ExpressionTree
{
    internal class Program
    {
        static void Main1(string[] args)
        {
            Expression<Func<Book, bool>> e1 = book => book.Price > 5;
            Expression<Func<Book, Book, double>> e2 = (book1, book2) => book1.Price + book2.Price;

            Func<Book, bool> f1 = book => book.Price > 5;
            Func<Book, Book, double> f2 = (book1, book2) => book1.Price + book2.Price;

            // Using expression tree ToString extension library.
            Console.WriteLine(e1.ToString("Object notation", "C#"));

            // 运行时决定生成什么样的表达式
            Console.WriteLine("generate expression: 1 for Greater than, 2 for less than");
            string compareOp = Console.ReadLine();

            // 手动构建表达式树 （麻烦）, MORE EASY WAY SEE BELOW (简单的看下边）
            // 根据 ExpressionTreeToString 打印出来的 log，我们来手动构建 Expression Tree
            ParameterExpression parameterExp = Expression.Parameter(typeof(Book), "book");
            ConstantExpression constExp = Expression.Constant(6.0);
            MemberExpression memberExp = Expression.MakeMemberAccess(parameterExp, typeof(Book).GetProperty("Price"));

            BinaryExpression binaryExp;
            // 动态生成 expression tree
            if (compareOp == "1")
            {
                binaryExp = Expression.GreaterThan(memberExp, constExp);
            }
            else
            {
                binaryExp = Expression.LessThan(memberExp, constExp);
            }

            Expression<Func<Book, bool>> selfMadeExp = Expression.Lambda<Func<Book, bool>>(binaryExp, parameterExp);

            using (var ctx = new MyDbContext())
            {
                var books = ctx.Books.Where(selfMadeExp).ToArray();
            }
        }

        static void Main(string[] args)
        {
            Expression<Func<Book, bool>> e1 = book => book.Price > 5;

            // Print out the results can be used for generating expression tree.
            Console.WriteLine(e1.ToString("Factory Methods", "C#"));

            Console.WriteLine("please enter 1 for great than, otherwise for less than");
            var op = Console.ReadLine();

            // using static System.Linq.Expressions.Expression
            var book = Parameter(typeof(Book), "book");

            MemberExpression memExp = MakeMemberAccess(book, typeof(Book).GetProperty("Price"));
            ConstantExpression const7 = Constant(7.0);

            BinaryExpression binaryExp = op == "1"  ? GreaterThan(memExp, const7) : LessThan(memExp, const7);

            var genExp = Lambda<Func<Book, bool>>(binaryExp, book);

            using (var ctx = new MyDbContext())
            {
                var books = ctx.Books.Where(genExp).ToArray();
            }
        }
    }
}
