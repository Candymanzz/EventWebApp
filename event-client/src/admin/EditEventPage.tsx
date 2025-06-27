import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../auth/AuthProvider';
import EventForm from './EventForm';

type EventFormData = {
  title: string;
  description: string;
  dateTime: string;
  location: string;
  category: string;
  maxParticipants: number;
  imageUrl: string;
};

export default function EditEventPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { token } = useAuth();

  const [initialData, setInitialData] = useState<EventFormData | null>(null);

  useEffect(() => {
    api.get(`/events/${id}`, {
      headers: { Authorization: `Bearer ${token}` }
    }).then(res => {
      const e = res.data;
      setInitialData({
        title: e.title,
        description: e.description,
        dateTime: e.dateTime.slice(0, 16),
        location: e.location,
        category: e.category,
        maxParticipants: e.maxParticipants,
        imageUrl: e.imageUrl,
      });
    });
  }, [id, token]);

  const handleUpdate = async (data: EventFormData) => {
    try {
      await api.put(`/events/${id}`, data, {
        headers: { Authorization: `Bearer ${token}` }
      });
      navigate('/admin/events');
    } catch (err) {
      alert('Ошибка при обновлении события');
    }
  };

  return (
    <div className="container mt-4">
      <h2>Редактирование события</h2>
      {initialData ? (
        <EventForm onSubmit={handleUpdate} buttonText="Сохранить" initialData={initialData} />
      ) : (
        <p>Загрузка...</p>
      )}
    </div>
  );
}
