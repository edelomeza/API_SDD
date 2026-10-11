---
name: 07-quality-supply-chain
description: NuGetAudit sin Trivy bloqueante (lite)
metadata:
  scope: lite
  variant: lite
  opencode/slash: "true"
---
# 07-quality-supply-chain MVP (lite)

## Incluye
- `NuGetAudit=true + source-mapping solo nuget.org + Dependabot + dotnet list --vulnerable`.

## Excluye
- Trivy/dockle/ZAP/RESTler bloqueantes, hardening/README obligatorio.

## Verify
- `dotnet list --vulnerable` 0 altos + Dependabot activo.
