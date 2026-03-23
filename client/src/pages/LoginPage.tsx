import { useState } from "react"
import api from "../services/api";
import { useNavigate } from "react-router-dom";

function LoginPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [errorMsg, setError] = useState("");
    const navigate = useNavigate();
    async function login() {
        try {
            const response = await api.post("/api/auth/login", { email, password })
            localStorage.setItem("token", response.data.token);
            navigate("/dashboard");
        }
        catch {
            setError("Invalid email or password")
        }
    }
    return (
        <>
            <div>Login Page</div>
            <form onSubmit={(e) => { e.preventDefault(); login() }}>
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
                <button>Login</button>
            </form>
            {errorMsg && <p>{errorMsg}</p>}
        </>
    )
}

export default LoginPage