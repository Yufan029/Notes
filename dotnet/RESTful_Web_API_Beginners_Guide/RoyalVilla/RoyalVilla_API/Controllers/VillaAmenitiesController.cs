using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa-amenities")]
    [ApiController]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public VillaAmenitiesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenities()
        {
            try
            {
                var villaAmenities = await this.dbContext.VillaAmenities.Include(x => x.Villa).ToListAsync();

                var villaAmenitiesDto = this.mapper.Map<List<VillaAmenitiesDTO>>(villaAmenities);
                var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(villaAmenitiesDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.InternalServerError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa amenities ID must be greater than 0"));
                }

                var villaAmenities = await dbContext.VillaAmenities.Include(x => x.Villa).FirstOrDefaultAsync(x => x.Id == id);
                if (villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa amenities with ID {id} not found"));
                }

                var villaAmenitiesDto = this.mapper.Map<VillaAmenitiesDTO>(villaAmenities);
                return Ok(ApiResponse<VillaAmenitiesDTO>.Ok(villaAmenitiesDto, $"Villa amenities with ID {id} retrieved successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while retrieving the villa amenities, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenities(VillaAmenitiesCreateDTO villaAmenitiesCreatedDto)
        {
            try
            {
                if (villaAmenitiesCreatedDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa amenity data is required."));
                }

                if (villaAmenitiesCreatedDto.VillaId < 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Villa ID must be greater than 0"));
                }

                var villa = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Id == villaAmenitiesCreatedDto.VillaId);
                if (villa == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {villaAmenitiesCreatedDto.VillaId} not found."));
                }

                var duplicated = await this.dbContext.VillaAmenities.FirstOrDefaultAsync(x => x.Name.ToLower() == villaAmenitiesCreatedDto.Name.ToLower());
                if (duplicated != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa amenitiy with name {villaAmenitiesCreatedDto.Name} already exists."));
                }

                var villaAmenities = this.mapper.Map<VillaAmenities>(villaAmenitiesCreatedDto);
                villaAmenities.CreatedDate = DateTime.Now;

                await dbContext.VillaAmenities.AddAsync(villaAmenities);
                await dbContext.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.CreatedAt(this.mapper.Map<VillaAmenitiesDTO>(villaAmenities), $"Villa amenities with ID {villaAmenities.Id} created successfully");
                return CreatedAtAction(nameof(GetVillaAmenitiesById), new { id = villaAmenities.Id }, response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while creating the villa amenities, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO villaAmenitiesUpdatedDto)
        {
            try
            {
                if (villaAmenitiesUpdatedDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa amenities data is null."));
                }

                if (id != villaAmenitiesUpdatedDto.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa amenities ID mismatch."));
                }

                var existingVillaAmenities = await dbContext.VillaAmenities.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa amenities with ID {id} not found."));
                }

                var villa = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Id == villaAmenitiesUpdatedDto.VillaId);
                if (villa == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {villaAmenitiesUpdatedDto.VillaId} not found."));
                }

                var duplicated = await this.dbContext.VillaAmenities.FirstOrDefaultAsync(x => x.Name.ToLower() == villaAmenitiesUpdatedDto.Name.ToLower() && x.Id != id);
                if (duplicated != null)
                {
                    return Conflict(ApiResponse<VillaAmenitiesDTO>.Conflict($"Villa amenities with name {villaAmenitiesUpdatedDto.Name} already exists."));
                }

                this.mapper.Map(villaAmenitiesUpdatedDto, existingVillaAmenities);
                existingVillaAmenities.UpdatedDate = DateTime.Now;

                await dbContext.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.Ok(this.mapper.Map<VillaAmenitiesDTO>(villaAmenitiesUpdatedDto), $"Villa amenities with ID {id} updated successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while updating the villa amenities, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
        {
            try
            {
                var existingVillaAmenities = await this.dbContext.VillaAmenities.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa amenities with ID {id} not found."));
                }

                this.dbContext.VillaAmenities.Remove(existingVillaAmenities);
                await this.dbContext.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent($"Villa amenities with ID {id} deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.InternalServerError($"An error occurred while deleting the villa amenities, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
