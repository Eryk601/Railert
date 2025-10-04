import { Navigate } from "react-router-dom";
import { useAuth } from "./AuthContext";

export default function ProtectedRoute({ roles, children }) {
  const { user, loading } = useAuth();

  if (loading) {
    return <p>Ładowanie...</p>; // można podmienić na spinner
  }

  if (!user) return <Navigate to="/logowanie" replace />;

  if (roles && roles.length > 0 && !roles.includes(user.role)) {
    // np. Admin próbował wejść na /admin przeznaczone dla Superadmina
    return <Navigate to="/profil" replace />;
  }

  return children;
}
