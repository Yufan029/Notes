using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public string GetVillas()
        {
            return "This is the list of all villas";
        }

        [HttpGet("{id:int}")]
        public string GetVillaById(int id)
        {
            return $"This is the villa with id: {id}";
        }
    }
}
