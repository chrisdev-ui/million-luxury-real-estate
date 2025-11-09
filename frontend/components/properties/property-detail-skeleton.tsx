import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { Skeleton } from "@/components/ui/skeleton";
import { Calendar, User } from "lucide-react";

export function PropertyDetailSkeleton() {
  return (
    <div className="container mx-auto px-4 py-8 max-w-6xl">
      {/* Back Button Skeleton */}
      <Skeleton className="h-11 w-44 mb-6" />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Left Column - Images */}
        <div className="space-y-4">
          {/* Main Image Skeleton */}
          <Skeleton className="aspect-4/3 w-full rounded-lg" />

          {/* Additional Images Gallery Skeleton */}
          <div className="grid grid-cols-3 gap-2">
            {Array.from({ length: 6 }).map((_, index) => (
              <Skeleton key={index} className="aspect-video rounded-md" />
            ))}
          </div>
        </div>

        {/* Right Column - Property Details */}
        <div className="space-y-6">
          {/* Title, Address, Price Section */}
          <div>
            {/* Title Skeleton */}
            <Skeleton className="h-10 w-3/4 mb-2" />

            {/* Address Skeleton */}
            <div className="flex items-center mb-4">
              <Skeleton className="w-5 h-5 mr-2" />
              <Skeleton className="h-4 w-2/3" />
            </div>

            {/* Price Badge Skeleton */}
            <Skeleton className="h-10 w-40" />
          </div>

          <Separator />

          {/* Owner Information Card Skeleton */}
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <User className="w-5 h-5" />
                <Skeleton className="h-6 w-40" />
              </div>
            </CardHeader>
            <CardContent className="space-y-2">
              <div className="flex items-center gap-3">
                {/* Avatar Skeleton */}
                <Skeleton className="w-10 h-10 rounded-full" />
                <div className="flex-1">
                  <Skeleton className="h-5 w-32 mb-2" />
                  <Skeleton className="h-4 w-48" />
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Sales History Card Skeleton */}
          <Card>
            <CardHeader>
              <div className="flex items-center gap-2">
                <Calendar className="w-5 h-5" />
                <Skeleton className="h-6 w-32" />
              </div>
            </CardHeader>
            <CardContent>
              <div className="space-y-3">
                {Array.from({ length: 3 }).map((_, index) => (
                  <div
                    key={index}
                    className="flex justify-between items-start p-3 rounded-md bg-muted/50"
                  >
                    <div className="space-y-2">
                      <Skeleton className="h-5 w-32" />
                      <Skeleton className="h-4 w-40" />
                    </div>
                    <div className="space-y-2 text-right">
                      <Skeleton className="h-5 w-24" />
                      <Skeleton className="h-4 w-20" />
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
