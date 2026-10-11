# Agents.md — CrudDevSecOpsMVP

> Rector: `.specify/memory/constitution.md` manda ante conflicto. Espejo: `./constitution.md`.
> Este archivo dice **cómo** trabajar. La constitución dice **qué** no se negocia.

## 1. Forma de trabajo
> Stack: ver `constitution.md §3`. Nada fuera sin excepción documentada.
- Setup: `API_SDD/appsettings.Example.json → API_SDD/appsettings.json`; `ConnectionStrings__Default` apunta a **DB externa**; `UseInMemoryDatabase=false`; `dotnet restore`; Docker solo para tests.
- Comandos: `dotnet restore`, `dotnet build -c Release --no-restore`, `dotnet test <csproj> -c Release --no-build` (Unit, Integration, Security, Contract, Database), `dotnet run --project API_SDD/API_SDD.csproj`, `dotnet publish -c Release -o ./publish`.
- CI `ci-pr.yml`: restore → build → unit → integration → security → critic → endpoints → contract → semgrep. Sin deploy.
- PR = 1 slice CRUD; `CHECKLIST_PR.md` + 1 revisor + `Memoria.md` al día.
- Naming: inglés código; DB `Prefijo* + str/int/dec/dte/bln`; JSON legacy medido, NO PascalCase puro.
- Sin secretos, sin TODO sin issue, sin usings muertos, sin código muerto.

## 2. Memoria
- Plantilla canónica `.specify/templates/spec-template.md` (alias `specs/_template.md`), DoD por CRUD en `NV-04`.
- `AGENTS.md`: manual operativo vivo.
- `Memoria.md`: estado, decisiones, lecciones aprendidas por fase y metodología de diagnóstico de chaos. Actualizar al cerrar cada fase.
- Al empezar, lee `Memoria.md` para conocer el estado del proyecto y las decisiones tomadas.
- Al terminar una tarea, actualízalo: estado actual, decisiones importantes (con su porqué) y errores a evitar.
- Al cerrar una fase, concilia su `spec.md`/`plan.md` con lo implementado (rutas de tests, criterios, nombres reales de archivos) y resuelve el bloque de aprobación: Borrador con evidencia, o firmado solo si el usuario indicó revisor.
- Mantenlo breve (máximo ~10000 líneas): resume o elimina lo que ya no aporte.
- Si algo se convierte en una regla permanente, propón moverlo a AGENTS.md en lugar de dejarlo en la memoria.
- No guardes nunca datos sensibles (claves, tokens, datos personales).

## 3. Límites — no hacer
- No InMemory para negocio; solo 1 test dummy `byte[]{1}`.
- No `TokenBlacklist` estático sin `Reset()`.
- No hardcodear connection strings/keys.
- No Argon2id débil; BCrypt solo migración.
- No tocar pipeline sin orden §4 constitución.
- No relajar rate en prod; sin `PERF_*` en este MVP.
- No config eager en DI con overrides WAF — `IOptionsMonitor` lazy.
- No duplicar tablas vivas — canónico en `docs/`.
- No estados saga — no existe `06-04` aquí.
- No `Invoke-WebRequest` sin `-UseBasicParsing` en PS5.1.

## 4. Verificación por CRUD
- `build Release 0/0` antes de test.
- `Unit/Integration/Security/Contract/Database --no-build` 100% verdes.
- `critic-guardrails.ps1 → PASS`, `check_endpoints.ps1 → OK`, `check_coverage.py ≥45%`, `semgrep scan --config=auto --config=.semgrep/semgrep.yaml --error` 0 nuevos.
- `Stryker` solo nightly opcional; tras él siempre `build` antes de `--no-build`.
- Contract `CONTRACT_CAPTURE=1` para regenerar, sin var solo valida.
- `curl /health/ready` 200 con DB externa, 503 si cae.

## 5. Convenciones técnicas
- Rutas `api/v{version}/[controller]`; DTOs + FluentValidation; `RowVersion base64` en Update/Delete.
- Health `/health,/health/ready`; `/scalar` solo Dev. Sin `health-ui/metrics` full.
- Keys IMemory solo `blacklist:{jti},attempts:{user},lockout:{user}` 2FA/blacklist TTL 120s. Password nunca en caché.
- Migrations: `dotnet ef migrations add --project API_SDD`.
- Auth primero `NV-03` da `AdminPolicy`; demás CRUDs la reutilizan. 2FA TOTP `Otp.NET ±1 + DataProtection` desde CRUD1.

## 6. Seguridad DevSecOps
- JWT HS256 real `UseJwtBearer=true` siempre (sin flag legacy), `OnTokenValidated` blacklist.
- Headers outermost + HSTS365d no-Dev + CORS 1 origen.
- Rate 5 policies + 429 uniforme + `UseRateLimiter` antes Auth.
- `Assembly integrity + timeout 60s + Serilog filtro secretos`.
- SAST Semgrep bloqueante; Sonar informativo; `Dependabot` activo. `Trivy/ZAP/RESTler` diferidos documentados, no exigidos.

## 7. Testing quirks
- `xUnit+FluentAssertions+Moq+FsCheck`; WAF para Integration/Security.
- `SecurityTest` reuse `UnitTest/Common/TestDataFactory`.
- `Testcontainers.MsSql` puerto fijo. `NBomber borra reportes` — no aplica aquí, pero separa `reports/` si lo usas futuro.
- `Contract IsConventional` legacy, no PascalCase puro.

## 8. Observabilidad mínima
- Serilog JSON + consola, sin secretos. CorrelationId por request.
- Sin OTel/Prometheus/Grafana por ahora.

## 9. Operación manual — sin hosting aún
- DB externa: migraciones + seed idempotente + backup documentado fuera de repo.
- Publish listo `dotnet publish`, no desplegado. Cuando haya hosting se añade runbook con `env` + `KeysPath` + verificación post-subida.
- Sin Redis: single-instance documentado. Sin AWS/SQS.

## 10. Lecciones
- Fuente: `Memoria.md`. Si se vuelve regla, mover a AGENTS.md.

## 11. Sub-agentes
- `slice-scaffolder` real-first sin Fase fake; `security-reviewer` gate sin editar; `traceability-clerk` reporta drift no edita.
- En CI solo `critic` ligero bloqueantes 1-4.

## Solution structure
| Project | Path | Type |
|---|---|---|
| `API_SDD` | `API_SDD/` | Web API `Program.cs` |
| `UnitTest` | `UnitTest/` | xUnit |
| `IntegrationTest` | `IntegrationTest/` | WAF + Testcontainers |
| `SecurityTest` | `SecurityTest/` | WAF |
| `DatabaseTest` | `DatabaseTest/` | Testcontainers migraciones/seed |
| `ContractTest` | `ContractTest/` | captura + convención |
| `deploy/` | `deploy/` | compose local SQL opcional (futuro, ver `deploy/README.md`) |
| `scripts/` | `scripts/` | critic, endpoints, coverage |
| `docs/` | `docs/` | endpoints, rate-matrix, asvs |
