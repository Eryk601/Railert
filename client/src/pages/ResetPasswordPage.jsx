import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { useEffect, useState } from "react";
import eyeOpen from "../assets/eyeopen.png";
import eyeClosed from "../assets/eyeclosed.png";
import AuthHeader from "../components/AuthHeader";
import Alert from "../components/Alert"; // 🟢 nowy komponent

export default function ResetPasswordPage() {
  const [params] = useSearchParams();
  const token = params.get("token") || "";
  const [newPassword, setNewPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const [show, setShow] = useState(false);
  const [alert, setAlert] = useState({ message: "", type: "" }); // 🟢 alert
  const [saving, setSaving] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (!token)
      setAlert({
        message: "Brak tokenu resetującego.",
        type: "error",
      });
  }, [token]);

  async function handleSubmit(e) {
    e.preventDefault();
    setAlert({ message: "", type: "" });

    if (!token) {
      setAlert({
        message: "Brak tokenu resetującego.",
        type: "error",
      });
      return;
    }
    if (newPassword !== confirm) {
      setAlert({
        message: "Hasła nie są identyczne.",
        type: "error",
      });
      return;
    }

    try {
      setSaving(true);
      const res = await fetch(
        "https://localhost:7265/api/Auth/reset-password",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ token, newPassword }),
        }
      );

      if (!res.ok) throw new Error(await res.text());

      setAlert({
        message: "Hasło zostało ustawione. Przekierowywanie...",
        type: "success",
      });

      setTimeout(() => {
        navigate("/logowanie", {
          state: { info: "Hasło zmienione. Zaloguj się nowym hasłem." },
        });
      }, 1200);
    } catch (e) {
      setAlert({
        message: "Nie udało się zmienić hasła. " + (e?.message || ""),
        type: "error",
      });
    } finally {
      setSaving(false);
    }
  }

  return (
    <>
      {/* === ALERT === */}
      {alert.message && (
        <Alert
          message={alert.message}
          type={alert.type}
          onClose={() => setAlert({ message: "", type: "" })}
        />
      )}

      {/* === HEADER === */}
      <AuthHeader />

      {/* === FORMULARZ === */}
      <div className="auth-container">
        <h2 className="auth-heading">USTAW NOWE HASŁO</h2>

        <form className="auth-form-r" onSubmit={handleSubmit}>
          <label className="password-input">
            Nowe hasło
            <input
              type={show ? "text" : "password"}
              placeholder="Nowe hasło"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
            />
            <img
              src={show ? eyeClosed : eyeOpen}
              alt={show ? "Ukryj hasło" : "Pokaż hasło"}
              title={show ? "Ukryj hasło" : "Pokaż hasło"}
              onClick={() => setShow(!show)}
              style={{ cursor: "pointer", width: 20, height: 20 }}
            />
          </label>

          <label>
            Potwierdź hasło
            <input
              type={show ? "text" : "password"}
              placeholder="Powtórz hasło"
              value={confirm}
              onChange={(e) => setConfirm(e.target.value)}
              required
            />
          </label>

          <button type="submit" className="auth-button" disabled={saving}>
            {saving ? "Zapisywanie…" : "USTAW NOWE HASŁO"}
          </button>

          <Link to="/logowanie" className="auth-link">
            WRÓĆ DO LOGOWANIA
          </Link>
        </form>
      </div>
    </>
  );
}
