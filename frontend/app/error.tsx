"use client";

import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import { useEffect } from "react";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    console.error(error);
  }, [error]);

  return (
    <div className="container mx-auto px-4 py-8">
      <Alert
        variant="destructive"
        className="flex items-center justify-center flex-col gap-2.5"
      >
        <AlertTitle className="text-xl">Something went wrong!</AlertTitle>
        <AlertDescription className="italic">{error.message}</AlertDescription>
      </Alert>
      <div className="w-full flex items-center justify-center">
        <Button onClick={reset} className="mt-4">
          Try again
        </Button>
      </div>
    </div>
  );
}
