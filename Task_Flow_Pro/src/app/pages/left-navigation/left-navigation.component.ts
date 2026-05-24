import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-left-navigation',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatListModule, RouterModule],
  templateUrl: './left-navigation.component.html',
  styleUrl: './left-navigation.component.css'
})
export class LeftNavigationComponent {
  @Output() navItemClicked = new EventEmitter<void>();

  selectedNavItem = 'dashboard';

  navItems = [
    { icon: 'dashboard', label: 'Dashboard', value: 'dashboard', route: '/dashboard' },
    { icon: 'task', label: 'Tasks', value: 'tasks', route: '/tasks' },
    { icon: 'schedule', label: 'Time Tracking', value: 'time-tracking', route: '/time-tracking' },
    { icon: 'bar_chart', label: 'Reports', value: 'reports', route: '/reports' },
    { icon: 'person', label: 'Profile', value: 'profile', route: '/profile' }
  ];

  constructor(private router: Router) {
    // Update selected item based on current route
    this.router.events.subscribe(() => {
      const currentRoute = this.router.url;
      const activeItem = this.navItems.find(item => currentRoute.startsWith(item.route));
      if (activeItem) {
        this.selectedNavItem = activeItem.value;
      }
    });
  }

  selectNavItem(item: any) {
    this.selectedNavItem = item.value;
    this.router.navigate([item.route]);
    this.navItemClicked.emit();
  }
}
