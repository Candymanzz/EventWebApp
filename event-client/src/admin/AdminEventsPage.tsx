import { useEffect, useState } from 'react';
import api from '../api/axios';
import EventForm from './EventForm';

export type EventDto = {
    id: string;
    title: string;
    description: string;
    dateTime: string;
    location: string;
    category: string;
    maxParticipants: number;
    imageUrl: string;
};

export default function AdminEventsPage() {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [showForm, setShowForm] = useState(false);
    const [editEvent, setEditEvent] = useState<EventDto | null>(null);

    const fetchEvents = async () => {
        setLoading(true);
        try {
            const res = await api.get('/events');
            setEvents(res.data);
        } catch (e: any) {
            setError(e.response?.data?.message || 'Ошибка загрузки');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvents();
    }, []);

    const handleDelete = async (id: string) => {
        if (!window.confirm('Удалить событие?')) return;
        try {
            await api.delete(`/events/${id}`);
            setEvents(events => events.filter(e => e.id !== id));
        } catch (e: any) {
            setError(e.response?.data?.message || 'Ошибка удаления');
        }
    };

    const handleEdit = (event: EventDto) => {
        setEditEvent(event);
        setShowForm(true);
    };

    const handleCreate = () => {
        setEditEvent(null);
        setShowForm(true);
    };

    const handleFormSubmit = async (data: any) => {
        try {
            if (editEvent) {
                await api.put('/events', { ...data, id: editEvent.id });
            } else {
                await api.post('/events', data);
            }
            setShowForm(false);
            fetchEvents();
        } catch (e: any) {
            setError(e.response?.data?.message || 'Ошибка сохранения');
        }
    };

    if (loading) return <div>Загрузка...</div>;
    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="container mt-4">
            <h2>Управление событиями</h2>
            <button className="btn btn-success mb-3" onClick={handleCreate}>Создать событие</button>
            {showForm && (
                <EventForm
                    initialData={editEvent || undefined}
                    onSubmit={handleFormSubmit}
                    buttonText={editEvent ? 'Сохранить' : 'Создать'}
                />
            )}
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th>Название</th>
                        <th>Дата</th>
                        <th>Место</th>
                        <th>Категория</th>
                        <th>Макс. участников</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {events.map(ev => (
                        <tr key={ev.id}>
                            <td>{ev.title}</td>
                            <td>{new Date(ev.dateTime).toLocaleString()}</td>
                            <td>{ev.location}</td>
                            <td>{ev.category}</td>
                            <td>{ev.maxParticipants}</td>
                            <td>
                                <button className="btn btn-primary btn-sm me-2" onClick={() => handleEdit(ev)}>Редактировать</button>
                                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(ev.id)}>Удалить</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
