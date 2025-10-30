using Microsoft.EntityFrameworkCore;
using OneToMany;

namespace 自引用
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var ctx = new MyDbContext())
            {
                /*
                var rootOrg = new OrgUnit { Name = "global" };
                var asiaOrg = new OrgUnit { Name = "Asian" };
                var chinaOrg = new OrgUnit { Name = "China" };
                var sgOrg = new OrgUnit { Name = "Singapore" };
                var americaOrg = new OrgUnit { Name = "America" };
                var usaOrg = new OrgUnit { Name = "USA" };
                var canadaOrg = new OrgUnit { Name = "Canada" };

                asiaOrg.Parent = rootOrg;
                americaOrg.Parent = rootOrg;

                chinaOrg.Parent = asiaOrg;
                sgOrg.Parent = asiaOrg;

                usaOrg.Parent = americaOrg;
                canadaOrg.Parent = americaOrg;

                // just give the enough infomation for EFCore to infer the relationship
                ctx.OrgUnits.Add(chinaOrg);
                ctx.OrgUnits.Add(sgOrg);
                ctx.OrgUnits.Add(usaOrg);
                ctx.OrgUnits.Add(canadaOrg);

                await ctx.SaveChangesAsync();
                */

                var orgRoot = ctx.OrgUnits.Single(o => o.Parent == null);
                PrintChildren(0, ctx,orgRoot);
            }
        }

        static void PrintChildren(int indentLevel, MyDbContext ctx, OrgUnit node)
        {
            var children = ctx.OrgUnits.Where(o => o.Parent == node);
            Console.WriteLine($"{new string('\t', indentLevel)}-{node.Name}");
            foreach (var child in children)
            {
                PrintChildren(indentLevel + 1, ctx, child);
            }
        }
    }
}
