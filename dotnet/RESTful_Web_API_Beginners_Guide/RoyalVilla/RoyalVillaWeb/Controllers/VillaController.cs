using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService villaService;
        private readonly IMapper mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
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

        public IActionResult Create()
        {
            return View();
        }
    }
}
