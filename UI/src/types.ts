export interface Meal {
  id: string;
  name: string;
  calories: number;
  date: string;
  profile: string;
}

export interface WeightEntry {
  id: string;
  weightKg: number;
  date: string;
  profile: string;
}

export interface Profile {
  profileName: string;
  targetCalories: number;
}

export interface MealDto {
  name: string;
  calories: number;
  date: string;
}

export interface WeightEntryDto {
  weightKg: number;
  date: string;
}

export interface ProfileUpdateDto {
  targetCalories: number;
}

export interface AdminDashboard {
  profiles: Profile[];
  meals: Meal[];
  weights: WeightEntry[];
}
