import logo from "../assets/logo2.png";
import "../styles/main.css";

export default function AuthHeader() {
  return (
    <header className="home-header">
      <div className="logo-section">
        <img src={logo} alt="Railert Logo" className="logo" />
      </div>
    </header>
  );
}
