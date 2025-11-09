import { Button } from "@/components/ui/button";
import { Home, Search } from "lucide-react";
import Link from "next/link";

export default function NotFound() {
  return (
    <div className="container mx-auto px-4 py-16 flex flex-col items-center justify-center min-h-[60vh] text-center">
      <div className="space-y-6 max-w-md">
        <div className="text-9xl font-bold text-muted-foreground">404</div>
        <h1 className="text-3xl font-bold">Page Not Found</h1>
        <p className="text-muted-foreground">
          Sorry, we couldn&apos;t find the page you&apos;re looking for. The
          property or page may have been moved or doesn&apos;t exist.
        </p>
        <div className="flex gap-4 justify-center pt-4">
          <Button asChild variant="default">
            <Link href="/">
              <Home className="w-4 h-4 mr-2" />
              Go Home
            </Link>
          </Button>
          <Button asChild variant="outline">
            <Link href="/">
              <Search className="w-4 h-4 mr-2" />
              Browse Properties
            </Link>
          </Button>
        </div>
      </div>
    </div>
  );
}
