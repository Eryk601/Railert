import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

import AuthHeader from "../components/AuthHeader";

export default function ModeratorProfile() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <AuthHeader />
      <div className="dashboard-container">
        <h2 className="dashboard-heading">Panel Moderatora</h2>
        <div className="dashboard-grid">
          <button onClick={() => navigate("/moderacja-alertow")}>
            ⚠️ Sprawdź alerty użytkowników
          </button>
          <button onClick={() => navigate("/lista-przejazdow")}>
            🚆 Podgląd przejazdów
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
