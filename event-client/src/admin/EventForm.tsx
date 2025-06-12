import { useState } from 'react';

type EventFormData = {
    title: string;
    description: string;
    dateTime: string;
    location: string;
    category: string;
    maxParticipants: number;
    imageUrl: string;
};

type Props = {
    initialData?: EventFormData;
    onSubmit: (data: EventFormData) => void;
    buttonText: string;
};

export default function EventForm({ initialData, onSubmit, buttonText }: Props) {
    const [formData, setFormData] = useState<EventFormData>(
        initialData || {
            title: '',
            description: '',
            dateTime: '',
            location: '',
            category: '',
            maxParticipants: 1,
            imageUrl: '',
        }
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: name === 'maxParticipants' ? parseInt(value) : value,
        }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="mb-3">
                <label className="form-label">Название</label>
                <input type="text" className="form-control" name="title" value={formData.title} onChange={handleChange} required />
            </div>
            <div className="mb-3">
                <label className="form-label">Описание</label>
                <textarea className="form-control" name="description" value={formData.description} onChange={handleChange} required />
            </div>
            <div className="mb-3">
                <label className="form-label">Дата и время</label>
                <input type="datetime-local" className="form-control" name="dateTime" value={formData.dateTime} onChange={handleChange} required />
            </div>
            <div className="mb-3">
                <label className="form-label">Место</label>
                <input type="text" className="form-control" name="location" value={formData.location} onChange={handleChange} required />
            </div>
            <div className="mb-3">
                <label className="form-label">Категория</label>
                <input type="text" className="form-control" name="category" value={formData.category} onChange={handleChange} required />
            </div>
            <div className="mb-3">
                <label className="form-label">Макс. участников</label>
                <input type="number" className="form-control" name="maxParticipants" value={formData.maxParticipants} onChange={handleChange} required min={1} />
            </div>
            <div className="mb-3">
                <label className="form-label">Ссылка на изображение</label>
                <input type="url" className="form-control" name="imageUrl" value={formData.imageUrl} onChange={handleChange} />
            </div>
            <button type="submit" className="btn btn-success">{buttonText}</button>
        </form>
    );
}
