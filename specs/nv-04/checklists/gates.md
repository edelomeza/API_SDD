# Checklist — nv-04 CRUD SDDUsuario

> Trazabilidad: `spec §1-§7 ↔ plan ↔ task`. Estado: 🚧 Borrador, sin firmar.
> Archivo de higiene SDD (`check_sdd.ps1` B1 exige `checklists/` por spec). Contenido derivado de `spec.md §2`, sin ampliar alcance.

- [ ] Entidad `SDDUsuario` + migración (sin `_`) + `DbSet`
- [ ] DTOs + validators espejo (registro por reflexión, molde `nv-02`)
- [ ] `ISDDUsuarioService` + `SDDUsuariosController` v1.0 + `RowVersion` en Update/Delete
- [ ] Argon2id real; DTOs sin hash ni plano (DevSecOps)
- [ ] 5 suites verdes + 5 filas en `docs/endpoints.md`
- [ ] Puerta `nv-03`: delta informativo + Stryker advisory; global ≥45%
- [ ] `Memoria.md` + tag `v1.0.0-crud4`
- [ ] Aprobación firmada: Revisor ________ Fecha ________
