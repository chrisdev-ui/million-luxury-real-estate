import { cn } from "@/lib/utils";
import { cva, VariantProps } from "class-variance-authority";

const pageContainerVariants = cva(
  "grid grid-cols-1 items-center gap-8 pt-6 pb-8 md:py-10 px-4 xl:px-0",
  {
    variants: {
      variant: {
        default: "container mx-auto",
        sidebar: "",
        centered:
          "container flex h-dvh max-w-2xl flex-col justify-center py-16",
        markdown: "container max-w-3xl py-8 md:py-10 lg:py-10",
      },
    },
    defaultVariants: {
      variant: "default",
    },
  }
);

interface PageContainerProps
  extends React.HTMLAttributes<HTMLDivElement>,
    VariantProps<typeof pageContainerVariants> {
  as?: React.ElementType;
}

function PageContainer({
  className,
  as: Comp = "section",
  variant,
  ...props
}: PageContainerProps) {
  return (
    <Comp
      className={cn(pageContainerVariants({ variant }), className)}
      {...props}
    />
  );
}

export { PageContainer, pageContainerVariants };
