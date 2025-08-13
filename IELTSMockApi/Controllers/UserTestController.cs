using IELTSMockApi.Data;
using IELTSMockApi.DTOs;
using IELTSMockApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IELTSMockApi.Controllers;

[ApiController]
[Route("api/test")]
[Authorize(Roles = "User")]
public class UserTestController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserTestController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/test/start
    [HttpPost("start")]
    public async Task<IActionResult> StartTest([FromBody] StartTestDto dto)
    {
        var test = await _context.Tests
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == dto.TestId);

        if (test == null)
            return NotFound("Test not found");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var session = new TestSession
        {
            TestId = test.Id,
            UserId = userId,
            StartedAt = DateTime.UtcNow
        };

        _context.TestSessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            SessionId = session.Id,
            TestId = test.Id,
            test.Title,
            test.DurationMinutes,
            StartedAt = session.StartedAt,
            Questions = test.Questions.Select(q => new
            {
                q.Id,
                q.Text,
                q.OptionA,
                q.OptionB,
                q.OptionC,
                q.OptionD
            })
        });
    }

    // POST: api/test/submit
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAnswers([FromBody] SubmitTestDto dto)
    {
        var session = await _context.TestSessions
            .Include(s => s.Test)
                .ThenInclude(t => t.Questions)
            .FirstOrDefaultAsync(s => s.Id == dto.SessionId);

        if (session == null)
            return NotFound("Test session not found");

        if (session.IsSubmitted)
            return BadRequest("Test has already been submitted.");

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (session.UserId != userId)
            return Forbid("You are not authorized to submit this test.");

        var elapsed = (DateTime.UtcNow - session.StartedAt).TotalMinutes;
        if (elapsed > session.Test.DurationMinutes)
            return BadRequest("Time expired. Submission rejected.");

        int total = session.Test.Questions.Count;
        int correct = 0;

        foreach (var answer in dto.Answers)
        {
            var question = session.Test.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question != null &&
                question.CorrectAnswer.Equals(answer.SelectedAnswer, StringComparison.OrdinalIgnoreCase))
            {
                correct++;
            }
        }

        int percent = (int)((double)correct / total * 100);

        var result = new TestResult
        {
            UserId = userId,
            TestId = session.TestId,
            CorrectAnswers = correct,
            TotalQuestions = total,
            ScorePercent = percent,
            SubmittedAt = DateTime.UtcNow
        };

        _context.TestResults.Add(result);
        session.IsSubmitted = true;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            TotalQuestions = total,
            CorrectAnswers = correct,
            ScorePercent = percent
        });
    }
}
