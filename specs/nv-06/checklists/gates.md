# Checklist — nv-06 Logout (revocación JWT)

> Trazabilidad: `spec §1-§7 ↔ plan ↔ task`. Estado: 🚧 Borrador, sin firmar.
> Archivo de higiene SDD (`check_sdd.ps1` B1 exige `checklists/` por spec). Contenido derivado de `spec.md §2`, sin ampliar alcance.

- [ ] `POST /api/v1/auth/logout [Authorize]` → `204`
- [ ] `blacklist:{jti}` en `IMemoryCache` con TTL = vida remanente; reúso → `401`
- [ ] `ITokenRevocationService`; sin estado en SQL, sin superficie nueva
- [ ] Reutiliza `OnTokenValidated` anti-blacklist ya cableado
- [ ] Tests + 1 fila en `docs/endpoints.md`
- [ ] Puerta `nv-03`: delta informativo + Stryker advisory; global ≥45%
- [ ] Tag `v1.0.0-auth6`
- [ ] Aprobación firmada: Revisor ________ Fecha ________
