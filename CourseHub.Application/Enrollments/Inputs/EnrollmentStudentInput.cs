using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Enrollments.Inputs;

public record EnrollmentStudentInput(int StudentId, int CourseInstanceId);
