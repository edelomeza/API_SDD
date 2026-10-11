# CHECKLIST_PR.md — Obligatorio

> Constitución §8. 1 revisor. `main` protegida.

- [ ] Spec en `specs/NV-0X` aprobada y enlazada (sin cambio de contrato sin spec)
- [ ] Build Release `dotnet build -c Release` sin errores CA3000+
- [ ] Unit verdes
- [ ] Integration SQL real (Testcontainers puerto fijo) verdes
- [ ] Security + Database verdes
- [ ] Contract captura `CONTRACT_CAPTURE=1` + `IsConventional` + `RowVersion AQ==`
- [ ] Endpoints: `docs/endpoints.md` actualizado
- [ ] Rate limits: `docs/rate-limit-matrix.md` verificado
- [ ] ASVS: `docs/asvs-l2-checklist.md` verificado
- [ ] Semgrep sin altos
- [ ] Cobertura ≥45% (`python scripts/check_coverage.py`)
- [ ] Headers + `/health/ready` + `401` verificados manualmente
- [ ] `Memoria.md` actualizada, sin secretos
- [ ] YAML válido (`yaml.safe_load`), `dotnet build` después de Stryker si se usó
- [ ] Revisor: ________  Fecha: ________
