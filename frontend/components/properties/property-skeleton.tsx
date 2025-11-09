import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";

export function PropertyCardSkeleton() {
  return (
    <Card className="overflow-hidden border-0 shadow-lg bg-card pt-0">
      <div className="relative">
        {/* Image skeleton */}
        <Skeleton className="w-full h-56" />

        {/* Badge skeletons */}
        <div className="absolute top-3 left-3 flex gap-2">
          <Skeleton className="h-6 w-20 rounded-full" />
          <Skeleton className="h-6 w-16 rounded-full" />
        </div>
      </div>

      <CardContent>
        {/* Stats row skeleton */}
        <div className="flex items-center justify-between gap-4 mb-4">
          <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
            <Skeleton className="w-5 h-5 rounded" />
            <Skeleton className="h-4 w-14" />
          </div>
          <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
            <Skeleton className="w-5 h-5 rounded" />
            <Skeleton className="h-4 w-16" />
          </div>
          <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
            <Skeleton className="w-5 h-5 rounded" />
            <Skeleton className="h-4 w-12" />
          </div>
        </div>

        {/* Property details skeleton */}
        <div className="space-y-1 bg-background px-8 py-5 rounded-b-xl rounded-t">
          <Skeleton className="h-9 w-32 mb-2" />
          <Skeleton className="h-5 w-48 mb-1" />
          <Skeleton className="h-4 w-56" />
        </div>
      </CardContent>
    </Card>
  );
}

export function PropertyListSkeleton({ length }: { length: number }) {
  return (
    <>
      {/* Results Count */}
      <div className="mb-2">
        <Skeleton className="h-5 w-40" />
      </div>

      {/* Property Cards Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {Array.from({ length }).map((_, index) => (
          <PropertyCardSkeleton key={index} />
        ))}
      </div>
    </>
  );
}
