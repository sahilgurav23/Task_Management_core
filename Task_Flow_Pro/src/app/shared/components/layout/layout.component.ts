import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { TopNavigationComponent } from '../../../pages/top-navigation/top-navigation.component';
import { LeftNavigationComponent } from '../../../pages/left-navigation/left-navigation.component';
import { UserNavigationComponent } from '../../../pages/user-navigation/user-navigation.component';
import { AdministratorNavigationComponent } from '../../../pages/administrator-navigation/administrator-navigation.component';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    MatSidenavModule,
    TopNavigationComponent,
    LeftNavigationComponent,
    UserNavigationComponent,
    AdministratorNavigationComponent
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit {
  private breakpointObserver = inject(BreakpointObserver);
  
  isSidebarOpen = true;
  isMobile = false;
  userRole: string = '';

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  ngOnInit() {
    this.getUserRole();
    this.breakpointObserver.observe([Breakpoints.Handset, Breakpoints.TabletPortrait])
      .subscribe(result => {
        this.isMobile = result.matches;
        if (this.isMobile) {
          this.isSidebarOpen = false;
        } else {
          this.isSidebarOpen = true;
        }
      });
  }

  getUserRole() {
    const userProfile = localStorage.getItem('user');
    if (userProfile) {
      try {
        const profile = JSON.parse(userProfile);
        // Handle role as string or number
        const role = profile.role;
        if (typeof role === 'number') {
          // Convert number to role name
          const roleMap: { [key: number]: string } = {
            1: 'User',
            2: 'Admin',
            3: 'Administrator'
          };
          this.userRole = roleMap[role] || '';
        } else {
          // Use string role (case-insensitive comparison later)
          this.userRole = role || '';
        }
        console.log('User role from localStorage:', this.userRole);
      } catch (e) {
        this.userRole = '';
        console.error('Error parsing userProfile:', e);
      }
    } else {
      console.log('No userProfile found in localStorage');
    }
  }

  isAdminRole(): boolean {
    return this.userRole.toLowerCase() === 'admin';
  }

  isUserRole(): boolean {
    return this.userRole.toLowerCase() === 'user';
  }

  isAdministratorRole(): boolean {
    return this.userRole.toLowerCase() === 'administrator';
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }
}
