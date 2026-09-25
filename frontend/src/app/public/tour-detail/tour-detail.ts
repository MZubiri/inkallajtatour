import { Component, OnInit, signal, inject, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Meta, Title } from '@angular/platform-browser';
import { ApiService } from '../../core/services/api.service';
import { TranslationService } from '../../core/services/translation.service';
import { Tour, BookingCreateRequest } from '../../core/models/tour.model';

@Component({
  selector: 'app-tour-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './tour-detail.html',
  styleUrls: ['./tour-detail.css']
})
export class TourDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);
  private meta = inject(Meta);
  private titleService = inject(Title);
  public tr = inject(TranslationService);

  tour = signal<Tour | null>(null);
  isLoading = signal(true);
  notFound = signal(false);
  selectedCurrency = signal<'USD' | 'PEN'>('USD');
  exchangeRate = 3.75;

  // Parsed data
  itinerary = signal<{ time: string; title: string; desc: string }[]>([]);
  includes = signal<string[]>([]);
  excludes = signal<string[]>([]);
  galleryImages = signal<string[]>([]);
  activeGalleryIndex = signal(0);
  showLightbox = signal(false);

  // Booking
  showBookingForm = signal(false);
  bookingSuccess = signal(false);
  bookingSubmitting = signal(false);
  lastBookingId = signal<number | null>(null);
  bookingForm: BookingCreateRequest = {
    clientName: '', email: '', phone: '', country: '',
    tourId: 0, travelDate: '', numberOfPeople: 2, specialRequests: ''
  };

  constructor() {
    // Re-update meta whenever language or tour changes
    effect(() => {
      const t = this.tour();
      if (t) {
        this.updateMeta(t);
      }
    });
  }

  ngOnInit(): void {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    this.bookingForm.travelDate = tomorrow.toISOString().split('T')[0];

    this.route.params.subscribe(params => {
      const slug = params['slug'];
      if (slug) {
        this.loadTour(slug);
      }
    });
  }

  loadTour(slug: string): void {
    this.isLoading.set(true);
    this.notFound.set(false);
    this.api.getTourBySlug(slug).subscribe({
      next: (tour) => {
        this.tour.set(tour);
        this.bookingForm.tourId = tour.id;
        this.parseTourData(tour);
        this.updateMeta(tour);
        this.isLoading.set(false);
      },
      error: () => {
        this.notFound.set(true);
        this.isLoading.set(false);
      }
    });
  }

  private parseTourData(tour: Tour): void {
    // Itinerary
    try {
      this.itinerary.set(tour.itinerary ? JSON.parse(tour.itinerary) : []);
    } catch { this.itinerary.set([]); }

    // Includes / Excludes
    this.includes.set(tour.includes ? tour.includes.split(',').map(s => s.trim()) : []);
    this.excludes.set(tour.excludes ? tour.excludes.split(',').map(s => s.trim()) : []);

    // Gallery
    try {
      const gallery = tour.galleryImages ? JSON.parse(tour.galleryImages) : [];
      this.galleryImages.set([tour.imageUrl, ...gallery]);
    } catch {
      this.galleryImages.set([tour.imageUrl]);
    }
  }

  private updateMeta(tour: Tour): void {
    const localizedTitle = this.tr.getTourTitle(tour.id, tour.title);
    const localizedDesc = this.tr.getTourShortDesc(tour.id, tour.shortDescription);
    this.titleService.setTitle(`${localizedTitle} | Inkallajta Tour`);
    this.meta.updateTag({ name: 'description', content: localizedDesc });
    this.meta.updateTag({ property: 'og:title', content: localizedTitle });
    this.meta.updateTag({ property: 'og:description', content: localizedDesc });
    this.meta.updateTag({ property: 'og:image', content: tour.imageUrl });
  }

  setGalleryIndex(index: number): void {
    this.activeGalleryIndex.set(index);
  }

  openLightbox(index: number): void {
    this.activeGalleryIndex.set(index);
    this.showLightbox.set(true);
  }

  closeLightbox(): void {
    this.showLightbox.set(false);
  }

  nextImage(): void {
    const imgs = this.galleryImages();
    this.activeGalleryIndex.set((this.activeGalleryIndex() + 1) % imgs.length);
  }

  prevImage(): void {
    const imgs = this.galleryImages();
    this.activeGalleryIndex.set((this.activeGalleryIndex() - 1 + imgs.length) % imgs.length);
  }

  toggleCurrency(): void {
    this.selectedCurrency.set(this.selectedCurrency() === 'USD' ? 'PEN' : 'USD');
  }

  formatPrice(priceUSD: number): string {
    if (this.selectedCurrency() === 'PEN') {
      return `S/. ${Math.round(priceUSD * this.exchangeRate)}`;
    }
    return `$${priceUSD} USD`;
  }

  calculateSubtotal(): number {
    const tour = this.tour();
    if (!tour) return 0;
    return tour.price * (this.bookingForm.numberOfPeople || 1);
  }

  onImgError(event: Event): void {
    (event.target as HTMLImageElement).src = 'https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800&q=80';
  }

  toggleBookingForm(): void {
    this.showBookingForm.set(!this.showBookingForm());
  }

  submitBooking(): void {
    if (!this.bookingForm.clientName || !this.bookingForm.email || !this.bookingForm.phone || !this.bookingForm.travelDate) {
      return;
    }
    this.bookingSubmitting.set(true);
    this.api.createBooking(this.bookingForm).subscribe({
      next: (res) => {
        this.bookingSubmitting.set(false);
        this.bookingSuccess.set(true);
        this.lastBookingId.set(res.id);
      },
      error: () => {
        this.bookingSubmitting.set(false);
      }
    });
  }

  getWhatsAppUrl(): string {
    const tour = this.tour();
    const isEn = this.tr.currentLang() === 'en';
    const title = tour ? this.tr.getTourTitle(tour.id, tour.title) : '';
    const msg = isEn
      ? `Hello Inkallajta Tour! I am interested in the "${title}" tour. Travel Date: ${this.bookingForm.travelDate}, Travelers: ${this.bookingForm.numberOfPeople}. My name is ${this.bookingForm.clientName}.`
      : `¡Hola Inkallajta Tour! Me interesa el tour "${tour?.title}". Fecha: ${this.bookingForm.travelDate}, Personas: ${this.bookingForm.numberOfPeople}. Mi nombre es ${this.bookingForm.clientName}.`;
    return `https://wa.me/51984000000?text=${encodeURIComponent(msg)}`;
  }
}
