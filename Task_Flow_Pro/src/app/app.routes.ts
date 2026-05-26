import { Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './pages/login/login.component';
import { TasksManagementComponent } from './pages/tasks-management/tasks-management.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: '', 
    canActivate: [authGuard],
    children: [
      { path: 'admin/dashboard', component: DashboardComponent },
      { path: 'user/dashboard', component: DashboardComponent },
      { path: 'administrator/dashboard', component: DashboardComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'tasks', component: TasksManagementComponent },
      { path: 'profile', component: ProfileComponent }
    ]
  }
];
