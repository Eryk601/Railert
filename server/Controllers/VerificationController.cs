using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VerificationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VerificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{reportId}")]
        public async Task<IActionResult> VerifyReport(int reportId, [FromQuery] bool confirm)
        {
            // pobierz userId z JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można pobrać ID użytkownika.");

            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
                return NotFound("Nie znaleziono zgłoszenia.");

            // sprawdź, czy użytkownik już głosował
            var existing = await _context.Verifications
                .FirstOrDefaultAsync(v => v.ReportId == reportId && v.UserId == userId);

            if (existing != null)
            {
                // jeśli już głosował tak samo, nie rób nic
                if (existing.IsConfirmed == confirm)
                    return BadRequest("Już oddałeś taki sam głos na to zgłoszenie.");

                // jeśli zmienia zdanie → zaktualizuj liczby
                if (existing.IsConfirmed)
                    report.ConfirmationsCount--;
                else
                    report.RejectionsCount--;

                existing.IsConfirmed = confirm;
            }
            else
            {
                // nowy głos
                existing = new Verification
                {
                    ReportId = reportId,
                    UserId = userId,
                    IsConfirmed = confirm,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Verifications.Add(existing);
            }

            // aktualizacja liczników
            if (confirm)
                report.ConfirmationsCount++;
            else
                report.RejectionsCount++;

            // zaktualizuj IsActive w modelu Report
            report.UpdateActiveStatus();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = confirm ? "Zgłoszenie potwierdzone" : "Zgłoszenie odrzucone",
                report.Id,
                report.ConfirmationsCount,
                report.RejectionsCount,
                report.IsActive
            });
        }
    }
}
