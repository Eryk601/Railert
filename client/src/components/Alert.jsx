import { useEffect } from "react";
import "../styles/main.css";

export default function Alert({ message, type = "success", onClose }) {
  useEffect(() => {
    const timer = setTimeout(onClose, 4000); // znika po 4s
    return () => clearTimeout(timer);
  }, [onClose]);

  if (!message) return null;

  return <div className={`alert alert-${type}`}>{message}</div>;
}
