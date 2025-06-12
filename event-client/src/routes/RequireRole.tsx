import { Navigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthProvider';

export const RequireRole = ({ role, children }: { role: string, children: React.JSX.Element }) => {
    const { token } = useAuth();

    const payload = token ? JSON.parse(atob(token.split('.')[1])) : null;
    const userRole = payload?.role;

    if (!token || userRole !== role) {
        return <Navigate to="/login" />;
    }

    return children;
};
