import { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import L from "leaflet";
import "leaflet/dist/leaflet.css";

const API_URL = "https://localhost:7265/api/report";

// 🟢 Ikony dla różnych typów transportu
const icons = {
  Bus: new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/2972/2972185.png",
    iconSize: [35, 35],
  }),
  Tram: new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/858/858373.png",
    iconSize: [35, 35],
  }),
  Train: new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/1150/1150745.png",
    iconSize: [35, 35],
  }),
  Metro: new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/622/622669.png",
    iconSize: [35, 35],
  }),
  Other: new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/25/25694.png",
    iconSize: [35, 35],
  }),
};

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
        center={[50.0676, 19.9864]} // Tauron Arena Kraków
        zoom={13}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a>'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        {reports.map((r) => (
          <Marker
            key={r.id}
            position={[r.latitude, r.longitude]}
            icon={icons[r.transportType] || icons.Other}
          >
            <Popup>
              <b>{r.title || "Zgłoszenie"}</b>
              <br />
              <b>Linia:</b> {r.lineNumber} <br />
              <b>Typ:</b> {r.incidentType} <br />
              <b>Opis:</b> {r.description} <br />
              <b>Lokalizacja:</b> {r.locationName} <br />
              <b>Zgłosił:</b> {r.userEmail}
            </Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
}
