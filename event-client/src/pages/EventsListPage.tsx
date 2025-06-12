import { useEffect, useState } from 'react';
import axios from 'axios';

type EventDto = {
    id: string;
    title: string;
    description: string;
    dateTime: string;
    location: string;
    category: string;
    maxParticipants: number;
    usersCount: number;
    imageUrl: string;
};

export default function EventsListPage() {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        axios.get('http://localhost:5114/api/events')
            .then(res => {
                setEvents(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error('Ошибка при загрузке событий', err);
                setLoading(false);
            });
    }, []);

    if (loading) return <div className="container mt-5">Загрузка событий...</div>;

    return (
        <div className="container mt-4">
            <h2 className="mb-4">Список событий</h2>
            <div className="row">
                {events.map(event => (
                    <div className="col-md-4 mb-4" key={event.id}>
                        <div className="card h-100">
                            {event.imageUrl && (
                                <img
                                    src={event.imageUrl}
                                    className="card-img-top"
                                    alt={event.title}
                                    style={{ height: '200px', objectFit: 'cover' }}
                                />
                            )}
                            <div className="card-body d-flex flex-column">
                                <h5 className="card-title">{event.title}</h5>
                                <p className="card-text">{event.description}</p>
                                <p className="text-muted">{new Date(event.dateTime).toLocaleString()}</p>
                                <p>{event.location} | {event.category}</p>
                                <p>
                                    Мест: {event.usersCount}/{event.maxParticipants}{" "}
                                    {event.usersCount >= event.maxParticipants && (
                                        <span className="badge bg-danger ms-2">Нет мест</span>
                                    )}
                                </p>
                                <a href={`/events/${event.id}`} className="btn btn-outline-primary mt-auto">Подробнее</a>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
