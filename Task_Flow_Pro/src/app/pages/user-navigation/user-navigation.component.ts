import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-navigation',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatListModule, RouterModule],
  templateUrl: './user-navigation.component.html',
  styleUrl: './user-navigation.component.css'
})
export class UserNavigationComponent {
  @Output() navItemClicked = new EventEmitter<void>();

  selectedNavItem = 'tasks';

  navItems = [
    { icon: 'task', label: 'Tasks', value: 'tasks', route: '/tasks' },
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
    
    // Set initial selected item based on current route
    const currentRoute = this.router.url;
    const activeItem = this.navItems.find(item => currentRoute.startsWith(item.route));
    if (activeItem) {
      this.selectedNavItem = activeItem.value;
    }
  }

  selectNavItem(item: any) {
    this.selectedNavItem = item.value;
    this.router.navigate([item.route]);
    this.navItemClicked.emit();
  }
}
