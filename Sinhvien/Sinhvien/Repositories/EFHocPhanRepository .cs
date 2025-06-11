using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sinhvien.Models
{
    public class EFHocPhanRepository : IHocPhanRepository
    {
        private readonly ApplicationDbContext _context;

        public EFHocPhanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đã sửa kiểu trả về và DbSet thành HocPhans
        public async Task<IEnumerable<HocPhan>> GetAllAsync()
        {
            return await _context.HocPhans.ToListAsync();
        }

        // Đã sửa kiểu trả về và DbSet thành HocPhans
        public async Task<HocPhan> GetByIdAsync(string id)
        {
            return await _context.HocPhans.FindAsync(id);
        }

        // Đã sửa kiểu tham số và DbSet thành HocPhans
        public async Task AddAsync(HocPhan hocPhan)
        {
            _context.HocPhans.Add(hocPhan);
            await _context.SaveChangesAsync();
        }

        // Đã sửa kiểu tham số và DbSet thành HocPhans
        public async Task UpdateAsync(HocPhan hocPhan)
        {
            _context.HocPhans.Update(hocPhan);
            await _context.SaveChangesAsync();
        }

        // Đã sửa DbSet thành HocPhans
        public async Task DeleteAsync(string id)
        {
            var hocPhan = await _context.HocPhans.FindAsync(id);
            if (hocPhan != null)
            {
                _context.HocPhans.Remove(hocPhan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
