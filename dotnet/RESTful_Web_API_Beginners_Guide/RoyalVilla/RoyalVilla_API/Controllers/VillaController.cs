using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    //[Authorize(Roles = "Customer, Admin" )]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public VillaController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
        {
            var villas = await dbContext.Villa.ToListAsync();
            var villaDtos = this.mapper.Map<List<VillaDTO>>(villas);

            var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(villaDtos);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa ID must be greater than 0"));
                }

                var villa = await dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found"));
                }

                var villaDto = this.mapper.Map<VillaDTO>(villa);
                return Ok(ApiResponse<VillaDTO>.Ok(villaDto, $"Villa with ID {id} retrieved successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while retrieving the villa, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> CreateVilla(VillaCreateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required."));
                }

                var duplicated = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDto.Name.ToLower());
                if (duplicated != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with name {villaDto.Name} already exists."));
                }

                var villa = this.mapper.Map<Villa>(villaDto);
                villa.CreatedDate = DateTime.Now;

                await dbContext.Villa.AddAsync(villa);
                await dbContext.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.CreatedAt(this.mapper.Map<VillaDTO>(villa), $"Villa with ID {villa.Id} created successfully");
                return CreatedAtAction(nameof(GetVillaById), new {id = villa.Id}, response);
            }
            catch (Exception ex)
            {
                var  errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while creating the villa, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id, VillaUpdateDTO villaupdatedDto)
        {
            try
            {
                if (villaupdatedDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is null."));
                }

                if (id != villaupdatedDto.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa ID mismatch."));
                }

                var existingVilla = await dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found."));
                }

                var duplicated = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Name.ToLower() == villaupdatedDto.Name.ToLower() && x.Id != id);
                if (duplicated != null)
                {
                    return Conflict(ApiResponse<VillaDTO>.Conflict($"Villa with name {villaupdatedDto.Name} already exists."));
                }

                this.mapper.Map(villaupdatedDto, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;

                await dbContext.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.Ok(this.mapper.Map<VillaDTO>(villaupdatedDto), $"Villa with ID {id} updated successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while updating the villa, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} not found."));
                }

                this.dbContext.Villa.Remove(existingVilla);
                await this.dbContext.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent($"Villa with ID {id} deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while deleting the villa, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }            
        }
    }
}
