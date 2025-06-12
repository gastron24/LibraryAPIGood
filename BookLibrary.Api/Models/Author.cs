namespace BookLibrary.Api.Models;

public class Author
{
    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
    public List<Book> Books { get; set; } = new();
}