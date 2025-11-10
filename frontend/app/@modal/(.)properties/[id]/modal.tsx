"use client";

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { createPortal } from "react-dom";

export function Modal({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [open, setOpen] = useState(true);

  const onDismiss = (isOpen: boolean) => {
    if (!isOpen) {
      setOpen(false);
      router.back();
    }
  };

  // Only render on client-side
  if (typeof window === "undefined") return null;

  return createPortal(
    <Dialog open={open} onOpenChange={onDismiss}>
      <DialogContent className="max-w-5xl max-h-[90vh] overflow-y-auto">
        <DialogHeader className="sr-only">
          <DialogTitle>Property Details</DialogTitle>
          <DialogDescription>
            View detailed information about this property
          </DialogDescription>
        </DialogHeader>
        {children}
      </DialogContent>
    </Dialog>,
    document.getElementById("modal-root")!
  );
}
