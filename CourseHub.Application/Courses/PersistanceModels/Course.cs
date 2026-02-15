using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Courses.PersistanceModels
{
    public class Course
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public int DurationDays { get; private set; }

    }
}
