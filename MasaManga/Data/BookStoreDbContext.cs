using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace MasaManga.Data
{
    public class BookStoreDbContext: DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(x => 
            {
                x.HasMany(t => t.Sections).WithOne().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<BookSection>(x => 
            {
                x.HasMany(t => t.Pics).WithOne().OnDelete(DeleteBehavior.Cascade);
            });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookSection> BookSections { get; set; }
        public DbSet<BookPic> BookPics { get; set; }
    }
}
