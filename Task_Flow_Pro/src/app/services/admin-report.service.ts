import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfig } from '../config/api.config';
import { ApiResponse } from './profile.service';

export interface MonthlyCompletion {
  monthLabel: string;
  taskCount: number;
}

export interface DailyCompletion {
  dateLabel: string;
  taskCount: number;
}

export interface AdminReportData {
  monthlyTrend: MonthlyCompletion[];
  dailyTrend: DailyCompletion[];
}

@Injectable({
  providedIn: 'root'
})
export class AdminReportService {
  private apiUrl = ApiConfig.endpoints.report;

  constructor(private http: HttpClient) {}

  getCompletionTrends(): Observable<ApiResponse<AdminReportData>> {
    return this.http.get<ApiResponse<AdminReportData>>(`${this.apiUrl}/completion-trends`);
  }
}
