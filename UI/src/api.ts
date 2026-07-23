import type { AdminDashboard, Meal, Profile, WeightEntry, MealDto, ProfileUpdateDto, WeightEntryDto } from './types';

const BASE = '/api';

function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    return response.text().then((text) => {
      throw new Error(text || response.statusText);
    });
  }
  return response.json() as Promise<T>;
}

export async function getProfile(profile: string): Promise<Profile> {
  return handleResponse<Profile>(await fetch(`${BASE}/profiles/${encodeURIComponent(profile)}`));
}

export async function updateProfileTarget(profile: string, payload: ProfileUpdateDto): Promise<void> {
  const response = await fetch(`${BASE}/profiles/${encodeURIComponent(profile)}/target`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  if (!response.ok) {
    throw new Error(await response.text());
  }
}

export async function getMeals(profile: string): Promise<Meal[]> {
  return handleResponse<Meal[]>(await fetch(`${BASE}/meals/${encodeURIComponent(profile)}`));
}

export async function addMeal(profile: string, payload: MealDto): Promise<Meal> {
  return handleResponse<Meal>(await fetch(`${BASE}/meals/${encodeURIComponent(profile)}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  }));
}

export async function deleteMeal(profile: string, id: string): Promise<void> {
  const response = await fetch(`${BASE}/meals/${encodeURIComponent(profile)}/${encodeURIComponent(id)}`, {
    method: 'DELETE'
  });
  if (!response.ok) {
    throw new Error(await response.text());
  }
}

export async function getWeights(profile: string): Promise<WeightEntry[]> {
  return handleResponse<WeightEntry[]>(await fetch(`${BASE}/weights/${encodeURIComponent(profile)}`));
}

export async function addWeight(profile: string, payload: WeightEntryDto): Promise<WeightEntry> {
  return handleResponse<WeightEntry>(await fetch(`${BASE}/weights/${encodeURIComponent(profile)}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  }));
}

export async function deleteWeight(profile: string, id: string): Promise<void> {
  const response = await fetch(`${BASE}/weights/${encodeURIComponent(profile)}/${encodeURIComponent(id)}`, {
    method: 'DELETE'
  });
  if (!response.ok) {
    throw new Error(await response.text());
  }
}

export async function getAdminDashboard(): Promise<AdminDashboard> {
  return handleResponse<AdminDashboard>(await fetch(`${BASE}/admin/dashboard`));
}

export async function deleteAdminMeal(id: string): Promise<void> {
  const response = await fetch(`${BASE}/admin/meals/${encodeURIComponent(id)}`, { method: 'DELETE' });
  if (!response.ok) {
    throw new Error(await response.text());
  }
}

export async function deleteAdminWeight(id: string): Promise<void> {
  const response = await fetch(`${BASE}/admin/weights/${encodeURIComponent(id)}`, { method: 'DELETE' });
  if (!response.ok) {
    throw new Error(await response.text());
  }
}
