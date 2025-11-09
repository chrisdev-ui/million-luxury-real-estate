"use client";

import { useForm } from "@tanstack/react-form";
import { parseAsInteger, parseAsString, useQueryStates } from "nuqs";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import {
  Field,
  FieldDescription,
  FieldError,
  FieldGroup,
} from "@/components/ui/field";
import { CurrencyInput, SearchInput } from "@/components/ui/input";
import { useDebouncedCallback } from "@/hooks/use-debounced-callback";
import { propertyFilterSchema } from "@/lib/validations/property";
import { useCallback } from "react";

const propertyFiltersParsers = {
  name: parseAsString.withDefault(""),
  address: parseAsString.withDefault(""),
  minPrice: parseAsInteger,
  maxPrice: parseAsInteger,
};

export function PropertyFilters() {
  const [filters, setFilters] = useQueryStates(propertyFiltersParsers, {
    history: "push",
    shallow: false,
  });

  const form = useForm({
    defaultValues: {
      name: filters.name || undefined,
      address: filters.address || undefined,
      minPrice: filters.minPrice || undefined,
      maxPrice: filters.maxPrice || undefined,
    },
    validators: {
      onSubmit: propertyFilterSchema,
    },
    onSubmit: async ({ value }) => {
      await setFilters({
        name: value.name || null,
        address: value.address || null,
        minPrice: value.minPrice || null,
        maxPrice: value.maxPrice || null,
      });
    },
  });

  const debouncedSetFilters = useDebouncedCallback(
    (values: typeof form.state.values) => {
      setFilters({
        name: values.name || null,
        address: values.address || null,
        minPrice: values.minPrice || null,
        maxPrice: values.maxPrice || null,
      });
    },
    500
  );

  const handleDebouncedUpdate = useCallback(() => {
    debouncedSetFilters(form.state.values);
  }, [debouncedSetFilters, form.state.values]);

  const handleReset = () => {
    form.reset();
    // Clear all filters from URL
    setFilters({
      name: null,
      address: null,
      minPrice: null,
      maxPrice: null,
    });
  };

  return (
    <Card className="pb-6 pt-3 gap-3">
      <CardContent className="p-6">
        <form
          id="property-filters-form"
          onSubmit={(e) => {
            e.preventDefault();
            form.handleSubmit();
          }}
          className="space-y-4"
        >
          <FieldGroup>
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
              <form.Field name="name">
                {(field) => {
                  const isInvalid =
                    field.state.meta.isTouched && !field.state.meta.isValid;
                  return (
                    <Field data-invalid={isInvalid}>
                      <SearchInput
                        id={field.name}
                        name={field.name}
                        value={field.state.value ?? ""}
                        onBlur={field.handleBlur}
                        onChange={(e) => {
                          const value = e.target.value;
                          field.handleChange(value);
                          handleDebouncedUpdate();
                        }}
                        aria-invalid={isInvalid}
                        placeholder="Search by property name..."
                        autoComplete="off"
                      />
                      {isInvalid && (
                        <FieldError errors={field.state.meta.errors} />
                      )}
                    </Field>
                  );
                }}
              </form.Field>

              <form.Field name="address">
                {(field) => {
                  const isInvalid =
                    field.state.meta.isTouched && !field.state.meta.isValid;
                  return (
                    <Field data-invalid={isInvalid}>
                      <SearchInput
                        id={field.name}
                        name={field.name}
                        value={field.state.value ?? ""}
                        onBlur={field.handleBlur}
                        onChange={(e) => {
                          const value = e.target.value;
                          field.handleChange(value);
                          handleDebouncedUpdate();
                        }}
                        aria-invalid={isInvalid}
                        placeholder="Search by address..."
                        autoComplete="off"
                      />
                      {isInvalid && (
                        <FieldError errors={field.state.meta.errors} />
                      )}
                    </Field>
                  );
                }}
              </form.Field>

              <form.Field name="minPrice">
                {(field) => {
                  const isInvalid =
                    field.state.meta.isTouched && !field.state.meta.isValid;
                  return (
                    <Field data-invalid={isInvalid}>
                      <CurrencyInput
                        id={field.name}
                        name={field.name}
                        type="number"
                        value={field.state.value ?? ""}
                        onBlur={field.handleBlur}
                        onChange={(e) => {
                          const value = e.target.value;
                          const numValue =
                            value === "" ? undefined : Number(value);
                          field.handleChange(numValue);
                          handleDebouncedUpdate();
                        }}
                        aria-invalid={isInvalid}
                        placeholder="0"
                        min="0"
                        step="1"
                      />
                      <FieldDescription>
                        Enter the minimum price in USD
                      </FieldDescription>
                      {isInvalid && (
                        <FieldError errors={field.state.meta.errors} />
                      )}
                    </Field>
                  );
                }}
              </form.Field>

              <form.Field name="maxPrice">
                {(field) => {
                  const isInvalid =
                    field.state.meta.isTouched && !field.state.meta.isValid;
                  return (
                    <Field data-invalid={isInvalid}>
                      <CurrencyInput
                        id={field.name}
                        name={field.name}
                        type="number"
                        value={field.state.value ?? ""}
                        onBlur={field.handleBlur}
                        onChange={(e) => {
                          const value = e.target.value;
                          const numValue =
                            value === "" ? undefined : Number(value);
                          field.handleChange(numValue);
                          handleDebouncedUpdate();
                        }}
                        aria-invalid={isInvalid}
                        placeholder="999999999999"
                        min="0"
                        step="1"
                      />
                      <FieldDescription>
                        Enter the maximum price in USD
                      </FieldDescription>
                      {isInvalid && (
                        <FieldError errors={field.state.meta.errors} />
                      )}
                    </Field>
                  );
                }}
              </form.Field>
            </div>
            <FieldDescription>
              Filters are automatically applied as you type and synced with the
              URL for easy sharing.
            </FieldDescription>
          </FieldGroup>
        </form>
      </CardContent>
      <CardFooter>
        <Field orientation="horizontal">
          <Button type="button" variant="outline" onClick={handleReset}>
            Clear Filters
          </Button>
          <Button type="submit" form="property-filters-form">
            Apply Filters
          </Button>
        </Field>
      </CardFooter>
    </Card>
  );
}
