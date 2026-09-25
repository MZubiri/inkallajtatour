import { Component, OnInit, AfterViewInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { TranslationService } from '../../core/services/translation.service';
import { Tour, Category, Testimonial, BookingCreateRequest, ContactCreateRequest } from '../../core/models/tour.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit, AfterViewInit {
  private api = inject(ApiService);
  public tr = inject(TranslationService);

  // Data signals
  tours = signal<Tour[]>([]);
  categories = signal<Category[]>([]);
  testimonials = signal<Testimonial[]>([]);
  isLoading = signal<boolean>(true);

  // Filters & State
  selectedCategory = signal<number | null>(null);
  searchQuery = signal<string>('');
  selectedCurrency = signal<'USD' | 'PEN'>('USD');
  exchangeRate = 3.75;

  // Mobile menu
  mobileMenuOpen = signal(false);

  // Testimonial carousel
  activeTestimonialIndex = signal(0);
  private testimonialInterval: ReturnType<typeof setInterval> | null = null;

  // Modal / Detail state
  activeTour = signal<Tour | null>(null);
  activeTourItinerary = signal<{ time: string; title: string; desc: string }[]>([]);
  activeTourIncludes = signal<string[]>([]);
  activeTourExcludes = signal<string[]>([]);
  showBookingModal = signal<boolean>(false);
  bookingSuccess = signal<boolean>(false);
  bookingSubmitting = signal<boolean>(false);
  lastBookingId = signal<number | null>(null);

  // Booking Form model
  bookingForm: BookingCreateRequest = {
    clientName: '',
    email: '',
    phone: '',
    country: '',
    tourId: 0,
    travelDate: '',
    numberOfPeople: 2,
    specialRequests: ''
  };

  // Contact Form model
  contactForm: ContactCreateRequest = {
    name: '',
    email: '',
    phone: '',
    subject: '',
    message: ''
  };
  contactSuccess = signal<boolean>(false);
  contactSubmitting = signal<boolean>(false);

  // Computed filtered tours (bilingual search)
  filteredTours = computed(() => {
    let list = this.tours();
    const catId = this.selectedCategory();
    const query = this.searchQuery().toLowerCase().trim();

    if (catId !== null) {
      list = list.filter(t => t.categoryId === catId);
    }
    if (query) {
      list = list.filter(t => {
        const esTitle = t.title.toLowerCase();
        const enTitle = this.tr.getTourTitle(t.id, t.title).toLowerCase();
        const esDesc = t.shortDescription.toLowerCase();
        const enDesc = this.tr.getTourShortDesc(t.id, t.shortDescription).toLowerCase();
        const loc = t.location.toLowerCase();
        return esTitle.includes(query) || enTitle.includes(query) ||
               esDesc.includes(query) || enDesc.includes(query) ||
               loc.includes(query);
      });
    }
    return list;
  });

  getCategoryDisplayName(cat: Category): string {
    if (this.tr.currentLang() === 'en') {
      if (cat.slug.includes('machu-picchu')) return 'Machu Picchu Tours';
      if (cat.slug.includes('day-tours')) return 'Day Tours';
      if (cat.slug.includes('treks')) return 'Adventure Treks';
      if (cat.slug.includes('peru')) return 'Peru Circuits';
    }
    return cat.name;
  }

  ngOnInit(): void {
    this.loadData();
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    this.bookingForm.travelDate = tomorrow.toISOString().split('T')[0];
  }

  ngAfterViewInit(): void {
    this.initScrollAnimations();
    this.startTestimonialCarousel();
  }

  loadData(): void {
    this.isLoading.set(true);
    this.api.getCategories().subscribe({
      next: (cats) => this.categories.set(cats),
      error: (err) => console.error('Error loading categories', err)
    });

    this.api.getTours().subscribe({
      next: (data) => {
        this.tours.set(data);
        this.isLoading.set(false);
        // Re-observe after DOM updates
        setTimeout(() => this.initScrollAnimations(), 100);
      },
      error: (err) => {
        console.error('Error loading tours', err);
        this.isLoading.set(false);
      }
    });

    this.api.getTestimonials().subscribe({
      next: (tests) => this.testimonials.set(tests),
      error: (err) => console.error('Error loading testimonials', err)
    });
  }

  // ===== SCROLL ANIMATIONS =====
  private initScrollAnimations(): void {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            entry.target.classList.add('scroll-visible');
            observer.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.1, rootMargin: '0px 0px -50px 0px' }
    );

    document.querySelectorAll('.scroll-reveal').forEach(el => observer.observe(el));
  }

  // ===== TESTIMONIAL CAROUSEL =====
  private startTestimonialCarousel(): void {
    this.testimonialInterval = setInterval(() => {
      const total = this.testimonials().length;
      if (total > 0) {
        this.activeTestimonialIndex.set(
          (this.activeTestimonialIndex() + 1) % total
        );
      }
    }, 5000);
  }

  setTestimonialIndex(i: number): void {
    this.activeTestimonialIndex.set(i);
  }

  // ===== IMAGE FALLBACK =====
  onImgError(event: Event): void {
    (event.target as HTMLImageElement).src = 'https://images.unsplash.com/photo-1526392060635-9d6019884377?w=800&q=80';
  }

  // ===== MOBILE MENU =====
  toggleMobileMenu(): void {
    this.mobileMenuOpen.set(!this.mobileMenuOpen());
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen.set(false);
  }

  setCategory(catId: number | null): void {
    this.selectedCategory.set(catId);
    this.closeMobileMenu();
  }

  toggleCurrency(): void {
    this.selectedCurrency.set(this.selectedCurrency() === 'USD' ? 'PEN' : 'USD');
  }

  formatPrice(priceUSD: number): string {
    if (this.selectedCurrency() === 'PEN') {
      const pen = Math.round(priceUSD * this.exchangeRate);
      return `S/. ${pen}`;
    }
    return `$${priceUSD} USD`;
  }

  openTourDetail(tour: Tour): void {
    this.activeTour.set(tour);
    this.bookingForm.tourId = tour.id;
    this.bookingSuccess.set(false);

    try {
      if (tour.itinerary) {
        this.activeTourItinerary.set(JSON.parse(tour.itinerary));
      } else {
        this.activeTourItinerary.set([]);
      }
    } catch {
      this.activeTourItinerary.set([]);
    }

    this.activeTourIncludes.set(tour.includes ? tour.includes.split(',') : []);
    this.activeTourExcludes.set(tour.excludes ? tour.excludes.split(',') : []);

    this.showBookingModal.set(true);
  }

  closeTourDetail(): void {
    this.showBookingModal.set(false);
    this.activeTour.set(null);
  }

  changePeople(delta: number): void {
    const current = this.bookingForm.numberOfPeople || 1;
    this.bookingForm.numberOfPeople = Math.max(1, current + delta);
  }

  calculateSubtotal(): number {
    const tour = this.activeTour();
    if (!tour) return 0;
    return tour.price * (this.bookingForm.numberOfPeople || 1);
  }

  submitBooking(): void {
    if (!this.bookingForm.clientName || !this.bookingForm.email || !this.bookingForm.phone || !this.bookingForm.travelDate) {
      alert('Por favor completa todos los campos requeridos para la reserva.');
      return;
    }

    this.bookingSubmitting.set(true);
    this.api.createBooking(this.bookingForm).subscribe({
      next: (res) => {
        this.bookingSubmitting.set(false);
        this.bookingSuccess.set(true);
        this.lastBookingId.set(res.id);
      },
      error: (err) => {
        console.error('Error saving booking', err);
        this.bookingSubmitting.set(false);
        alert('Hubo un problema al procesar tu reserva. Intenta de nuevo.');
      }
    });
  }

  getWhatsAppBookingUrl(): string {
    const tour = this.activeTour();
    const phone = '51984000000';
    const isEn = this.tr.currentLang() === 'en';
    const tourTitle = tour ? this.tr.getTourTitle(tour.id, tour.title) : '';
    const msgText = isEn
      ? `Hello Inkallajta Tour! I want to confirm my reservation #${this.lastBookingId() || ''} for "${tourTitle}". Travel Date: ${this.bookingForm.travelDate}, Travelers: ${this.bookingForm.numberOfPeople}. My name is ${this.bookingForm.clientName}.`
      : `¡Hola Inkallajta Tour! Deseo confirmar mi reserva #${this.lastBookingId() || ''} para el "${tour?.title}". Fecha: ${this.bookingForm.travelDate}, Personas: ${this.bookingForm.numberOfPeople}. Mi nombre es ${this.bookingForm.clientName}.`;
    return `https://wa.me/${phone}?text=${encodeURIComponent(msgText)}`;
  }

  submitContact(): void {
    if (!this.contactForm.name || !this.contactForm.email || !this.contactForm.message) {
      alert('Por favor completa nombre, email y tu consulta.');
      return;
    }

    this.contactSubmitting.set(true);
    this.api.sendContactMessage(this.contactForm).subscribe({
      next: () => {
        this.contactSubmitting.set(false);
        this.contactSuccess.set(true);
        this.contactForm = { name: '', email: '', phone: '', subject: '', message: '' };
        setTimeout(() => this.contactSuccess.set(false), 6000);
      },
      error: (err) => {
        console.error('Error sending message', err);
        this.contactSubmitting.set(false);
        alert('Error al enviar mensaje. Por favor intenta nuevamente.');
      }
    });
  }

  scrollToSection(id: string): void {
    this.closeMobileMenu();
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth' });
    }
  }
}
