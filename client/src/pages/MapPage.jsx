import { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import alertRed from "../assets/alertred.png";
import alertYellow from "../assets/alertyellow.png";
import { useAuth } from "../context/AuthContext";

const API_URL = "https://localhost:7265/api/report";
const VERIFY_URL = "https://localhost:7265/api/verification";

// ikony alertów
const icons = {
  red: new L.Icon({
    iconUrl: alertRed,
    iconSize: [35, 35],
  }),
  yellow: new L.Icon({
    iconUrl: alertYellow,
    iconSize: [35, 35],
  }),
};

// dopasowanie widoku mapy
function FitBounds({ reports }) {
  const map = useMap();

  useEffect(() => {
    if (reports.length === 0) return;
    const bounds = L.latLngBounds(
      reports.map((r) => [r.latitude, r.longitude])
    );
    map.fitBounds(bounds, { padding: [50, 50] });
  }, [reports, map]);

  return null;
}

export default function MapPage() {
  const { user } = useAuth();
  const [reports, setReports] = useState([]);

  // === Pobranie raportów ===
  useEffect(() => {
    fetchReports();
  }, []);

  async function fetchReports() {
    try {
      const res = await fetch(API_URL);
      const data = await res.json();
      setReports(data);
    } catch (err) {
      console.error("Błąd ładowania raportów:", err);
    }
  }

  // === Funkcja głosowania (potwierdzenie / zaprzeczenie) ===
  async function handleVerification(reportId, confirm) {
    if (!user) {
      alert("Musisz być zalogowany, aby głosować.");
      return;
    }

    try {
      const res = await fetch(`${VERIFY_URL}/${reportId}?confirm=${confirm}`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${user.token}`,
        },
      });

      if (!res.ok) {
        const errMsg = await res.text();
        alert(errMsg);
        return;
      }

      const updated = await res.json();

      // zaktualizuj lokalny stan raportów
      setReports((prev) =>
        prev.map((r) =>
          r.id === reportId
            ? {
                ...r,
                confirmationsCount: updated.confirmationsCount,
                rejectionsCount: updated.rejectionsCount,
                isActive: updated.isActive,
              }
            : r
        )
      );
    } catch (err) {
      console.error("Błąd weryfikacji:", err);
      alert("Nie udało się zweryfikować zgłoszenia.");
    }
  }

  return (
    <div className="map-wrapper">
      <MapContainer
        center={[52, 19]}
        zoom={6}
        style={{ height: "70vh", width: "100%" }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a>'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        <FitBounds reports={reports} />

        {reports
          // .filter((r) => r.isActive) // dodanie powoduje znikanie nie aktywnych zgłoszeń z mapy
          .map((r) => {
            const delayMinutes = Math.abs(
              (new Date(r.scheduledArrival) - new Date(r.createdAt)) / 60000
            );

            const icon = delayMinutes >= 30 ? icons.red : icons.yellow;

            return (
              <Marker
                key={r.id}
                position={[r.latitude, r.longitude]}
                icon={icon}
              >
                <Popup>
                  <b>{r.title || "Zgłoszenie"}</b>
                  <br />
                  <b>Linia:</b> {r.lineNumber}
                  <br />
                  <b>Opis:</b> {r.description}
                  <br />
                  <b>Lokalizacja:</b> {r.locationName}
                  <br />
                  <b>Ilość potwierdzeń:</b> {r.confirmationsCount}
                  <br />
                  <b>Ilość zaprzeczeń:</b> {r.rejectionsCount}
                  <br />
                  <b>Data zgłoszenia:</b>{" "}
                  {r.createdAt
                    ? new Date(r.createdAt).toLocaleString("pl-PL", {
                        dateStyle: "medium",
                        timeStyle: "short",
                      })
                    : "—"}
                  <br />
                  <b>Przewidywany przyjazd:</b>{" "}
                  {r.scheduledArrival
                    ? new Date(r.scheduledArrival).toLocaleString("pl-PL", {
                        dateStyle: "medium",
                        timeStyle: "short",
                      })
                    : "—"}
                  <br />
                  <b>Zgłosił:</b> {r.userDisplayName || "Anonim"}
                  <br />
                  {r.isActive ? (
                    <div
                      style={{
                        marginTop: "10px",
                        textAlign: "center",
                      }}
                    >
                      <button
                        className="confirm-btn"
                        onClick={() => handleVerification(r.id, true)}
                      >
                        👍 Potwierdź
                      </button>
                      <button
                        className="reject-btn"
                        onClick={() => handleVerification(r.id, false)}
                      >
                        👎 Zaprzecz
                      </button>
                    </div>
                  ) : (
                    <p style={{ color: "#777", textAlign: "center" }}>
                      (Zgłoszenie nieaktywne)
                    </p>
                  )}
                </Popup>
              </Marker>
            );
          })}
      </MapContainer>
    </div>
  );
}
