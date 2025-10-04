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
    public class JourneyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JourneyController(AppDbContext context)
        {
            _context = context;
        }

        // [POST] /api/journey - tworzy podróż użytkownika z listą RideId
        [HttpPost]
        public async Task<IActionResult> CreateJourney([FromBody] List<int> rideIds)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można pobrać ID użytkownika.");

            if (rideIds == null || !rideIds.Any())
                return BadRequest("Podaj przynajmniej jeden RideId.");

            var journey = new Journey
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            int seq = 1;
            foreach (var rideId in rideIds)
            {
                journey.Segments.Add(new JourneySegment
                {
                    RideId = rideId,
                    Sequence = seq++
                });
            }

            _context.Journeys.Add(journey);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Podróż została zapisana.",
                journey.Id,
                rides = rideIds
            });
        }

        // [GET] /api/journey/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyJourneys()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Nie można pobrać ID użytkownika.");

            var journeys = await _context.Journeys
                .Where(j => j.UserId == userId)
                .Include(j => j.Segments)
                    .ThenInclude(s => s.Ride)
                .Select(j => new
                {
                    j.Id,
                    j.HasWarning,
                    j.SuggestedAlternative,
                    Segments = j.Segments.OrderBy(s => s.Sequence).Select(s => new
                    {
                        s.Sequence,
                        s.Ride.LineNumber,
                        s.Ride.StartStation,
                        s.Ride.EndStation,
                        s.Ride.ScheduledDeparture,
                        s.Ride.ScheduledArrival,
                        s.Ride.DelayMinutes
                    })
                })
                .ToListAsync();

            return Ok(journeys);
        }
    }
}
