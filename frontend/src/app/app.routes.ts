import { Routes } from '@angular/router';
import { HomeComponent } from './public/home/home';
import { TourDetailComponent } from './public/tour-detail/tour-detail';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Inkallajta Tour | Agencia de Viajes y Turismo Cusco & Machu Picchu'
  },
  {
    path: 'tours/:slug',
    component: TourDetailComponent,
    title: 'Tour | Inkallajta Tour'
  },
  {
    path: 'admin/login',
    loadComponent: () => import('./admin/login/admin-login').then(m => m.AdminLoginComponent),
    title: 'Iniciar Sesión | Admin Inkallajta'
  },
  {
    path: 'admin',
    loadComponent: () => import('./admin/layout/admin-layout').then(m => m.AdminLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./admin/dashboard/admin-dashboard').then(m => m.AdminDashboardComponent),
        title: 'Dashboard | Admin Inkallajta'
      },
      {
        path: 'tours',
        loadComponent: () => import('./admin/tours/admin-tours').then(m => m.AdminToursComponent),
        title: 'Gestión de Tours | Admin Inkallajta'
      },
      {
        path: 'bookings',
        loadComponent: () => import('./admin/bookings/admin-bookings').then(m => m.AdminBookingsComponent),
        title: 'Gestión de Reservas | Admin Inkallajta'
      },
      {
        path: 'messages',
        loadComponent: () => import('./admin/messages/admin-messages').then(m => m.AdminMessagesComponent),
        title: 'Bandeja de Mensajes | Admin Inkallajta'
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
