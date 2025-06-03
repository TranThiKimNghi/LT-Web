using Microsoft.EntityFrameworkCore;

namespace Webbanhang.Models
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {

            return await _context.Categories
            .Include(p => p.Products)
            .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
           .Include(p => p.Products)
           .SingleOrDefaultAsync(p => p.Id == id);

        }


        public async Task AddAsync(Category product)
        {
            _context.Categories.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category product)
        {
            _context.Categories.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
