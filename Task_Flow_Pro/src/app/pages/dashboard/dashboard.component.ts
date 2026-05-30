import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { DashboardService, DashboardData, DashboardFilter } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    LayoutComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  
  loading = true;
  error: string | null = null;
  dashboardData: DashboardData | null = null;
  
  selectedPeriod = 'This Week';
  timeFilter: number = 1; // 1 = This Week, 2 = This Month, 3 = This Year

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loading = true;
    this.error = null;

    const filter: DashboardFilter = { timeFilter: this.timeFilter };
    
    this.dashboardService.getDashboardData(filter).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.dashboardData = response.data;
        } else {
          this.error = response.message || 'Failed to load dashboard data';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load dashboard data', err);
        this.error = 'Failed to load dashboard data';
        this.loading = false;
      }
    });
  }

  onPeriodChange(period: string, filterValue: number) {
    this.selectedPeriod = period;
    this.timeFilter = filterValue;
    this.loadDashboardData();
  }

  get summaryCards() {
    if (!this.dashboardData) return [];
    
    const summary = this.dashboardData.summary;
    const total = summary.totalTasks;
    const completed = summary.completedTasks;
    const pending = summary.pendingTasks;
    
    const completedPercentage = total > 0 ? Math.round((completed / total) * 100) : 0;
    const pendingPercentage = total > 0 ? Math.round((pending / total) * 100) : 0;
    
    return [
      { title: 'Total Tasks', value: total.toString(), indicator: '100%', color: 'blue' },
      { title: 'Completed', value: completed.toString(), indicator: `${completedPercentage}%`, color: 'green' },
      { title: 'Pending', value: pending.toString(), indicator: `${pendingPercentage}%`, color: 'yellow' }
    ];
  }

  getBarPercentage(count: number, total: number): number {
    return total > 0 ? (count / total) * 100 : 0;
  }

  getBarColorClass(index: number): string {
    const colors = ['bar-gray', 'bar-yellow', 'bar-purple', 'bar-green'];
    return colors[index % colors.length];
  }

  getDonutSegment(index: number): string {
    if (!this.dashboardData?.pieChartData?.priorities) return '0 100';
    
    const priorities = this.dashboardData.pieChartData.priorities;
    const colors = ['#4285F4', '#F4B400', '#EA4335'];
    
    let accumulatedPercentage = 0;
    for (let i = 0; i < index; i++) {
      accumulatedPercentage += priorities[i]?.percentage || 0;
    }
    
    const currentPercentage = priorities[index]?.percentage || 0;
    const segmentLength = currentPercentage;
    const gapLength = 100 - segmentLength - accumulatedPercentage;
    
    return `${segmentLength} ${gapLength} ${accumulatedPercentage} 0`;
  }

  getPriorityColor(index: number): string {
    const colors = ['#4285F4', '#F4B400', '#EA4335'];
    return colors[index % colors.length];
  }
}
