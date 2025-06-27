import { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../auth/AuthProvider';

type EventDto = {
  id: string;
  title: string;
  dateTime: string;
  location: string;
};

export default function MyEventsPage() {
  const { token } = useAuth();
  const location = useLocation();
  const [events, setEvents] = useState<EventDto[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchEvents = async () => {
    try {
      const res = await api.get('/users/me/events', {
        headers: { Authorization: `Bearer ${token}` }
      });
      setEvents(res.data);
    } catch (err) {
      console.error('Ошибка загрузки', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = async (eventId: string) => {
    try {
      await api.post('/users/cancel-from-event', { EventId: eventId });
      alert('Вы отменили участие в событии');
      fetchEvents();
    } catch (error) {
      console.error('Ошибка отмены регистрации', error);
      alert('Ошибка при отмене участия');
    }
  };

  useEffect(() => {
    fetchEvents();
  }, [token, location.key]);

  if (loading) {
    return <div className="container mt-5">Загрузка...</div>;
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Мои события</h2>
      {events.length === 0 ? (
        <p>Вы пока не записаны на события.</p>
      ) : (
        <ul className="list-group">
          {events.map(e => (
            <li key={e.id} className="list-group-item d-flex justify-content-between align-items-center">
              <span>
                <strong>{e.title}</strong><br />
                {new Date(e.dateTime).toLocaleString()} — {e.location}
              </span>
              <a href={`/events/${e.id}`} className="btn btn-sm btn-outline-primary">Подробнее</a>
              <button onClick={() => handleCancel(e.id)} className="btn btn-sm btn-outline-danger">Отменить</button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
