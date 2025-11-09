"use client";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { getPropertyById } from "@/lib/api/properties";
import { currencyFormatter } from "@/lib/utils";
import { ArrowLeft, Calendar, MapPin, User } from "lucide-react";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { use } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "../ui/avatar";

interface PropertyDetailProps {
  promise: Promise<Awaited<ReturnType<typeof getPropertyById>>>;
}

export function PropertyDetail({ promise }: PropertyDetailProps) {
  const { data: property } = use(promise);

  const router = useRouter();

  return (
    <div className="container mx-auto px-4 py-8 max-w-6xl">
      <Button
        variant="ghost"
        className="mb-6 min-h-11"
        onClick={() => router.back()}
      >
        <ArrowLeft className="w-4 h-4 mr-2" />
        Back to Properties
      </Button>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Main Image */}
        <div className="space-y-4">
          <div className="relative aspect-4/3 w-full overflow-hidden rounded-lg">
            <Image
              src={property.mainImage || "/placeholder-property.jpg"}
              alt={property.name}
              fill
              className="object-cover"
              priority
              sizes="(max-width: 1024px) 100vw, 50vw"
            />
          </div>

          {/* Additional Images Gallery */}
          {property.images && property.images.length > 0 && (
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
                    sizes="(max-width: 1024px) 33vw, 16vw"
                  />
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Property Details */}
        <div className="space-y-6">
          <div>
            <h1 className="text-4xl font-bold mb-2">{property.name}</h1>
            <div className="flex items-center text-muted-foreground mb-4">
              <MapPin className="w-5 h-5 mr-2" />
              <span>{property.address}</span>
            </div>
            <Badge variant="secondary" className="text-2xl font-bold px-4 py-2">
              {currencyFormatter.format(property.price)}
            </Badge>
          </div>

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
                          {new Date(trace.dateSale).toLocaleDateString(
                            "en-US",
                            {
                              year: "numeric",
                              month: "long",
                              day: "numeric",
                            }
                          )}
                        </p>
                      </div>
                      <div className="text-right">
                        <p className="font-semibold">
                          {new Intl.NumberFormat("en-US", {
                            style: "currency",
                            currency: "USD",
                          }).format(trace.value)}
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
      </div>
    </div>
  );
}
