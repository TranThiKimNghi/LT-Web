using Microsoft.AspNetCore.Mvc;
using Sinhvien.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Để sử dụng [Authorize]
using Microsoft.EntityFrameworkCore; // Cần thiết khi làm việc trực tiếp với DbContext

namespace Sinhvien.Controllers
{
    [Authorize] // Yêu cầu đăng nhập để truy cập tất cả các action trong controller này
    public class HocPhansController : Controller
    {
        private readonly IHocPhanRepository _hocPhanRepository; // Repository cho HocPhan
        private readonly ApplicationDbContext _context; // DbContext để thao tác với DangKy và ChiTietDangKy

        public HocPhansController(IHocPhanRepository hocPhanRepository, ApplicationDbContext context) // Inject cả hai
        {
            _hocPhanRepository = hocPhanRepository;
            _context = context;
        }

        // GET: HocPhans
        // Hiển thị danh sách các học phần
        public async Task<IActionResult> Index()
        {
            // Lấy dữ liệu HocPhan thông qua Repository
            var hocPhans = await _hocPhanRepository.GetAllAsync();
            return View(hocPhans);
        }

        // POST: HocPhans/DangKy/{maHP}
        // Xử lý đăng ký học phần cho sinh viên đã đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(string maHP)
        {
            if (string.IsNullOrEmpty(maHP))
            {
                // Xử lý trường hợp không có mã học phần
                TempData["ErrorMessage"] = "Mã học phần không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            // Lấy MaSV của sinh viên đang đăng nhập (MaSV được dùng làm UserName trong Identity)
            string maSV = User.Identity.Name;

            if (string.IsNullOrEmpty(maSV))
            {
                // Nếu không lấy được MaSV từ User.Identity (chưa đăng nhập), chuyển hướng đến trang đăng nhập
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // 1. Tìm hoặc tạo bản ghi DangKy (header đăng ký) cho sinh viên này
            var dangKyHeader = await _context.DangKys
                                             .FirstOrDefaultAsync(dk => dk.MaSV == maSV);

            if (dangKyHeader == null)
            {
                // Nếu chưa có DangKy header cho sinh viên này, tạo mới
                dangKyHeader = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Now // Sử dụng NgayDK theo model mới
                };
                _context.DangKys.Add(dangKyHeader);
                await _context.SaveChangesAsync(); // Lưu để có MaDK
            }

            // 2. Kiểm tra xem sinh viên đã đăng ký học phần này (ChiTietDangKy) chưa
            // Kiểm tra trên bảng ChiTietDangKy với MaDK (từ header) và MaHP
            var existingChiTietDangKy = await _context.ChiTietDangKys
                                                      .FirstOrDefaultAsync(ct => ct.MaDK == dangKyHeader.MaDK && ct.MaHP == maHP);

            if (existingChiTietDangKy != null)
            {
                TempData["ErrorMessage"] = "Bạn đã đăng ký học phần này rồi.";
                return RedirectToAction(nameof(Index));
            }

            // 3. Nếu chưa đăng ký, tạo một ChiTietDangKy mới
            var chiTietDangKy = new ChiTietDangKy
            {
                MaDK = dangKyHeader.MaDK, // Lấy MaDK từ header đã tìm/tạo
                MaHP = maHP // Mã học phần
            };

            _context.ChiTietDangKys.Add(chiTietDangKy);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đăng ký học phần '{maHP}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Bạn có thể thêm các action CRUD khác cho Học phần nếu cần (Add, Update, Delete)
        // Hiện tại chỉ tập trung vào hiển thị và đăng ký.
    }
}
