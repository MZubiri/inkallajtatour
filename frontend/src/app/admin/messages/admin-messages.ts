import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../core/services/api.service';
import { ContactMessage } from '../../core/models/tour.model';

@Component({
  selector: 'app-admin-messages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-messages.html',
  styleUrls: ['./admin-messages.css']
})
export class AdminMessagesComponent implements OnInit {
  private api = inject(ApiService);

  messages = signal<ContactMessage[]>([]);
  isLoading = signal<boolean>(true);

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.isLoading.set(true);
    this.api.getContactMessages().subscribe({
      next: (list) => {
        this.messages.set(list);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading messages', err);
        this.isLoading.set(false);
      }
    });
  }

  markAsRead(id: number): void {
    this.api.markMessageRead(id).subscribe({
      next: () => this.loadMessages(),
      error: (err) => console.error('Error marking as read', err)
    });
  }

  deleteMessage(id: number): void {
    if (confirm('¿Eliminar este mensaje?')) {
      this.api.deleteContactMessage(id).subscribe({
        next: () => this.loadMessages(),
        error: (err) => console.error('Error deleting message', err)
      });
    }
  }
}
