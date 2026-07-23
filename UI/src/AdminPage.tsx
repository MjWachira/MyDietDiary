import { useEffect, useState } from 'react';
import { deleteAdminMeal, deleteAdminWeight, getAdminDashboard } from './api';
import type { AdminDashboard } from './types';

function formatDate(date: string): string {
  return new Date(date).toLocaleDateString();
}

function AdminPage() {
  const [dashboard, setDashboard] = useState<AdminDashboard | null>(null);
  const [status, setStatus] = useState('Loading admin dashboard...');

  async function loadDashboard() {
    try {
      const result = await getAdminDashboard();
      setDashboard(result);
      setStatus('Admin dashboard loaded.');
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Unable to load admin dashboard');
    }
  }

  useEffect(() => {
    void loadDashboard();
  }, []);

  const removeMeal = async (id: string) => {
    try {
      await deleteAdminMeal(id);
      await loadDashboard();
      setStatus('Meal removed.');
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to delete meal');
    }
  };

  const removeWeight = async (id: string) => {
    try {
      await deleteAdminWeight(id);
      await loadDashboard();
      setStatus('Weight removed.');
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to delete weight');
    }
  };

  return (
    <div className="page-shell">
      <header>
        <h1>MyDietDiary Admin</h1>
        <p>Manage all profiles, meals, and weight entries.</p>
      </header>

      <section className="card">
        <div className="row summary-row">
          <div><strong>Profiles:</strong> {dashboard?.profiles.length ?? 0}</div>
          <div><strong>Meals:</strong> {dashboard?.meals.length ?? 0}</div>
          <div><strong>Weights:</strong> {dashboard?.weights.length ?? 0}</div>
        </div>
      </section>

      <section className="card">
        <h2>Profiles</h2>
        <div className="list">
          {dashboard?.profiles.length ? (
            dashboard.profiles.map((entry) => (
              <div key={entry.profileName} className="list-item">
                <div>
                  <strong>{entry.profileName}</strong>
                  <div>Target: {entry.targetCalories} kcal</div>
                </div>
              </div>
            ))
          ) : (
            <p>No profiles found.</p>
          )}
        </div>
      </section>

      <section className="card">
        <h2>All meals</h2>
        <div className="list">
          {dashboard?.meals.length ? (
            dashboard.meals.map((meal) => (
              <div key={`${meal.id}-${meal.profile}-${meal.date}`} className="list-item">
                <div>
                  <strong>{meal.name}</strong>
                  <div>{meal.profile} • {formatDate(meal.date)} • {meal.calories} kcal</div>
                </div>
                <button type="button" onClick={() => void removeMeal(meal.id)}>Delete</button>
              </div>
            ))
          ) : (
            <p>No meals found.</p>
          )}
        </div>
      </section>

      <section className="card">
        <h2>All weight entries</h2>
        <div className="list">
          {dashboard?.weights.length ? (
            dashboard.weights.map((weight) => (
              <div key={`${weight.id}-${weight.profile}-${weight.date}`} className="list-item">
                <div>
                  <strong>{weight.weightKg.toFixed(1)} kg</strong>
                  <div>{weight.profile} • {formatDate(weight.date)}</div>
                </div>
                <button type="button" onClick={() => void removeWeight(weight.id)}>Delete</button>
              </div>
            ))
          ) : (
            <p>No weight entries found.</p>
          )}
        </div>
      </section>

      <footer>
        <p>{status}</p>
      </footer>
    </div>
  );
}

export default AdminPage;
