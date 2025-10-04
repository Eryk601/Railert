import "../styles/main.css";
import logo from "../assets/logo2.png";
import iconUser from "../assets/user.png";
import iconMenu from "../assets/menu.png";
import iconLocation from "../assets/location.png";
import iconSwap from "../assets/swap.png";
import iconCalendar from "../assets/calendar.png";
import iconClock from "../assets/clock.png";
import DatePicker from "react-datepicker";
import MapPage from "./MapPage";
import "react-datepicker/dist/react-datepicker.css";
import { pl } from "date-fns/locale";
import { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function HomePage() {
  const navigate = useNavigate();
  const { user } = useAuth();

  const [from, setFrom] = useState("");
  const [to, setTo] = useState("");
  const [date, setDate] = useState("");
  const [time, setTime] = useState("");
  const [passengers, setPassengers] = useState(1);
  const [openDate, setOpenDate] = useState(false);
  const [openTime, setOpenTime] = useState(false);

  const calendarRef = useRef(null);
  const timeRef = useRef(null);

  const handleSwap = () => {
    setFrom(to);
    setTo(from);
  };

  // zamykanie pop-upów po kliknięciu poza
  useEffect(() => {
    const handleClickOutside = (e) => {
      if (
        (calendarRef.current && calendarRef.current.contains(e.target)) ||
        (timeRef.current && timeRef.current.contains(e.target))
      ) {
        return;
      }
      setOpenDate(false);
      setOpenTime(false);
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  return (
    <div className="home-container">
      {/* === HEADER === */}
      <header className="home-header">
        <div className="logo-section">
          <img src={logo} alt="Railert Logo" className="logo" />
        </div>

        <div className="user-box">
          <img
            src={iconUser}
            alt="User"
            className="clickable-icon"
            onClick={() => {
              if (user) navigate("/profil"); // jeśli zalogowany
              else navigate("/logowanie"); // jeśli nie
            }}
          />
          <img src={iconMenu} alt="Menu" className="icon" />
        </div>
      </header>

      {/* === MAIN === */}
      <main>
        {/* === PANEL WYSZUKIWANIA === */}
        <section className="search-section">
          <div className="search-box">
            <div className="inputs-row">
              {/* SKĄD */}
              <div className="input-group">
                <img src={iconLocation} alt="From" className="icon" />
                <input
                  type="text"
                  placeholder="Skąd jedziemy?"
                  value={from}
                  onChange={(e) => setFrom(e.target.value)}
                />
              </div>

              {/* ZAMIANA */}
              <div className="input-group swap">
                <img
                  src={iconSwap}
                  alt="Swap"
                  className="icon-swap"
                  onClick={handleSwap}
                />
              </div>

              {/* DOKĄD */}
              <div className="input-group">
                <input
                  type="text"
                  placeholder="Dokąd jedziemy?"
                  value={to}
                  onChange={(e) => setTo(e.target.value)}
                />
              </div>

              {/* DATA */}
              <div className="input-group">
                <input
                  type="text"
                  placeholder="Kiedy?"
                  value={date ? date.toLocaleDateString("pl-PL") : ""}
                  readOnly
                />
                <img
                  src={iconCalendar}
                  alt="Calendar"
                  className="icon clickable"
                  onClick={() => {
                    setOpenDate(!openDate);
                    setOpenTime(false);
                  }}
                />
                {openDate && (
                  <div className="calendar-popup" ref={calendarRef}>
                    <DatePicker
                      selected={date}
                      onChange={(d) => {
                        setDate(d);
                        setOpenDate(false);
                      }}
                      inline
                      locale={pl}
                    />
                  </div>
                )}
              </div>

              {/* GODZINA */}
              <div className="input-group">
                <input
                  type="text"
                  placeholder="O której?"
                  value={time}
                  readOnly
                />
                <img
                  src={iconClock}
                  alt="Clock"
                  className="icon clickable"
                  onClick={() => {
                    setOpenTime(!openTime);
                    setOpenDate(false);
                  }}
                />
                {openTime && (
                  <div className="calendar-popup" ref={timeRef}>
                    <DatePicker
                      selected={
                        time ? new Date(`1970-01-01T${time}`) : new Date()
                      }
                      onChange={(d) => {
                        const formatted = d
                          .toLocaleTimeString("pl-PL", {
                            hour: "2-digit",
                            minute: "2-digit",
                            hour12: false,
                          })
                          .replace(":", ":");
                        setTime(formatted);
                        setOpenTime(false);
                      }}
                      showTimeSelect
                      showTimeSelectOnly
                      timeIntervals={15}
                      timeCaption="Godzina"
                      dateFormat="HH:mm"
                      inline
                      locale={pl}
                    />
                  </div>
                )}
              </div>
            </div>

            {/* RZĄD 2 */}
            <div className="inputs-row">
              <div className="input-group-q">
                <label>Ilość Przesiadek:</label>
                <select>
                  <option>0</option>
                  <option>1</option>
                  <option>2+</option>
                </select>
              </div>

              <div className="input-group-q">
                <label>Ilość Pasażerów:</label>
                <input
                  type="text"
                  inputMode="numeric"
                  pattern="[0-9]*"
                  value={passengers}
                  onChange={(e) => {
                    const value = e.target.value.replace(/\D/g, ""); // tylko cyfry
                    setPassengers(value === "" ? "" : value);
                  }}
                />
              </div>
            </div>

            <div className="search-btn-container">
              <button className="search-btn">Szukaj</button>
            </div>
          </div>
        </section>

        {/* === MAPA POD PANELEM === */}
        <section className="map-section">
          <MapPage />
        </section>
      </main>
    </div>
  );
}
