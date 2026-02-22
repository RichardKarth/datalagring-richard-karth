import { useEffect, useState } from "react";
import { api } from "../api/courseHubApi";

export default function CoursesPage() {
  const [items, setItems] = useState([]);
  const [error, setError] = useState("");

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [durationDays, setDurationDays] = useState(1);

  const [editingId, setEditingId] = useState(null);

  async function load() {
    setError("");
    try {
      const data = await api.getCourses();
      setItems(data);
    } catch (e) {
      setError(e.message);
    }
  }

  useEffect(() => {
  let cancelled = false;

  (async () => {
    setError("");
    try {
      const data = await api.getCourses();
      if (!cancelled) setItems(data);
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

    const dto = { title, description: description || null, durationDays: Number(durationDays) };

    try {
      if (editingId) {
        await api.updateCourse(editingId, dto);
      } else {
        await api.createCourse(dto);
      }

      setTitle("");
      setDescription("");
      setDurationDays(1);
      setEditingId(null);

      load();
    } catch (e) {
      setError(e.message);
    }
  }

  function startEdit(c) {
    setEditingId(c.id);
    setTitle(c.title);
    setDescription(c.description ?? "");
    setDurationDays(c.durationDays);
  }

  async function remove(id) {
    if (!confirm("Delete course?")) return;
    try {
      await api.deleteCourse(id);
      load();
    } catch (e) {
      setError(e.message);
    }
  }

  return (
    <div>
      <h2>Courses</h2>
      {error && <p className="error">{error}</p>}

      <form className="card" onSubmit={save}>
        <h3>{editingId ? "Edit Course" : "Add Course"}</h3>

        <label>Title</label>
        <input value={title} onChange={(e) => setTitle(e.target.value)} required />

        <label>Description</label>
        <input value={description} onChange={(e) => setDescription(e.target.value)} />

        <label>DurationDays</label>
        <input
          type="number"
          value={durationDays}
          onChange={(e) => setDurationDays(e.target.value)}
          min="1"
          required
        />

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
        {items.length === 0 ? (
          <p className="muted">No courses yet.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Days</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {items.map((c) => (
                <tr key={c.id}>
                  <td>{c.title}</td>
                  <td>{c.durationDays}</td>
                  <td className="row">
                    <button onClick={() => startEdit(c)}>Edit</button>
                    <button onClick={() => remove(c.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}