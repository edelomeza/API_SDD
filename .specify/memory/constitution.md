---
version: 1.0.1
ratified: 2026-10-10
last_amended: 2026-10-11
status: active
source_of_truth: .specify/memory/constitution.md
mirror: ./constitution.md
methodology: SDD
---

# Constitución — API_SDD

> Rector SDD. Ante conflicto con otro artefacto, **manda esta constitución**.
> Filosofía madre: cada CRUD sale liberable con todo `00+01+02+03+04` aplicado.
> Fuera de alcance en esta etapa: AWS, saga/MassTransit/SQS, Redis distribuido,
> OTel/Prometheus/Grafana full, NBomber, Chaos, Pact-real, hosting/CD.

## 1. Propósito y alcance

- API REST stateless 1-instancia en ASP.NET Core 10 + EF + SQL Server **externo**.
- Metodología SDD. Toda funcionalidad nace de spec en `/specs` (`NV-00…NV-0X`).
- Objetivo: cada CRUD `spec→migración→DTO→servicio→controller→tests→gates→tag` liberable con `dotnet publish` listo, sin desplegar aún.

## 2. Principios no negociables

1. **Stateless 1-instancia**: sin sesión en proceso. Persistente en SQL (`SegBloqueo`, refresh). Efímero solo en `IMemoryCache` para `blacklist:{jti}` (TTL = vida remanente del token, ver `nv-06`) + `attempts 2FA` TTL 120s — deuda documentada, se pierde al reciclar.
2. **Seguridad primero**: validar en frontera, denegar por defecto, mínimo privilegio, sin secretos en código/logs/errores.
3. **Sin estado oculto**: todo compartido en DI; lo estático expone `Reset()` + `lock`.
4. **Fail-fast**: errores uniformes, health real con check SQL, sin tragar excepciones. `Detail` solo no-prod.
5. **Empírico antes que teórico**: medir antes de fijar timeouts/nombres/umbrales.
6. **Documentar límites** en `Memoria.md`, prohibido workaround sin issue.
7. **Real-first**: prohibido `fake SHA256 / AnonymousChallenge / token opaco`. Cada slice usa `JWT real + Argon2id real + TOTP real` desde CRUD1.

## 3. Stack obligatorio

| Capa | Tecnología |
|---|---|
| Runtime | .NET 10 / ASP.NET Core 10 |
| Persistencia | SQL Server externo vía EF Core, migraciones. InMemory solo 1 test dummy, nunca integración |
| Tests | xUnit + FluentAssertions + Moq + FsCheck determinista; WebApplicationFactory; Testcontainers.MsSql puerto fijo |
| Auth | JWT HS256 + refresh rotación + 2FA TOTP Otp.NET + DataProtection |
| Hash | Argon2id 64MB/3iter, BCrypt solo `Verify $2a$/$2b$` migración |
| Rate | Global 1000/min, Login 5/5min, Login2faVerify 10/5min, Admin 200/min, ConcurrentWrites 10 |
| Validación | FluentValidation |
| Logs | Serilog JSON rolling + consola, filtro secretos, CorrelationId |
| SAST | Semgrep + SonarAnalyzer.CSharp + NuGetAudit |
| CI | GitHub Actions solo `ci-pr.yml`, sin deploy |

No nuevas librerías sin evaluar licencia/mantenimiento/seguridad. Regla de excepción en `NV-00`.

## 4. Arquitectura

- Rutas `api/v{version:apiVersion}/[controller]`, v1.0 estricta sin versión → 404.
- JSON convención legacy medida `prefijos minúsculos + resto PascalCase + id/sufijo`, NO PascalCase puro. `PropertyNamingPolicy=null`.
- Sin saga. Sin caché entidades. Cero `CacheService` de entidades.
- Pipeline fijo: `SecurityHeadersMiddleware outermost → ExceptionHandlingMiddleware → ForwardedHeaders → HSTS no-Dev → HttpsRedirection → CORS 1 origen → UseRateLimiter antes Auth → UseAuthentication → UseAuthorization → Controllers`.
- Health: `/health liveness + /health/ready con check SQL externo`. Sin `health-ui`, sin `/metrics` full — 4 gauges diferidos a fase observabilidad.
- OpenAPI + Scalar solo Dev.
- DTOs validados, códigos `201/200/204/400/401/403/404/409/422/429/500`. `PagedResult{Items,TotalCount,Page,PageSize}`.

