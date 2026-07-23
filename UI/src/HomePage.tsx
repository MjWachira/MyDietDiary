import { useEffect, useMemo, useState, type FormEvent } from 'react';
import { addMeal, addWeight, deleteMeal, deleteWeight, getMeals, getProfile, getWeights, updateProfileTarget } from './api';
import type { Meal, MealDto, Profile, WeightEntry, WeightEntryDto } from './types';

const todayIso = new Date().toISOString().slice(0, 10);

function profileKey(name: string): string {
  return name.trim() || 'default';
}

function formatDate(date: string): string {
  return new Date(date).toLocaleDateString();
}

function HomePage() {
  const [profile, setProfile] = useState('default');
  const [profileData, setProfileData] = useState<Profile | null>(null);
  const [meals, setMeals] = useState<Meal[]>([]);
  const [weights, setWeights] = useState<WeightEntry[]>([]);
  const [status, setStatus] = useState('');
  const [mealForm, setMealForm] = useState<MealDto>({ name: '', calories: 0, date: todayIso });
  const [weightForm, setWeightForm] = useState<WeightEntryDto>({ weightKg: 0, date: todayIso });
  const [targetCalories, setTargetCalories] = useState(2000);

  const profileId = useMemo(() => profileKey(profile), [profile]);

  async function loadAll() {
    setStatus('Loading profile...');
    try {
      const [profileResult, mealsResult, weightsResult] = await Promise.all([
        getProfile(profileId),
        getMeals(profileId),
        getWeights(profileId)
      ]);

      setProfileData(profileResult);
      setTargetCalories(profileResult.targetCalories);
      setMeals(mealsResult);
      setWeights(weightsResult);
      setStatus('Loaded data.');
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to load data');
      setProfileData(null);
      setMeals([]);
      setWeights([]);
    }
  }

  useEffect(() => {
    void loadAll();
  }, [profileId]);

  const totalCalories = useMemo(() => meals.reduce((sum, meal) => sum + meal.calories, 0), [meals]);

  const loadClicked = async () => {
    await loadAll();
  };

  const saveTarget = async () => {
    try {
      await updateProfileTarget(profileId, { targetCalories });
      setStatus('Target saved.');
      await loadAll();
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to update target');
    }
  };

  const submitMeal = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await addMeal(profileId, mealForm);
      setMealForm({ name: '', calories: 0, date: todayIso });
      setStatus('Meal added.');
      await loadAll();
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to add meal');
    }
  };

  const submitWeight = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await addWeight(profileId, weightForm);
      setWeightForm({ weightKg: 0, date: todayIso });
      setStatus('Weight entry added.');
      await loadAll();
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to add weight entry');
    }
  };

  const removeMeal = async (id: string) => {
    try {
      await deleteMeal(profileId, id);
      setStatus('Meal removed.');
      await loadAll();
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to delete meal');
    }
  };

  const removeWeight = async (id: string) => {
    try {
      await deleteWeight(profileId, id);
      setStatus('Weight entry removed.');
      await loadAll();
    } catch (error) {
      setStatus(error instanceof Error ? error.message : 'Failed to delete weight');
    }
  };

  return (
    <div className="page-shell">
      <header>
        <h1>MyDietDiary</h1>
        <p>React UI for the diet diary API</p>
      </header>

      <section className="card">
        <div className="row">
          <label>
            Profile name
            <input value={profile} onChange={(e) => setProfile(e.target.value)} />
          </label>
          <button type="button" onClick={loadClicked}>Load profile</button>
        </div>

        <div className="row summary-row">
          <div>
            <h2>{profileId}</h2>
            {profileData ? (
              <p>Target: {profileData.targetCalories} kcal</p>
            ) : (
              <p>No profile loaded yet.</p>
            )}
          </div>
          <div>
            <strong>Total calories:</strong>
            <div>{totalCalories} kcal</div>
          </div>
        </div>

        <div className="row">
          <label>
            Target calories
            <input
              type="number"
              value={targetCalories}
              onChange={(e) => setTargetCalories(Number(e.target.value))}
            />
          </label>
          <button type="button" onClick={saveTarget}>Save target</button>
        </div>
      </section>

      <section className="grid">
        <article className="card">
          <h2>Meals</h2>
          <form onSubmit={submitMeal} className="stack">
            <label>
              Name
              <input
                value={mealForm.name}
                onChange={(e) => setMealForm({ ...mealForm, name: e.target.value })}
                required
              />
            </label>
            <label>
              Calories
              <input
                type="number"
                min="0"
                value={mealForm.calories}
                onChange={(e) => setMealForm({ ...mealForm, calories: Number(e.target.value) })}
                required
              />
            </label>
            <label>
              Date
              <input
                type="date"
                value={mealForm.date}
                onChange={(e) => setMealForm({ ...mealForm, date: e.target.value })}
                required
              />
            </label>
            <button type="submit">Add meal</button>
          </form>

          <div className="list">
            {meals.length === 0 ? (
              <p>No meals yet.</p>
            ) : (
              meals.map((meal) => (
                <div key={`${meal.id}-${meal.profile}-${meal.date}`} className="list-item">
                  <div>
                    <strong>{meal.name}</strong>
                    <div>{formatDate(meal.date)} • {meal.calories} kcal</div>
                  </div>
                  <button onClick={() => void removeMeal(meal.id)}>Delete</button>
                </div>
              ))
            )}
          </div>
        </article>

        <article className="card">
          <h2>Weight</h2>
          <form onSubmit={submitWeight} className="stack">
            <label>
              Weight (kg)
              <input
                type="number"
                step="0.1"
                min="0"
                value={weightForm.weightKg}
                onChange={(e) => setWeightForm({ ...weightForm, weightKg: Number(e.target.value) })}
                required
              />
            </label>
            <label>
              Date
              <input
                type="date"
                value={weightForm.date}
                onChange={(e) => setWeightForm({ ...weightForm, date: e.target.value })}
                required
              />
            </label>
            <button type="submit">Add weight</button>
          </form>

          <div className="list">
            {weights.length === 0 ? (
              <p>No weight records yet.</p>
            ) : (
              weights.map((weight) => (
                <div key={`${weight.id}-${weight.profile}-${weight.date}`} className="list-item">
                  <div>
                    <strong>{weight.weightKg.toFixed(1)} kg</strong>
                    <div>{formatDate(weight.date)}</div>
                  </div>
                  <button onClick={() => void removeWeight(weight.id)}>Delete</button>
                </div>
              ))
            )}
          </div>
        </article>
      </section>

      <footer>
        <p>{status}</p>
      </footer>
    </div>
  );
}

export default HomePage;
