import { PageContainer } from "@/components/page-container";
import { PropertyFilters } from "@/components/properties/property-filters";
import { PropertyFiltersSkeleton } from "@/components/properties/property-filters-skeleton";
import { PropertyList } from "@/components/properties/property-list";
import { PropertyListSkeleton } from "@/components/properties/property-skeleton";
import {
  getAllProperties,
  getFilteredProperties,
  searchParamsCache,
} from "@/lib/api/properties";
import { SearchParams } from "@/types";
import { Suspense } from "react";

interface HomeProps {
  searchParams: Promise<SearchParams>;
}

export default function Home(props: HomeProps) {
  return (
    <PageContainer>
      <Suspense fallback={<PropertyFiltersSkeleton />}>
        <PropertyFilters />
      </Suspense>
      <Suspense fallback={<PropertyListSkeleton length={10} />}>
        <PropertyListWrapper {...props} />
      </Suspense>
    </PageContainer>
  );
}

async function PropertyListWrapper(props: HomeProps) {
  const searchParams = await props.searchParams;
  const filters = searchParamsCache.parse(searchParams);

  const pageNumber = filters.page || 1;
  const pageSize = filters.pageSize || 10;

  // Check if any actual filter values exist (not empty strings)
  const hasFilters = !!(
    (filters.name && filters.name.trim() !== "") ||
    (filters.address && filters.address.trim() !== "") ||
    filters.minPrice ||
    filters.maxPrice
  );

  const promise = hasFilters
    ? getFilteredProperties({
        name: filters.name || undefined,
        address: filters.address || undefined,
        minPrice: filters.minPrice || undefined,
        maxPrice: filters.maxPrice || undefined,
        pageNumber,
        pageSize,
      })
    : getAllProperties(pageNumber, pageSize);

  return <PropertyList promise={promise} />;
}
