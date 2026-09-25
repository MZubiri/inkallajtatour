import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tour, Category } from '../../core/models/tour.model';

@Component({
  selector: 'app-admin-tours',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-tours.html',
  styleUrls: ['./admin-tours.css']
})
export class AdminToursComponent implements OnInit {
  private api = inject(ApiService);

  tours = signal<Tour[]>([]);
  categories = signal<Category[]>([]);
  isLoading = signal<boolean>(true);

  // Modal create/edit state
  showModal = signal<boolean>(false);
  isEditing = signal<boolean>(false);
  selectedTourId = signal<number | null>(null);

  // Form model
  tourForm: any = {
    title: '',
    slug: '',
    shortDescription: '',
    description: '',
    itinerary: '',
    includes: '',
    excludes: '',
    price: 100,
    priceType: 'per_person',
    duration: '1 día',
    difficulty: 'easy',
    maxGroupSize: 16,
    location: 'Cusco',
    imageUrl: 'https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800',
    rating: 4.8,
    reviewCount: 50,
    categoryId: 1,
    isFeatured: false,
    isActive: true,
    sortOrder: 1
  };

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading.set(true);
    this.api.getCategories().subscribe({
      next: (cats) => this.categories.set(cats)
    });

    this.api.getAllAdminTours().subscribe({
      next: (list) => {
        this.tours.set(list);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading tours', err);
        this.isLoading.set(false);
      }
    });
  }

  openCreateModal(): void {
    this.isEditing.set(false);
    this.selectedTourId.set(null);
    this.tourForm = {
      title: '',
      slug: '',
      shortDescription: '',
      description: '',
      itinerary: '[]',
      includes: 'Transporte,Guía profesional,Entradas',
      excludes: 'Alimentación,Propinas',
      price: 99,
      priceType: 'per_person',
      duration: '1 día',
      difficulty: 'easy',
      maxGroupSize: 16,
      location: 'Cusco',
      imageUrl: 'https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=800',
      rating: 4.8,
      reviewCount: 10,
      categoryId: this.categories()[0]?.id || 1,
      isFeatured: false,
      isActive: true,
      sortOrder: this.tours().length + 1
    };
    this.showModal.set(true);
  }

  openEditModal(tour: Tour): void {
    this.isEditing.set(true);
    this.selectedTourId.set(tour.id);
    this.api.getTourById(tour.id).subscribe({
      next: (fullTour) => {
        this.tourForm = {
          title: fullTour.title,
          slug: fullTour.slug,
          shortDescription: fullTour.shortDescription,
          description: fullTour.description || '',
          itinerary: fullTour.itinerary || '[]',
          includes: fullTour.includes || '',
          excludes: fullTour.excludes || '',
          price: fullTour.price,
          priceType: fullTour.priceType || 'per_person',
          duration: fullTour.duration,
          difficulty: fullTour.difficulty,
          maxGroupSize: fullTour.maxGroupSize || 16,
          location: fullTour.location,
          imageUrl: fullTour.imageUrl,
          rating: fullTour.rating,
          reviewCount: fullTour.reviewCount,
          categoryId: fullTour.categoryId,
          isFeatured: fullTour.isFeatured,
          isActive: true,
          sortOrder: 1
        };
        this.showModal.set(true);
      }
    });
  }

  closeModal(): void {
    this.showModal.set(false);
  }

  generateSlug(): void {
    if (!this.isEditing() && this.tourForm.title) {
      this.tourForm.slug = this.tourForm.title
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .replace(/[^a-z0-9]+/g, '-')
        .replace(/(^-|-$)+/g, '');
    }
  }

  saveTour(): void {
    if (!this.tourForm.title || !this.tourForm.slug || !this.tourForm.price) {
      alert('Por favor completa título, slug y precio.');
      return;
    }

    if (this.isEditing() && this.selectedTourId()) {
      this.api.updateTour(this.selectedTourId()!, this.tourForm).subscribe({
        next: () => {
          this.closeModal();
          this.loadData();
        },
        error: (err) => console.error('Error updating tour', err)
      });
    } else {
      this.api.createTour(this.tourForm).subscribe({
        next: () => {
          this.closeModal();
          this.loadData();
        },
        error: (err) => console.error('Error creating tour', err)
      });
    }
  }

  deleteTour(id: number, title: string): void {
    if (confirm(`¿Estás seguro de eliminar el tour "${title}"?`)) {
      this.api.deleteTour(id).subscribe({
        next: () => this.loadData(),
        error: (err) => console.error('Error deleting tour', err)
      });
    }
  }
}
