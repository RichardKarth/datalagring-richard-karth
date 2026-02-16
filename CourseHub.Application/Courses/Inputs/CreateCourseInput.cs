using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Courses.Inputs
{
    public class CreateCourseInput
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationDays { get; set; }

    }
}
