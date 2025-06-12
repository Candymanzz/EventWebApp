import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthProvider';

export default function Navbar() {
    const { token, setToken } = useAuth();
    const navigate = useNavigate();

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
