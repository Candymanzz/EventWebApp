import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
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
    isUserRegistered: boolean;
};

export default function EventDetailPage() {
    const { id } = useParams();
    const [event, setEvent] = useState<EventDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [registered, setRegistered] = useState(false);

    useEffect(() => {
        axios.get(`http://localhost:5114/api/events/${id}`)
            .then(res => {
                setEvent(res.data);
                setRegistered(res.data.isUserRegistered || false);
                setLoading(false);
            })
            .catch(err => {
                console.error('Ошибка при загрузке события', err);
                setLoading(false);
            });
    }, [id]);

    const handleRegister = async () => {
        try {
            await axios.post(`http://localhost:5114/api/events/${id}/register`);
            setRegistered(true);
            setEvent((prev) => prev ? { ...prev, usersCount: prev.usersCount + 1 } : prev);
        } catch (err) {
            console.error('Ошибка при регистрации на событие', err);
        }
    };

    if (loading) return <div className="container mt-5">Загрузка...</div>;
    if (!event) return <div className="container mt-5">Событие не найдено</div>;

    return (
        <div className="container mt-4">
            <h2 className="mb-3">{event.title}</h2>
            {event.imageUrl && (
                <img
                    src={event.imageUrl}
                    alt={event.title}
                    className="img-fluid mb-3"
                    style={{ maxHeight: '400px', objectFit: 'cover' }}
                />
            )}
            <p><strong>Описание:</strong> {event.description}</p>
            <p><strong>Дата и время:</strong> {new Date(event.dateTime).toLocaleString()}</p>
            <p><strong>Место:</strong> {event.location}</p>
            <p><strong>Категория:</strong> {event.category}</p>
            <p><strong>Участники:</strong> {event.usersCount} / {event.maxParticipants}</p>

            {registered ? (
                <div className="alert alert-success">Вы зарегистрированы на это событие</div>
            ) : event.usersCount >= event.maxParticipants ? (
                <div className="alert alert-danger">Мест больше нет</div>
            ) : (
                <button onClick={handleRegister} className="btn btn-primary">Записаться</button>
            )}
        </div>
    );
}
