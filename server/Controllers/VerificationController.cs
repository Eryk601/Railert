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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można pobrać ID użytkownika.");

            var report = await _context.Reports
                .Include(r => r.User)
                .Include(r => r.Ride)
                .FirstOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
                return NotFound("Nie znaleziono zgłoszenia.");

            // blokada głosowania po zakończeniu kursu lub nieaktywnym raporcie
            if (!report.IsActive || (report.Ride != null && report.Ride.ScheduledArrival <= DateTime.UtcNow))
                return BadRequest("Nie można głosować na nieaktywne lub zakończone zgłoszenie.");

            var existing = await _context.Verifications
                .FirstOrDefaultAsync(v => v.ReportId == reportId && v.UserId == userId);

            if (existing != null)
            {
                if (existing.IsConfirmed == confirm)
                    return BadRequest("Już oddałeś taki sam głos.");

                // Cofnięcie starego głosu
                if (existing.IsConfirmed)
                {
                    report.ConfirmationsCount--;
                    report.User.ReputationPoints--;
                }
                else
                {
                    report.RejectionsCount--;
                    report.User.ReputationPoints++;
                }

                existing.IsConfirmed = confirm;
            }
            else
            {
                existing = new Verification
                {
                    ReportId = reportId,
                    UserId = userId,
                    IsConfirmed = confirm,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Verifications.Add(existing);
            }

            // aktualizacja liczników i reputacji autora
            if (confirm)
            {
                report.ConfirmationsCount++;
                report.User.ReputationPoints++;
            }
            else
            {
                report.RejectionsCount++;
                report.User.ReputationPoints--;
            }

            // report.UpdateActiveStatus();
            report.IsActive = true;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = confirm ? "Zgłoszenie potwierdzone" : "Zgłoszenie odrzucone",
                report.Id,
                report.ConfirmationsCount,
                report.RejectionsCount,
                report.IsActive,
                authorReputation = report.User.ReputationPoints
            });
        }
    }
}
