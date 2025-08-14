using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using IELTSMockApi.Data;
using IELTSMockApi.Models;
using IELTSMockApi.DTOs;

namespace IELTSMockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuestionController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/question
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var questions = await _context.Questions
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                TestId = q.TestId
            }).ToListAsync();

        return Ok(questions);
    }

    // GET: api/question/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var q = await _context.Questions.FindAsync(id);
        if (q == null) return NotFound("Question not found");

        var dto = new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            OptionA = q.OptionA,
            OptionB = q.OptionB,
            OptionC = q.OptionC,
            OptionD = q.OptionD,
            TestId = q.TestId
        };

        return Ok(dto);
    }

    // POST: api/question
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto)
    {
        var testExists = await _context.Tests.AnyAsync(t => t.Id == dto.TestId);
        if (!testExists)
            return BadRequest("Invalid TestId. The test does not exist.");

        var question = new Question
        {
            TestId = dto.TestId,
            Text = dto.Text,
            OptionA = dto.OptionA,
            OptionB = dto.OptionB,
            OptionC = dto.OptionC,
            OptionD = dto.OptionD,
            CorrectAnswer = dto.CorrectAnswer
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = question.Id }, new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            OptionA = question.OptionA,
            OptionB = question.OptionB,
            OptionC = question.OptionC,
            OptionD = question.OptionD,
            TestId = question.TestId
        });
    }

    // PUT: api/question/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateQuestionDto dto)
    {
        var q = await _context.Questions.FindAsync(id);
        if (q == null) return NotFound("Question not found");

        var testExists = await _context.Tests.AnyAsync(t => t.Id == dto.TestId);
        if (!testExists)
            return BadRequest("Invalid TestId.");

        q.TestId = dto.TestId;
        q.Text = dto.Text;
        q.OptionA = dto.OptionA;
        q.OptionB = dto.OptionB;
        q.OptionC = dto.OptionC;
        q.OptionD = dto.OptionD;
        q.CorrectAnswer = dto.CorrectAnswer;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/question/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var q = await _context.Questions.FindAsync(id);
        if (q == null) return NotFound("Question not found");

        _context.Questions.Remove(q);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/question/by-test/{testId}
    [HttpGet("by-test/{testId}")]
    [Authorize]
    public async Task<IActionResult> GetByTest(int testId)
    {
        var questions = await _context.Questions
            .Where(q => q.TestId == testId)
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                TestId = q.TestId
            }).ToListAsync();

        return Ok(questions);
    }
}
