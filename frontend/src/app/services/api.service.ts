import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Destination, DestinationFilter, PaginatedResponse, CreateDestinationDto, UpdateDestinationDto } from '../models/destination.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5259/api/v1.0';

  constructor(private http: HttpClient) {}

  getDestinations(filter: DestinationFilter = {}): Observable<PaginatedResponse<Destination>> {
    let params = new HttpParams();
    if (filter.search) params = params.set('search', filter.search);
    if (filter.countryCode) params = params.set('countryCode', filter.countryCode);
    if (filter.type) params = params.set('type', filter.type);
    if (filter.page) params = params.set('page', filter.page.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());
    return this.http.get<PaginatedResponse<Destination>>(`${this.baseUrl}/destinations`, { params });
  }

  deleteDestination(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/destinations/${id}`);
  }

  getCountries(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/destinations/countries`);
  }

  getDestinationTypes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/destinations/types`);
  }

  getDestinationById(id: number): Observable<Destination> {
    return this.http.get<Destination>(`${this.baseUrl}/destinations/${id}`);
  }

  createDestination(destination: CreateDestinationDto): Observable<Destination> {
    return this.http.post<Destination>(`${this.baseUrl}/destinations`, destination);
  }

  updateDestination(id: number, destination: UpdateDestinationDto): Observable<Destination> {
    return this.http.put<Destination>(`${this.baseUrl}/destinations/${id}`, destination);
  }
}
