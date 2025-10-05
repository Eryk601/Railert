import { useEffect, useState } from "react";
import AuthHeader from "../components/AuthHeader";

const API_BASE = "https://localhost:7265/api";

async function apiFetch(url, options = {}) {
  // pobieramy cały obiekt "auth"
  const authData = JSON.parse(localStorage.getItem("auth"));
  const token = authData?.token;

  if (!token) {
    console.warn("⚠️ Brak tokenu w localStorage – użytkownik niezalogowany");
  }

  const headers = {
    "Content-Type": "application/json",
    ...(token && { Authorization: `Bearer ${token}` }),
    ...options.headers,
  };

  const res = await fetch(url, { ...options, headers });
  if (!res.ok) {
    console.error(`❌ Błąd API (${res.status}) przy ${url}`);
    throw new Error(`Błąd API: ${res.status}`);
  }

  // jeżeli brak treści (np. 204) — nie próbujemy parsować
  if (res.status === 204) return null;

  return res.json();
}

export default function JourneyPlanner() {
  const [rides, setRides] = useState([]);
  const [selectedRides, setSelectedRides] = useState([]);
  const [journeys, setJourneys] = useState([]);
  const [loading, setLoading] = useState(true);
  const [msg, setMsg] = useState("");

  // === pobierz dostępne przejazdy ===
  useEffect(() => {
    apiFetch(`${API_BASE}/ride`)
      .then(setRides)
      .catch((err) => console.error("Błąd przy pobieraniu ride:", err))
      .finally(() => setLoading(false));
  }, []);

  // === pobierz moje podróże ===
  useEffect(() => {
    apiFetch(`${API_BASE}/journey/my`)
      .then(setJourneys)
      .catch((err) => {
        console.warn("Nie udało się pobrać podróży:", err.message);
        setJourneys([]);
      });
  }, [msg]);

  // === dodawanie / usuwanie z planu ===
  function toggleRide(id) {
    setSelectedRides((prev) =>
      prev.includes(id) ? prev.filter((r) => r !== id) : [...prev, id]
    );
  }

  // === zapis podróży ===
  async function handleSave() {
    if (selectedRides.length === 0) {
      setMsg("⚠️ Wybierz co najmniej jeden przejazd!");
      return;
    }

    try {
      await apiFetch(`${API_BASE}/journey`, {
        method: "POST",
        body: JSON.stringify(selectedRides),
      });
      setMsg("✅ Podróż została zapisana! 🚆");
      setSelectedRides([]);
    } catch (err) {
      console.error(err);
      setMsg("❌ Błąd przy zapisie podróży.");
    }
  }

  return (
    <>
      <AuthHeader />
      <div className="journey-container">
        <h2 className="journey-title">🗺️ Planowanie podróży</h2>

        {msg && <div className="alert-info">{msg}</div>}

        <div className="journey-grid">
          <div className="journey-list">
            <h3>Dostępne przejazdy</h3>
            {loading ? (
              <p>Ładowanie...</p>
            ) : (
              rides.map((r) => (
                <div
                  key={r.id}
                  className={`ride-card ${
                    selectedRides.includes(r.id) ? "selected" : ""
                  }`}
                  onClick={() => toggleRide(r.id)}
                >
                  <div className="ride-header">
                    <strong>{r.lineNumber}</strong> — {r.startStation} →{" "}
                    {r.endStation}
                  </div>
                  <div className="ride-meta">
                    <span>
                      🕒 {new Date(r.scheduledDeparture).toLocaleTimeString()} –{" "}
                      {new Date(r.scheduledArrival).toLocaleTimeString()}
                    </span>
                    <span>
                      ⏱️{" "}
                      {r.delayMinutes > 0
                        ? `${r.delayMinutes} min`
                        : "Brak opóźnień"}
                    </span>
                  </div>
                </div>
              ))
            )}
          </div>

          <div className="journey-summary">
            <h3>Twój plan podróży</h3>
            {selectedRides.length === 0 ? (
              <p>Nie wybrano żadnych przejazdów.</p>
            ) : (
              <ul>
                {selectedRides.map((id, i) => {
                  const ride = rides.find((r) => r.id === id);
                  return (
                    <li key={id}>
                      {i + 1}. {ride.lineNumber} — {ride.startStation} →{" "}
                      {ride.endStation}
                    </li>
                  );
                })}
              </ul>
            )}
            <button className="alert-btn" onClick={handleSave}>
              💾 Zapisz podróż
            </button>
          </div>
        </div>

        <div className="journey-history">
          <h3>📜 Moje podróże</h3>
          {journeys.length === 0 ? (
            <p>Brak zapisanych podróży.</p>
          ) : (
            journeys.map((j) => (
              <div key={j.id} className="journey-card">
                <h4>
                  Podróż #{j.id}{" "}
                  {j.hasWarning && (
                    <span className="warning">⚠️ Ostrzeżenie</span>
                  )}
                </h4>
                <ul>
                  {j.segments.map((s, i) => (
                    <li key={i}>
                      {s.sequence}. {s.lineNumber} — {s.startStation} →{" "}
                      {s.endStation} (
                      {new Date(s.scheduledDeparture).toLocaleTimeString()} →{" "}
                      {new Date(s.scheduledArrival).toLocaleTimeString()} )
                    </li>
                  ))}
                </ul>
              </div>
            ))
          )}
        </div>
      </div>
    </>
  );
}
