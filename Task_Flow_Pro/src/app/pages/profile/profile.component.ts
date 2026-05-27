import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LayoutComponent } from '../../shared/components/layout/layout.component';
import { ProfileService } from '../../services/profile.service';
import { RoleEnum } from '../../enums/role.enum';
import { ProfileSyncService } from '../../services/profile-sync.service';

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
    MatSnackBarModule,
    LayoutComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private profileService = inject(ProfileService);
  private profileSyncService = inject(ProfileSyncService);
  private snackBar = inject(MatSnackBar);

  profileForm: FormGroup;
  passwordForm: FormGroup;
  profileImageUrl: string = '';
  selectedFile: File | null = null;

  constructor() {
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      role: [{ value: '', disabled: true }]
    });

    this.passwordForm = this.fb.group({
      currentPassword: [''],
      newPassword: ['', [Validators.minLength(6)]],
      confirmPassword: ['']
    });
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (response) => {
        if (response.success) {
          const profile = response.data;
          this.profileForm.patchValue({
            fullName: profile.fullName,
            email: profile.emailAddress,
            role: this.getRoleName(profile.role)
          });
          if (profile.profileImageUrl) {
            this.profileImageUrl = profile.profileImageUrl;
          }
        }
      },
      error: (err) => {
        this.snackBar.open('Failed to load profile details', 'Close', { 
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      }
    });
  }

  getRoleName(role: any): string {
    const roleId = typeof role === 'string' ? parseInt(role) : role;
    switch (roleId) {
      case RoleEnum.User: return 'User';
      case RoleEnum.Admin: return 'Admin';
      case RoleEnum.Administrator: return 'Administrator';
      default: return 'Unknown';
    }
  }

  onUpdateProfile(): void {
    if (this.profileForm.invalid) {
      this.snackBar.open('Please fill all required fields correctly', 'Close', { duration: 3000 });
      return;
    }

    const formData = new FormData();
    formData.append('FullName', this.profileForm.get('fullName')?.value);
    formData.append('EmailAddress', this.profileForm.get('email')?.value);
    
    const currentPwd = this.passwordForm.get('currentPassword')?.value;
    const newPwd = this.passwordForm.get('newPassword')?.value;
    const confirmPwd = this.passwordForm.get('confirmPassword')?.value;

    // Password change logic: only if any password field is filled
    if (currentPwd || newPwd || confirmPwd) {
      if (!currentPwd || !newPwd || !confirmPwd) {
        this.snackBar.open('All password fields are required when changing password', 'Close', { duration: 3000 });
        return;
      }
      if (newPwd !== confirmPwd) {
        this.snackBar.open('New password and confirm password do not match', 'Close', { duration: 3000 });
        return;
      }
      formData.append('OldPassword', currentPwd);
      formData.append('NewPassword', newPwd);
    }
    
    if (this.selectedFile) {
      formData.append('ProfileImage', this.selectedFile);
    }

    this.profileService.updateProfile(formData).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Profile updated successfully', 'Close', { 
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
          this.passwordForm.reset();
          this.selectedFile = null;
          this.loadProfile(); // Re-fetch data to sync UI
          this.profileSyncService.notifyProfileUpdate(); // Notify Top Navigation to refresh
        } else {
          this.snackBar.open(response.message || 'Update failed', 'Close', { 
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
        }
      },
      error: (err) => {
        const errorMsg = err.error?.message || 'An error occurred while updating profile';
        this.snackBar.open(errorMsg, 'Close', { 
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.profileImageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onUploadPhoto(): void {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }
}
