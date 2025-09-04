import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DestinationDto {
  id: number;
  name: string;
  description: string;
  countryCode: string;
  type: number; // enum como número en API
  lastModif: string;
}

export interface DestinationFilterDto {
  searchTerm?: string;
  countryCode?: string;
  type?: string | number;
  page?: number;
  pageSize?: number;
}

export interface PagedResultDto<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class DestinationsService {
  private readonly http = inject(HttpClient);

  getDestinations(filter: DestinationFilterDto): Observable<PagedResultDto<DestinationDto>> {
    let params = new HttpParams();
    Object.entries(filter).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    });
    return this.http.get<PagedResultDto<DestinationDto>>('destinations', { params });
  }

  getDestination(id: number): Observable<DestinationDto> {
    return this.http.get<DestinationDto>(`destinations/${id}`);
  }

  createDestination(payload: Omit<DestinationDto, 'id' | 'lastModif'>): Observable<DestinationDto> {
    return this.http.post<DestinationDto>('destinations', payload);
  }

  updateDestination(id: number, payload: Partial<Omit<DestinationDto, 'id' | 'lastModif'>>): Observable<DestinationDto> {
    return this.http.put<DestinationDto>(`destinations/${id}`, payload);
  }

  deleteDestination(id: number): Observable<void> {
    return this.http.delete<void>(`destinations/${id}`);
  }

  getCountries(): Observable<string[]> {
    return this.http.get<string[]>('destinations/countries');
  }

  getTypes(): Observable<string[]> {
    return this.http.get<string[]>('destinations/types');
  }
}


