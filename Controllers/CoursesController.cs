using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StARKS.Data;
using StARKS.Models;

namespace StARKS.Controllers
{
    public class CoursesController : Controller
    {
        private readonly WebAppContext _context;

        public CoursesController(WebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Code == id.ToString());
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Description")] Course course)
        {
            if (IsCourseCodeExist(course) == true)
            {
                ModelState.AddModelError("Code", "CourseCode already exists");
            }

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Description")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (IsCourseCodeExist(course) == true)
            {
                ModelState.AddModelError("Code", "CourseCode already exists");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        private bool IsCourseCodeExist(Course course)
        {
            return _context.Course.Any(x => x.Code == course.Code && x.Id != course.Id);
        }
    }
}
