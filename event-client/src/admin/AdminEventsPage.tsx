import { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { useAuth } from '../auth/AuthProvider';
import { Modal, Button } from 'react-bootstrap';

type EventDto = {
    id: string;
    title: string;
    dateTime: string;
};

export default function AdminEventsPage() {
    const { token } = useAuth();
    const [events, setEvents] = useState<EventDto[]>([]);
    const [showModal, setShowModal] = useState(false);
    const [eventToDelete, setEventToDelete] = useState<string | null>(null);

    useEffect(() => {
        axios.get('http://localhost:5114/api/events', {
            headers: { Authorization: `Bearer ${token}` }
        }).then(res => setEvents(res.data));
    }, [token]);

    const handleDelete = async (id: string) => {
        setEventToDelete(id);
        setShowModal(true);
    };

    const confirmDelete = async () => {
        if (!eventToDelete) return;
        await axios.delete(`http://localhost:5114/api/events/${eventToDelete}`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        setEvents(events => events.filter(e => e.id !== eventToDelete));
        setShowModal(false);
        setEventToDelete(null);
    };

    return (
        <div className="container mt-4">
            <h2>Админ-панель: события</h2>
            <Link to="/admin/events/create" className="btn btn-success mb-3">Создать новое</Link>
            <ul className="list-group">
                {events.map(e => (
                    <li key={e.id} className="list-group-item d-flex justify-content-between align-items-center">
                        {e.title} — {new Date(e.dateTime).toLocaleString()}
                        <div>
                            <Link to={`/admin/events/edit/${e.id}`} className="btn btn-sm btn-outline-primary me-2">Редактировать</Link>
                            <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(e.id)}>Удалить</button>
                        </div>
                    </li>
                ))}
            </ul>

            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>Вы уверены, что хотите удалить это событие?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>Отмена</Button>
                    <Button variant="danger" onClick={confirmDelete}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}
