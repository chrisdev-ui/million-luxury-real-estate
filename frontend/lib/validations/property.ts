import z from "zod";

export const propertyFilterSchema = z
  .object({
    name: z
      .string()
      .min(1, "Name must be at least 1 character")
      .max(100, "Name must be at most 100 characters"),
    address: z
      .string()
      .min(1, "Address must be at least 1 character")
      .max(100, "Address must be at most 100 characters"),
    minPrice: z.number().positive().or(z.literal(0)),
    maxPrice: z.number().positive().or(z.literal(0)),
  })
  .refine(
    (data) => {
      if (data.minPrice !== undefined && data.maxPrice !== undefined) {
        return data.minPrice <= data.maxPrice;
      }
      return true;
    },
    {
      message: "Min price must be less than or equal to max price",
      path: ["minPrice"],
    }
  );

export type PropertyFilterInput = z.infer<typeof propertyFilterSchema>;
