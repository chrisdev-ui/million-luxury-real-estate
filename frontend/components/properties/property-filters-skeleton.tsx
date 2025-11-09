import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";

export function PropertyFiltersSkeleton() {
  return (
    <Card className="pb-6 pt-3 gap-3">
      <CardContent className="p-6">
        <div className="space-y-4">
          {/* Input Fields Grid */}
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
            {/* Name Search Input Skeleton */}
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
            </div>

            {/* Address Search Input Skeleton */}
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
            </div>

            {/* Min Price Input Skeleton */}
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-4 w-3/4" />
            </div>

            {/* Max Price Input Skeleton */}
            <div className="space-y-2">
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-4 w-3/4" />
            </div>
          </div>

          {/* Description Text Skeleton */}
          <Skeleton className="h-4 w-full max-w-xl" />
        </div>
      </CardContent>
      <CardFooter>
        <div className="flex gap-2">
          {/* Clear Filters Button Skeleton */}
          <Skeleton className="h-10 w-32" />
          {/* Apply Filters Button Skeleton */}
          <Skeleton className="h-10 w-32" />
        </div>
      </CardFooter>
    </Card>
  );
}
