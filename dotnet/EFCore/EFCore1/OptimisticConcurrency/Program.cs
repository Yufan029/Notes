using Microsoft.EntityFrameworkCore;
using System.Data;

namespace OptimisticConcurrency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 需要db中有id=4的house.
            // 同时运行两个，一个叫tom，一个叫jerry
            // 然后hit enter to let the program to run, 
            // the first one will get the house
            // and the second one will be ackowledged who got the house, since the exception happens.
            using (var ctx = new MyDbContext())
            {
                Console.WriteLine("Please enter the name");
                var name = Console.ReadLine();

                var house = ctx.Houses.Single(h => h.Id == 4);
                house.Owner = name;
                Thread.Sleep(5000);
                try
                {
                    ctx.SaveChanges();
                    Console.WriteLine($"{name} got the house");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("concurrency error.");
                    var owner = ex.Entries.First().GetDatabaseValues().GetValue<string>(nameof(House.Owner));
                    Console.WriteLine($"house has been get by {owner}");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadLine();
                }
                Console.ReadLine();
            }
        }
    }
}
