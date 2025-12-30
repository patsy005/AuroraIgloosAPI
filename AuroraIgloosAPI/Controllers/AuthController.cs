using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuroraIgloosAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _cfg;

        public AuthController(CompanyContext context, IPasswordHasher<User> passwordHasher, IConfiguration cfg)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _cfg = cfg;
        }

        // POST: /api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginRequestDTO req)
        {
            if (string.IsNullOrWhiteSpace(req.Login) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Login and password are required");

            var user = await _context.User
                .Include(u => u.Role)
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(u => u.Login == req.Login);

            if (user is null)
                return Unauthorized("Invalid credentials");

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (verify == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials");

            var token = CreateJwt(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Login = user.Login,
                Role = user.Role?.Name ?? "",
                UserType = user.UserType?.Type ?? ""
            });
        }

        // GET: /api/auth/me
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MeResponseDTO>> Me()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var user = await _context.User
                .Include(u => u.Role)
                .Include(u => u.UserType)
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Person)
                .Include(u => u.Customer)
                    .ThenInclude(c => c.Person)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) return Unauthorized();

            var person = user.Employee?.Person ?? user.Customer?.Person;
            if (person is null) return BadRequest("User has no profile");

            return Ok(new MeResponseDTO
            {
                Id = user.Id,
                Login = user.Login,
                Role = user.Role?.Name ?? "",
                UserType = user.UserType?.Type ?? "",
                Name = person.Name ?? "",
                Surname = person.Surname ?? "",
                Email = person.Email ?? "",
                PhotoUrl = user.Employee?.PhotoUrl ?? "",
            });
        }

        private string CreateJwt(User user)
        {
            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
            };

            // to umożliwia [Authorize(Roles="Admin")]
            if (!string.IsNullOrWhiteSpace(user.Role?.Name))
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
            
            var expires = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
