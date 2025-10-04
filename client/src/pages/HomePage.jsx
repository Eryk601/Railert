import "./HomePage.css";
import logo from "../assets/logo.png"; // Railert logo (PNG)
import iconUser from "../assets/user.png"; // ikona użytkownika
import iconMenu from "../assets/menu.png"; // ikona menu
import iconLocation from "../assets/location.png"; // ikona "skąd jedziemy"
import iconSwap from "../assets/swap.png"; // ikona zmiany kierunku
import iconCalendar from "../assets/calendar.png"; // ikona kalendarza

export default function HomePage() {
  return (
    <div className="home-container">
      {/* === Header === */}
      <header className="home-header">
        <div className="logo-section">
          <img src={logo} alt="Railert Logo" className="logo" />
        </div>
        <div className="user-box">
          <button className="icon-btn">
            <img src={iconUser} alt="User" />
          </button>
          <button className="icon-btn">
            <img src={iconMenu} alt="Menu" />
          </button>
        </div>
      </header>

      {/* === Search Box === */}
      <main className="search-section">
        <div className="search-box">
          <div className="inputs-row">
            <div className="input-group">
              <img src={iconLocation} alt="From" className="icon" />
              <input type="text" placeholder="Skąd Jedziemy?" />
            </div>

            <div className="input-group swap">
              <img src={iconSwap} alt="Swap" className="icon-swap" />
            </div>

            <div className="input-group">
              <input type="text" placeholder="Dokąd Jedziemy?" />
            </div>

            <div className="input-group">
              <input type="text" placeholder="Kiedy?" />
              <img src={iconCalendar} alt="Calendar" className="icon" />
            </div>
          </div>

          <div className="inputs-row">
            <div className="input-group">
              <label>Ilość Przesiadek:</label>
              <select>
                <option>0</option>
                <option>1</option>
                <option>2+</option>
              </select>
            </div>

            <div className="input-group">
              <label>Ilość Pasażerów:</label>
              <select>
                <option>1</option>
                <option>2</option>
                <option>3+</option>
              </select>
            </div>
          </div>

          <div className="search-btn-container">
            <button className="search-btn">Szukaj</button>
          </div>
        </div>
      </main>
    </div>
  );
}
