using BookLibrary.Api.Data;
using BookLibrary.Api.Models;
using BookLibrary.Api.Dto;
namespace BookLibrary.Api.Services;
using Microsoft.EntityFrameworkCore;

public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext _context;
   
    public AuthorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync();
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AuthorDto> CreateAuthorAsync(Author author)
    {
        _context.Add(author);
        await _context.SaveChangesAsync();

        return new AuthorDto()
        {
           Id = author.Id,
           Name = author.Name
        };
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
        
    
}