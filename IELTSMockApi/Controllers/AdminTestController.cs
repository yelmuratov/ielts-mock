using IELTSMockApi.Data;
using IELTSMockApi.DTOs;
using IELTSMockApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IELTSMockApi.Controllers;

[ApiController]
[Route("api/admin/tests")]
[Authorize(Roles = "Admin")] 
public class AdminTestController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminTestController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/admin/tests
    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestDto dto)
    {
        var test = new Test
        {
            Title = dto.Title,
            DurationMinutes = dto.DurationMinutes
            // Questions are created separately
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTestById), new { id = test.Id }, new
        {
            test.Id,
            test.Title,
            test.DurationMinutes,
            test.CreatedAt
        });
    }

    // GET: api/admin/tests
    [HttpGet]
    public async Task<IActionResult> GetAllTests()
    {
        var tests = await _context.Tests
            .Include(t => t.Questions)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.DurationMinutes,
                t.CreatedAt,
                QuestionCount = t.Questions.Count
            })
            .ToListAsync();

        return Ok(tests);
    }

    // GET: api/admin/tests/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTestById(int id)
    {
        var test = await _context.Tests
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null)
            return NotFound();

        var result = new
        {
            test.Id,
            test.Title,
            test.DurationMinutes,
            test.CreatedAt,
            Questions = test.Questions.Select(q => new
            {
                q.Id,
                q.Text,
                q.OptionA,
                q.OptionB,
                q.OptionC,
                q.OptionD
                // No correct answers exposed here either
            }).ToList()
        };

        return Ok(result);
    }

    // DELETE: api/admin/tests/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test == null) return NotFound();

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
