using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using System.Security.Claims;
using server.DTOs;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // tylko zalogowani użytkownicy mogą dodawać zgłoszenia
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // === GET: /api/report (publiczny podgląd wszystkich zgłoszeń) ===
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _context.Reports
                .Where(r => r.IsActive)
                .Include(r => r.User)
                .Select(r => new
                {
                    r.Id,
                    r.TransportType,
                    r.LineNumber,
                    r.IncidentType,
                    r.Title,
                    r.Description,
                    r.Latitude,
                    r.Longitude,
                    r.LocationName,
                    r.CreatedAt,
                    r.ConfirmationsCount,
                    r.RejectionsCount,
                    UserDisplayName = string.IsNullOrEmpty(r.User.DisplayName)
                        ? "Użytkownik anonimowy"
                        : r.User.DisplayName
                })
                .ToListAsync();

            return Ok(reports);
        }

        // === POST: /api/report ===
        [HttpPost]
        public async Task<IActionResult> AddReport([FromBody] CreateReportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można pobrać ID użytkownika.");

            var report = new Report
            {
                TransportType = request.TransportType,
                IncidentType = request.IncidentType,
                LineNumber = request.LineNumber,
                Title = request.Title,
                Description = request.Description,
                Latitude = 50.0676,
                Longitude = 19.9864,
                LocationName = "Tauron Arena Kraków",
                CreatedAt = DateTime.UtcNow,
                ConfirmationsCount = 0,
                RejectionsCount = 0,
                UserId = userId
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }

    }
}
