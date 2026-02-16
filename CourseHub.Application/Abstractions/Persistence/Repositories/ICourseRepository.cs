using CourseHub.Application.Courses.PersistanceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Abstractions.Persistence.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course, int>
    {
    }
}
