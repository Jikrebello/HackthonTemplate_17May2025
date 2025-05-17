-- Insert missing migration records into __EFMigrationsHistory table
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20250517101724_AddUserPermissions', '9.0.5'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250517101724_AddUserPermissions');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20250517110313_AddedPendingChanges', '9.0.5'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250517110313_AddedPendingChanges');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20250517110725_AddBarcodeToProduct', '9.0.5'
WHERE NOT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250517110725_AddBarcodeToProduct');
