import { useNavigate } from "react-router-dom";

function LogOutButton() {
    const navigate = useNavigate();
    function logout() {
        localStorage.removeItem("token");
        navigate("/login");
    }
    return (
        <>
            <button
                onClick={() => logout()}
            >Log out</button>
        </>
    )
}
export default LogOutButton