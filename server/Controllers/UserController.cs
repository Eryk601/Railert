using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Models;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można uzyskać identyfikatora użytkownika.");

            var user = await _context.Users
                .Include(u => u.Reports)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("Użytkownik nie istnieje.");

            var reportCount = user.Reports.Count;

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                displayName = user.DisplayName,
                city = user.City ?? "nie podano",
                role = user.Role.ToString(),
                reputation = user.ReputationPoints,
                reports = reportCount,
                joinedAt = user.CreatedAt.ToString("yyyy-MM-dd")
            });
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers(
            string? search = null,
            string? roleFilter = null,
            string? city = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u =>
                    u.Email.Contains(search) ||
                    (u.DisplayName != null && u.DisplayName.Contains(search)));

            if (!string.IsNullOrWhiteSpace(roleFilter) &&
                Enum.TryParse<UserRole>(roleFilter, true, out var roleEnum))
            {
                query = query.Where(u => u.Role == roleEnum);
            }

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(u => u.City != null && u.City.Contains(city));

            var total = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.ReputationPoints)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    DisplayName = u.DisplayName,
                    Role = u.Role.ToString(),
                    City = u.City,
                    ReputationPoints = u.ReputationPoints,
                    ReportsCount = u.Reports.Count,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(new { total, users });
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("reputation/{userId}")]
        public async Task<IActionResult> UpdateReputation(int userId, [FromBody] UpdateReputationRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("Nie znaleziono użytkownika.");

            user.ReputationPoints = request.ReputationPoints;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reputacja użytkownika została zaktualizowana." });
        }
    }
}
