using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sinhvien.Models
{
    public interface IHocPhanRepository
    {
        Task<IEnumerable<HocPhan>> GetAllAsync();
        Task<HocPhan> GetByIdAsync(string id);
        Task AddAsync(HocPhan hocPhan); // Đã đổi tên tham số từ nganhHoc sang hocPhan
        Task UpdateAsync(HocPhan hocPhan); // Đã đổi tên tham số từ nganhHoc sang hocPhan
        Task DeleteAsync(string id);
    }
}
