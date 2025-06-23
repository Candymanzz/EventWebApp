import { useLocation, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import api from '../api/axios';
import { useAuth } from '../auth/AuthProvider';

export default function RegisterDetailsPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const { setToken } = useAuth();

  const email = (location.state as { email: string })?.email;
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const [isAdmin, setIsAdmin] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!email) {
      navigate('/login');
    }
  }, [email, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      setError('Пароли не совпадают');
      return;
    }

    if (password.length < 6) {
      setError('Пароль должен содержать минимум 6 символов');
      return;
    }

    try {
      const res = await api.post('/auth/register-details', {
        email,
        password,
        firstName,
        lastName,
        birthDate: new Date(birthDate).toISOString(),
        role: isAdmin ? 'Admin' : 'User'
      });

      setToken(res.data.accessToken);
      navigate('/events');
    } catch (err) {
      setError('Ошибка при регистрации пользователя');
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: '500px' }}>
      <h2 className="mb-4">Завершение регистрации</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Имя</label>
          <input
            type="text"
            className="form-control"
            required
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Фамилия</label>
          <input
            type="text"
            className="form-control"
            required
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Пароль</label>
          <input
            type="password"
            className="form-control"
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            minLength={6}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Подтвердите пароль</label>
          <input
            type="password"
            className="form-control"
            required
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            minLength={6}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Дата рождения</label>
          <input
            type="date"
            className="form-control"
            required
            value={birthDate}
            onChange={(e) => setBirthDate(e.target.value)}
          />
        </div>
        <div className="form-check mb-3">
          <input
            className="form-check-input"
            type="checkbox"
            id="isAdmin"
            checked={isAdmin}
            onChange={(e) => setIsAdmin(e.target.checked)}
          />
          <label className="form-check-label" htmlFor="isAdmin">
            Зарегистрировать как администратора
          </label>
        </div>
        <button type="submit" className="btn btn-primary w-100">Завершить регистрацию</button>
      </form>
    </div>
  );
}
