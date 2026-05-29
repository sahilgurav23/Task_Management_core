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

export interface TaskListItem {
  taskId: string;
  title: string;
  priority: string;
  priorityId: number;
  assigneeName: string;
  assigneeImageUrl: string;
  dueDate: string;
  status?: string;
  department?: string;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = ApiConfig.endpoints.task;

  constructor(private http: HttpClient) {}

  getTaskList(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = '', statusId?: number): Observable<ApiResponse<PaginatedResponse<TaskListItem>>> {
    let params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    
    if (searchTerm) params = params.set('SearchTerm', searchTerm);
    if (statusId) params = params.set('StatusId', statusId.toString());

    return this.http.get<ApiResponse<PaginatedResponse<TaskListItem>>>(`${this.apiUrl}/list`, { params });
  }

  createTask(task: CreateTaskRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl, task);
  }
}
