import EventForm from './EventForm';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../auth/AuthProvider';

export default function CreateEventPage() {
    const { token } = useAuth();
    const navigate = useNavigate();

    const handleCreate = async (data: any) => {
        try {
            await axios.post('http://localhost:5114/api/events', data, {
                headers: { Authorization: `Bearer ${token}` }
            });
            navigate('/admin/events');
        } catch (err) {
            alert('Ошибка при создании события');
        }
    };

    return (
        <div className="container mt-4">
            <h2>Создание события</h2>
            <EventForm onSubmit={handleCreate} buttonText="Создать" />
        </div>
    );
}
