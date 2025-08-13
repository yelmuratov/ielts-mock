using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IELTSMockApi.Models;

public class Question
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string OptionA { get; set; } = string.Empty;

    [Required]
    public string OptionB { get; set; } = string.Empty;

    [Required]
    public string OptionC { get; set; } = string.Empty;

    [Required]
    public string OptionD { get; set; } = string.Empty;

    [Required]
    [RegularExpression("A|B|C|D")]
    public string CorrectAnswer { get; set; } = string.Empty; // "A", "B", "C", or "D"

    // Foreign key
    public int TestId { get; set; }

    // Navigation
    [JsonIgnore]
    public Test Test { get; set; } = null!;
}
