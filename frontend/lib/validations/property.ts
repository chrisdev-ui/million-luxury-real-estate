import z from "zod";

export const propertyFilterSchema = z.object({
  name: z
    .string()
    .max(100, "Name must be at most 100 characters")
    .or(z.undefined()),
  address: z
    .string()
    .max(100, "Address must be at most 100 characters")
    .or(z.undefined()),
  minPrice: z
    .number()
    .nonnegative("Min price must be positive")
    .or(z.undefined()),
  maxPrice: z
    .number()
    .nonnegative("Max price must be positive")
    .or(z.undefined()),
});

export type PropertyFilterInput = z.infer<typeof propertyFilterSchema>;
