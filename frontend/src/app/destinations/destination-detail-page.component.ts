import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DestinationDto, ApiClient } from '../services/api-client';
import { LoadingComponent } from '../shared/loading/loading.component';
import { AlertComponent } from '../shared/alert/alert.component';
import { ButtonComponent } from '../shared/button/button.component';

@Component({
  selector: 'app-destination-detail-page',
  standalone: true,
  imports: [CommonModule, RouterModule, LoadingComponent, AlertComponent, ButtonComponent],
  templateUrl: './destination-detail-page.component.html',
  styleUrls: ['./destination-detail-page.component.css']
})
export class DestinationDetailPageComponent implements OnInit {
  private readonly apiService = inject(ApiClient);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  destination = signal<DestinationDto | null>(null);
  loading = signal<boolean>(false);
  alert = signal<{ show: boolean; type: string; message: string }>({
    show: false,
    type: 'info',
    message: ''
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadDestination(parseInt(id, 10));
    } else {
      this.showAlert('error', 'ID de destino no válido');
    }
  }

  loadDestination(id: number): void {
    this.loading.set(true);
    this.hideAlert();

    this.apiService.destinationsGET2(id).subscribe({
      next: (destination: DestinationDto) => {
        this.destination.set(destination);
        this.loading.set(false);
      },
      error: (error: any) => {
        this.loading.set(false);
        this.showAlert('error', 'Error al cargar el destino: ' + error.message);
      }
    });
  }

  getTypeLabel(type: string | number): string {
    return String(type);
  }

  onBack(): void {
    this.router.navigate(['/destinations']);
  }

  onEdit(): void {
    const destination = this.destination();
    if (destination?.id) {
      this.router.navigate(['/destinations'], { 
        queryParams: { edit: destination.id } 
      });
    }
  }

  showAlert(type: string, message: string): void {
    this.alert.set({ show: true, type, message });
  }

  hideAlert(): void {
    this.alert.set({ show: false, type: 'info', message: '' });
  }

  onAlertDismissed(): void {
    this.hideAlert();
  }
}
