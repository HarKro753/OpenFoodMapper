CREATE TABLE public.products (
	code numeric NOT NULL,
	url text NULL,
	product_name text NULL,
	brands text NULL,
	image_url text NULL,
	nutriscore_score int4 NULL,
	nova_group int4 NULL,
	environmental_score_score numeric NULL,
	completeness numeric NULL,
	last_image_datetime text NULL,
	last_modified_datetime text NULL,
	additives_en text NULL,
	ingredients_tags text NULL,
	"energy-kcal_100g" numeric NULL,
	"energy-from-fat_100g" numeric NULL,
	fat_100g numeric NULL,
	"saturated-fat_100g" numeric NULL,
	"trans-fat_100g" numeric NULL,
	cholesterol_100g numeric NULL,
	carbohydrates_100g numeric NULL,
	sugars_100g numeric NULL,
	"added-sugars_100g" numeric NULL,
	fiber_100g numeric NULL,
	proteins_100g numeric NULL,
	salt_100g numeric NULL,
	sodium_100g numeric NULL,
	alcohol_100g numeric NULL,
	"vitamin-a_100g" numeric NULL,
	"vitamin-c_100g" numeric NULL,
	calcium_100g numeric NULL,
	iron_100g numeric NULL,
	magnesium_100g numeric NULL,
	zinc_100g numeric NULL,
	potassium_100g numeric NULL,
	categories_en text NULL,
	CONSTRAINT products_pkey PRIMARY KEY (code)
);

CREATE TABLE public.categories (
	id serial4 NOT NULL,
	"name" text NOT NULL,
	CONSTRAINT categories_name_key UNIQUE (name),
	CONSTRAINT categories_pkey PRIMARY KEY (id)
);

CREATE TABLE public.product_categories (
	product_code numeric NOT NULL,
	category_id int4 NOT NULL,
	CONSTRAINT product_categories_pkey PRIMARY KEY (product_code, category_id),
	CONSTRAINT product_categories_category_id_fkey FOREIGN KEY (category_id) REFERENCES public.categories(id) ON DELETE CASCADE,
	CONSTRAINT product_categories_product_code_fkey FOREIGN KEY (product_code) REFERENCES public.products(code) ON DELETE CASCADE
);
CREATE INDEX idx_product_categories_category ON public.product_categories USING btree (category_id);



