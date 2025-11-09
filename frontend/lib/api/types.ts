export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
  timestamp: string;
}

export interface PagedResponse<T> {
  items: T[];
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface Property {
  idProperty: string;
  idOwner: string;
  name: string;
  address: string;
  price: number;
  codeInternal: string;
  year: number;
  mainImage: string;
  createdAt: string; // ISO date string
  updatedAt: string; // ISO date string
  enabled: boolean;
}

export interface PropertyDetail extends Property {
  owner?: Owner | null;
  images?: PropertyImage[];
  traces?: PropertyTrace[];
}

export interface PropertyImage {
  idPropertyImage: string;
  file: string; // Image URL
  enabled: boolean;
}

export interface PropertyTrace {
  idPropertyTrace: string;
  dateSale: string;
  name: string;
  value: number;
  tax: number;
}

export interface Owner {
  idOwner: string;
  name: string;
  address: string;
  photo: string;
}

export interface PropertyFilters {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  pageNumber?: number; // Default: 1
  pageSize?: number; // Default: 10
}
