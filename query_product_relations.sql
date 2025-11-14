-- Query to fetch all categories for product with code 4337256425049
SELECT
    p.code,
    p.product_name,
    c.id as category_id,
    c.name as category_name
FROM
    products p
    JOIN product_categories pc ON p.code = pc.product_code
    JOIN categories c ON pc.category_id = c.id
WHERE
    p.code = 4337256425049
ORDER BY
    c.name;

-- Query to fetch all countries for product with code 4337256425049
SELECT
    p.code,
    p.product_name,
    co.id as country_id,
    co.name as country_name
FROM
    products p
    JOIN product_countries pco ON p.code = pco.product_code
    JOIN countries co ON pco.country_id = co.id
WHERE
    p.code = 4337256425049
ORDER BY
    co.name;

-- Combined query to fetch both categories and countries for product
SELECT
    p.code,
    p.product_name,
    p.categories_en,
    p.countries,
    (
        SELECT string_agg(c.name, ', ' ORDER BY c.name)
        FROM product_categories pc
        JOIN categories c ON pc.category_id = c.id
        WHERE pc.product_code = p.code
    ) as parsed_categories,
    (
        SELECT string_agg(co.name, ', ' ORDER BY co.name)
        FROM product_countries pco
        JOIN countries co ON pco.country_id = co.id
        WHERE pco.product_code = p.code
    ) as parsed_countries
FROM
    products p
WHERE
    p.code = 4337256425049;
