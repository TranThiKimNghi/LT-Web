using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList
using Sinhvien.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Required for ToListAsync(), FindAsync(), Include()

namespace Sinhvien.Controllers
{
    public class SinhViensController : Controller
    {
        private readonly ApplicationDbContext _context; // Use DbContext directly

        // Constructor: Only inject ApplicationDbContext
        public SinhViensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SinhViens
        // Displays the list of students
        public async Task<IActionResult> Index()
        {
            // Get students directly from DbContext, including NganhHoc
            var sinhViens = await _context.SinhViens.Include(s => s.NganhHoc).ToListAsync();
            return View(sinhViens);
        }

        // GET: SinhViens/Add
        // Displays the form to add a new student
        public async Task<IActionResult> Add()
        {
            // Get list of NganhHoc directly from DbContext for the dropdown
            var nganhHocs = await _context.NganhHocs.ToListAsync();

            // Assign data to ViewBag for the dropdown list
            ViewBag.NganhHocs = new SelectList(nganhHocs, "MaNganh", "TenNganh");

            return View(); // Return the Add.cshtml View
        }

        // POST: SinhViens/Add
        // Handles adding a new student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SinhVien sinhVien, IFormFile hinhAnh)
        {
            // Remove ModelState validation for Hinh if you handle file upload separately and allow nulls
            ModelState.Remove("Hinh");

            if (ModelState.IsValid)
            {
                if (hinhAnh != null)
                {
                    sinhVien.Hinh = await SaveImage(hinhAnh); // Save image and assign path
                }

                _context.SinhViens.Add(sinhVien); // Add student to DB context
                await _context.SaveChangesAsync(); // Save changes to DB
                return RedirectToAction(nameof(Index)); // Redirect to the student list page
            }

            // If ModelState is invalid, reload NganhHoc list for the dropdown
            var nganhHocs = await _context.NganhHocs.ToListAsync();
            ViewBag.NganhHocs = new SelectList(nganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien); // Redisplay the form with validation errors
        }

        // GET: SinhViens/Update/{id}
        // Displays the form to update a student
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Find student directly from DbContext
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            // Get list of NganhHoc directly from DbContext for the dropdown
            var nganhHocs = await _context.NganhHocs.ToListAsync();
            ViewBag.NganhHocs = new SelectList(nganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // POST: SinhViens/Update/{id}
        // Handles updating a student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, SinhVien sinhVien, IFormFile hinhAnh)
        {
            ModelState.Remove("Hinh"); // Remove ModelState validation for Hinh

            if (id != sinhVien.MaSV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get existing student from DB for update
                var existingSinhVien = await _context.SinhViens.FindAsync(id);
                if (existingSinhVien == null)
                {
                    return NotFound();
                }

                if (hinhAnh == null)
                {
                    sinhVien.Hinh = existingSinhVien.Hinh; // Keep existing image if no new one is uploaded
                }
                else
                {
                    sinhVien.Hinh = await SaveImage(hinhAnh); // Save new image
                }

                // Update properties of existingSinhVien from the posted sinhVien model
                existingSinhVien.HoTen = sinhVien.HoTen;
                existingSinhVien.GioiTinh = sinhVien.GioiTinh;
                existingSinhVien.NgaySinh = sinhVien.NgaySinh;
                existingSinhVien.MaNganh = sinhVien.MaNganh;
                existingSinhVien.Hinh = sinhVien.Hinh;

                _context.SinhViens.Update(existingSinhVien); // Mark entity as modified
                await _context.SaveChangesAsync(); // Save changes to DB
                return RedirectToAction(nameof(Index));
            }
            // Reload NganhHoc list on validation failure
            var nganhHocs = await _context.NganhHocs.ToListAsync();
            ViewBag.NganhHocs = new SelectList(nganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // GET: SinhViens/Display/{id}
        // Displays student details
        public async Task<IActionResult> Display(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Get student and include NganhHoc for display
            var sinhVien = await _context.SinhViens.Include(s => s.NganhHoc).FirstOrDefaultAsync(s => s.MaSV == id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // GET: SinhViens/Delete/{id}
        // Displays the delete confirmation form
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Get student and include NganhHoc for display
            var sinhVien = await _context.SinhViens.Include(s => s.NganhHoc).FirstOrDefaultAsync(s => s.MaSV == id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhViens/Delete/{id}
        // Handles deleting a student
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Find student to delete
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound(); // Or handle other error if student doesn't exist when trying to delete
            }

            // Delete associated image file if it exists
            if (!string.IsNullOrEmpty(sinhVien.Hinh))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", sinhVien.Hinh.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.SinhViens.Remove(sinhVien); // Remove student from DB context
            await _context.SaveChangesAsync(); // Save changes to DB
            return RedirectToAction(nameof(Index));
        }

        // Helper method to save image (can be kept here or in a separate service)
        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName; // Return relative path
        }
    }
}
