import { Component, OnInit, computed, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DestinationsService, DestinationDto, DestinationFilterDto } from './destinations.service';

@Component({
  selector: 'app-destinations-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './destinations-page.component.html',
  styleUrls: ['./destinations-page.component.css']
})
export class DestinationsPageComponent implements OnInit {
  private readonly service = inject(DestinationsService);

  // Estado de filtros y datos
  filter = signal<DestinationFilterDto>({ page: 1, pageSize: 20 });
  countries = signal<string[]>([]);
  types = signal<string[]>([]);
  totalCount = signal<number>(0);
  items = signal<DestinationDto[]>([]);
  loading = signal<boolean>(false);
  selectedId = signal<number | null>(null);

  // Modelos para ngModel
  country = '';
  type: string | number = '';
  pageSize = 20;

  ngOnInit(): void {
    this.loadReferenceData();
    this.country = this.filter().countryCode ?? '';
    this.type = this.filter().type ?? '';
    this.pageSize = this.filter().pageSize ?? 20;
    this.loadPage();
  }

  loadReferenceData(): void {
    this.service.getCountries().subscribe(c => this.countries.set(c));
    this.service.getTypes().subscribe(t => this.types.set(t));
  }

  loadPage(): void {
    this.loading.set(true);
    this.service.getDestinations(this.filter()).subscribe({
      next: page => {
        this.items.set(page.items);
        this.totalCount.set(page.totalCount);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onSearch(term: string): void {
    this.filter.update(f => ({ ...f, searchTerm: term, page: 1 }));
    this.loadPage();
  }

  onFilterChange(): void {
    this.filter.update(f => ({
      ...f,
      page: 1,
      countryCode: this.country || undefined,
      type: this.type || undefined,
      pageSize: this.pageSize
    }));
    this.loadPage();
  }

  changePage(offset: number): void {
    this.filter.update(f => ({ ...f, page: Math.max(1, (f.page ?? 1) + offset) }));
    this.loadPage();
  }

  typeLabel(type: number): string {
    const list = this.types();
    return list?.[type] ?? String(type);
  }

  selectRow(id: number): void {
    this.selectedId.set(id === this.selectedId() ? null : id);
  }

  onCreate(): void {
    // esqueleto: aquí abriremos un modal/formulario
    console.log('Create destiny');
  }

  onEdit(): void {
    if (this.selectedId() == null) return;
    console.log('Modify destiny', this.selectedId());
  }

  onRemove(): void {
    const id = this.selectedId();
    if (id == null) return;
    if (!confirm(`¿Eliminar destino ${id}?`)) return;
    this.loading.set(true);
    this.service.deleteDestination(id).subscribe({
      next: () => {
        this.selectedId.set(null);
        this.loadPage();
      },
      error: () => this.loading.set(false)
    });
  }
}


