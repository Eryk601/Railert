import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import AuthHeader from "../components/AuthHeader";
import { FaLock, FaSignOutAlt } from "react-icons/fa";

export default function AdminProfile() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <AuthHeader />
      <div className="profile-container">
        <div className="profile-card">
          <h2 className="profile-title">👨‍💼 Profil Administratora</h2>

          <div className="profile-actions">
            <button
              className="profile-btn"
              onClick={() => navigate("/zarzadzanie-przejazdami")}
            >
              🚆 Zarządzaj przejazdami
            </button>
            <button
              className="profile-btn"
              onClick={() => navigate("/zarzadzanie-uzytkownikami")}
            >
              👥 Zarządzaj użytkownikami
            </button>
            <button
              className="profile-btn"
              onClick={() => navigate("/dodaj-alert")}
            >
              ⚠️ Dodawanie alertów
            </button>

            <button
              className="profile-btn"
              onClick={() => navigate("/zmien-haslo")}
            >
              <FaLock /> Zmień hasło
            </button>
          </div>

          <button className="logout-btn" onClick={logout}>
            <FaSignOutAlt /> Wyloguj
          </button>
        </div>
      </div>
    </>
  );
}
