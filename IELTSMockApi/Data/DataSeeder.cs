using IELTSMockApi.Models;

namespace IELTSMockApi.Data;

public static class DataSeeder
{
    public static void SeedDatabase(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Seed only if database is empty
        if (context.Tests.Any() || context.Users.Any()) return;

        // Seed users
        var admin = new User
        {
            Username = "admin",
            Email = "admin@ieltsmock.com",
            Password = "admin123", // Plaintext for dev/testing only
            Role = "Admin"
        };

        var user = new User
        {
            Username = "testuser",
            Email = "user@ieltsmock.com",
            Password = "user123", // Plaintext for dev/testing only
            Role = "User"
        };

        context.Users.AddRange(admin, user);

        // Seed test
        var sampleTest = new Test
        {
            Title = "Sample IELTS Reading Test",
            DurationMinutes = 20,
            Questions = new List<Question>
            {
                new()
                {
                    Text = "What is the capital of France?",
                    OptionA = "London",
                    OptionB = "Berlin",
                    OptionC = "Paris",
                    OptionD = "Rome",
                    CorrectAnswer = "C"
                },
                new()
                {
                    Text = "Which planet is known as the Red Planet?",
                    OptionA = "Earth",
                    OptionB = "Mars",
                    OptionC = "Jupiter",
                    OptionD = "Venus",
                    CorrectAnswer = "B"
                },
                new()
                {
                    Text = "Which language is used for .NET development?",
                    OptionA = "Python",
                    OptionB = "C#",
                    OptionC = "JavaScript",
                    OptionD = "Ruby",
                    CorrectAnswer = "B"
                }
            }
        };

        context.Tests.Add(sampleTest);
        context.SaveChanges();
    }
}
