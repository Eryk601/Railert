import { BrowserRouter, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./auth/LoginPage";
import RegisterPage from "./auth/RegisterPage";
import AuthProvider from "./context/AuthContext";
import ForgotPasswordPage from "./password/ForgotPasswordPage";
import ResetPasswordPage from "./password/ResetPasswordPage";
import ChangePasswordPage from "./password/ChangePasswordPage";
import ProtectedRoute from "./context/ProtectedRoute";
import UserProfile from "./user/UserProfile";
import AddAlertPage from "./pages/AddAlertPage";
import RideManager from "./pages/RideManager";
import AdminProfile from "./admin/AdminProfile";
import ModeratorProfile from "./moderator/ModeratorProfile";
import UserManager from "./admin/UserManager";
import JourneyPlanner from "./user/JourneyPlanner";
import CheckAlert from "./moderator/CheckAlert";

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
          <Route path="/zarzadzanie-uzytkownikami" element={<UserManager />} />
          <Route path="/sprawdzanie-alertów" element={<CheckAlert />} />
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
          <Route path="/planowanie-podrozy" element={<JourneyPlanner />} />

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
