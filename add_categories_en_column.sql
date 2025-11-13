-- Add categories_en column to products table
ALTER TABLE public.products ADD COLUMN IF NOT EXISTS categories_en text NULL;
