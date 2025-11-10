import { Badge } from "@/components/ui/badge";
import { Card, CardContent } from "@/components/ui/card";
import { Property } from "@/lib/api/types";
import { currencyFormatter } from "@/lib/utils";
import { Bath, Bed, Maximize2 } from "lucide-react";
import Image from "next/image";
import Link from "next/link";

interface PropertyCardProps {
  property: Property;
}

export function PropertyCard({ property }: PropertyCardProps) {
  const badges = [property.codeInternal, property.year];
  return (
    <Link href={`/properties/${property.idProperty}`}>
      <Card className="overflow-hidden border-0 shadow-lg hover:shadow-xl transition-all duration-300 hover:-translate-y-1 bg-card pt-0">
        <div className="relative">
          <Image
            src={property.mainImage || "/images/placeholder.png"}
            alt={property.name}
            width={400}
            height={300}
            className="object-cover w-full h-56"
            sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
          />
          <div className="absolute top-3 left-3 flex gap-2">
            {badges.map((badge, index) => (
              <Badge
                key={index}
                variant="secondary"
                className={
                  index === 0
                    ? "bg-white text-gray-900 hover:bg-white font-medium shadow-md"
                    : "bg-blue-500 text-white hover:bg-blue-600 font-medium shadow-md"
                }
              >
                {badge}
              </Badge>
            ))}
          </div>
        </div>

        <CardContent>
          <div className="flex items-center justify-between text-muted-foreground mb-4 gap-4">
            <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
              <Bed className="w-5 h-5" />
              <span className="text-sm font-medium">3 Beds</span>
            </div>
            <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
              <Bath className="w-5 h-5" />
              <span className="text-sm font-medium">2 Baths</span>
            </div>
            <div className="flex items-center justify-center py-2.5 gap-2 bg-background rounded-md w-full">
              <Maximize2 className="w-5 h-5" />
              <span className="text-sm font-medium">1520 Ft</span>
            </div>
          </div>
          <div className="space-y-1 bg-background px-8 py-5 rounded-b-xl rounded-t">
            <p className="text-3xl font-bold text-foreground">
              {currencyFormatter.format(property.price)}
            </p>
            <p className="text-base font-semibold text-foreground">
              {property.name}
            </p>
            <p className="text-sm text-muted-foreground">{property.address}</p>
          </div>
        </CardContent>
      </Card>
    </Link>
  );
}
