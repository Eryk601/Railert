import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";
import { Link } from "react-router-dom";
import "../styles/main.css";

export default function UserManager() {
  const { user } = useAuth();
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editingUserId, setEditingUserId] = useState(null);
  const [newReputation, setNewReputation] = useState("");
  const API = "https://localhost:7265/api/User/all";

  useEffect(() => {
    fetchUsers();
  }, []);

  async function fetchUsers() {
    try {
      const res = await fetch(API, {
        headers: { Authorization: `Bearer ${user?.token}` },
      });
      const data = await res.json();
      setUsers(data.users || []);
    } catch (e) {
      console.error("Błąd ładowania użytkowników:", e);
    } finally {
      setLoading(false);
    }
  }

  async function handleReputationSave(userId) {
    try {
      const res = await fetch(
        `https://localhost:7265/api/User/reputation/${userId}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${user?.token}`,
          },
          body: JSON.stringify({ reputationPoints: Number(newReputation) }),
        }
      );

      if (!res.ok) throw new Error("Błąd aktualizacji reputacji");

      // aktualizuj lokalnie
      setUsers((prev) =>
        prev.map((u) =>
          u.id === userId
            ? { ...u, reputationPoints: Number(newReputation) }
            : u
        )
      );

      setEditingUserId(null);
      setNewReputation("");
    } catch (err) {
      console.error("Błąd aktualizacji reputacji:", err);
      alert("Nie udało się zaktualizować reputacji.");
    }
  }

  return (
    <div className="ride-panel">
      {/* === PRZYCISK POWROTU === */}
      <Link to="/profil-admina" className="back-btn">
        ← Powrót
      </Link>

      <h2>👥 Zarządzanie użytkownikami</h2>

      {loading ? (
        <p>Ładowanie danych...</p>
      ) : (
        <table className="ride-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Email</th>
              <th>Wyświetlana nazwa</th>
              <th>Rola</th>
              <th>Miasto</th>
              <th>Punkty reputacji</th>
              <th>Zgłoszenia</th>
              <th>Data rejestracji</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {users.map((u) => (
              <tr key={u.id}>
                <td>{u.id}</td>
                <td>{u.email}</td>
                <td>{u.displayName || "—"}</td>
                <td>{u.role}</td>
                <td>{u.city || "—"}</td>

                <td>
                  {editingUserId === u.id ? (
                    <input
                      type="number"
                      value={newReputation}
                      onChange={(e) => setNewReputation(e.target.value)}
                      style={{ width: "80px" }}
                    />
                  ) : (
                    u.reputationPoints
                  )}
                </td>

                <td>{u.reportsCount}</td>
                <td>
                  {new Date(u.createdAt).toLocaleDateString("pl-PL", {
                    day: "2-digit",
                    month: "2-digit",
                    year: "numeric",
                  })}
                </td>

                <td>
                  {editingUserId === u.id ? (
                    <>
                      <button
                        className="small-btn save-btn"
                        onClick={() => handleReputationSave(u.id)}
                      >
                        Zapisz
                      </button>
                      <button
                        className="small-btn cancel-btn"
                        onClick={() => setEditingUserId(null)}
                      >
                        Anuluj
                      </button>
                    </>
                  ) : (
                    <button
                      className="small-btn edit-btn"
                      onClick={() => {
                        setEditingUserId(u.id);
                        setNewReputation(u.reputationPoints);
                      }}
                    >
                      ✏️
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
