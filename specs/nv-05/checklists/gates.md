# Checklist — nv-05 Login + JWT sobre SDDUsuario

> Trazabilidad: `spec §1-§7 ↔ plan ↔ task`. Estado: 🚧 Borrador, sin firmar.
> Archivo de higiene SDD (`check_sdd.ps1` B1 exige `checklists/` por spec). Contenido derivado de `spec.md §2`, sin ampliar alcance.

- [ ] `POST /api/v1/auth/login [AllowAnonymous]` + emisión JWT HS256 real (`sub/jti/role`)
- [ ] Anti-enumeración + lockout DB + rate Login; sin revelar usuario-vs-contraseña
- [ ] `AdminPolicy` (madre auth) + `SDDSegBloqueo` + mejoras M1/M2/M4 a `nv-04`
- [ ] `jti` + blacklist listos para `nv-06`
- [ ] Tests + 1 fila en `docs/endpoints.md`
- [ ] Puerta `nv-03`: delta informativo + Stryker advisory; global ≥45%
- [ ] Tag `v1.0.0-auth5`
- [ ] Aprobación firmada: Revisor ________ Fecha ________
