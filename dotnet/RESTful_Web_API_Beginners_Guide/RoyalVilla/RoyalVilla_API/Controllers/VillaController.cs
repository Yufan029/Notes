using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var villas = await dbContext.Villa.ToListAsync();

            return Ok(this.mapper.Map<List<VillaDTO>>(villas));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return new ApiResponse<VillaDTO>() 
                    { 
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = "Villa ID must be greater than 0",                        
                    };
                }

                var villa = await dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (villa == null)
                {
                    return new ApiResponse<VillaDTO>()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Errors = $"Villa with ID {id} not found"
                    };
                }

                return new ApiResponse<VillaDTO>()
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = this.mapper.Map<VillaDTO>(villa),
                    Message = $"Villa with ID {id} retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<VillaDTO>()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Errors = $"An error occurred while retrieving villa with ID {id}, {ex.Message}"
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult<VillaDTO>> CreateVilla(VillaCreateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest("Villa data is null.");
                }

                var duplicated = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDto.Name.ToLower());
                if (duplicated != null)
                {
                    return Conflict($"Villa with name {villaDto.Name} already exists.");
                }

                var villa = this.mapper.Map<Villa>(villaDto);
                villa.CreatedDate = DateTime.Now;

                await dbContext.Villa.AddAsync(villa);
                await dbContext.SaveChangesAsync();


                return CreatedAtAction(nameof(GetVillaById), new {id = villa.Id}, this.mapper.Map<VillaDTO>(villa));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while creating the villa, {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaUpdateDTO>> UpdateVilla(int id, VillaUpdateDTO villaDto)
        {
            try
            {
                if (villaDto == null)
                {
                    return BadRequest("Villa data is null.");
                }

                if (id != villaDto.Id)
                {
                    return BadRequest("Villa ID mismatch.");
                }

                var existingVilla = await dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                var duplicated = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDto.Name.ToLower() && x.Id != id);
                if (duplicated != null)
                {
                    return Conflict($"Villa with name {villaDto.Name} already exists.");
                }

                this.mapper.Map(villaDto, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(villaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while update the villa, {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Villa>> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await this.dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                this.dbContext.Villa.Remove(existingVilla);
                await this.dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"delete {id} villa fail, {ex.Message}");
            }
            
        }
    }
}
