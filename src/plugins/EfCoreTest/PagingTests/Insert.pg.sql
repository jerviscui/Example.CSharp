-- Table: public.fact_sales

-- DROP TABLE IF EXISTS public.fact_sales;

CREATE TABLE IF NOT EXISTS public.fact_sales
(
    date_id integer NOT NULL,
    product_id integer NOT NULL,
    store_id integer NOT NULL,
    quantity integer NOT NULL,
    unit_price numeric(7,2) NOT NULL,
    other_data character(1000) COLLATE pg_catalog."default" NOT NULL
);

ALTER TABLE IF EXISTS public.fact_sales
    OWNER to postgres;
-- Index: ci

-- DROP INDEX IF EXISTS public.ci;

CREATE INDEX IF NOT EXISTS ci
    ON public.fact_sales USING btree
    (date_id ASC NULLS LAST)
;

do $$
begin
	for r in 1..1000000 loop
		INSERT INTO public.fact_sales VALUES(20080800 + (r%30) + 1, r%10000, r%200, random() - 25, (r%3) + 1, '');
	end loop;
	
	for r in 1..1000000 loop
		INSERT INTO public.fact_sales VALUES(20080900 + (r%30) + 1, r%10000, r%200, random() - 25, (r%3) + 1, '');
	end loop;
end;
$$;
