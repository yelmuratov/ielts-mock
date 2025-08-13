namespace IELTSMockApi.Models;

public class TestResult
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TestId { get; set; }
    public Test Test { get; set; } = null!;

    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public int ScorePercent { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
