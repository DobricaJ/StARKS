using StARKS.Data;
using StARKS.Models;
using System.Collections.Generic;
using System.Linq;

namespace StARKS.Factores
{
    public class ViewModelFactory
    {
        private readonly WebAppContext _context;

        public ViewModelFactory(WebAppContext context)
        {
            _context = context;
        }

        public MainTableViewModel CreateMainTableView()
        {

            var students = _context.Student;
            var courses = _context.Course;
            var marks = _context.Mark;

            var studentList = students.Select(s => new StudentViewModel
            {
                StudentId = s.Id,
                StudentFullName = s.FirstName + " " + s.LastName,

            }).ToList();

            // For every Student add the Marks for each Couse
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

            return new MainTableViewModel()
            {
                Courses = courseView.ToList(),
                Students = studentList
            };
        }
    }
}
