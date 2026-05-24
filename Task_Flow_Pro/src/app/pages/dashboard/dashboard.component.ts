import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { LayoutComponent } from '../../shared/components/layout/layout.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatMenuModule,
    LayoutComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  summaryCards = [
    { title: 'Total Tasks', value: '124', indicator: '12%', color: 'green' },
    { title: 'Completed', value: '89', indicator: '8%', color: 'green' },
    { title: 'Pending', value: '35', indicator: '3%', color: 'yellow' },
    { title: 'Hours Logged', value: '342h', indicator: '24%', color: 'purple' }
  ];
}
