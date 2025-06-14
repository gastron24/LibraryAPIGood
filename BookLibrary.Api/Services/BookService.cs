using BookLibrary.Api.Models;
using BookLibrary.Api.Dto;
using BookLibrary.Api.Data;
using Microsoft.EntityFrameworkCore;


namespace BookLibrary.Api.Services;

public class BookService : IBookService
{
  private readonly ApplicationDbContext _context;

  public BookService(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
  {
    return await _context.Books
      .Include(b => b.Author)
      .Select(b => new BookDto
      {
        Id = b.Id,
        Title = b.Title,
        Year = b.Year,
        AuthorId = b.AuthorId
      })
      .ToListAsync();
  }

  public async Task<BookDto?> GetBookByIdAsync(int id)
  {
    return await _context.Books
      .Include(b => b.Author)
      .Where(b => b.Id == id)
      .Select(b => new BookDto
      {
        Id = b.Id,
        Title = b.Title,
        Year = b.Year,
        AuthorId = b.AuthorId
      })
      .FirstOrDefaultAsync();
  }

  public async Task<BookDto> CreateBookAsync(Book book)
  {
    _context.Books.Add(book);
    await _context.SaveChangesAsync();

    return new BookDto()
    {
      Id = book.Id,
      Title = book.Title,
      Year = book.Year,
      AuthorId = book.AuthorId,
    };
  }

  public async Task UpdateBookAsync(Book book)
  {
    _context.Books.Update(book);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteBookAsync(int id)
  {
    var book = await _context.Books.FindAsync(id);
    if (book != null)
    {
      _context.Books.Remove(book);
      await _context.SaveChangesAsync();
    }
  }
}