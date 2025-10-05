import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import { Link } from "react-router-dom"; // ⬅️ dodaj to
import "../styles/main.css";

export default function RideManager() {
  const { user } = useAuth();
  const [rides, setRides] = useState([]);
  const [loading, setLoading] = useState(true);
  const API = "https://localhost:7265/api/Ride";

  useEffect(() => {
    fetchRides();
    const interval = setInterval(fetchRides, 15000); // odśwież co 15s
    return () => clearInterval(interval);
  }, []);

  async function fetchRides() {
    try {
      const res = await fetch(API, {
        headers: { Authorization: `Bearer ${user?.token}` },
      });
      const data = await res.json();
      setRides(data);
    } catch (e) {
      console.error("Błąd ładowania przejazdów:", e);
    } finally {
      setLoading(false);
    }
  }

  function getStatusColor(delay, cancelled) {
    if (cancelled) return "cancelled";
    if (delay >= 30) return "red";
    if (delay > 0) return "yellow";
    return "green";
  }

  return (
    <div className="ride-panel">
      {/* === PRZYCISK POWROTU === */}
      <Link to="/profil-admina" className="back-btn">
        ← Powrót
      </Link>

      <h2>📊 Zarządzanie przejazdami</h2>

      {loading ? (
        <p>Ładowanie danych...</p>
      ) : (
        <table className="ride-table">
          <thead>
            <tr>
              <th>Linia</th>
              <th>Trasa</th>
              <th>Przyjazd</th>
              <th>Opóźnienie</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {rides.map((r) => (
              <tr key={r.id}>
                <td>{r.lineNumber}</td>
                <td>
                  {r.startStation} → {r.endStation}
                </td>
                <td>{new Date(r.scheduledArrival).toLocaleString("pl-PL")}</td>
                <td>{r.delayMinutes > 0 ? `+${r.delayMinutes} min` : "—"}</td>
                <td>
                  <span
                    className={`status-dot ${getStatusColor(
                      r.delayMinutes,
                      r.isCancelled
                    )}`}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
