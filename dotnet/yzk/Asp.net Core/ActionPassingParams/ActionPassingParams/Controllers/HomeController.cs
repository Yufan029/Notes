using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ActionPassingParams.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController
    {
        [HttpGet("{i}/{j}")]
        public int Mutiple(int i, int j)
        {
            return i * j;
        }

        [HttpGet("{i1}/{i2}")]
        public int Add([FromRoute(Name = "i1")] int i, [FromRoute(Name = "i2")] int j)
        {
            return i * j;
        }

        [HttpPost]
        public int Add2([FromQuery(Name = "i")] int left, int right)
        {
            return left + right;
        }

        [HttpPost]
        public string AddPerson(Person p, [FromHeader(Name ="user-agent")] string userAgent)
        {
            return "success! add person " + p.Id + ": " + p.Name + ", User Agent = " + userAgent;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] ChildrenNames { get; set; }
    }
}
