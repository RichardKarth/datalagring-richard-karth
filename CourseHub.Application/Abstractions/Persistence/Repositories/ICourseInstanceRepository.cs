using CourseHub.Application.CourseInstances.PersistanceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Abstractions.Persistence.Repositories
{
    public interface ICourseInstanceRepository : IRepositoryBase<CourseInstance, int>
    {
    }
}
