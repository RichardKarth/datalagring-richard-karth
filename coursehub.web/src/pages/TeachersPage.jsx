import { useEffect, useState } from "react";
import { api } from "../api/courseHubApi";

export default function TeachersPage() {
  const [items, setItems] = useState([]);
  const [error, setError] = useState("");

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");

  const [editingId, setEditingId] = useState(null);

  async function load() {
    setError("");
    try {
      setItems(await api.getTeachers());
    } catch (e) {
      setError(e.message);
    }
  }

  useEffect(() => {
  let cancelled = false;

  (async () => {
    setError("");
    try {
      const data = await api.getTeachers();
      if (!cancelled) setItems(data);
    } catch (e) {
      if (!cancelled) setError(e.message);
    }
  })();

  return () => { cancelled = true; };
}, []);

  async function save(e) {
    e.preventDefault();
    setError("");

    const dto = { firstName, lastName, email };

    try {
      if (editingId) await api.updateTeacher(editingId, dto);
      else await api.createTeacher(dto);

      setFirstName("");
      setLastName("");
      setEmail("");
      setEditingId(null);

      load();
    } catch (e) {
      setError(e.message);
    }
  }

  function startEdit(t) {
    setEditingId(t.id);
    setFirstName(t.firstName);
    setLastName(t.lastName);
    setEmail(t.email);
  }

  async function remove(id) {
    if (!confirm("Delete teacher?")) return;
    try {
      await api.deleteTeacher(id);
      load();
    } catch (e) {
      setError(e.message);
    }
  }

  return (
    <div>
      <h2>Teachers</h2>
      {error && <p className="error">{error}</p>}

      <form className="card" onSubmit={save}>
        <h3>{editingId ? "Edit Teacher" : "Add Teacher"}</h3>

        <label>First name</label>
        <input value={firstName} onChange={(e) => setFirstName(e.target.value)} required />

        <label>Last name</label>
        <input value={lastName} onChange={(e) => setLastName(e.target.value)} required />

        <label>Email</label>
        <input value={email} onChange={(e) => setEmail(e.target.value)} required />

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
          <p className="muted">No teachers yet.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {items.map((t) => (
                <tr key={t.id}>
                  <td>{t.firstName} {t.lastName}</td>
                  <td>{t.email}</td>
                  <td className="row">
                    <button onClick={() => startEdit(t)}>Edit</button>
                    <button onClick={() => remove(t.id)}>Delete</button>
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