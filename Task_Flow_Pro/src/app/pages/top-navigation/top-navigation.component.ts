import { Component, EventEmitter, Output, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { Router, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProfileService, NavigationProfile } from '../../services/profile.service';
import { AuthService } from '../../services/auth.service';
import { ProfileSyncService } from '../../services/profile-sync.service';

@Component({
  selector: 'app-top-navigation',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatBadgeModule,
    MatMenuModule,
    MatDividerModule,
    RouterModule
  ],
  templateUrl: './top-navigation.component.html',
  styleUrl: './top-navigation.component.css'
})
export class TopNavigationComponent implements OnInit, OnDestroy {
  @Output() toggleSidebar = new EventEmitter<void>();

  private profileService = inject(ProfileService);
  private authService = inject(AuthService);
  private profileSyncService = inject(ProfileSyncService);
  private router = inject(Router);
  private syncSubscription?: Subscription;

  navProfile: NavigationProfile | null = null;

  ngOnInit() {
    this.loadNavigationDetails();
    
    // Listen for profile updates from other components
    this.syncSubscription = this.profileSyncService.profileUpdated$.subscribe(() => {
      this.loadNavigationDetails();
    });
  }

  ngOnDestroy() {
    if (this.syncSubscription) {
      this.syncSubscription.unsubscribe();
    }
  }

  loadNavigationDetails() {
    this.profileService.getNavigationDetails().subscribe({
      next: (response) => {
        if (response.success) {
          this.navProfile = response.data;
        }
      },
      error: (err) => {
        console.error('Failed to load top navigation profile', err);
      }
    });
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  onLogout() {
    this.authService.logout();
  }
}
