import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import AuthHeader from "../components/AuthHeader";
import "../styles/main.css";
import {
  FaTrain,
  FaBell,
  FaSignOutAlt,
  FaLock,
  FaMapMarkedAlt,
} from "react-icons/fa";

export default function UserProfile() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <AuthHeader />
      <div className="profile-container">
        <div className="profile-card">
          <h2 className="profile-title">🚆 Profil podróżnika</h2>

          <div className="user-summary">
            <div className="avatar-circle">
              <FaTrain size={30} color="#fff" />
            </div>
            <div className="user-text">
              <h3>{user?.displayName || "Użytkownik"}</h3>
              <p>{user?.email}</p>
              <span className="user-role">
                Rola: <b>{user?.role}</b>
              </span>
            </div>
          </div>

          <div className="stats-row">
            <div className="stat-card">
              <span className="stat-label">Reputacja</span>
              <span className="stat-value">{user?.reputation ?? 0}</span>
            </div>
            <div className="stat-card">
              <span className="stat-label">Zgłoszenia</span>
              <span className="stat-value">{user?.reports ?? 0}</span>
            </div>
            <div className="stat-card">
              <span className="stat-label">Dołączył</span>
              <span className="stat-value">
                {user?.joinedAt
                  ? new Date(user.joinedAt).toLocaleDateString("pl-PL")
                  : "—"}
              </span>
            </div>
          </div>

          <div className="profile-actions">
            <button
              className="profile-btn"
              onClick={() => navigate("/dodaj-alert")}
            >
              <FaBell /> Dodaj alert
            </button>

            <button
              className="profile-btn"
              onClick={() => navigate("/planowanie-podrozy")}
            >
              <FaMapMarkedAlt /> Zaplanuj podróż
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
