using Microsoft.EntityFrameworkCore;
// using System.Security.AccessControl; // Không cần thiết cho repository này, có thể xóa

using Sinhvien.Models;


namespace Sinhvien.Models
{
    // Đã đổi tên từ EFProductRepository sang EFSinhVienRepository
    public class EFSinhVienRepository : ISinhVienRepository // Kế thừa interface đã đổi tên
    {
        private readonly ApplicationDbContext _context;

        public EFSinhVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SinhVien>> GetAllAsync()
        {
            return await _context.SinhViens
                .Include(s => s.NganhHoc) // Include thông tin về Ngành học
                .ToListAsync();
        }

        public async Task<SinhVien> GetByIdAsync(string id) // Đã đổi kiểu từ int sang string
        {
            return await _context.SinhViens
                .Include(s => s.NganhHoc)
                // Đã sửa: Tìm kiếm theo MaSV thay vì MaNganh
                .SingleOrDefaultAsync(s => s.MaSV == id);
        }

        public async Task AddAsync(SinhVien sinhVien) // Đã đổi tên tham số để rõ ràng hơn
        {
            _context.SinhViens.Add(sinhVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SinhVien sinhVien) // Đã đổi tên tham số để rõ ràng hơn
        {
            _context.SinhViens.Update(sinhVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id) // Đã đổi kiểu từ int sang string
        {
            // Sử dụng FindAsync trực tiếp với khóa chính string
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null) // Kiểm tra null trước khi xóa
            {
                _context.SinhViens.Remove(sinhVien);
                await _context.SaveChangesAsync();
            }
            // Có thể throw exception hoặc log nếu không tìm thấy
        }
    }
}