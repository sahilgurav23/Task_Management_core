import { Component, Inject, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProfileService } from '../../services/profile.service';
import { TaskService, CreateTaskRequest } from '../../services/task.service';

@Component({
  selector: 'app-create-edit-task',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './create-edit-task.component.html',
  styleUrl: './create-edit-task.component.css'
})
export class CreateEditTaskComponent implements OnInit {
  private fb = inject(FormBuilder);
  private profileService = inject(ProfileService);
  private taskService = inject(TaskService);
  private snackBar = inject(MatSnackBar);

  taskForm: FormGroup;
  isEditMode = false;

  priorities = [
    { id: 1, label: 'Low' },
    { id: 2, label: 'Medium' },
    { id: 3, label: 'High' },
    { id: 4, label: 'Critical' }
  ];
  statuses = [
    { id: 1, label: 'To Do' },
    { id: 2, label: 'In Progress' },
    { id: 3, label: 'Review' },
    { id: 4, label: 'Done' }
  ];
  users: any[] = [];

  constructor(
    public dialogRef: MatDialogRef<CreateEditTaskComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      priorityId: [3, Validators.required], // Default to High (id: 3)
      statusId: [1, Validators.required],   // Default to To Do (id: 1)
      assigneeId: [null, Validators.required],
      dueDate: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadAssignees();
    if (this.data && this.data.task) {
      this.isEditMode = true;
      this.taskForm.patchValue(this.data.task);
    }
  }

  loadAssignees() {
    this.profileService.getAssigneeDropdown().subscribe({
      next: (response) => {
        if (response.success) {
          this.users = response.data.items;
        }
      },
      error: (err) => {
        console.error('Failed to load assignees', err);
      }
    });
  }

  getSelectedUserImage(): string {
    const userId = this.taskForm.get('assigneeId')?.value;
    const user = this.users.find(u => u.id === userId);
    return user ? user.profileImageUrl : '';
  }

  getSelectedUserName(): string {
    const userId = this.taskForm.get('assigneeId')?.value;
    const user = this.users.find(u => u.id === userId);
    return user ? user.fullName : '';
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    if (this.taskForm.valid) {
      const taskData: CreateTaskRequest = {
        ...this.taskForm.value,
        dueDate: new Date(this.taskForm.value.dueDate).toISOString()
      };

      this.taskService.createTask(taskData).subscribe({
        next: (response) => {
          if (response.success) {
            this.snackBar.open('Task created successfully', 'Close', {
              duration: 3000,
              verticalPosition: 'top',
              horizontalPosition: 'center'
            });
            this.dialogRef.close(true);
          } else {
            this.snackBar.open(response.message || 'Failed to create task', 'Close', {
              duration: 3000,
              verticalPosition: 'top',
              horizontalPosition: 'center'
            });
          }
        },
        error: (err) => {
          const errorMsg = err.error?.message || 'An error occurred while creating the task';
          this.snackBar.open(errorMsg, 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
        }
      });
    }
  }
}
