DO
$$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'DiscountDb') THEN
        CREATE DATABASE "DiscountDb";
    END IF;
END
$$;