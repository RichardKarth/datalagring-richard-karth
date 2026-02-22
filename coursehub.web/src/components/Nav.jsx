import { NavLink } from "react-router-dom";

export default function Nav() {
  const linkStyle = ({ isActive }) => ({
    padding: "8px 10px",
    borderRadius: 8,
    textDecoration: "none",
    background: isActive ? "black" : "#eee",
    color: isActive ? "white" : "black",
  });

  return (
    <div className="nav">
      <NavLink to="/" style={linkStyle}>Home</NavLink>
      <NavLink to="/courses" style={linkStyle}>Courses</NavLink>
      <NavLink to="/teachers" style={linkStyle}>Teachers</NavLink>
      <NavLink to="/students" style={linkStyle}>Students</NavLink>
      <NavLink to="/instances" style={linkStyle}>Instances</NavLink>
    </div>
  );
}