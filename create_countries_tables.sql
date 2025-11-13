-- Create countries table
CREATE TABLE IF NOT EXISTS public.countries (
    id serial4 NOT NULL,
    "name" text NOT NULL,
    CONSTRAINT countries_name_key UNIQUE (name),
    CONSTRAINT countries_pkey PRIMARY KEY (id)
);

-- Create product_countries junction table
CREATE TABLE IF NOT EXISTS public.product_countries (
    product_code numeric NOT NULL,
    country_id int4 NOT NULL,
    CONSTRAINT product_countries_pkey PRIMARY KEY (product_code, country_id),
    CONSTRAINT product_countries_country_id_fkey FOREIGN KEY (country_id) REFERENCES public.countries(id) ON DELETE CASCADE,
    CONSTRAINT product_countries_product_code_fkey FOREIGN KEY (product_code) REFERENCES public.products(code) ON DELETE CASCADE
);

-- Create index for product_countries
CREATE INDEX IF NOT EXISTS idx_product_countries_country ON public.product_countries USING btree (country_id);
