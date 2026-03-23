import { useState } from "react";
import api from "../services/api";
import { useNavigate } from "react-router-dom";

function RegisterPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errorMsg, setError] = useState("");
  const navigate = useNavigate();
  async function register() {
    try {
      if (password !== confirmPassword) {
        setError("Passwords do not match!");
        return;
      }
      await api.post("/api/auth/register", { email, password })
      navigate("/login")
    } catch {
      setError("Email has already been registered")
    }
  }

  return (
    <>
      <div>Register Page</div>
      <form onSubmit={(e) => { e.preventDefault(); register() }}>
        <label htmlFor="email">Email</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <label htmlFor="password">Password</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
        />
        <button>Register</button>
      </form>
      {errorMsg && <p>{errorMsg}</p>}
    </>
  )
}

export default RegisterPage