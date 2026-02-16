using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Courses.Outputs
{
    public class CourseOutput
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationDays { get; set; }
     
    }
}
