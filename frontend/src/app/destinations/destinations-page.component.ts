import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DestinationDto, CreateDestinationDto, UpdateDestinationDto, DestinationType, DestinationDtoPagedResultDto, ApiClient } from '../services/api-client';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { LoadingComponent } from '../shared/loading/loading.component';
import { AlertComponent } from '../shared/alert/alert.component';
import { ButtonComponent } from '../shared/button/button.component';
import { ModalComponent } from '../shared/modal/modal.component';
import { ConfirmComponent } from '../shared/confirm/confirm.component';

@Component({
  selector: 'app-destinations-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, LoadingComponent, AlertComponent, ButtonComponent, ModalComponent, ConfirmComponent],
  templateUrl: './destinations-page.component.html',
  styleUrls: ['./destinations-page.component.css']
})
export class DestinationsPageComponent implements OnInit, OnDestroy {
  private readonly apiService = inject(ApiClient);

  // Estado de filtros y datos
  filter = signal<{ page?: number; pageSize?: number; search?: string; countryCode?: string; type?: string }>({ page: 1, pageSize: 5 });
  destinations = signal<DestinationDto[]>([]);
  totalCount = signal<number>(0);
  loading = signal<boolean>(false);
  selectedId = signal<number | null>(null);
  sortState = signal<{ key: 'id' | 'name' | 'description' | 'countryCode' | 'type' | 'lastModif'; dir: 'asc' | 'desc' } | null>(null);
  
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
  pageSize = 5;

  // Opciones para selects
  destinationTypes = signal<string[]>([]);
  countries = signal<string[]>([]);

  // Estado modal y formulario
  isModalOpen = signal<boolean>(false);
  isEditing = signal<boolean>(false);
  formModel: { id?: number; name: string; description: string; countryCode: string; type: number } =
    { name: '', description: '', countryCode: '', type: 0 };

  // Estado confirmación
  isConfirmOpen = signal<boolean>(false);
  confirmData = signal<{ title: string; message: string; onConfirm: () => void } | null>(null);

  // Búsqueda con debounce
  private searchChange$ = new Subject<string>();
  private subscriptions: Subscription[] = [];

  ngOnInit(): void {
    this.loadCountries();
    this.loadDestinationTypes();
    this.loadDestinations();

    // Debounce de la búsqueda
    const sub = this.searchChange$
      .pipe(debounceTime(700), distinctUntilChanged())
      .subscribe(term => {
        this.filter.update(f => ({ ...f, search: term || undefined, page: 1 }));
        this.loadDestinations();
      });
    this.subscriptions.push(sub);
  }

