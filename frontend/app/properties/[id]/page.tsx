import { PropertyDetail } from "@/components/properties/property-detail";
import { PropertyDetailSkeleton } from "@/components/properties/property-detail-skeleton";
import { siteConfig } from "@/config/site";
import { getPropertyById } from "@/lib/api/properties";
import { currencyFormatter } from "@/lib/utils";
import { Suspense } from "react";

interface PropertyPageProps {
  params: Promise<{ id: string }>;
}

export async function generateMetadata({ params }: PropertyPageProps) {
  const { id } = await params;
  const response = await getPropertyById(id);

  if (!response.success || !response.data) {
    return {
      title: "Property Not Found",
    };
  }

  return {
    title: `${response.data.name} - ${siteConfig.name}`,
    description: `${response.data.name} located at ${
      response.data.address
    }. Price: ${currencyFormatter.format(response.data.price)}`,
  };
}

export default function PropertyPage({ params }: PropertyPageProps) {
  return (
    <Suspense fallback={<PropertyDetailSkeleton />}>
      <PropertyPageContent params={params} />
    </Suspense>
  );
}

async function PropertyPageContent({ params }: PropertyPageProps) {
  const { id } = await params;
  const promise = getPropertyById(id);

  return <PropertyDetail promise={promise} />;
}
