import { PropertyDetailModal } from "@/components/properties/property-detail-modal";
import { Skeleton } from "@/components/ui/skeleton";
import { getPropertyById } from "@/lib/api/properties";
import { Suspense } from "react";
import { Modal } from "./modal";

interface PropertyDetailModalPageProps {
  params: Promise<{ id: string }>;
}

function PropertyDetailSkeleton() {
  return (
    <div className="space-y-6">
      <Skeleton className="aspect-video w-full rounded-lg" />
      <div className="space-y-2">
        <Skeleton className="h-8 w-3/4" />
        <Skeleton className="h-4 w-1/2" />
        <Skeleton className="h-8 w-32" />
      </div>
      <div className="grid grid-cols-3 gap-2">
        <Skeleton className="aspect-video rounded-md" />
        <Skeleton className="aspect-video rounded-md" />
        <Skeleton className="aspect-video rounded-md" />
      </div>
    </div>
  );
}

export default async function PropertyDetailModalPage({
  params,
}: PropertyDetailModalPageProps) {
  const { id } = await params;
  const promise = getPropertyById(id);

  return (
    <Modal>
      <Suspense fallback={<PropertyDetailSkeleton />}>
        <PropertyDetailModal promise={promise} />
      </Suspense>
    </Modal>
  );
}
