const BASE_URL = "https://localhost:7237"; // ändra vid behov

async function request(path, options = {}) {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `HTTP ${res.status}`);
  }

  // FIXEN – funkar för 200, 201, 204 och tom body
  const text = await res.text();
  return text ? JSON.parse(text) : null;
}

export const api = {
  // COURSES
  getCourses: () => request("/courses/"),
  createCourse: (dto) => request("/courses/", { method: "POST", body: JSON.stringify(dto) }),
  updateCourse: (id, dto) => request(`/courses/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  deleteCourse: (id) => request(`/courses/${id}`, { method: "DELETE" }),

  // TEACHERS
  getTeachers: () => request("/teachers/"),
  createTeacher: (dto) => request("/teachers/", { method: "POST", body: JSON.stringify(dto) }),
  updateTeacher: (id, dto) => request(`/teachers/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  deleteTeacher: (id) => request(`/teachers/${id}`, { method: "DELETE" }),

  // STUDENTS
  getStudents: () => request("/students/"),
  createStudent: (dto) => request("/students/", { method: "POST", body: JSON.stringify(dto) }),
  updateStudent: (id, dto) => request(`/students/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  deleteStudent: (id) => request(`/students/${id}`, { method: "DELETE" }),

  // INSTANCES
  getInstances: () => request("/course-instances/"),
  createInstance: (dto) => request("/course-instances/", { method: "POST", body: JSON.stringify(dto) }),
  updateInstance: (id, dto) => request(`/course-instances/${id}`, { method: "PUT", body: JSON.stringify(dto) }),
  deleteInstance: (id) => request(`/course-instances/${id}`, { method: "DELETE" }),

  // ENROLLMENTS
  enrollStudent: (studentId, courseInstanceId) =>
    request("/enrollments/", {
      method: "POST",
      body: JSON.stringify({ studentId, courseInstanceId }),
    }),

  unenrollStudent: (studentId, courseInstanceId) =>
    request(`/enrollments/students/${studentId}/instances/${courseInstanceId}`, {
      method: "DELETE",
    }),

  getStudentIdsInInstance: (courseInstanceId) =>
    request(`/enrollments/instances/${courseInstanceId}/students`),
};