import { Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from '../pages/LoginPage';
import EventsListPage from '../pages/EventsListPage';
import EventDetailPage from '../pages/EventDetailPage';
import { PrivateRoute } from './PrivateRoute';
import MyEventsPage from '../pages/MyEventsPage';
import AdminEventsPage from '../admin/AdminEventsPage';
import { RequireRole } from './RequireRole';
import CreateEventPage from '../admin/CreateEventPage';
import EditEventPage from '../admin/EditEventPage';
import RegisterDetailsPage from '../pages/RegisterDetailsPage';

export const AppRouter = () => {
    return (
        <Routes>
            <Route path="/" element={<Navigate to="/events" />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/events" element={<EventsListPage />} />
            <Route
                path="/events/:id"
                element={
                    <PrivateRoute>
                        <EventDetailPage />
                    </PrivateRoute>
                }
            />
            <Route
                path="/my-events"
                element={
                    <PrivateRoute>
                        <MyEventsPage />
                    </PrivateRoute>
                }
            />
            <Route
                path="/admin/events"
                element={
                    <RequireRole role="Admin">
                        <AdminEventsPage />
                    </RequireRole>
                }
            />
            <Route
                path="/admin/events/create"
                element={
                    <RequireRole role="Admin">
                        <CreateEventPage />
                    </RequireRole>
                }
            />
            <Route
                path="/admin/events/edit/:id"
                element={
                    <RequireRole role="Admin">
                        <EditEventPage />
                    </RequireRole>
                }
            />
            <Route path="/register-details" element={<RegisterDetailsPage />} />
        </Routes>
    );
};
