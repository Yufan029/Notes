using AutoMapper;
using Azure;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaCreateDTO villaCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(villaCreateDTO);
            }
            
            try
            {
                var response = await villaService.CreateAsync<ApiResponse<VillaDTO>>(villaCreateDTO, "");
                if (response != null && response.Success)
                {
                    TempData["Success"] = "Villa created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while loading the home page: {ex.Message}";
            }

            return View(villaCreateDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
            {
                TempData["Error"] = "Invalid villa ID.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var response = await villaService.GetAsync<ApiResponse<VillaDTO>>(id, "");
                if (response != null && response.Success)
                {
                    return View(response.Data);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while loading the home page: {ex.Message}";
            }

            await villaService.DeleteAsync<ApiResponse<VillaDTO>>(id, ""); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VillaDTO villaDTO)
        {
            try
            {
                var response = await villaService.DeleteAsync<ApiResponse<object>>(villaDTO.Id, "");
                if (response != null && response.Success)
                {
                    TempData["Success"] = "Villa deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while deleting the villa: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id < 0)
            {
                TempData["Error"] = "Invalid villa ID.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var response = await villaService.GetAsync<ApiResponse<VillaDTO>>(id, "");
                if (response != null && response.Success)
                {
                    var villaUpdateDTO = mapper.Map<VillaUpdateDTO>(response.Data);
                    return View(villaUpdateDTO);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while loading the villa details: {ex.Message}";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                var response = await villaService.UpdateAsync<ApiResponse<object>>(villaUpdateDTO, "");
                if (response != null && response.Success)
                {
                    TempData["Success"] = "Villa updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while updating the villa: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
