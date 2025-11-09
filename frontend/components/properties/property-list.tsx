"use client";

import { PropertyCard } from "@/components/properties/property-card";
import { getAllProperties, getFilteredProperties } from "@/lib/api/properties";
import { Building2 } from "lucide-react";
import { use } from "react";
import { PropertyPagination } from "./property-pagination";

interface PropertyListProps {
  promise: Promise<
    | Awaited<ReturnType<typeof getAllProperties>>
    | Awaited<ReturnType<typeof getFilteredProperties>>
  >;
}

export function PropertyList({ promise }: PropertyListProps) {
  const { data } = use(promise);

  return (
    <>
      {/* Results Count */}
      <div className="mb-2">
        <p className="text-muted-foreground">
          <span className="font-semibold text-foreground">
            {data.totalCount}
          </span>
          <Building2 className="inline-block w-4 h-4 mb-0.5 ml-1 mr-0.5" />
          {data.totalCount === 1 ? "property" : "properties"} found
        </p>
      </div>
      {/* Property Cards Grid */}
      {data.totalCount === 0 && (
        <div className="text-center py-12">
          <Building2 className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
          <h3 className="text-lg font-semibold mb-2">No properties found</h3>
          <p className="text-muted-foreground">
            Try adjusting your filters to see more results
          </p>
        </div>
      )}
      {data.totalCount > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data.items.map((property) => (
            <PropertyCard key={property.idProperty} property={property} />
          ))}
        </div>
      )}
      {/* Pagination component */}
      <PropertyPagination
        currentPage={data.currentPage}
        totalPages={data.totalPages}
        totalCount={data.totalCount}
      />
    </>
  );
}
