using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNS.Models;
using Microsoft.AspNetCore.Authorization;

namespace QLNS.Controllers
{

    public class BookController : Controller
    {
            private readonly BookDBContext _context;

            public BookController(BookDBContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

        public IActionResult Index(int? categoryId)
        {
            var books = _context.Books.Include(b => b.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                books = books.Where(b => b.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(books.ToList());
        }


        // GET: /Book/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: /Book/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _context.Books.Find(id.Value);
            if (book == null)
                return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // POST: /Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.Id == book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: /Book/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _context.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id.Value);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: /Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Book/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _context.Books.Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id.Value);

            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
