import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { TaskService, TaskDetails, TaskActivity } from '../../services/task.service';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    LayoutComponent
  ],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css'
})
export class TaskDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private taskService = inject(TaskService);
  taskId: string | null = null;
  loading = true;
  error: string | null = null;

  task: TaskDetails | null = null;
  activities: TaskActivity[] = [];

  ngOnInit() {
    this.taskId = this.route.snapshot.paramMap.get('id');
    if (this.taskId) {
      this.loadTaskData();
    } else {
      this.error = 'Task ID not found';
      this.loading = false;
    }
  }

  loadTaskData() {
    this.loading = true;
    this.error = null;

    this.taskService.getTaskDetails(this.taskId!).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.task = response.data;
          this.loadTaskActivities();
        } else {
          this.error = response.message || 'Failed to load task details';
          this.loading = false;
        }
      },
      error: (err) => {
        console.error('Failed to load task details', err);
        this.error = 'Failed to load task details';
        this.loading = false;
      }
    });
  }

  loadTaskActivities() {
    this.taskService.getTaskActivities(this.taskId!).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.activities = response.data;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load task activities', err);
        this.activities = [];
        this.loading = false;
      }
    });
  }

  getPriorityClass(priority: string): string {
    return `priority-${priority.toLowerCase()}`;
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
  }

  formatDateTime(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric', 
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
