import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { CreateEditTaskComponent } from '../create-edit-task/create-edit-task.component';
import { TaskService, TaskListItem } from '../../services/task.service';

@Component({
  selector: 'app-tasks-management',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatDialogModule,
    LayoutComponent
  ],
  templateUrl: './tasks-management.component.html',
  styleUrl: './tasks-management.component.css'
})
export class TasksManagementComponent implements OnInit {
  protected readonly Math = Math;
  private dialog = inject(MatDialog);
  private taskService = inject(TaskService);
  private router = inject(Router);

  displayedColumns: string[] = ['name', 'priority', 'assignee', 'dueDate', 'actions'];
  
  tasks: TaskListItem[] = [];
  filteredTasks: TaskListItem[] = [];
  totalCount = 0;
  pageSize = 10;
  pageNumber = 1;
  currentStatusId?: number;

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTaskList(this.pageNumber, this.pageSize, '', this.currentStatusId).subscribe({
      next: (response) => {
        if (response.success) {
          this.tasks = response.data.items;
          this.filteredTasks = [...this.tasks];
          this.totalCount = response.data.totalCount;
        }
      },
      error: (err) => {
        console.error('Failed to load tasks', err);
      }
    });
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get pageNumbers(): number[] {
    const pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }

  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.pageNumber = page;
      this.loadTasks();
    }
  }

  openAddTaskDialog() {
    const dialogRef = this.dialog.open(CreateEditTaskComponent, {
      width: '800px',
      maxWidth: '95vw',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTasks(); // Refresh list after successful creation
      }
    });
  }

  onTabChange(event: any) {
    const label = event.tab.textLabel;
    if (label === 'All Tasks') {
      this.currentStatusId = undefined;
    } else if (label === 'Active') {
      this.currentStatusId = 1; // Mapping to 'To Do' or similar active state
    } else if (label === 'Completed') {
      this.currentStatusId = 4; // Mapping to 'Done'
    }
    this.loadTasks();
  }

  getPriorityClass(priority: string): string {
    return `priority-${priority.toLowerCase()}`;
  }

  viewTaskDetails(taskId: string) {
    this.router.navigate(['/tasks', taskId]);
  }
}
