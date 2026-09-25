import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Tour,
  Category,
  Booking,
  BookingCreateRequest,
  Testimonial,
  ContactMessage,
  ContactCreateRequest,
  DashboardStats
} from '../models/tour.model';
import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: token ? `Bearer ${token}` : ''
    });
  }

  // --- TOURS ---
  getTours(categoryId?: number, featured?: boolean, search?: string): Observable<Tour[]> {
    let params = new HttpParams();
    if (categoryId) params = params.set('categoryId', categoryId.toString());
    if (featured !== undefined) params = params.set('featured', featured.toString());
    if (search) params = params.set('search', search);
    return this.http.get<Tour[]>(`${this.baseUrl}/tours`, { params });
  }

  getTourBySlug(slug: string): Observable<Tour> {
    return this.http.get<Tour>(`${this.baseUrl}/tours/${slug}`);
  }

  getTourById(id: number): Observable<Tour> {
    return this.http.get<Tour>(`${this.baseUrl}/tours/by-id/${id}`);
  }

  getAllAdminTours(): Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.baseUrl}/tours/admin/all`, {
      headers: this.getAuthHeaders()
    });
  }

  createTour(tour: Partial<Tour>): Observable<Tour> {
    return this.http.post<Tour>(`${this.baseUrl}/tours`, tour, {
      headers: this.getAuthHeaders()
    });
  }

  updateTour(id: number, tour: Partial<Tour>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/tours/${id}`, tour, {
      headers: this.getAuthHeaders()
    });
  }

  deleteTour(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/tours/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // --- CATEGORIES ---
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/categories`);
  }

  // --- TESTIMONIALS ---
  getTestimonials(): Observable<Testimonial[]> {
    return this.http.get<Testimonial[]>(`${this.baseUrl}/testimonials`);
  }

  // --- BOOKINGS ---
  createBooking(booking: BookingCreateRequest): Observable<{ id: number; totalPrice: number }> {
    return this.http.post<{ id: number; totalPrice: number }>(`${this.baseUrl}/bookings`, booking);
  }

  getAllBookings(status?: string): Observable<Booking[]> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    return this.http.get<Booking[]>(`${this.baseUrl}/bookings`, {
      headers: this.getAuthHeaders(),
      params
    });
  }

  updateBookingStatus(id: number, status: 'pending' | 'confirmed' | 'cancelled'): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/bookings/${id}/status`,
      { status },
      { headers: this.getAuthHeaders() }
    );
  }

  deleteBooking(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/bookings/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // --- CONTACT ---
  sendContactMessage(msg: ContactCreateRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.baseUrl}/contact`, msg);
  }

  getContactMessages(): Observable<ContactMessage[]> {
    return this.http.get<ContactMessage[]>(`${this.baseUrl}/contact`, {
      headers: this.getAuthHeaders()
    });
  }

  markMessageRead(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/contact/${id}/read`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  deleteContactMessage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/contact/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // --- DASHBOARD ---
  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/dashboard/stats`, {
      headers: this.getAuthHeaders()
    });
  }
}
