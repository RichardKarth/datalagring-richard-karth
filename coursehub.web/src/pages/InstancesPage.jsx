import { useEffect, useMemo, useState } from "react";
import { api } from "../api/courseHubApi";

export default function InstancesPage() {
  const [instances, setInstances] = useState([]);
  const [courses, setCourses] = useState([]);
  const [teachers, setTeachers] = useState([]);
  const [students, setStudents] = useState([]);

  const [error, setError] = useState("");

  // form
  const [editingId, setEditingId] = useState(null);
  const [courseId, setCourseId] = useState("");
  const [teacherId, setTeacherId] = useState("");
  const [startDateUtc, setStartDateUtc] = useState("");
  const [endDateUtc, setEndDateUtc] = useState("");
  const [location, setLocation] = useState("");
  const [capacity, setCapacity] = useState(10);

  // enroll
  const [selectedInstanceId, setSelectedInstanceId] = useState(null);
  const [enrolledStudentIds, setEnrolledStudentIds] = useState([]);
  const [studentToEnroll, setStudentToEnroll] = useState("");

  async function loadAll() {
    setError("");
    const [inst, c, t, s] = await Promise.all([
      api.getInstances(),
      api.getCourses(),
      api.getTeachers(),
      api.getStudents(),
    ]);

    setInstances(inst);
    setCourses(c);
    setTeachers(t);
    setStudents(s);
  }

  useEffect(() => {
    let cancelled = false;

    (async () => {
      setError("");
      try {
        const [inst, c, t, s] = await Promise.all([
          api.getInstances(),
          api.getCourses(),
          api.getTeachers(),
          api.getStudents(),
        ]);

        if (cancelled) return;

        setInstances(inst);
        setCourses(c);
        setTeachers(t);
        setStudents(s);
      } catch (e) {
        if (!cancelled) setError(e.message);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, []);

  async function save(e) {
    e.preventDefault();
    setError("");

    const dto = {
      courseId: Number(courseId),
      teacherId: Number(teacherId),
      startDateUtc,
      endDateUtc,
      location,
      capacity: Number(capacity),
    };

    try {
      if (editingId) await api.updateInstance(editingId, dto);
      else await api.createInstance(dto);

      setEditingId(null);
      setCourseId("");
      setTeacherId("");
      setStartDateUtc("");
      setEndDateUtc("");
      setLocation("");
      setCapacity(10);

      await loadAll();
    } catch (e) {
      setError(e.message);
    }
  }

  function startEdit(i) {
    setEditingId(i.id);
    setCourseId(String(i.courseId));
    setTeacherId(String(i.teacherId));
    setStartDateUtc(i.startDateUtc);
    setEndDateUtc(i.endDateUtc);
    setLocation(i.location);
    setCapacity(i.capacity);
  }

  async function removeInstance(id) {
    if (!confirm("Delete instance?")) return;

    try {
      await api.deleteInstance(id);
      if (selectedInstanceId === id) setSelectedInstanceId(null);
      await loadAll();
    } catch (e) {
      setError(e.message);
    }
  }

  function courseTitle(id) {
    return courses.find((c) => c.id === id)?.title ?? `Course #${id}`;
  }

  function teacherName(id) {
    const t = teachers.find((x) => x.id === id);
    return t ? `${t.firstName} ${t.lastName}` : `Teacher #${id}`;
  }

  async function openEnrollments(instanceId) {
    setSelectedInstanceId(instanceId);
    setStudentToEnroll("");
    setError("");

    try {
      const ids = await api.getStudentIdsInInstance(instanceId);
      setEnrolledStudentIds(ids);
    } catch (e) {
      setError(e.message);
    }
  }

  const enrolledStudents = useMemo(() => {
    return enrolledStudentIds
      .map((id) => students.find((s) => s.id === id))
      .filter(Boolean);
  }, [enrolledStudentIds, students]);

  const availableStudents = useMemo(() => {
    return students.filter((s) => !enrolledStudentIds.includes(s.id));
  }, [students, enrolledStudentIds]);

  async function enroll() {
    if (!selectedInstanceId || !studentToEnroll) return;

    setError("");
    try {
      await api.enrollStudent(Number(studentToEnroll), Number(selectedInstanceId));
      const ids = await api.getStudentIdsInInstance(selectedInstanceId);
      setEnrolledStudentIds(ids);
      setStudentToEnroll("");
    } catch (e) {
      setError(e.message);
    }
  }

  async function unenroll(studentId) {
    if (!selectedInstanceId) return;
    if (!confirm("Remove student from instance?")) return;

    setError("");
    try {
      await api.unenrollStudent(studentId, selectedInstanceId);
      const ids = await api.getStudentIdsInInstance(selectedInstanceId);
      setEnrolledStudentIds(ids);
    } catch (e) {
      setError(e.message);
    }
  }

  return (
    <div>
      <h2>Instances</h2>
      {error && <p className="error">{error}</p>}

      <form className="card" onSubmit={save}>
        <h3>{editingId ? "Edit Instance" : "Add Instance"}</h3>

        <label>Course</label>
        <select value={courseId} onChange={(e) => setCourseId(e.target.value)} required>
          <option value="">-- select --</option>
          {courses.map((c) => (
            <option key={c.id} value={c.id}>{c.title}</option>
          ))}
        </select>

        <label>Teacher</label>
        <select value={teacherId} onChange={(e) => setTeacherId(e.target.value)} required>
          <option value="">-- select --</option>
          {teachers.map((t) => (
            <option key={t.id} value={t.id}>{t.firstName} {t.lastName}</option>
          ))}
        </select>

        <label>Start date</label>
        <input type="date" value={startDateUtc} onChange={(e) => setStartDateUtc(e.target.value)} required />

        <label>End date</label>
        <input type="date" value={endDateUtc} onChange={(e) => setEndDateUtc(e.target.value)} required />

        <label>Location</label>
        <input value={location} onChange={(e) => setLocation(e.target.value)} required />

        <label>Capacity</label>
        <input type="number" value={capacity} onChange={(e) => setCapacity(e.target.value)} min="1" required />

        <div className="row">
          <button type="submit">{editingId ? "Update" : "Create"}</button>
          {editingId && (
            <button type="button" onClick={() => setEditingId(null)}>
              Cancel
            </button>
          )}
        </div>
      </form>

      <div className="card">
        {instances.length === 0 ? (
          <p className="muted">No instances yet.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Course</th>
                <th>Teacher</th>
                <th>Dates</th>
                <th>Location</th>
                <th>Cap</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {instances.map((i) => (
                <tr key={i.id}>
                  <td>{courseTitle(i.courseId)}</td>
                  <td>{teacherName(i.teacherId)}</td>
                  {/* ASCII istället för pil */}
                  <td>{i.startDateUtc} - {i.endDateUtc}</td>
                  <td>{i.location}</td>
                  <td>{i.capacity}</td>
                  <td className="row">
                    <button onClick={() => openEnrollments(i.id)}>Enrollments</button>
                    <button onClick={() => startEdit(i)}>Edit</button>
                    <button onClick={() => removeInstance(i.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {selectedInstanceId && (
        <div className="card">
          <h3>Enrollments (Instance #{selectedInstanceId})</h3>

          <div className="row">
            <select value={studentToEnroll} onChange={(e) => setStudentToEnroll(e.target.value)}>
              <option value="">-- select student --</option>
              {availableStudents.map((s) => (
                <option key={s.id} value={s.id}>{s.firstName} {s.lastName}</option>
              ))}
            </select>
            <button onClick={enroll} disabled={!studentToEnroll}>Enroll</button>
          </div>

          {enrolledStudents.length === 0 ? (
            <p className="muted">No students enrolled.</p>
          ) : (
            <ul>
              {enrolledStudents.map((s) => (
                <li key={s.id} className="row">
                  <span>{s.firstName} {s.lastName} ({s.email})</span>
                  <button onClick={() => unenroll(s.id)}>Remove</button>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
}