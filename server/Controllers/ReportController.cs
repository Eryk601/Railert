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
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _context.Reports
                .Where(r => r.IsActive)
                .Include(r => r.User)
                .Include(r => r.Ride)
                .Select(r => new ReportListResponse
                {
                    Id = r.Id,
                    TransportTypeName = r.TransportType.ToString(),
                    LineNumber = r.LineNumber,
                    Title = r.Title,
                    Description = r.Description,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    LocationName = r.LocationName,
                    CreatedAt = r.CreatedAt,
                    IsActive = r.IsActive,
                    ConfirmationsCount = r.ConfirmationsCount,
                    RejectionsCount = r.RejectionsCount,

                    UserDisplayName = string.IsNullOrEmpty(r.User.DisplayName)
                        ? "Użytkownik anonimowy"
                        : r.User.DisplayName,
                    ScheduledArrival = r.Ride != null ? r.Ride.ScheduledArrival : null
                })
                .ToListAsync();

            return Ok(reports);
        }

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
                IsActive = true,
                UserId = userId,
                RideId = request.RideId
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            if (request.RideId.HasValue && request.DelayMinutes.HasValue && request.DelayMinutes.Value > 0)
            {
                var ride = await _context.Rides
                    .Include(r => r.Reports)
                    .FirstOrDefaultAsync(r => r.Id == request.RideId.Value);

                if (ride != null)
                {
                    // wszystkie raporty z wypełnionym DelayMinutes > 0
                    var delayReports = await _context.Reports
                        .Where(r => r.RideId == ride.Id && r.Description != null)
                        .ToListAsync();

                    // średnia ze wszystkich zgłoszeń z DelayMinutes
                    var delays = delayReports
                        .Select(r =>
                        {
                            return request.DelayMinutes.Value;
                        })
                        .ToList();

                    delays.Add(request.DelayMinutes.Value);

                    if (delays.Any())
                    {
                        var avgDelay = (int)Math.Round(delays.Average());

                        // Aktualizacja przejazdu
                        ride.DelayMinutes = avgDelay;
                        ride.ScheduledArrival = ride.ScheduledArrival.AddMinutes(avgDelay);

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return Ok(new
            {
                message = "Zgłoszenie dodane pomyślnie.",
                report.Id,
                report.Title,
                report.IncidentType,
                report.LineNumber,
                report.CreatedAt
            });
        }
    }
}
