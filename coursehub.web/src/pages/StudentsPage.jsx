import { useEffect, useState } from "react";
import { api } from "../api/courseHubApi";

export default function StudentsPage() {
  const [items, setItems] = useState([]);
  const [error, setError] = useState("");

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");

  const [editingId, setEditingId] = useState(null);

  async function load() {
    setError("");
    try {
      setItems(await api.getStudents());
    } catch (e) {
      setError(e.message);
    }
  }

  useEffect(() => {
  let cancelled = false;

  (async () => {
    setError("");
    try {
      const data = await api.getStudents();
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

    const dto = { firstName, lastName, email, phoneNumber: phoneNumber || null };

    try {
      if (editingId) await api.updateStudent(editingId, dto);
      else await api.createStudent(dto);

      setFirstName("");
      setLastName("");
      setEmail("");
      setPhoneNumber("");
      setEditingId(null);

      load();
    } catch (e) {
      setError(e.message);
    }
  }

  function startEdit(s) {
    setEditingId(s.id);
    setFirstName(s.firstName);
    setLastName(s.lastName);
    setEmail(s.email);
    setPhoneNumber(s.phoneNumber ?? "");
  }

  async function remove(id) {
    if (!confirm("Delete student?")) return;
    try {
      await api.deleteStudent(id);
      load();
    } catch (e) {
      setError(e.message);
    }
  }

  return (
    <div>
      <h2>Students</h2>
      {error && <p className="error">{error}</p>}

      <form className="card" onSubmit={save}>
        <h3>{editingId ? "Edit Student" : "Add Student"}</h3>

        <label>First name</label>
        <input value={firstName} onChange={(e) => setFirstName(e.target.value)} required />

        <label>Last name</label>
        <input value={lastName} onChange={(e) => setLastName(e.target.value)} required />

        <label>Email</label>
        <input value={email} onChange={(e) => setEmail(e.target.value)} required />

        <label>Phone</label>
        <input value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} />

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
          <p className="muted">No students yet.</p>
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
              {items.map((s) => (
                <tr key={s.id}>
                  <td>{s.firstName} {s.lastName}</td>
                  <td>{s.email}</td>
                  <td className="row">
                    <button onClick={() => startEdit(s)}>Edit</button>
                    <button onClick={() => remove(s.id)}>Delete</button>
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