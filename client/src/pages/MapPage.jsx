import { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import alertRed from "../assets/alertred.png";
import alertYellow from "../assets/alertyellow.png";

const API_URL = "https://localhost:7265/api/report";

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

// komponent pomocniczy do dopasowania widoku
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
  const [reports, setReports] = useState([]);

  useEffect(() => {
    fetch(API_URL)
      .then((res) => res.json())
      .then(setReports)
      .catch(console.error);
  }, []);

  return (
    <div className="map-wrapper">
      <MapContainer
        center={[52, 19]} // środek Polski
        zoom={6}
        style={{ height: "70vh", width: "100%" }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a>'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        <FitBounds reports={reports} />

        {reports.map((r) => {
          // oblicz opóźnienie w minutach
          const delayMinutes = Math.abs(
            (new Date(r.scheduledArrival) - new Date(r.createdAt)) / 60000
          );

          const icon = delayMinutes >= 30 ? icons.red : icons.yellow;

          return (
            <Marker key={r.id} position={[r.latitude, r.longitude]} icon={icon}>
              <Popup>
                <b>{r.title || "Zgłoszenie"}</b>
                <br />
                <b>Linia:</b> {r.lineNumber} <br />
                <b>Opis:</b> {r.description} <br />
                <b>Lokalizacja:</b> {r.locationName} <br />
                <b>Ilość potwierdzeń:</b> {r.confirmationsCount} <br />
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
                <b>Zgłosił:</b> {r.userDisplayName}
              </Popup>
            </Marker>
          );
        })}
      </MapContainer>
    </div>
  );
}
