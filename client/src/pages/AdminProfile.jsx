import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import AuthHeader from "../components/AuthHeader";

export default function AdminProfile() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <AuthHeader />
      <div className="dashboard-container">
        <h2 className="dashboard-heading">Panel Administratora</h2>
        <div className="dashboard-grid">
          <button onClick={() => navigate("/zarzadzanie-przejazdami")}>
            🚆 Zarządzaj przejazdami
          </button>
          <button onClick={() => navigate("/zarzadzanie-uzytkownikami")}>
            👥 Zarządzaj użytkownikami
          </button>
          <button onClick={() => navigate("/moderacja-alertow")}>
            ⚠️ Moderacja alertów
          </button>
          <div style={{ textAlign: "center" }}>
            <button className="auth-button" onClick={logout}>
              Wyloguj
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
