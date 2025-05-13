using Baitap.Models;
using Microsoft.AspNetCore.Mvc;

namespace Baitap.Controllers
{
    public class StudentController1 : Controller
    {
        private static List<Students> students = new List<Students>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowKQ(Students sv)
        {
            students.Add(sv);
            int SL = students.Count(s => s.ChuyenNganh == sv.ChuyenNganh);
            ViewBag.MSSV = sv.MSSV;
            ViewBag.HoTen = sv.HoTen;
            ViewBag.ChuyenNganh = sv.ChuyenNganh;
            ViewBag.SoLuong = SL;
            return View();
        }
    }
}
