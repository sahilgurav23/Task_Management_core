import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { LayoutComponent } from '../../shared/components/layout/layout.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    LayoutComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  passwordForm: FormGroup;
  profileImageUrl: string = 'https://i.pravatar.cc/150?u=alex-morgan';

  constructor(private fb: FormBuilder) {
    this.profileForm = this.fb.group({
      fullName: ['Alex Morgan', Validators.required],
      email: ['alex.morgan@example.com', [Validators.required, Validators.email]],
      role: [{ value: 'Admin / Developer', disabled: true }]
    });

    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }

  ngOnInit(): void {}

  onUpdateProfile(): void {
    if (this.profileForm.valid && this.passwordForm.valid) {
      console.log('Profile updated', {
        profile: this.profileForm.value,
        passwords: this.passwordForm.value
      });
    }
  }

  onUploadPhoto(): void {
    console.log('Upload photo clicked');
  }
}
