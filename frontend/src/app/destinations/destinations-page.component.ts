import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DestinationDto, CreateDestinationDto, UpdateDestinationDto, DestinationType, DestinationDtoPagedResultDto, ApiClient } from '../services/api-client';
import { LoadingComponent } from '../shared/loading/loading.component';
import { AlertComponent } from '../shared/alert/alert.component';
import { ButtonComponent } from '../shared/button/button.component';

@Component({
  selector: 'app-destinations-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, LoadingComponent, AlertComponent, ButtonComponent],
  templateUrl: './destinations-page.component.html',
  styleUrls: ['./destinations-page.component.css']
})
export class DestinationsPageComponent implements OnInit {
  private readonly apiService = inject(ApiClient);

  // Estado de filtros y datos
  filter = signal<{ page?: number; pageSize?: number; search?: string; countryCode?: string; type?: string }>({ page: 1, pageSize: 20 });
  destinations = signal<DestinationDto[]>([]);
  totalCount = signal<number>(0);
  loading = signal<boolean>(false);
  selectedId = signal<number | null>(null);
  
  // Estados de alertas
  alert = signal<{ show: boolean; type: string; message: string }>({
    show: false,
    type: 'info',
    message: ''
  });

  // Modelos para ngModel
  searchTerm = '';
  countryCode = '';
  type: string = '';
  pageSize = 20;

  // Opciones para selects
  destinationTypes = signal<string[]>([]);
  countries = signal<string[]>([]);

  ngOnInit(): void {
    this.loadCountries();
    this.loadDestinationTypes();
    this.loadDestinations();
  }

  loadCountries(): void {
    this.apiService.countries().subscribe({
      next: (countries: string[]) => this.countries.set(countries),
      error: (error: any) => console.error('Error al cargar países:', error)
    });
  }

  loadDestinationTypes(): void {
    this.apiService.types().subscribe({
      next: (types: string[]) => this.destinationTypes.set(types),
      error: (error: any) => console.error('Error al cargar tipos de destino:', error)
    });
  }

  loadDestinations(): void {
    this.loading.set(true);
    this.hideAlert();
    
    const currentFilter = this.filter();
    this.apiService.destinationsGET(
      currentFilter.search,
      currentFilter.countryCode,
      currentFilter.type as unknown as DestinationType,
      currentFilter.page,
      currentFilter.pageSize
    ).subscribe({
      next: (response: DestinationDtoPagedResultDto) => {
        this.destinations.set(response.items || []);
        this.totalCount.set(response.totalCount || 0);
        this.loading.set(false);
      },
      error: (error: any) => {
        this.loading.set(false);
        this.showAlert('error', 'Error al cargar los destinos: ' + error.message);
      }
    });
  }

  onSearch(): void {
    this.filter.update(f => ({ 
      ...f, 
      search: this.searchTerm || undefined, 
      page: 1 
    }));
    this.loadDestinations();
  }

  onFilterChange(): void {
    this.filter.update(f => ({
      ...f,
      page: 1,
      countryCode: this.countryCode || undefined,
      type: this.type || undefined,
      pageSize: this.pageSize
    }));
    this.loadDestinations();
  }

  changePage(offset: number): void {
    const currentPage = this.filter().page || 1;
    const newPage = Math.max(1, currentPage + offset);
    this.filter.update(f => ({ ...f, page: newPage }));
    this.loadDestinations();
  }

  canGoToPreviousPage(): boolean {
    return (this.filter().page || 1) > 1;
  }

  canGoToNextPage(): boolean {
    const currentPage = this.filter().page || 1;
    const totalPages = Math.ceil(this.totalCount() / (this.filter().pageSize || 20));
    return currentPage < totalPages;
  }

  selectRow(id: number): void {
    this.selectedId.set(id === this.selectedId() ? null : id);
  }

  onCreate(): void {
    const newDestination = new CreateDestinationDto();
    newDestination.name = 'Nuevo Destino';
    newDestination.description = 'Descripción del nuevo destino';
    newDestination.countryCode = this.countries()[0] || 'MEX';
    newDestination.type = (this.destinationTypes()[0] as unknown as DestinationType) || DestinationType._0;

    this.apiService.destinationsPOST(newDestination).subscribe({
      next: (destination: DestinationDto) => {
        this.showAlert('success', `Destino "${destination.name}" creado exitosamente`);
        this.loadDestinations();
      },
      error: (error: any) => {
        console.error('Error al crear destino:', error);
        this.showAlert('error', 'Error al crear el destino');
      }
    });
  }

  onEdit(): void {
    if (this.selectedId() == null) {
      this.showAlert('warning', 'Selecciona un destino para editar');
      return;
    }

    const updateData = new UpdateDestinationDto();
    updateData.name = 'Destino Editado';
    updateData.description = 'Descripción actualizada';

    this.apiService.destinationsPUT(this.selectedId()!, updateData).subscribe({
      next: (updatedDestination: DestinationDto) => {
        this.showAlert('success', `Destino "${updatedDestination.name}" actualizado exitosamente`);
        this.loadDestinations();
      },
      error: (error: any) => {
        console.error('Error al actualizar destino:', error);
        this.showAlert('error', 'Error al actualizar el destino');
      }
    });
  }

  onRemove(): void {
    const id = this.selectedId();
    if (id == null) {
      this.showAlert('warning', 'Selecciona un destino para eliminar');
      return;
    }
    
    if (!confirm(`¿Eliminar destino ${id}?`)) return;
    
    this.loading.set(true);
    this.apiService.destinationsDELETE(id).subscribe({
      next: () => {
        this.selectedId.set(null);
        this.loadDestinations();
        this.showAlert('success', 'Destino eliminado correctamente');
      },
      error: (error: any) => {
        this.loading.set(false);
        this.showAlert('error', 'Error al eliminar el destino: ' + error.message);
      }
    });
  }

  getTypeLabel(type: string): string {
    // Mapeo automático basado en el enum del backend
    const typeLabels: Record<string, string> = {
      [DestinationType._0]: 'Playa',
      [DestinationType._1]: 'Montaña', 
      [DestinationType._2]: 'Ciudad',
      [DestinationType._3]: 'Cultural',
      [DestinationType._4]: 'Aventura',
      [DestinationType._5]: 'Relajación'
    };
    return typeLabels[type] || type;
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


