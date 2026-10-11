---
name: 10-testing-strategy
description: Qué suite cubre qué sin perf/caos (lite)
metadata:
  scope: lite
  variant: lite
  opencode/slash: "true"
---
# 10-testing-strategy MVP (lite)

## Incluye
- `Unit lógica + Integration/Security WAF SQL real + Database migraciones + Contract captura + FsCheck seeds + maxParallelThreads:4 + --blame-crash`.

## Excluye
- NBomber, Chaos, Pact-real, perf/caos.

## Verify
- `dotnet test <csproj> -c Release --no-build --blame-crash` 5 suites verdes + cobertura ≥45%.
