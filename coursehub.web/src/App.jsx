import { Routes, Route, Navigate } from "react-router-dom";
import Nav from "./components/Nav";

import HomePage from "./pages/HomePage";
import CoursesPage from "./pages/CoursesPage";
import TeachersPage from "./pages/TeachersPage";
import StudentsPage from "./pages/StudentsPage";
import InstancesPage from "./pages/InstancesPage";

export default function App() {
  return (
    <div className="container">
      <h1>CourseHub</h1>
      <Nav />

      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/courses" element={<CoursesPage />} />
        <Route path="/teachers" element={<TeachersPage />} />
        <Route path="/students" element={<StudentsPage />} />
        <Route path="/instances" element={<InstancesPage />} />

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </div>
  );
}