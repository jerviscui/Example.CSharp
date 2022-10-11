-- Table: public.fact_sales

-- DROP TABLE IF EXISTS public.fact_sales;

CREATE TABLE IF NOT EXISTS public.fact_sales
(
    id integer NOT NULL,
    date_id integer,
    product_id integer,
    store_id integer,
    quantity integer,
    unit_price numeric(7,2),
    other_data character(1000) COLLATE pg_catalog."default",
    CONSTRAINT "PK_fact_sales" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.fact_sales
    OWNER to postgres;
-- Index: ci

-- DROP INDEX IF EXISTS public.ci;

CREATE INDEX IF NOT EXISTS ci
    ON public.fact_sales USING btree
    (date_id ASC NULLS LAST)
    TABLESPACE pg_default;

do $$
begin
	for r in 1..1000000 loop
		INSERT INTO public.fact_sales VALUES(r, 20080800 + (r%30) + 1, r%10000, r%200, random() - 25, (r%3) + 1, '');
	end loop;
	
	for r in 1000001..2000000 loop
		INSERT INTO public.fact_sales VALUES(r, 20080900 + (r%30) + 1, r%10000, r%200, random() - 25, (r%3) + 1, '');
	end loop;
end;
$$;
