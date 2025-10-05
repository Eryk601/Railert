using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Moderator")]
    public class RideController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RideController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rides = await _context.Rides
                .Include(r => r.Reports)
                .Select(r => new
                {
                    r.Id,
                    r.TransportType,
                    r.LineNumber,
                    r.StartStation,
                    r.EndStation,
                    r.DistanceKm,
                    r.ScheduledDeparture,
                    r.ScheduledArrival,
                    r.DelayMinutes,
                    r.IsCancelled,
                    ActiveReports = r.Reports.Count(rp => rp.IsActive)
                })
                .OrderBy(r => r.ScheduledDeparture)
                .ToListAsync();

            return Ok(rides);
        }

        [HttpPost]
        public async Task<IActionResult> AddRide([FromBody] Ride ride)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();

            return Ok(ride);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRide(int id, [FromBody] Ride updated)
        {
            var ride = await _context.Rides.FindAsync(id);
            if (ride == null) return NotFound();

            ride.DelayMinutes = updated.DelayMinutes;
            ride.IsCancelled = updated.IsCancelled;
            await _context.SaveChangesAsync();

            return Ok(ride);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchRides([FromQuery] string from, [FromQuery] string to, [FromQuery] TimeSpan? time)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Wymagane są parametry 'from' i 'to'.");

            var searchTime = time ?? DateTime.UtcNow.TimeOfDay;

            // przejazdy pasujące do stacji i daty
            var rides = await _context.Rides
                .Where(r =>
                    EF.Functions.ILike(r.StartStation, $"%{from}%") &&
                    EF.Functions.ILike(r.EndStation, $"%{to}%") &&
                    r.ScheduledDeparture.TimeOfDay >= searchTime)
                .OrderBy(r => r.ScheduledDeparture)
                .Select(r => new
                {
                    r.Id,
                    r.TransportType,
                    r.LineNumber,
                    r.StartStation,
                    r.EndStation,
                    r.ScheduledDeparture,
                    r.ScheduledArrival,
                    r.DelayMinutes,
                    r.IsCancelled
                })
                .ToListAsync();

            if (!rides.Any())
            {
                rides = await _context.Rides
                    .Where(r =>
                        EF.Functions.ILike(r.StartStation, $"%{from}%") &&
                        EF.Functions.ILike(r.EndStation, $"%{to}%"))
                    .OrderByDescending(r => r.ScheduledDeparture)
                    .Take(5)
                    .Select(r => new
                    {
                        r.Id,
                        r.TransportType,
                        r.LineNumber,
                        r.StartStation,
                        r.EndStation,
                        r.ScheduledDeparture,
                        r.ScheduledArrival,
                        r.DelayMinutes,
                        r.IsCancelled
                    })
                    .ToListAsync();
            }

            return Ok(rides);
        }
    }
}

