using Microsoft.AspNetCore.Mvc;
using StARKS.Data;
using StARKS.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StARKS.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebAppContext _context;

        public HomeController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var students = _context.Student;
            var courses = _context.Course;
            var marks = _context.Mark;

            var studentList = students.Select(s => new StudentViewModel
            {
                StudentId = s.Id,
                StudentFullName = s.FirstName + " " + s.LastName,

            }).ToList();

            foreach (var student in studentList)
            {

                List<Mark> markList = new List<Mark>();
                foreach (var course in courses)
                {
                    Mark mark = (marks.FirstOrDefault(m => m.StudentId == student.StudentId && m.CourseCode == course.Code));
                    markList.Add(new Mark
                    {
                        CourseCode = course.Code,
                        StudentId = student.StudentId,
                        MarkValue = mark != null ? mark.MarkValue : default
                    });
                }

                student.Marks = markList;
            }

            var courseView = courses
                .Select(c => new CourseViewModel
                {
                    Code = c.Code,
                    Name = c.Name
                });

            return View(
                new MainTableViewModel()
                {
                    Courses = courseView.ToList(),
                    Students = studentList
                }
            );
        }

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
