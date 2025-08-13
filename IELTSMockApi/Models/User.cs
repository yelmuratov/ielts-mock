namespace IELTSMockApi.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // "Admin" or "User"
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
