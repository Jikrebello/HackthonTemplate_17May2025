-- Check if the barcode column exists, if not add it
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = 'Products' AND column_name = 'Barcode'
    ) THEN
        ALTER TABLE "Products" ADD COLUMN "Barcode" text NOT NULL DEFAULT '';
    END IF;
END $$;
