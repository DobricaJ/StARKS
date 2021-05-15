using StARKS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StARKS.Models
{
    public class MainTableViewModel
    {
        public List<CourseViewModel> Courses { get; set; }

        public List<StudentViewModel> Students { get; set; }

    }

    public class CourseViewModel
    {
            public string Code { get; set; }

            public string Name { get; set; }
    }

    public class StudentViewModel
    {
        public int StudentId { get; set; }

        public string StudentFullName { get; set; }

        public List<Mark> Marks { get; set; }
    }
}
