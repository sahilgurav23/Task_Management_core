import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiConfig } from '../config/api.config';
import { ApiResponse } from './profile.service';

export interface DashboardSummary {
  totalTasks: number;
  completedTasks: number;
  pendingTasks: number;
}

export interface DashboardBarChartItem {
  statusId: number;
  statusName: string;
  taskCount: number;
}

export interface DashboardPieChartItem {
  priorityId: number;
  priorityName: string;
  taskCount: number;
  percentage: number;
}

export interface DashboardPieChart {
  totalTasks: number;
  priorities: DashboardPieChartItem[];
}

export interface DashboardData {
  summary: DashboardSummary;
  barChartData: DashboardBarChartItem[];
  pieChartData: DashboardPieChart;
}

export interface DashboardFilter {
  timeFilter: number; // 1 = This Week, 2 = This Month, 3 = This Year
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = ApiConfig.endpoints.dashboard;

  constructor(private http: HttpClient) {}

  getDashboardData(filter: DashboardFilter = { timeFilter: 1 }): Observable<ApiResponse<DashboardData>> {
    let params = new HttpParams();
    if (filter.timeFilter) {
      params = params.set('TimeFilter', filter.timeFilter.toString());
    }
    return this.http.get<ApiResponse<DashboardData>>(this.apiUrl, { params });
  }
}