## 5. Código y API

- Inglés en código. Tablas/columnas `Prefijo* + str/int/dec/dte/bln + RowVersion rowversion + decimal(18,2)`.
- `IOptionsMonitor` lazy en DI si hay overrides WAF. Nada de `new` para servicios clave.
- Complejidad `≤10 método / ≤15 clase`. `.editorconfig` CA3000+ error.
- Sin `try/catch` ad-hoc en controllers. `NotFound→404, Forbidden→403, Concurrencia→409, Validation→422`.
- Migraciones sin `_` (CA1707). `S1135` caza `todo` incluso minúsculas.

## 6. Seguridad DevSecOps

- Headers: `nosniff, DENY, strict-origin-when-cross-origin, X-XSS 0, CSP nonce 16B/request exención scalar/openapi, HSTS 365d solo no-Dev, CORS single origin, Kestrel 10MB/2min, timeout 60s`.
- JWT: `key ≥32B solo env, ClockSkew=Zero, ValidAlgorithms=[HS256], Issuer/Audience, claims sub/jti/role/2fa_temp, OnTokenValidated anti-blacklist`.
- Login: `fake hash anti-enumeración 401 idéntico, lockout DB 5→15min TimeProvider fail-closed`.
- Password nunca en caché/logs. `strPWD nvarchar(200)` PHC basta.
- Assembly integrity `ExpectedSha256`. `DataProtection:KeysPath` persistente fuera de wwwroot.
- `docs/endpoints.md + rate-limit-matrix.md + asvs-l2-checklist.md` vivos, specs solo enlazan no duplican.

## 7. Testing y QA

- Suites: `Unit, Integration SQL real, Security, Database, Contract captura`. Sin `Mutation/Performance/Chaos` obligatorios — `Stryker ≥80%` y `FsCheck .Trim()` van nightly/opcional, no bloquean release.
- `ContractTest/Fixtures` captura real `CONTRACT_CAPTURE=1`, `IsConventional`, `RowVersion AQ==`, `xunit.runner.json` serie en Contract.
- Cobertura `≥45% python scripts/check_coverage.py`. Gate real futuro es mutation, hoy no bloquea.
- Puerta incremental advisory nv-03 (no bloquea): `check_delta_coverage.py --min 80 --advisory` sobre el delta (`merge-base main`, higiene `Migrations/*`, `Program.cs`, `Dtos/*`) + Stryker PR `since:main` (`thresholds.break=0`, `continue-on-error`).
- `Testcontainers.MsSql WithPortBinding(puertoFijo,1433)`. `/provider-states` gateado `EnableProviderStates+no-prod`.

## 8. CI y release manual

- CI-PR orden: `restore → build → unit → integration → security → database → contract → critic → endpoints → sdd → coverage-merge → coverage → delta-check → stryker → semgrep`, `--no-build` Release.
- Sin CD. Release manual: `dotnet publish -c Release -o ./publish` + set `env ConnectionStrings__Default, Jwt__Key/Issuer/Audience, DataProtection__KeysPath` + `curl /health/ready + login + 401 + headers` + tag `vX.Y.Z-(crud|auth)N` (`crudN` entidades, `authN` slices auth).
- `CHECKLIST_PR.md` obligatorio. 1 revisor. `main` protegida.
- YAML validado `yaml.safe_load`. `Stryker` deja mutantes — siempre `dotnet build` después.

## 9. Observabilidad mínima

- `Serilog JSON + CorrelationId + /health*`. Sin OTel/Prometheus/Grafana por ahora.
- Audit: tabla + `TraceId`, sin PII en logs. Hash-chain full diferido, no verde falso.

## 10. Memoria

- `Memoria.md` por fase/slice: estado, decisiones con porqué, errores a evitar. Máx 10000 líneas. Sin datos sensibles.

## 11. Gobernanza SDD

- Specs en `/specs/NV-0X` con plantilla 7 secciones + Aprobación. Sin cambio de contrato sin spec. `AGENTS.md` dice cómo, constitución dice qué no se negocia.
