using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Services
{
    public class JourneyService
    {
        private readonly AppDbContext _context;

        public JourneyService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Sprawdza, czy użytkownik zdąży na przesiadki w swoich podróżach.
        /// Jeśli nie, ustawia HasWarning=true i proponuje alternatywną trasę.
        /// </summary>
        public async Task CheckConnectionsAsync()
        {
            var journeys = await _context.Journeys
                .Include(j => j.Segments)
                    .ThenInclude(s => s.Ride)
                .ToListAsync();

            foreach (var journey in journeys)
            {
                bool hasConflict = false;
                string? alternative = null;

                var segments = journey.Segments.OrderBy(s => s.Sequence).ToList();

                for (int i = 0; i < segments.Count - 1; i++)
                {
                    var current = segments[i].Ride;
                    var next = segments[i + 1].Ride;

                    // faktyczny przyjazd z opóźnieniem
                    var actualArrival = current.ScheduledArrival.AddMinutes(current.DelayMinutes);

                    // załóżmy, że pasażer potrzebuje min. 5 minut na przesiadkę
                    if (actualArrival.AddMinutes(5) > next.ScheduledDeparture)
                    {
                        hasConflict = true;

                        // znajdź alternatywę (np. kolejny pociąg)
                        alternative = await _context.Rides
                            .Where(r => r.StartStation == next.StartStation &&
                                        r.EndStation == next.EndStation &&
                                        r.ScheduledDeparture > next.ScheduledDeparture)
                            .OrderBy(r => r.ScheduledDeparture)
                            .Select(r => $"Alternatywa: {r.TransportType} {r.LineNumber} o {r.ScheduledDeparture:HH:mm}")
                            .FirstOrDefaultAsync();

                        break;
                    }
                }

                if (hasConflict)
                {
                    journey.HasWarning = true;
                    journey.SuggestedAlternative = alternative ?? "Brak alternatywy w systemie.";
                }
                else
                {
                    journey.HasWarning = false;
                    journey.SuggestedAlternative = null;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
