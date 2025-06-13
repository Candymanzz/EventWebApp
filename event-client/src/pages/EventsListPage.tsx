import { useEffect, useState } from 'react';
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
};

export default function EventsListPage() {
    const [events, setEvents] = useState<EventDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [filters, setFilters] = useState({
        category: '',
        location: '',
        dateTime: '',
        title: ''
    });
    const [appliedFilters, setAppliedFilters] = useState({
        category: '',
        location: '',
        dateTime: '',
        title: ''
    });

    const fetchEvents = async () => {
        try {
            setLoading(true);
            setError(null);
            const params = new URLSearchParams();

            if (appliedFilters.category) {
                params.append('category', appliedFilters.category);
            }
            if (appliedFilters.location) {
                params.append('location', appliedFilters.location);
            }
            if (appliedFilters.dateTime) {
                params.append('dateTime', appliedFilters.dateTime);
            }
            if (appliedFilters.title) {
                params.append('title', appliedFilters.title);
            }

            console.log('Sending request with params:', params.toString());
            const response = await api.get(`/events/filter?${params.toString()}`);
            console.log('Received response:', response.data);
            setEvents(response.data);
        } catch (err: any) {
            console.error('Error fetching events:', err);
            setError(err.response?.data?.message || 'Failed to fetch events');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEvents();
    }, [appliedFilters]);

    const handleApplyFilters = () => {
        setAppliedFilters(filters);
    };

    const handleResetFilters = () => {
        setFilters({
            category: '',
            location: '',
            dateTime: '',
            title: ''
        });
        setAppliedFilters({
            category: '',
            location: '',
            dateTime: '',
            title: ''
        });
    };

    const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFilters(prev => ({
            ...prev,
            [name]: value
        }));
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="container mt-4">
            <h2>События</h2>

            <div className="row mb-4">
                <div className="col-md-3">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Поиск по названию"
                        name="title"
                        value={filters.title}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="col-md-3">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Категория"
                        name="category"
                        value={filters.category}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="col-md-3">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Место"
                        name="location"
                        value={filters.location}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="col-md-3">
                    <input
                        type="date"
                        className="form-control"
                        name="dateTime"
                        value={filters.dateTime}
                        onChange={handleFilterChange}
                    />
                </div>
            </div>

            <div className="row mb-4">
                <div className="col">
                    <button className="btn btn-primary me-2" onClick={handleApplyFilters}>
                        Применить фильтры
                    </button>
                    <button className="btn btn-secondary" onClick={handleResetFilters}>
                        Сбросить фильтры
                    </button>
                </div>
            </div>

            {events.length === 0 ? (
                <p>События не найдены</p>
            ) : (
                <div className="row">
                    {events.map(event => (
                        <div key={event.id} className="col-md-4 mb-4">
                            <div className="card h-100">
                                {event.imageUrl && (
                                    <img
                                        src={event.imageUrl}
                                        className="card-img-top"
                                        alt={event.title}
                                        style={{ height: '200px', objectFit: 'cover' }}
                                    />
                                )}
                                <div className="card-body">
                                    <h5 className="card-title">{event.title}</h5>
                                    <p className="card-text">{event.description}</p>
                                    <p className="card-text">
                                        <small className="text-muted">
                                            {new Date(event.dateTime).toLocaleString()}
                                        </small>
                                    </p>
                                    <p className="card-text">
                                        <small className="text-muted">
                                            {event.location}
                                        </small>
                                    </p>
                                    <p className="card-text">
                                        <small className="text-muted">
                                            Участники: {event.currentParticipantsCount} / {event.maxParticipants}
                                        </small>
                                    </p>
                                    <a href={`/events/${event.id}`} className="btn btn-primary">
                                        Подробнее
                                    </a>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
