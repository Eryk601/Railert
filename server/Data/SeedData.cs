using server.Models;
using server.Services;

namespace server.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Automatyczna migracja bazy, jeśli potrzeba
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin@railert.com",
                        PasswordHash = PasswordHelper.HashPassword("admin123"),
                        DisplayName = "Admin",
                        Role = UserRole.Admin,
                        IsEmailConfirmed = true,
                        ReputationPoints = 100
                    },
                    new User
                    {
                        Email = "moderator@railert.com",
                        PasswordHash = PasswordHelper.HashPassword("moderator123"),
                        DisplayName = "Moderator",
                        Role = UserRole.Moderator,
                        IsEmailConfirmed = true,
                        ReputationPoints = 50
                    },
                    new User
                    {
                        Email = "user@railert.com",
                        PasswordHash = PasswordHelper.HashPassword("user123"),
                        DisplayName = "Passenger",
                        Role = UserRole.Passenger,
                        IsEmailConfirmed = true,
                        ReputationPoints = 10
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
                Console.WriteLine("✅ SeedData: Użytkownicy startowi zostali dodani do bazy.");
            }
            else
            {
                Console.WriteLine("ℹ️ SeedData: Użytkownicy już istnieją — pomijam inicjalizację.");
            }
        }
    }
}
