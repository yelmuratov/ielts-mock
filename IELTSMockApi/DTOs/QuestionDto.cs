namespace IELTSMockApi.DTOs;

public class QuestionDto
{
    public int Id { get; set; }
    public int TestId { get; set; }

    public string Text { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    // CorrectAnswer is excluded on purpose
}


public class CreateQuestionDto
{
    public int TestId { get; set; }

    public string Text { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}
