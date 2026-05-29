import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { LayoutComponent } from '../../shared/components/layout/layout.component';

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
    LayoutComponent
  ],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css'
})
export class TaskDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  taskId: string | null = null;

  task = {
    title: 'Refactor Clean Architecture layers',
    description: `The current implementation of the domain logic has become tightly coupled with the infrastructure layer, violating the dependency rule of Clean Architecture. This task involves decoupling these layers to ensure the core business rules are independent of frameworks, UI, and databases.
    
    Key objectives include:
    • Implementing the Repository pattern interfaces strictly within the Domain layer.
    • Moving concrete repository implementations to the Infrastructure layer.
    • Ensuring Use Cases (Interactors) only depend on Domain entities and Repository interfaces.
    • Writing unit tests for Use Cases mocking the repository interfaces to verify isolation.
    
    This refactor is critical before we scale the new reporting module, as the current coupling makes testing incredibly difficult and slows down feature velocity.`,
    status: 'In Progress',
    priority: 'High',
    project: 'Backend V2',
    dueDate: 'Oct 25, 2023',
    assignee: {
      name: 'Ajay N.',
      avatar: 'https://ui-avatars.com/api/?name=Ajay+N&background=0D8ABC&color=fff'
    },
    activities: [
      { type: 'priority', message: 'Priority updated to High', date: 'Oct 22, 09:15 AM', user: 'Ajay N.' },
      { type: 'status', message: 'Status changed to In Progress', date: 'Oct 21, 14:30 PM', user: 'Ajay N.' },
      { type: 'create', message: 'Created task', date: 'Oct 20, 10:00 AM', user: 'Ajay N.' }
    ]
  };

  ngOnInit() {
    this.taskId = this.route.snapshot.paramMap.get('id');
    // In a real app, fetch task by ID here
  }
}
