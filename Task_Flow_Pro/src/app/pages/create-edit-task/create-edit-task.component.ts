import { Component, Inject, OnInit } from '@angular/core';
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
    MatIconModule
  ],
  templateUrl: './create-edit-task.component.html',
  styleUrl: './create-edit-task.component.css'
})
export class CreateEditTaskComponent implements OnInit {
  taskForm: FormGroup;
  isEditMode = false;

  priorities = ['High', 'Medium', 'Low', 'Critical'];
  statuses = ['To Do', 'In Progress', 'Done', 'Archived'];
  users = [
    { id: 1, name: 'Ajay N.' },
    { id: 2, name: 'Sarah J.' },
    { id: 3, name: 'Marcus T.' },
    { id: 4, name: 'Jane D.' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateEditTaskComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      priority: ['High', Validators.required],
      status: ['To Do', Validators.required],
      assignee: [null, Validators.required],
      dueDate: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.data && this.data.task) {
      this.isEditMode = true;
      this.taskForm.patchValue(this.data.task);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    if (this.taskForm.valid) {
      this.dialogRef.close(this.taskForm.value);
    }
  }
}
