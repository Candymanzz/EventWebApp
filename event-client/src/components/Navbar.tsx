import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthProvider';

export default function Navbar() {
    const { token, setToken } = useAuth();
    const navigate = useNavigate();

    let userRole = null;
    if (token) {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        } catch { }
    }

    const logout = () => {
        setToken(null);
        navigate('/login');
    };

    return (
        <nav className="navbar navbar-expand navbar-dark bg-dark">
            <div className="container">
                <Link to="/events" className="navbar-brand">EventApp</Link>
                <div className="navbar-nav ms-auto">
                    {token ? (
                        <>
                            <Link to="/my-events" className="nav-link">Мои события</Link>
                            {userRole === 'Admin' && (
                                <Link to="/admin/events" className="nav-link">Админ-панель</Link>
                            )}
                            <button className="btn btn-outline-light btn-sm ms-2" onClick={logout}>Выход</button>
                        </>
                    ) : (
                        <Link to="/login" className="btn btn-outline-light btn-sm">Вход</Link>
                    )}
                </div>
            </div>
        </nav>
    );
}
