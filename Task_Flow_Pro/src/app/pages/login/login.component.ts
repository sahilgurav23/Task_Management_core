import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AuthService, LoginResponse } from '../../services/auth.service';
import { RoleEnum } from '../../enums/role.enum';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule, MatCardModule,
            MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  /* Reactive Form Variable */
  loginForm: FormGroup;

  /* Password Toggle */
  hidePassword = true;

  /* Loading State */
  isLoading = false;

  /* Error Message */
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    /*
      Form Initialization
    */
    this.loginForm = this.fb.group({

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6)
        ]
      ]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  /*
    Login Method
  */
  onLogin(): void {

    /*
      Show all validations
    */
  this.loginForm.markAllAsTouched();

    /*
      Stop if form invalid
    */
    if (this.loginForm.invalid) {
      return;
    }

    /*
      Clear previous error
    */
    this.errorMessage = '';

    /*
      Set loading state
    */
    this.isLoading = true;

    const { email, password } = this.loginForm.value;

    this.authService.login(email, password).subscribe({
      next: (response: LoginResponse) => {
        this.isLoading = false;

        // Store user data in localStorage
        localStorage.setItem('user', JSON.stringify({
          id: response.id,
          fullName: response.fullName,
          emailAddress: response.emailAddress,
          role: response.role
        }));

        // Store token if provided
        if (response.token) {
          localStorage.setItem('token', response.token);
        }

        // Redirect based on role
        this.redirectBasedOnRole(response.role);
      },
      error: (error) => {
        console.error('Login failed:', error);
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Login failed. Please check your credentials and try again.';
      }
    });
  }

  /*
    Redirect based on user role
  */
  private redirectBasedOnRole(role: number): void {
    switch (role) {
      case RoleEnum.User:
        this.router.navigate(['/user/dashboard']);
        break;
      case RoleEnum.Admin:
        this.router.navigate(['/admin/dashboard']);
        break;
      case RoleEnum.Administrator:
        this.router.navigate(['/administrator/dashboard']);
        break;
      default:
        this.router.navigate(['/dashboard']);
        break;
    }
  }
}
