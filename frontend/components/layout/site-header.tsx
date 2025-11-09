import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { siteConfig } from "@/config/site";
import { Bell, Heart, Menu } from "lucide-react";
import Image from "next/image";
import Link from "next/link";
import { ModeToggle } from "./mode-toggle";

export function SiteHeader() {
  return (
    <header className="border-b bg-background">
      <div className="container mx-auto px-4 py-2 lg:p-2 flex items-center justify-between">
        <Link href="/" className="flex items-center gap-3">
          <Menu className="h-6 w-6" />
          <Image
            src="/images/real-state-logo.webp"
            alt="Site Logo"
            width={120}
            height={120}
            className="size-20 lg:size-[120px]"
          />
          <span className="sr-only">{siteConfig.name}</span>
        </Link>
        <div className="flex items-center justify-end gap-2.5">
          <Button variant="secondary" size="icon" className="rounded-full">
            <Heart className="h-5 w-5" />
          </Button>
          <Button variant="secondary" size="icon" className="rounded-full">
            <Bell className="h-5 w-5" />
          </Button>
          <ModeToggle />
          <div className="relative">
            <span className="absolute top-0 right-0 inline-flex size-2 rounded-full bg-green-500 z-50" />
            <Avatar>
              <AvatarImage
                src={`https://github.com/${siteConfig.creator}.png`}
                alt={`@${siteConfig.creator}`}
              />
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
          </div>
        </div>
      </div>
    </header>
  );
}
