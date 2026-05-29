import { Component, Inject, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormControl } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProfileService } from '../../services/profile.service';
import { TaskService, CreateTaskRequest, UpdateTaskRequest } from '../../services/task.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

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
    MatSnackBarModule,
    MatProgressSpinnerModule
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
  userSearchControl = new FormControl('');
  isEditMode = false;
  taskId: string | null = null;
  canEditDetails = true;
  canEditStatus = true;
  loading = false;

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

    // Setup search with debounce
    this.userSearchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(searchTerm => {
      this.loadAssignees(searchTerm || '');
    });

    if (this.data && this.data.taskId) {
      this.isEditMode = true;
      this.taskId = this.data.taskId;
      this.loadTaskEditContext();
    }
  }

  loadTaskEditContext() {
    if (!this.taskId) return;

    this.loading = true;
    this.taskService.getTaskEditContext(this.taskId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const context = response.data;
          this.canEditDetails = context.canEditDetails;
          this.canEditStatus = context.canEditStatus;

          // Patch form with task details
          const taskDetails = context.taskDetails;
          this.taskForm.patchValue({
            title: taskDetails.title,
            description: taskDetails.description,
            priorityId: taskDetails.priorityId,
            statusId: taskDetails.statusId,
            assigneeId: taskDetails.assigneeId,
            dueDate: new Date(taskDetails.dueDate)
          });

          // Apply field disable/enable based on permissions
          this.applyFieldPermissions();
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load task edit context', err);
        this.snackBar.open('Failed to load task details', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
        this.loading = false;
      }
    });
  }

  applyFieldPermissions() {
    if (this.isEditMode) {
      // Disable fields based on permissions
      const titleControl = this.taskForm.get('title');
      const descriptionControl = this.taskForm.get('description');
      const priorityIdControl = this.taskForm.get('priorityId');
      const assigneeIdControl = this.taskForm.get('assigneeId');
      const dueDateControl = this.taskForm.get('dueDate');
      const statusIdControl = this.taskForm.get('statusId');

      if (!this.canEditDetails) {
        titleControl?.disable();
        descriptionControl?.disable();
        priorityIdControl?.disable();
        assigneeIdControl?.disable();
        dueDateControl?.disable();
      }

      if (!this.canEditStatus) {
        statusIdControl?.disable();
      }
    }
  }

  loadAssignees(searchTerm: string = '') {
    this.profileService.getAssigneeDropdown(1, 100, searchTerm).subscribe({
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
      if (this.isEditMode && this.taskId) {
        // Update existing task
        const updateData: UpdateTaskRequest = {
          title: this.taskForm.value.title,
          description: this.taskForm.value.description,
          priorityId: this.taskForm.value.priorityId,
          statusId: this.taskForm.value.statusId,
          assignedUserId: this.taskForm.value.assigneeId,
          dueDate: new Date(this.taskForm.value.dueDate).toISOString()
        };

        // Only include fields that are editable based on permissions
        if (!this.canEditDetails) {
          delete updateData.title;
          delete updateData.description;
          delete updateData.priorityId;
          delete updateData.assignedUserId;
          delete updateData.dueDate;
        }

        if (!this.canEditStatus) {
          delete updateData.statusId;
        }

        this.taskService.updateTask(this.taskId, updateData).subscribe({
          next: (response) => {
            if (response.success) {
              this.snackBar.open('Task updated successfully', 'Close', {
                duration: 3000,
                verticalPosition: 'top',
                horizontalPosition: 'center'
              });
              this.dialogRef.close(true);
            } else {
              this.snackBar.open(response.message || 'Failed to update task', 'Close', {
                duration: 3000,
                verticalPosition: 'top',
                horizontalPosition: 'center'
              });
            }
          },
          error: (err) => {
            const errorMsg = err.error?.message || 'An error occurred while updating the task';
            this.snackBar.open(errorMsg, 'Close', {
              duration: 3000,
              verticalPosition: 'top',
              horizontalPosition: 'center'
            });
          }
        });
      } else {
        // Create new task
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
}
