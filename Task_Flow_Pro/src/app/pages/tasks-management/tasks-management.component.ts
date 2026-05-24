import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { CreateEditTaskComponent } from '../create-edit-task/create-edit-task.component';

interface Task {
  id: number;
  name: string;
  department: string;
  priority: 'High' | 'Medium' | 'Low' | 'Critical';
  assignee: { name: string; avatar: string; initials?: string };
  dueDate: string;
  status: 'Active' | 'Completed';
}

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
export class TasksManagementComponent {
  private dialog = inject(MatDialog);
  displayedColumns: string[] = ['name', 'priority', 'assignee', 'dueDate', 'actions'];
  
  tasks: Task[] = [
    {
      id: 1,
      name: 'Review Q3 Financials',
      department: 'Finance Department',
      priority: 'High',
      assignee: { name: 'Sarah J.', avatar: 'https://i.pravatar.cc/150?u=sarah' },
      dueDate: 'Oct 15, 2023',
      status: 'Active'
    },
    {
      id: 2,
      name: 'Update Security Protocols',
      department: 'IT Infrastructure',
      priority: 'Critical',
      assignee: { name: 'Marcus T.', avatar: 'https://i.pravatar.cc/150?u=marcus' },
      dueDate: 'Oct 18, 2023',
      status: 'Completed'
    },
    {
      id: 3,
      name: 'Onboard New Hires',
      department: 'Human Resources',
      priority: 'Medium',
      assignee: { name: 'Jane D.', avatar: 'https://i.pravatar.cc/150?u=jane' },
      dueDate: 'Oct 20, 2023',
      status: 'Active'
    },
    {
      id: 4,
      name: 'Prepare Board Presentation',
      department: 'Executive Team',
      priority: 'High',
      assignee: { name: 'Alex W.', avatar: 'https://i.pravatar.cc/150?u=alex' },
      dueDate: 'Oct 25, 2023',
      status: 'Active'
    },
    {
      id: 5,
      name: 'Quarterly Team Outing Planning',
      department: 'Culture Committee',
      priority: 'Low',
      assignee: { name: 'Emily R.', avatar: 'https://i.pravatar.cc/150?u=emily' },
      dueDate: 'Nov 02, 2023',
      status: 'Active'
    }
  ];

  filteredTasks: Task[] = [...this.tasks];

  openAddTaskDialog() {
    const dialogRef = this.dialog.open(CreateEditTaskComponent, {
      width: '800px',
      maxWidth: '95vw',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('The dialog was closed', result);
        // Handle new task addition here if needed
      }
    });
  }

  onTabChange(event: any) {
    const label = event.tab.textLabel;
    if (label === 'All Tasks') {
      this.filteredTasks = [...this.tasks];
    } else if (label === 'Active') {
      this.filteredTasks = this.tasks.filter(t => t.status === 'Active');
    } else if (label === 'Completed') {
      this.filteredTasks = this.tasks.filter(t => t.status === 'Completed');
    }
  }

  getPriorityClass(priority: string): string {
    return `priority-${priority.toLowerCase()}`;
  }
}
