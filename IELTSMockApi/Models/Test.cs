using System.ComponentModel.DataAnnotations;

namespace IELTSMockApi.Models;

public class Test
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Range(1, 300)]
    public int DurationMinutes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Question> Questions { get; set; } = new();
}
