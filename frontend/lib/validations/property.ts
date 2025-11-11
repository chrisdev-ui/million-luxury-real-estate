import z from "zod";

export const propertyFilterSchema = z.object({
  name: z
    .string()
    .max(100, "Name must be at most 100 characters")
    .optional()
    .or(z.literal("")),
  address: z
    .string()
    .max(100, "Address must be at most 100 characters")
    .optional()
    .or(z.literal("")),
  minPrice: z
    .number()
    .nonnegative("Min price must be positive")
    .optional()
    .or(z.literal(0)),
  maxPrice: z
    .number()
    .nonnegative("Max price must be positive")
    .optional()
    .or(z.literal(0)),
});

export type PropertyFilterInput = z.infer<typeof propertyFilterSchema>;