  ngOnDestroy(): void {
    for (const s of this.subscriptions) {
      try { s.unsubscribe(); } catch {}
    }
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

  onSearchChange(value: string): void {
    this.searchTerm = value;
    this.searchChange$.next(value);
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
    const totalPages = Math.ceil(this.totalCount() / (this.filter().pageSize || 5));
    return currentPage < totalPages;
  }

  selectRow(id: number): void {
    this.selectedId.set(id === this.selectedId() ? null : id);
  }

  onSort(key: 'id' | 'name' | 'description' | 'countryCode' | 'type' | 'lastModif'): void {
    const current = this.sortState();
    if (current?.key === key) {
      const nextDir = current.dir === 'asc' ? 'desc' : 'asc';
      this.sortState.set({ key, dir: nextDir });
    } else {
      this.sortState.set({ key, dir: 'asc' });
    }
  }

  getSortedDestinations(): DestinationDto[] {
    const data = [...(this.destinations() || [])];
    const sort = this.sortState();
    if (!sort) return data;

    const compare = (a: any, b: any): number => {
      const av = a ?? '';
      const bv = b ?? '';
      if (typeof av === 'number' && typeof bv === 'number') return av - bv;
      const as = av instanceof Date ? av.getTime() : String(av).toLowerCase();
      const bs = bv instanceof Date ? bv.getTime() : String(bv).toLowerCase();
      if (as < bs) return -1;
      if (as > bs) return 1;
      return 0;
    };

    data.sort((a, b) => {
      let res = 0;
      switch (sort.key) {
        case 'id': res = compare(a.id, b.id); break;
        case 'name': res = compare(a.name, b.name); break;
        case 'description': res = compare(a.description, b.description); break;
        case 'countryCode': res = compare(a.countryCode, b.countryCode); break;
        case 'type': res = compare(a.type, b.type); break;
        case 'lastModif': res = compare(a.lastModif, b.lastModif); break;
      }
      return sort.dir === 'asc' ? res : -res;
    });
    return data;
  }

  onCreate(): void {
    this.isEditing.set(false);
    this.formModel = {
      name: '',
      description: '',
      countryCode: this.countries()[0] || '',
      type: 0
    };
    this.isModalOpen.set(true);
  }

  onEdit(): void {
    if (this.selectedId() == null) {
      this.showAlert('warning', 'Selecciona un destino para editar');
      return;
    }
    this.isEditing.set(true);
    const current = this.destinations().find(d => d.id === this.selectedId());
    if (!current) {
      this.showAlert('error', 'No se encontró el destino seleccionado');
      return;
    }
    this.formModel = {
      id: current.id!,
      name: current.name || '',
      description: current.description || '',
      countryCode: current.countryCode || '',
      type: (current.type as unknown as number) ?? 0
    };
    this.isModalOpen.set(true);
  }

  onModalClose(): void {
    this.isModalOpen.set(false);
  }

  onSubmitForm(): void {
    const model = this.formModel;
    if (!model.name?.trim() || !model.description?.trim() || !model.countryCode) {
      this.showAlert('warning', 'Completa los campos obligatorios');
      return;
    }
    this.loading.set(true);
    if (this.isEditing()) {
      const dto = new UpdateDestinationDto();
      dto.name = model.name.trim();
      dto.description = model.description.trim();
      dto.countryCode = model.countryCode;
      dto.type = model.type as DestinationType;
      this.apiService.destinationsPUT(model.id!, dto).subscribe({
        next: (updated: DestinationDto) => {
          this.loading.set(false);
          this.isModalOpen.set(false);
          this.showAlert('success', `Destino "${updated.name}" actualizado exitosamente`);
          this.loadDestinations();
        },
        error: (error: any) => {
          this.loading.set(false);
          this.showAlert('error', 'Error al actualizar el destino: ' + this.extractApiErrorDetails(error));
        }
      });
    } else {
      const dto = new CreateDestinationDto();
      dto.name = model.name.trim();
      dto.description = model.description.trim();
      dto.countryCode = model.countryCode;
      dto.type = model.type as DestinationType;
      this.apiService.destinationsPOST(dto).subscribe({
        next: (created: DestinationDto) => {
          this.loading.set(false);
          this.isModalOpen.set(false);
          this.showAlert('success', `Destino "${created.name}" creado exitosamente`);
          this.loadDestinations();
        },
        error: (error: any) => {
          this.loading.set(false);
          this.showAlert('error', 'Error al crear el destino: ' + this.extractApiErrorDetails(error));
        }
      });
    }
  }

  private extractApiErrorDetails(error: any): string {
    try {
      const result = error?.result ?? null;
      if (result?.errors) {
        const parts: string[] = [];
        for (const key of Object.keys(result.errors)) {
          const msgs = result.errors[key];
          if (Array.isArray(msgs)) {
            for (const m of msgs) parts.push(`${key}: ${m}`);
          }
        }
        if (parts.length) return parts.join(' | ');
      }
      if (result?.title) return result.title;
      if (result?.detail) return result.detail;
      return error?.message ?? 'Error desconocido';
    } catch {
      return error?.message ?? 'Error desconocido';
    }
  }

  onRemove(): void {
    const id = this.selectedId();
    if (id == null) {
      this.showAlert('warning', 'Selecciona un destino para eliminar');
      return;
    }
    
    const destination = this.destinations().find(d => d.id === id);
    const name = destination?.name || `ID ${id}`;
    
    this.confirmData.set({
      title: 'Eliminar destino',
      message: `¿Estás seguro de que quieres eliminar "${name}"? Esta acción no se puede deshacer.`,
      onConfirm: () => this.executeDelete(id)
    });
    this.isConfirmOpen.set(true);
  }

  private executeDelete(id: number): void {
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

  onConfirmClose(): void {
    this.isConfirmOpen.set(false);
    this.confirmData.set(null);
  }

  onConfirmAction(): void {
    const data = this.confirmData();
    if (data?.onConfirm) {
      data.onConfirm();
    }
    this.onConfirmClose();
  }

  getTypeLabel(type: string | number): string {
    const names = this.destinationTypes(); // e.g., ["Beach","Mountain",...]
    const toName = (t: string | number): string => {
      if (typeof t === 'number') {
        return names[t] ?? String(t);
      }
      return t;
    };
    const name = toName(type);
    const map: Record<string, string> = {
      Beach: 'Playa',
      Mountain: 'Montaña',
      City: 'Ciudad',
      Cultural: 'Cultural',
      Adventure: 'Aventura',
      Relax: 'Relajación'
    };
    return map[name] || name;
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


