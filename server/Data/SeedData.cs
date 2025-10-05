using server.Models;
using server.Services;
using Microsoft.EntityFrameworkCore;

namespace server.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // === USERS ===
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin",
                        PasswordHash = PasswordHelper.HashPassword("admin"),
                        DisplayName = "Admin",
                        Role = UserRole.Admin,
                        IsEmailConfirmed = true,
                        ReputationPoints = 100
                    },
                    new User
                    {
                        Email = "moderator",
                        PasswordHash = PasswordHelper.HashPassword("moderator"),
                        DisplayName = "Moderator",
                        Role = UserRole.Moderator,
                        IsEmailConfirmed = true,
                        ReputationPoints = 50
                    },
                    new User
                    {
                        Email = "user",
                        PasswordHash = PasswordHelper.HashPassword("user"),
                        DisplayName = "Passenger",
                        Role = UserRole.Passenger,
                        IsEmailConfirmed = true,
                        ReputationPoints = 10
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            var user = context.Users.First(u => u.Role == UserRole.Passenger);
            var moderator = context.Users.First(u => u.Role == UserRole.Moderator);

            // === RIDES ===
            if (!context.Rides.Any())
            {
                var now = DateTime.UtcNow;

                var rides = new List<Ride>
                {
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "IC135",
                        StartStation = "Kraków Główny",
                        EndStation = "Warszawa Centralna",
                        DistanceKm = 295.4,
                        ScheduledDeparture = now.AddHours(-2),
                        ScheduledArrival = now.AddMinutes(10),
                        DelayMinutes = 10,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "TLK451",
                        StartStation = "Katowice",
                        EndStation = "Wrocław Główny",
                        DistanceKm = 220.0,
                        ScheduledDeparture = now.AddHours(-1),
                        ScheduledArrival = now.AddHours(1),
                        DelayMinutes = 5,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "EIP3200",
                        StartStation = "Warszawa Centralna",
                        EndStation = "Gdańsk Główny",
                        DistanceKm = 330.0,
                        ScheduledDeparture = now.AddMinutes(30),
                        ScheduledArrival = now.AddHours(3),
                        DelayMinutes = 0,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "IC4321",
                        StartStation = "Kraków Główny",
                        EndStation = "Katowice",
                        DistanceKm = 80.0,
                        ScheduledDeparture = now.AddMinutes(-50),
                        ScheduledArrival = now.AddMinutes(10),
                        DelayMinutes = 15,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Tram,
                        LineNumber = "52",
                        StartStation = "Czerwone Maki",
                        EndStation = "Plac Centralny",
                        DistanceKm = 15.2,
                        ScheduledDeparture = now.AddMinutes(-30),
                        ScheduledArrival = now.AddMinutes(20),
                        DelayMinutes = 0,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Tram,
                        LineNumber = "8",
                        StartStation = "Bronowice Małe",
                        EndStation = "Borek Fałęcki",
                        DistanceKm = 14.0,
                        ScheduledDeparture = now.AddMinutes(-15),
                        ScheduledArrival = now.AddMinutes(10),
                        DelayMinutes = 2,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Bus,
                        LineNumber = "304",
                        StartStation = "Dworzec Główny Zachód",
                        EndStation = "Kopiec Kościuszki",
                        DistanceKm = 9.3,
                        ScheduledDeparture = now.AddMinutes(-20),
                        ScheduledArrival = now.AddMinutes(5),
                        DelayMinutes = 0,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Bus,
                        LineNumber = "501",
                        StartStation = "Nowy Bieżanów",
                        EndStation = "Azory",
                        DistanceKm = 16.5,
                        ScheduledDeparture = now.AddMinutes(-25),
                        ScheduledArrival = now.AddMinutes(15),
                        DelayMinutes = 8,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "IC6000",
                        StartStation = "Kraków Główny",
                        EndStation = "Poznań Główny",
                        DistanceKm = 450.0,
                        ScheduledDeparture = now.AddHours(-3),
                        ScheduledArrival = now.AddHours(1),
                        DelayMinutes = 25,
                        IsCancelled = false
                    },
                    new Ride
                    {
                        TransportType = TransportType.Train,
                        LineNumber = "EIP4500",
                        StartStation = "Poznań Główny",
                        EndStation = "Szczecin Główny",
                        DistanceKm = 270.0,
                        ScheduledDeparture = now.AddMinutes(15),
                        ScheduledArrival = now.AddHours(3),
                        DelayMinutes = 0,
                        IsCancelled = false
                    }
                };

                context.Rides.AddRange(rides);
                context.SaveChanges();
            }

            // === REPORTS ===
            if (!context.Reports.Any())
            {
                var ride2 = context.Rides.First(r => r.LineNumber == "TLK451");
                var ride3 = context.Rides.First(r => r.LineNumber == "IC6000");

                var reports = new List<Report>
                {
                    new Report
                    {
                        TransportType = TransportType.Train,
                        IncidentType = IncidentType.Breakdown,
                        LineNumber = ride2.LineNumber,
                        Title = "Awaria składu TLK451",
                        Description = "Awaria hamulców, skład stoi na stacji Katowice.",
                        Latitude = 50.2599,
                        Longitude = 19.0216,
                        LocationName = "Katowice",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                        ConfirmationsCount = 1,
                        RejectionsCount = 0,
                        IsActive = true,
                        UserId = moderator.Id,
                        RideId = ride2.Id
                    },
                    new Report
                    {
                        TransportType = TransportType.Train,
                        IncidentType = IncidentType.Delay,
                        LineNumber = ride3.LineNumber,
                        Title = "Pociąg IC6000 z opóźnieniem 25 minut",
                        Description = "Opóźnienie spowodowane awarią sieci trakcyjnej w okolicach Łodzi.",
                        Latitude = 52.1136,
                        Longitude = 19.4238,
                        LocationName = "Łódź Widzew",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-45),
                        ConfirmationsCount = 2,
                        RejectionsCount = 0,
                        IsActive = true,
                        UserId = user.Id,
                        RideId = ride3.Id
                    }
                };

                context.Reports.AddRange(reports);
                context.SaveChanges();
            }

            // === JOURNEYS ===
            if (!context.Journeys.Any())
            {
                var rideKrkWaw = context.Rides.First(r => r.LineNumber == "IC135");
                var rideWawGdansk = context.Rides.First(r => r.LineNumber == "EIP3200");
                var rideKrkKat = context.Rides.First(r => r.LineNumber == "IC4321");
                var rideKatWro = context.Rides.First(r => r.LineNumber == "TLK451");

                var journey1 = new Journey
                {
                    UserId = user.Id,
                    HasWarning = false,
                    SuggestedAlternative = null,
                    Segments = new List<JourneySegment>
                    {
                        new JourneySegment { Sequence = 1, RideId = rideKrkWaw.Id },
                        new JourneySegment { Sequence = 2, RideId = rideWawGdansk.Id }
                    }
                };

                var journey2 = new Journey
                {
                    UserId = user.Id,
                    HasWarning = false,
                    SuggestedAlternative = null,
                    Segments = new List<JourneySegment>
                    {
                        new JourneySegment { Sequence = 1, RideId = rideKrkKat.Id },
                        new JourneySegment { Sequence = 2, RideId = rideKatWro.Id }
                    }
                };

                context.Journeys.AddRange(journey1, journey2);
                context.SaveChanges();
            }
        }
    }
}
