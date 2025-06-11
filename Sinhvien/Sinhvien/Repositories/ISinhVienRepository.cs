namespace Sinhvien.Models
{
    public interface ISinhVienRepository
    {
        Task<IEnumerable<SinhVien>> GetAllAsync();
        Task<SinhVien> GetByIdAsync(string id); // Đã đổi kiểu từ int sang string
        Task AddAsync(SinhVien sinhVien);
        Task UpdateAsync(SinhVien sinhVien);
        Task DeleteAsync(string id);

    }
}
