namespace IELTSMockApi.Models;

public class TestSession
{
    public int Id { get; set; }

    public int TestId { get; set; }
    public Test Test { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public bool IsSubmitted { get; set; } = false;
}
