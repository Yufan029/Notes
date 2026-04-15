using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalVillaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public HomeController(IVillaService villaService, IMapper mapper)
        {
            this.villaService = villaService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villas = new List<VillaDTO>();
            try
            {
                var response = await villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>("");
                if (response != null && response.Success)
                {
                    villas = mapper.Map<List<VillaDTO>>(response.Data);
                    return View(villas);
                }
                else
                {
                    TempData["Error"] = "Failed to load villas. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while loading the home page: {ex.Message}";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
