import logo from "../assets/logo2.png";
import "../styles/main.css";
import { Link } from "react-router-dom";

export default function AuthHeader() {
  return (
    <header className="home-header">
      <Link to="/" className="logo-link">
        <img src={logo} alt="Railert logo" className="logo" />
        <div className="logo-text"></div>
      </Link>
    </header>
  );
}
