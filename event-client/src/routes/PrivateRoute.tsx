import { Navigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthProvider';

export const PrivateRoute = ({ children }: { children: React.JSX.Element }) => {
    const { token } = useAuth();

    return token ? children : <Navigate to="/login" />;
};
