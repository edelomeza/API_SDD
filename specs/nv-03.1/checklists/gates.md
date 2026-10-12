# Checklist — nv-03.1 Blindaje de solución (slnx canónico)

> Trazabilidad: `spec §1-§7 ↔ plan §1-§5 ↔ task T1`. Estado: 🚧 Borrador, sin firmar.

- [ ] `specs/nv-03.1/spec.md` existe con 7 secciones + Aprobación (plantilla canónica)
- [ ] `slnx` canónico declarado: 6 proyectos reales, sin `Chaos/Performance/MutationTest`
- [ ] Gate `scripts/check_slnx.ps1` diseñado: parse `<Project Path>` + `Test-Path`, FAIL exit 1 con ruta faltante
- [ ] Cableado dentro del step `critic` existente (sin job nuevo, orden CI intacto `B3`)
- [ ] Deuda diferida ratificada: NBomber/Chaos/perf y mutation-gate fuera de alcance (dueño `nv-03` para mutación)
- [ ] Clerk limitado a reportar drift (`edit:deny`); aprobación asignada a revisor humano
- [ ] `endpoints.md` intacto; `stryker-config.json` intacto; specs firmadas intactas
- [ ] `sdd-lint SDD-OK + critic PASS + slnx PASS + endpoints OK` + build slnx 0/0 + negativo verificado
- [ ] Aprobación firmada: Revisor ________ Fecha ________
