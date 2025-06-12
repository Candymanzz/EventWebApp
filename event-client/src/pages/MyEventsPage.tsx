import { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../auth/AuthProvider';

type EventDto = {
    id: string;
    title: string;
    dateTime: string;
    location: string;
};

export default function MyEventsPage() {
    const { token } = useAuth();
    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        axios.get('http://localhost:5114/api/users/me/events', {
            headers: { Authorization: `Bearer ${token}` }
        })
            .then(res => {
                setEvents(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error('Ошибка загрузки', err);
                setLoading(false);
            });
    }, [token]);

    if (loading) return <div className="container mt-5">Загрузка...</div>;

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
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
