using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (registerationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));
                }

                if (await authService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email '{registerationRequestDTO.Email}' already exists"));
                }

                var userDto = await authService.RegisterAsync(registerationRequestDTO);
                if (userDto == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.InternalServerError("Failed to register user"));
                }

                // auth service
                var response = ApiResponse<UserDTO>.CreatedAt(userDto, "User registered successfully");
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                var response = ApiResponse<object>.InternalServerError($"An error occurred while processing the registration, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));
                }

                var loginResponse = await authService.LoginAsync(loginRequestDTO);
                if (loginResponse == null)
                {
                    return NotFound(ApiResponse<object>.NotFound("Invalid email or password"));
                }

                return Ok(ApiResponse<LoginResponseDTO>.Ok(loginResponse, "Login successful"));
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                var response = ApiResponse<object>.InternalServerError($"An error occurred while processing the login, {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
