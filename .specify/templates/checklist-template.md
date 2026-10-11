# Checklist Template — Gates PR

> Orden CI-PR: restore → build → unit → integration → security → critic → endpoints → contract → semgrep (`--no-build` Release).

- [ ] Build Release sin warnings CA3000+
- [ ] Unit + Integration SQL real (Testcontainers puerto fijo) verdes
- [ ] Security + critic + semgrep sin hallazgos altos
- [ ] `docs/endpoints.md` actualizado y enlazado desde spec
- [ ] Contract captura `CONTRACT_CAPTURE=1` + `IsConventional` + `RowVersion AQ==`
- [ ] Cobertura ≥45% (`python scripts/check_coverage.py`)
- [ ] Headers verificados (nosniff, DENY, CORS 1 origen, HSTS no-Dev, CSP nonce)
- [ ] `Memoria.md` actualizada, sin secretos
- [ ] 1 revisor, `main` protegida
