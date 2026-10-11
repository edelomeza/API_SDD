---
name: 02-migrations-seed
description: Migración InitialCreate + seed idempotente DB externa
metadata:
  scope: full
  mvp: hosting-ready-sin-saga
  opencode/slash: "true"
---
# 02-migrations-seed MVP

## What I do
- `dotnet ef migrations add InitialCreate --project API_SDD`. UNIQUEs filtrados. Migraciones sin `_` (CA1707).
- Seeder: `AnyAsync + IDENTITY_INSERT + 1 transacción + idempotente`. Estados `base/race/perf`.
- `UseInMemoryDatabase=false` default. `ConnectionStrings__Default` por env externa.

## Verify
- `dotnet ef database update` contra SQL externa + seed re-ejecutable sin duplicar.
