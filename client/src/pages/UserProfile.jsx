import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import AuthHeader from "../components/AuthHeader";

export default function UserProfile() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <AuthHeader />
      <div className="profile">
        <h2 className="profile__title">Panel podróżnika</h2>

        <div className="profile-grid-user">
          <section className="card-profile-user">
            <h3 className="card__title-user">Dane osobowe</h3>

            <div className="user-info">
              <p>
                <b>Email:</b> {user?.email || "brak"}
              </p>
              <p>
                <b>Nazwa użytkownika:</b> {user?.displayName}
              </p>
              <p>
                <b>Reputacja:</b> {user?.reputation}
              </p>
              <p>
                <b>Ilość zgłoszeń:</b> {user?.reports}
              </p>
            </div>

            <div className="user__actions">
              <button
                className="auth-button-user"
                onClick={() => navigate("/dodaj-alert")}
              >
                Dodaj alert
              </button>

              <button
                className="auth-button-user"
                onClick={() => navigate("/zmien-haslo")}
              >
                Zmień hasło
              </button>
            </div>
          </section>
        </div>

        <div style={{ marginTop: 20, textAlign: "center" }}>
          <button className="auth-button" onClick={logout}>
            Wyloguj
          </button>
        </div>
      </div>
    </>
  );
}
