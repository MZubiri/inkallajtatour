export interface Tour {
  id: number;
  title: string;
  slug: string;
  shortDescription: string;
  description?: string;
  itinerary?: string;
  includes?: string;
  excludes?: string;
  price: number;
  priceType: string;
  discountPrice?: number | null;
  duration: string;
  difficulty: 'easy' | 'moderate' | 'hard';
  maxGroupSize?: number;
  location: string;
  imageUrl: string;
  galleryImages?: string;
  rating: number;
  reviewCount: number;
  categoryId: number;
  categoryName?: string;
  isFeatured: boolean;
  isActive?: boolean;
  sortOrder?: number;
}

export interface Category {
  id: number;
  name: string;
  slug: string;
  description: string;
  imageUrl?: string;
  icon: string;
  sortOrder: number;
  tourCount: number;
}

export interface Testimonial {
  id: number;
  clientName: string;
  origin: string;
  photoUrl?: string;
  comment: string;
  rating: number;
  tourId?: number;
  tourName?: string;
  createdAt: string;
}

export interface Booking {
  id: number;
  clientName: string;
  email: string;
  phone: string;
  country: string;
  tourId: number;
  tourName: string;
  travelDate: string;
  numberOfPeople: number;
  totalPrice: number;
  specialRequests?: string;
  status: 'pending' | 'confirmed' | 'cancelled';
  createdAt: string;
}

export interface BookingCreateRequest {
  clientName: string;
  email: string;
  phone: string;
  country: string;
  tourId: number;
  travelDate: string;
  numberOfPeople: number;
  specialRequests?: string;
}

export interface ContactMessage {
  id: number;
  name: string;
  email: string;
  phone?: string;
  subject: string;
  message: string;
  isRead: boolean;
  createdAt: string;
}

export interface ContactCreateRequest {
  name: string;
  email: string;
  phone?: string;
  subject: string;
  message: string;
}

export interface DashboardStats {
  totalTours: number;
  totalBookings: number;
  pendingBookings: number;
  totalMessages: number;
  unreadMessages: number;
  totalRevenue: number;
}

export interface LoginResponse {
  token: string;
  fullName: string;
  email: string;
  role: string;
}
