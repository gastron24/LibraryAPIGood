using Microsoft.AspNetCore.Mvc;
using BookLibrary.Api.Data;
using BookLibrary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (author == null)
        {
            return NotFound("Автора Сьели Шимпанзе");
        }

        return author;
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Author>> UpdateAuthor(int id, Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }

        _context.Entry(author).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExist(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();


        [HttpDelete("{id}")]
        async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            if (author == null)
            {
                return NotFound("Автора спи**или шимпанзе");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
            
        }

         bool AuthorExist(int id) => _context.Authors.Any(e => e.Id == id);
    }
}
