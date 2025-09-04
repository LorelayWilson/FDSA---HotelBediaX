import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ApiService } from '../services/api.service';
import { Destination, DestinationFilter, DestinationType, CreateDestinationDto, UpdateDestinationDto } from '../models/destination.model';
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
  private readonly apiService = inject(ApiService);

  // Estado de filtros y datos
  filter = signal<DestinationFilter>({ page: 1, pageSize: 20 });
  destinations = signal<Destination[]>([]);
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
  type: DestinationType | '' = '';
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
    this.apiService.getCountries().subscribe({
      next: countries => this.countries.set(countries),
      error: error => console.error('Error al cargar países:', error)
    });
  }

  loadDestinationTypes(): void {
    this.apiService.getDestinationTypes().subscribe({
      next: types => this.destinationTypes.set(types),
      error: error => console.error('Error al cargar tipos de destino:', error)
    });
  }

  loadDestinations(): void {
    this.loading.set(true);
    this.hideAlert();
    
    const currentFilter = this.filter();
    this.apiService.getDestinations(currentFilter).subscribe({
      next: response => {
        this.destinations.set(response.items);
        this.totalCount.set(response.totalCount);
        this.loading.set(false);
      },
      error: (error) => {
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
    const newDestination: CreateDestinationDto = {
      name: 'Nuevo Destino',
      description: 'Descripción del nuevo destino',
      countryCode: this.countries()[0] || 'MEX',
      type: this.destinationTypes()[0] as DestinationType || DestinationType.City
    };

    this.apiService.createDestination(newDestination).subscribe({
      next: (destination) => {
        this.showAlert('success', `Destino "${destination.name}" creado exitosamente`);
        this.loadDestinations();
      },
      error: (error) => {
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

    const updateData: UpdateDestinationDto = {
      name: 'Destino Editado',
      description: 'Descripción actualizada'
    };

    this.apiService.updateDestination(this.selectedId()!, updateData).subscribe({
      next: (updatedDestination) => {
        this.showAlert('success', `Destino "${updatedDestination.name}" actualizado exitosamente`);
        this.loadDestinations();
      },
      error: (error) => {
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
    this.apiService.deleteDestination(id).subscribe({
      next: () => {
        this.selectedId.set(null);
        this.loadDestinations();
        this.showAlert('success', 'Destino eliminado correctamente');
      },
      error: (error) => {
        this.loading.set(false);
        this.showAlert('error', 'Error al eliminar el destino: ' + error.message);
      }
    });
  }

  getTypeLabel(type: DestinationType | string): string {
    const typeLabels: { [key in DestinationType]: string } = {
      [DestinationType.Beach]: 'Playa',
      [DestinationType.Mountain]: 'Montaña',
      [DestinationType.City]: 'Ciudad',
      [DestinationType.Cultural]: 'Cultural',
      [DestinationType.Adventure]: 'Aventura',
      [DestinationType.Relax]: 'Relajación'
    };
    return typeLabels[type as DestinationType] || type;
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


