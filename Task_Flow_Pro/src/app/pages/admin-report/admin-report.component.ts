import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { AdminReportService, AdminReportData } from '../../services/admin-report.service';

@Component({
  selector: 'app-admin-report',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    LayoutComponent
  ],
  templateUrl: './admin-report.component.html',
  styleUrl: './admin-report.component.css'
})
export class AdminReportComponent implements OnInit {
  private adminReportService = inject(AdminReportService);
  
  loading = true;
  error: string | null = null;
  reportData: AdminReportData | null = null;
  protected readonly Math = Math;

  ngOnInit() {
    this.loadReportData();
  }

  loadReportData() {
    this.loading = true;
    this.error = null;

    this.adminReportService.getCompletionTrends().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.reportData = response.data;
        } else {
          this.error = response.message || 'Failed to load report data';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load report data', err);
        this.error = 'Failed to load report data';
        this.loading = false;
      }
    });
  }

  getMaxYValue(data: any[]): number {
    if (!data || data.length === 0) return 10;
    const max = Math.max(...data.map((item: any) => item.taskCount));
    return max > 0 ? Math.ceil(max / 10) * 10 : 10;
  }

  getXPosition(index: number, total: number, width: number): number {
    const padding = 60;
    const availableWidth = width - padding * 2;
    const step = availableWidth / (total - 1);
    return padding + (index * step);
  }

  getYPosition(value: number, maxValue: number, height: number): number {
    const xAxisY = 280;
    const topPadding = 30;
    const availableHeight = xAxisY - topPadding;
    const percentage = value / maxValue;
    return xAxisY - (percentage * availableHeight);
  }

  getBarHeight(value: number, maxValue: number, height: number): number {
    const xAxisY = 280;
    const topPadding = 30;
    const availableHeight = xAxisY - topPadding;
    const percentage = value / maxValue;
    return percentage * availableHeight;
  }

  getLinePoints(data: any[], width: number, height: number): string {
    return data.map((item: any, index: number) => {
      const x = this.getXPosition(index, data.length, width);
      const y = this.getYPosition(item.taskCount, this.getMaxYValue(data), height);
      return `${x},${y}`;
    }).join(' ');
  }
}
