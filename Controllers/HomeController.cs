using Microsoft.AspNetCore.Mvc;
using StARKS.Data;
using StARKS.Factores;
using StARKS.Models;
using System.Diagnostics;
using System.Linq;

namespace StARKS.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebAppContext _context;
        private readonly ViewModelFactory _viewModelFacotory;

        public HomeController(WebAppContext context, ViewModelFactory factory)
        {
            _context = context;
            _viewModelFacotory = factory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View
                ( 
                    _viewModelFacotory.CreateMainTableView() 
                );
        }

        // Create mark if don't exist, or edit if exist, of delete mark if the user submit empty MarkValue
        [HttpPost]
        public JsonResult MarkProcessing([Bind("CourseCode,StudentId,MarkValue")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                if (!IsMarkExists(mark))
                {
                     CreateMark(mark);
                }
                else
                {
                    EditMark(mark);
                }

                //  Send "Success"
                return Json(new { success = true, responseText = "Your message successfuly sent!" });
            }
            //  Send "false"
            return Json(new { success = false, responseText = "The attached file is not supported." });
        }


        public void CreateMark(Mark mark)
        {
           _context.Add(mark);
           _context.SaveChanges();
        }

        //Edit or delete
        public void EditMark(Mark mark)
        {
            var markDb =  _context.Mark.FirstOrDefault(e => e.CourseCode == mark.CourseCode && e.StudentId == mark.StudentId);

            if(mark.MarkValue != null)
            {
                 markDb.MarkValue = mark.MarkValue;
                _context.Update(markDb);
            }
            else
            {
                _context.Mark.Remove(markDb);
            }
            _context.SaveChanges();

        }

        public void DeleteMark(int id)
        {
            var mark = _context.Mark.Find(id);
            _context.Mark.Remove(mark);
            _context.SaveChanges();
        }

        private bool IsMarkExists(Mark mark)
        {
            return _context.Mark.Any(e => e.CourseCode == mark.CourseCode && e.StudentId == mark.StudentId);
        }

        [HttpGet]
        public IActionResult FilterStudents(string searchTerm)
        {
           var students = _context.Student
                .Where(s =>
                s.FirstName.Contains(searchTerm) ||
                s.LastName.Contains(searchTerm));

            return Json(new
            {
                results = students.Select(s => new
                {
                    id = s.Id,
                    text = s.FirstName + " " + s.LastName,
                })
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
