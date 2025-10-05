import { useEffect, useState, useCallback } from "react";
import { useAuth } from "../context/AuthContext";
import { Link } from "react-router-dom";

export default function CheckAlert() {
  const { user } = useAuth();
  const [alerts, setAlerts] = useState([]);
  const [loading, setLoading] = useState(true);

  // ✅ używamy useCallback, żeby uniknąć ostrzeżeń ESLint
  const fetchAlerts = useCallback(async () => {
    try {
      const res = await fetch("https://localhost:7265/api/report", {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${user?.token}`,
        },
      });
      if (!res.ok) throw new Error("Błąd pobierania danych");
      const data = await res.json();
      setAlerts(data);
    } catch (err) {
      console.error("Błąd:", err);
    } finally {
      setLoading(false);
    }
  }, [user?.token]);

  useEffect(() => {
    fetchAlerts();
  }, [fetchAlerts]); // ✅ poprawne zależności, bez ostrzeżeń

  // === WYZNACZANIE ŚCIEŻKI POWROTU W ZALEŻNOŚCI OD ROLI ===
  function getReturnPath() {
    if (!user) return "/";
    switch (user.role) {
      case "Admin":
        return "/profil-admina";
      case "Moderator":
        return "/profil-moderatora";
      case "Passenger":
      default:
        return "/profil-uzytkownika";
    }
  }

  return (
    <div
      className="alerts-panel"
      style={{ maxWidth: "1000px", margin: "40px auto" }}
    >
      <Link to={getReturnPath()} className="back-btn">
        ← Powrót
      </Link>

      <h2>🚨 Zarządzanie alertami użytkowników</h2>

      {loading ? (
        <p>Ładowanie danych...</p>
      ) : (
        <table className="alerts-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Użytkownik</th>
              <th>Tytuł</th>
              <th>Opis</th>
              <th>Linia</th>
              <th>Data zgłoszenia</th>
              <th>Status</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {alerts.length === 0 ? (
              <tr>
                <td colSpan="8">Brak zgłoszeń użytkowników</td>
              </tr>
            ) : (
              alerts.map((r) => (
                <tr key={r.id}>
                  <td>{r.id}</td>
                  <td>{r.userDisplayName || "Anonim"}</td>
                  <td>{r.title || "—"}</td>
                  <td>{r.description || "—"}</td>
                  <td>{r.lineNumber || "—"}</td>
                  <td>
                    {r.createdAt
                      ? new Date(r.createdAt).toLocaleString("pl-PL", {
                          dateStyle: "medium",
                          timeStyle: "short",
                        })
                      : "—"}
                  </td>
                  <td>
                    <span
                      className={`status-dot ${
                        r.isActive ? "green" : "cancelled"
                      }`}
                    ></span>
                  </td>
                  <td>
                    <button className="confirm-btn">✅ Akceptuj</button>
                    <button className="reject-btn">❌ Usuń</button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      )}
    </div>
  );
}
