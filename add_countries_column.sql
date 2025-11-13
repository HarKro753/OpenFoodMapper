-- Add countries column to products table
ALTER TABLE public.products ADD COLUMN IF NOT EXISTS countries text NULL;
