namespace IELTSMockApi.DTOs;

public class SubmitTestDto
{
    public int SessionId { get; set; }

    public List<SubmitAnswerDto> Answers { get; set; } = new();
}

public class SubmitAnswerDto
{
    public int QuestionId { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
}
