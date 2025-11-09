"use client";

import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { getPropertyById } from "@/lib/api/properties";
import { currencyFormatter } from "@/lib/utils";
import { Calendar, MapPin, User } from "lucide-react";
import Image from "next/image";
import { use } from "react";

interface PropertyDetailModalProps {
  promise: Promise<Awaited<ReturnType<typeof getPropertyById>>>;
}

export function PropertyDetailModal({ promise }: PropertyDetailModalProps) {
  const { data: property } = use(promise);

  if (!property) {
    return (
      <div className="p-8 text-center">
        <p className="text-muted-foreground">Property not found</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Main Image */}
      <div className="relative aspect-video w-full overflow-hidden rounded-lg">
        <Image
          src={property.mainImage || "/placeholder-property.jpg"}
          alt={property.name}
          fill
          className="object-cover"
          priority
          sizes="(max-width: 1024px) 100vw, 80vw"
        />
      </div>

      {/* Property Header */}
      <div>
        <h2 className="text-3xl font-bold mb-2">{property.name}</h2>
        <div className="flex items-center text-muted-foreground mb-4">
          <MapPin className="w-5 h-5 mr-2" />
          <span>{property.address}</span>
        </div>
        <Badge variant="secondary" className="text-2xl font-bold px-4 py-2">
          {currencyFormatter.format(property.price)}
        </Badge>
      </div>

      {/* Additional Images Gallery */}
      {property.images && property.images.length > 0 && (
        <div>
          <h3 className="text-lg font-semibold mb-3">Gallery</h3>
          <div className="grid grid-cols-3 gap-2">
            {property.images.slice(0, 6).map((image) => (
              <div
                key={image.idPropertyImage}
                className="relative aspect-video overflow-hidden rounded-md"
              >
                <Image
                  src={image.file}
                  alt="Property image"
                  fill
                  className="object-cover"
                  sizes="(max-width: 1024px) 33vw, 26vw"
                />
              </div>
            ))}
          </div>
        </div>
      )}

      <Separator />

      {/* Owner Information */}
      {property.owner && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="w-5 h-5" />
              Owner Information
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-2">
            <div className="flex items-center gap-3">
              {property.owner.photo && (
                <Avatar>
                  <AvatarImage
                    src={property.owner.photo}
                    alt={property.owner.name}
                  />
                  <AvatarFallback>
                    {property.owner.name.charAt(0)}
                  </AvatarFallback>
                </Avatar>
              )}
              <div>
                <p className="font-semibold">{property.owner.name}</p>
                <p className="text-sm text-muted-foreground">
                  {property.owner.address}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      )}

      {/* Property Traces (Sales History) */}
      {property.traces && property.traces.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Calendar className="w-5 h-5" />
              Sales History
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {property.traces.map((trace) => (
                <div
                  key={trace.idPropertyTrace}
                  className="flex justify-between items-start p-3 rounded-md bg-muted/50"
                >
                  <div>
                    <p className="font-medium">{trace.name}</p>
                    <p className="text-sm text-muted-foreground">
                      {new Date(trace.dateSale).toLocaleDateString("en-US", {
                        year: "numeric",
                        month: "long",
                        day: "numeric",
                      })}
                    </p>
                  </div>
                  <div className="text-right">
                    <p className="font-semibold">
                      {currencyFormatter.format(trace.value)}
                    </p>
                    <p className="text-sm text-muted-foreground">
                      Tax: ${trace.tax.toLocaleString()}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
