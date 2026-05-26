import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfig } from '../config/api.config';

export interface ProfileDetails {
  fullName: string;
  emailAddress: string;
  role: string;
  profileImageUrl: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private apiUrl = ApiConfig.endpoints.profile;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<ApiResponse<ProfileDetails>> {
    return this.http.get<ApiResponse<ProfileDetails>>(`${this.apiUrl}/me`);
  }

  updateProfile(formData: FormData): Observable<ApiResponse<ProfileDetails>> {
    return this.http.put<ApiResponse<ProfileDetails>>(`${this.apiUrl}/me`, formData);
  }
}
