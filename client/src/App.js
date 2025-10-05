import { BrowserRouter, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import AuthProvider from "./context/AuthContext";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import ResetPasswordPage from "./pages/ResetPasswordPage";
import ChangePasswordPage from "./pages/ChangePasswordPage";
import ProtectedRoute from "./context/ProtectedRoute";
import UserProfile from "./pages/UserProfile";
import AddAlertPage from "./pages/AddAlertPage";
import RideManager from "./pages/RideManager";
import AdminProfile from "./pages/AdminProfile";
import ModeratorProfile from "./pages/ModeratorProfile";

import "./styles/main.css";

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/logowanie" element={<LoginPage />} />
          <Route path="/rejestracja" element={<RegisterPage />} />
          <Route path="/przypomnij-haslo" element={<ForgotPasswordPage />} />
          <Route path="/resetuj-haslo" element={<ResetPasswordPage />} />
          <Route path="/zmien-haslo" element={<ChangePasswordPage />} />
          <Route
            path="/profil"
            element={
              <ProtectedRoute roles={["Passenger", "Admin", "Moderator"]}>
                <UserProfile />
              </ProtectedRoute>
            }
          />
          <Route
            path="/dodaj-alert"
            element={
              <ProtectedRoute roles={["Passenger", "Moderator", "Admin"]}>
                <AddAlertPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/zarzadzanie-przejazdami"
            element={
              <ProtectedRoute roles={["Admin", "Moderator"]}>
                <RideManager />
              </ProtectedRoute>
            }
          />

          <Route
            path="/profil-admina"
            element={
              <ProtectedRoute roles={["Admin"]}>
                <AdminProfile />
              </ProtectedRoute>
            }
          />

          <Route
            path="/profil-moderatora"
            element={
              <ProtectedRoute roles={["Moderator"]}>
                <ModeratorProfile />
              </ProtectedRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
