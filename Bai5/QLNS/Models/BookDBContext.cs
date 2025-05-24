using Microsoft.EntityFrameworkCore;

namespace QLNS.Models
{
    public class BookDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookImage> Images { get; set; }

        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options) { }
    }
}
