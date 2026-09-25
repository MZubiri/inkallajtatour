import { Component, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-login.html',
  styleUrls: ['./admin-login.css']
})
export class AdminLoginComponent {
  private auth = inject(AuthService);
  private router = inject(Router);

  email = signal<string>('admin@inkallajtatour.com');
  password = signal<string>('Admin123!');
  isLoading = signal<boolean>(false);
  errorMessage = signal<string>('');

  fillDemoCredentials(): void {
    this.email.set('admin@inkallajtatour.com');
    this.password.set('Admin123!');
    this.errorMessage.set('');
  }

  onSubmit(): void {
    if (!this.email() || !this.password()) {
      this.errorMessage.set('Por favor ingresa usuario y contraseña.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.auth.login({ email: this.email(), password: this.password() }).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/admin/dashboard']);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          err.error?.message || 'Credenciales incorrectas. Verifica el correo y la contraseña.'
        );
      }
    });
  }
}
