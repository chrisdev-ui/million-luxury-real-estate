import {
  parseAsString,
  parseAsInteger,
  createSearchParamsCache,
} from "nuqs/server";

import type {
  ApiResponse,
  Property,
  PropertyDetail,
  PropertyFilters,
  PagedResponse,
} from "./types";
import { apiFetch } from "./client";

export const searchParamsCache = createSearchParamsCache({
  name: parseAsString,
  address: parseAsString,
  minPrice: parseAsInteger,
  maxPrice: parseAsInteger,
  page: parseAsInteger.withDefault(1),
  pageSize: parseAsInteger.withDefault(10),
});

export async function getAllProperties(
  pageNumber: number = 1,
  pageSize: number = 10
): Promise<ApiResponse<PagedResponse<Property>>> {
  return apiFetch<ApiResponse<PagedResponse<Property>>>(
    `/api/properties?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );
}

export async function getFilteredProperties(
  filters: PropertyFilters
): Promise<ApiResponse<PagedResponse<Property>>> {
  // Build query params object, only including defined values
  const queryParams: Record<string, string | number> = {
    pageNumber: filters.pageNumber || 1,
    pageSize: filters.pageSize || 10,
  };

  // Only add filter params if they have actual values
  if (filters.name && filters.name.trim() !== "") {
    queryParams.name = filters.name;
  }
  if (filters.address && filters.address.trim() !== "") {
    queryParams.address = filters.address;
  }
  if (filters.minPrice !== undefined && filters.minPrice !== null) {
    queryParams.minPrice = filters.minPrice;
  }
  if (filters.maxPrice !== undefined && filters.maxPrice !== null) {
    queryParams.maxPrice = filters.maxPrice;
  }

  // Build query string manually
  const params = new URLSearchParams();
  Object.entries(queryParams).forEach(([key, value]) => {
    params.append(key, value.toString());
  });

  return apiFetch<ApiResponse<PagedResponse<Property>>>(
    `/api/properties/filter?${params.toString()}`
  );
}

export async function getPropertyById(
  id: string
): Promise<ApiResponse<PropertyDetail>> {
  if (!id) throw new Error("Property ID is required");

  return apiFetch<ApiResponse<PropertyDetail>>(`/api/properties/${id}`);
}
