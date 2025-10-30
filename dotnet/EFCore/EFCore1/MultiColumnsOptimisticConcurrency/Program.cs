using Microsoft.EntityFrameworkCore;
using OptimisticConcurrency;

namespace MultiColumnsOptimisticConcurrency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new MyDbContext())
            {
                // 需要DB本来的Owner值为NULL，
                // 如果本来是tom, 然后tom 和 jerry 抢，
                // 如果tom是第一个抢到了token,但是EFCore比较之后发现值没有变，不会生成update语句，不会更改rowversion，他就会成功
                // 所以等jerry第二个抢到后，他尝试update 他也会成功，
                Console.WriteLine("Please enter the name");
                var name = Console.ReadLine();

                var house = ctx.Houses.Single(x => x.Id == 1);
                house.Owner = name;

                Thread.Sleep(5000);
                try
                {
                    ctx.SaveChanges();
                    Console.WriteLine($"house owner is {name}");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var owner = ex.Entries.First().GetDatabaseValues().GetValue<string>(nameof(House.Owner));
                    Console.WriteLine($"sorry, house has been occupied, owner is {owner}");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.ReadLine();
            }
        }
    }
}
