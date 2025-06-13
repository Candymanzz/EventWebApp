import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api/axios';

type EventDto = {
    id: string;
    title: string;
    description: string;
    dateTime: string;
    location: string;
    category: string;
    maxParticipants: number;
    currentParticipantsCount: number;
    imageUrl: string;
    isUserRegistered: boolean;
};

export default function EventDetailPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [event, setEvent] = useState<EventDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [registered, setRegistered] = useState(false);

    const fetchEvent = async () => {
        try {
            const res = await api.get(`/events/${id}`);
            setEvent(res.data);
            setRegistered(res.data.isUserRegistered || false);
        } catch (err) {
            console.error('Ошибка при загрузке события', err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvent();
    }, [id]);

    const handleRegister = async () => {
        try {
            await api.post(`/users/register-to-event`, { EventId: id });
            setRegistered(true);
            setEvent((prev) => prev ? { ...prev, currentParticipantsCount: prev.currentParticipantsCount + 1 } : prev);
            navigate('/my-events', { replace: true });
        } catch (err: any) {
            console.error('Ошибка при регистрации на событие', err);
            const errorMessage = err.response?.data?.message || 'Произошла ошибка при регистрации на событие';
            alert(errorMessage);
            fetchEvent();
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
            <p><strong>Участники:</strong> {event.currentParticipantsCount} / {event.maxParticipants}</p>

            {registered ? (
                <div className="alert alert-success">Вы зарегистрированы на это событие</div>
            ) : event.currentParticipantsCount >= event.maxParticipants ? (
                <div className="alert alert-danger">Мест больше нет</div>
            ) : (
                <button onClick={handleRegister} className="btn btn-primary">Записаться</button>
            )}
        </div>
    );
}
