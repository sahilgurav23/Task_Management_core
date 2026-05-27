import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfig } from '../config/api.config';
import { ApiResponse } from './profile.service';

export interface UserDropdown {
  id: string;
  fullName: string;
  profileImageUrl: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface CreateTaskRequest {
  title: string;
  description: string;
  priorityId: number;
  statusId: number;
  assigneeId: string;
  dueDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = ApiConfig.endpoints.task;

  constructor(private http: HttpClient) {}

  createTask(task: CreateTaskRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl, task);
  }
}
