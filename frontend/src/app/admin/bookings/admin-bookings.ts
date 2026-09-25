import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../core/services/api.service';
import { Booking } from '../../core/models/tour.model';

@Component({
  selector: 'app-admin-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-bookings.html',
  styleUrls: ['./admin-bookings.css']
})
export class AdminBookingsComponent implements OnInit {
  private api = inject(ApiService);

  bookings = signal<Booking[]>([]);
  isLoading = signal<boolean>(true);
  statusFilter = signal<string>('all');

  // Detail drawer
  selectedBooking = signal<Booking | null>(null);

  filteredBookings = computed(() => {
    const list = this.bookings();
    const filter = this.statusFilter();
    if (filter === 'all') return list;
    return list.filter(b => b.status === filter);
  });

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading.set(true);
    this.api.getAllBookings().subscribe({
      next: (list) => {
        this.bookings.set(list);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading bookings', err);
        this.isLoading.set(false);
      }
    });
  }

  setStatusFilter(status: string): void {
    this.statusFilter.set(status);
  }

  updateStatus(id: number, status: 'pending' | 'confirmed' | 'cancelled'): void {
    this.api.updateBookingStatus(id, status).subscribe({
      next: () => {
        this.loadBookings();
        if (this.selectedBooking()?.id === id) {
          const updated = this.selectedBooking()!;
          this.selectedBooking.set({ ...updated, status });
        }
      },
      error: (err) => console.error('Error updating status', err)
    });
  }

  deleteBooking(id: number): void {
    if (confirm(`¿Estás seguro de eliminar la reserva #${id}?`)) {
      this.api.deleteBooking(id).subscribe({
        next: () => {
          this.loadBookings();
          if (this.selectedBooking()?.id === id) {
            this.selectedBooking.set(null);
          }
        },
        error: (err) => console.error('Error deleting booking', err)
      });
    }
  }

  viewDetail(booking: Booking): void {
    this.selectedBooking.set(booking);
  }

  closeDetail(): void {
    this.selectedBooking.set(null);
  }
}
