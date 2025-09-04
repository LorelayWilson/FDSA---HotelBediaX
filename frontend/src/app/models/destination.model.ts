export interface Destination {
  id: number;
  name: string;
  description: string;
  countryCode: string;
  type: DestinationType;
  lastModif: string;
}

export enum DestinationType {
  Beach = 'Beach',
  Mountain = 'Mountain',
  City = 'City',
  Cultural = 'Cultural',
  Adventure = 'Adventure',
  Relax = 'Relax'
}

export interface DestinationFilter {
  search?: string;
  countryCode?: string;
  type?: DestinationType;
  page?: number;
  pageSize?: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface CreateDestinationDto {
  name: string;
  description: string;
  countryCode: string;
  type: DestinationType;
}

export interface UpdateDestinationDto {
  name?: string;
  description?: string;
  countryCode?: string;
  type?: DestinationType;
}
