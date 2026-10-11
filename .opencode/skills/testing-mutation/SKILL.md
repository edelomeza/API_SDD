---
name: testing-mutation
description: Stryker nightly opcional (lite)
metadata:
  scope: lite
  variant: optional-nightly
  opencode/slash: "true"
---
# testing-mutation MVP (optional)

## Incluye
- `stryker-{entidad}.json acotado + test-case-filter UnitTest + ignore equivalentes documentado`.

## Excluye
- Gate release ≥80%, corre solo nightly/manual + `dotnet build` restaurativo después.

## Verify
- Tras Stryker siempre `dotnet build` antes de `--no-build`.
