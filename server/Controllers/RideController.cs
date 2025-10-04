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

        // === [GET] Wszystkie przejazdy ===
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

        // === [POST] Dodaj nowy przejazd ===
        [HttpPost]
        public async Task<IActionResult> AddRide([FromBody] Ride ride)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();

            return Ok(ride);
        }

        // === [PUT] Aktualizuj dane przejazdu ===
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
    }
}
