import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-left-navigation',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatListModule],
  templateUrl: './left-navigation.component.html',
  styleUrl: './left-navigation.component.css'
})
export class LeftNavigationComponent {
  @Output() navItemClicked = new EventEmitter<void>();

  selectedNavItem = 'dashboard';

  navItems = [
    { icon: 'dashboard', label: 'Dashboard', value: 'dashboard' },
    { icon: 'task', label: 'Tasks', value: 'tasks' },
    { icon: 'schedule', label: 'Time Tracking', value: 'time-tracking' },
    { icon: 'bar_chart', label: 'Reports', value: 'reports' },
    { icon: 'person', label: 'Profile', value: 'profile' }
  ];

  selectNavItem(item: string) {
    this.selectedNavItem = item;
    this.navItemClicked.emit();
  }
}
