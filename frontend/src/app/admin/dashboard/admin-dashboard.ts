import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { DashboardStats, Booking } from '../../core/models/tour.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css']
})
export class AdminDashboardComponent implements OnInit {
  private api = inject(ApiService);

  stats = signal<DashboardStats>({
    totalTours: 0,
    totalBookings: 0,
    pendingBookings: 0,
    totalMessages: 0,
    unreadMessages: 0,
    totalRevenue: 0
  });

  recentBookings = signal<Booking[]>([]);
  isLoading = signal<boolean>(true);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.isLoading.set(true);
    this.api.getDashboardStats().subscribe({
      next: (s) => this.stats.set(s),
      error: (err) => console.error('Error loading stats', err)
    });

    this.api.getAllBookings().subscribe({
      next: (list) => {
        this.recentBookings.set(list.slice(0, 5));
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading bookings', err);
        this.isLoading.set(false);
      }
    });
  }

  updateBookingStatus(id: number, status: 'confirmed' | 'cancelled'): void {
    this.api.updateBookingStatus(id, status).subscribe({
      next: () => {
        this.loadDashboardData();
      },
      error: (err) => console.error('Error updating status', err)
    });
  }
}
