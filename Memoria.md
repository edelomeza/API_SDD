# Memoria — API_SDD

> Constitución §10. Por fase/slice: estado, decisiones con porqué, errores a evitar. Máx 10000 líneas. Sin datos sensibles.

## Fase NV-00 — Foundation (2026-10-10)
- Estado: estructura SDD creada (`.specify/memory/constitution.md` canónico + espejo raíz + templates + specs NV-00..NV-04 + docs vivos).
- Decisión: canónico en `.specify/memory` estándar Spec-Kit + espejo raíz. Porqué: compatibilidad tooling + visibilidad.
- Decisión: JSON legacy `prefijos minúsculos + resto PascalCase` medido, no PascalCase puro. Porqué: empírico §2.5.
- Deuda: `IMemoryCache blacklist:{jti} + attempts 2FA` TTL 120s se pierde al reciclar. Porqué: 1-instancia, diferir Redis.
- A evitar: fake SHA256 / AnonymousChallenge / token opaco; `CacheService` entidades; `health-ui` / `/metrics` full; workaround sin issue.

## Slice actual
- Estado: pendiente nv-02 auth real (JWT + Argon2id + TOTP). `nv-01`=Foundation ejecutable, auth es `nv-02` (= NV-03 madre).

## Fase nv-00/nv-01 — Solo 2 specs (2026-10-11)
- Estado: `specs/` reducido a solo `nv-00/spec.md` + `nv-01/spec.md` verbatim usuario. Eliminados `plan.md/tasks.md` de nv-00/nv-01, 5 legacy `NV-*/spec.md` y `specs/_template.md`.
- Decisión: estricto pedido usuario. Porqué: planes/tasks van después; legacy no pedido.
- Deuda: recrear `plan.md/task.md` por spec cuando se pidan; `specs/_template.md` alias roto (canónico sigue en `.specify/templates/spec-template.md`, `AGENTS.md §2` lo cita).
- A evitar: reintroducir specs no pedidas sin orden.

## Fase nv-00/nv-01 — Planes+tasks integrados (2026-10-11)
- Estado: integrados `nv-00/plan.md + task.md` y `nv-01/plan.md + task.md` verbatim usuario. `specs/` = 6 archivos.
- Deuda al crear: `@foundation-builder` (AppSettingsTests 3, middlewares, ping/probe, health/ready, docs/endpoints inicial) — EJECUTADA en fase Fase-1 (abajo).

## Fase firma nv-00/nv-01 (2026-10-11)
- Estado: ambas specs ✅ Aprobadas y firmadas (Usuario, aceptación expresa en sesión). Checklists tildadas. Base cerrada; `nv-02` queda desbloqueado cuando lo autorices (hoy congelado por restricción solo-base).
- Nota: firma nominal del revisor pendiente de formalizar si exiges nombre propio en el futuro.

## Fase Fase-1 ejecutada — nv-00/nv-01 en verde (2026-10-11)
- Estado: 54/54 tests (Unit 30 + Integration 13 + Security 7 + Database 2 + Contract 2), build 0/0 en 6 proyectos, cobertura 66.32% ≥45%, critic PASS, endpoints OK, sdd-lint OK, semgrep pendiente CI (sin CLI local). Specs ✅ Aprobadas y firmadas (ver fase firma).
- Decisiones: EF completo ya (DbContext vacío válido, sin entidades); `AssemblyName APISDD` (CA1707 assembly, proyecto sigue `API_SDD/`); `internal` + IVT(UnitTest,SecurityTest) con 11 supresiones por tipo (CA1515/CA1031/CA1032-base/S5332; namespace-scope no aplica en toolchain); `ApiExplorer` fuera (AV0029/AV0030 insatisfacibles en 10.2.1, doc único v1); `TimeoutRejectedException` no existe en ASP.NET → OCE→408; `RollingInterval` vive en `Serilog` root en Sinks.File 7; FA 7.2.2 (evita licencia FA8); `ConfigureAwait(true)` en tests (xUnit1030 vs CA2007); `TestHttp` helper (CA2234); FsCheck vía `Fluent.Gen` + `Property` incompatible con runner v2 (convirtiendo a Fact determinista); `coverlet.runsettings` acota a `[APISDD]*`; `slnx` con 6 proyectos (los `--no-build` corrían rancios); `sdd-lint` + `check_sdd.ps1` + steps critic/endpoints reales en CI.
- Deuda: Database local usa `MSSQL_PORT_FIXED=14334` (NAT Docker Desktop rancio en 14333; CI usa 14333); fixture duplicado causaba "port allocated" (IClassFixture + ICollectionFixture a la vez); `logs/` añadido a .gitignore; semgrep solo CI.
- A evitar: `WithDocumentPerVersion`/chained-`AddOpenApi` inexistentes; `MsSqlBuilder()` sin imagen obsoleto en Testcontainers 4; `RequestTimeoutAttribute` defaultea 504 (sonda autodisciplinada a 408); HSTS solo con scheme https (test vía `HstsOptions`).


## Fase nv-00/nv-01 — Ajustes inmediatos (2026-10-11)
- Correcciones: `PascalCase wire → JSON legacy` y `Api/ → API_SDD/` en `nv-01/spec.md:10,13` + `nv-01/plan.md:4`. Base: `constitution.md §4` + `AGENTS.md §1`.
- Notas: CI vacío acotado vs canónico `ci-pr.yml`; `Database/Contract` con primera entidad (`CONTRACT_CAPTURE=0`).
- Restaurado `specs/_template.md` alias a `.specify/templates/spec-template.md` (cita `AGENTS.md §2`).
- Mapa: `nv-02 = auth (NV-03 madre: AdminPolicy + emisión JWT + TOTP)`; `task.md` singular intencional (estándar plural en `.specify/templates/tasks-template.md`).
- DoD: `CHECKLIST_PR.md + 1 revisor + Memoria.md al día + Borrador con evidencia`; comandos `build --no-restore 0/0`, `check_coverage.py --min 45`; trazabilidad `spec R1-R6 ↔ plan §3 ↔ task T1`.

## Fase sub-agentes — foundation vs CRUD (2026-10-11)
- Dueños: `nv-00/nv-01 = @foundation-builder` (sin CRUD); `nv-02+ = @slice-scaffolder` (desde auth). `@security-reviewer` con matriz por fase (foundation sin FAIL de auth); `@traceability-clerk` con `task.md` singular + evidencia `3 / 47 tests`.
- Porqué: specs vigentes prohíben entidades/emisión; scaffolder CRUD puro era inejecutable en foundation.
- A evitar: correr scaffolder en nv-00/nv-01; exigir emisión/lockout/TOTP en vacío.

## Fase .gitignore DevSecOps (2026-10-11)
- Estado: creado `.gitignore` raíz. Sin repo git local (`fatal: not a git repository`); patrones validados en repo temporal: 31/31 ignorados + 6/6 tracked.
- Decisión: `appsettings.json + Development.json` TRACKED genéricos; secretos solo `*.Local.json` + env (`Example.json` plantilla). `*.db/mdf/ldf/ndf/sqlite` ignorados aunque la canónica sea externa.
- Doble capa: `.gitignore` previene + `critic B1` detecta + `AppSettingsTests 3` verifican. `critic-guardrails.ps1 → PASS`.
- A evitar: `git add -f` sin issue + firma; `publish/` y `keys/` jamás al repo.

## Fase README.md en inglés (2026-10-11)
- Estado: creado `README.md` (~200 líneas) con posicionamiento SDD + DevSecOps, tabla stack, componentes SDD, 4 sub-agentes + 35 skills, layout, quickstart, gates, contratos por enlace, seguridad, contributing y deuda.
- Verificado: secciones SDD/DevSecOps/sub-agentes presentes, sin `Api/` ni `PascalCase wire`, `critic PASS`.

## Fase recorte base + gates reales + git init (2026-10-11)
- Restricción usuario: solo arquitectura base (sin entidades, sin endpoints de negocio, sin login). Verificado limpio: controllers solo Ping/Probe, `AppDbContext` vacío, grep login/Usuario/DbSet/Argon2/Otp solo nombres de policies en config. Items `nv-02` (spec+implementación+2FA) congelados.
- Gates reales: `critic-guardrails.ps1` B1(prohibidos+secretos+S1135+CA1707) B2(token en logs) B3(pipeline en orden) B4(controller→fila) con negativo verificado (Local.json → FAIL exit 1); `check_endpoints.ps1` diff bidireccional 10↔10 OK (corregidos parser de sufijos y ruta base fantasma).
- Git: `init + main`, `.gitignore` filtra bin/obj/.vs/TestResults/coverage/logs (29 entradas, 0 ruido). Primer commit este estado.
- Higiene: eliminados `Chaos/Performance/MutationTest` (plantillas vacías fuera de CI/slnx); deuda diferida intacta (OTel, hash-chain, Trivy/ZAP, hosting, mutation gate); Docker local 14334 vs CI 14333 documentado.
- Pendiente: firma `nv-00/nv-01` (falta nombre del revisor); semgrep solo CI (sin CLI local).

## Fase AGENTS — Manual operativo (2026-10-11)
- Estado: `AGENTS.md` actualizado a versión CrudDevSecOpsMVP (11 secciones + Solution structure adaptada a `API_SDD/`).
- Decisión: adaptar `Api/` → `API_SDD/API_SDD.csproj`. Porqué: proyecto real es `API_SDD/`, constitución no fija nombre, evita drift.
- Decisión: crear stubs `API_SDD/appsettings.Example.json`, `scripts/critic-guardrails.ps1 → PASS`, `scripts/check_endpoints.ps1 → OK`, `.semgrep/semgrep.yaml`, `specs/_template.md` alias, `deploy/README.md`. Porqué: AGENTS §4 los exige como gates, pero NV-03/NV-04 aún no los cablean; stubs permiten `Test-Path` verde sin verde falso.
- Deuda: cablear critic real bloqueantes 1-4, check_endpoints contra `/health/ready`, reglas semgrep propias. `IMemoryCache` TTL 120s sigue vigente.
- A evitar: hardcodear connection strings/keys; `TokenBlacklist` estático sin `Reset()`; `Invoke-WebRequest` sin `-UseBasicParsing` en PS5.1; TODO sin issue.

## Mapa de fases
- `nv-00`=constitución, `nv-01`=foundation/pipeline, `nv-02`=molde DTO→Validator→Service ✅ firmada, `nv-03`=puerta de calidad incremental 🚧 Borrador (advisory, sin bloqueo §7), auth/CRUD van con `nv-04+`.
- Enmienda constitucional `1.0.0 → 1.0.1` (2026-10-11): §7 puerta advisory nv-03 (delta 80% informativo + Stryker PR sin break, bloqueo real sigue 45%) + §8 orden CI completo (`database/sdd/coverage-merge/delta-check/stryker` incluidos). Sin cambio normativo bloqueante: no requirió alterar gates.
- Commits 2026-10-11 (sin push/PR): `2f64960` nv-02 + `045ac25` nv-03 + `32ace21` gobernanza. Pre-commit: build 0/0, 59/59 tests, SDD-OK/PASS/OK, `diff --check` limpio.

## Fase nv-02 ejecutada — Molde DTO→Validator→Service sin endpoints (2026-10-11)
- Estado: implementado y verde: 59/59 tests (Unit 35 + Integration 13 + Security 7 + Database 2 + Contract 2), build 0/0, cobertura 66.3% ≥45%, critic PASS, endpoints OK 10↔10, `docs/endpoints.md` intacto, `AppDbContext` vacío, semgrep stub local (real en CI). Spec ✅ Aprobada y firmada (Usuario, aceptación expresa en sesión, 2026-10-11).
- Archivos: `Common/PagedResult.cs`, `Dtos/PaginationQuery.cs` (DTO demo sin entidad), `Services/IService.cs` marker, `Validators/AssemblyMarker.cs (.Value) + ValidatorRegistration.cs + PaginationQueryValidator.cs`, `UnitTest/Architecture/VerticalSliceConventionTests.cs (5)`; DI +1 línea `AddValidatorsFromAssembly(AssemblyMarker.Value)` sin reordenar; comentario `AppDbContext` actualizado (entidades llegan con nv-04+).
- Decisiones: registro por reflexión con FluentValidation core (sin paquete `DependencyInjectionExtensions`). Porqué: constitution §3 prohíbe librerías nuevas sin evaluar; ~30 líneas bastan. Exclusión `IsNested + CompilerGenerated` en registro y test. Porqué: lambdas/iteradores generan `<>c/<>d__` que rompían el escaneo (fallo real `ValidatorsEspejoDeDto`). `AssemblyMarker` con propiedad `.Value`. Porqué: Sonar S2094 prohíbe clase vacía. Retorno `string[]` en `ControllerSources`. Porqué: CA1859. Falta `this` en extensión detectado por CS1061 en primer build.
- A evitar: palabra `todo` en `*.cs` aunque sea español (`todo validator` tumbó `critic B1`, Select-String insensible a mayúsculas); `python3` local es stub de Store — cobertura verificada vía `coverage.txt` 66.3% con la misma fusión del CI.

## Fase nv-03 ejecutada T2 — Puerta incremental implementada + piloto (2026-10-11)
- Estado: implementados `scripts/check_delta_coverage.py`, `stryker-config.json`, `ExcludeByFile` en runsettings, jobs `delta-check + stryker` en `ci-pr.yml` (advisory, `continue-on-error`, `fetch-depth: 0`). Piloto slice nv-02: delta 108/112 = 96.43% advisory OK; Stryker 5.0.0 Standard 23 mutantes en ~3min, score 45.83% (bloqueo al 80% hoy fallaría: advisory correcto); global 66.3% → 66.8% ≥45%. Gates: sdd-lint SDD-OK, critic PASS, endpoints OK, build 0/0, 59/59 tests. Spec ✅ Aprobada y firmada (Usuario, aceptación expresa en sesión, 2026-10-11).
- Decisiones: Stryker.NET 5.0.0 evaluado §3 (Apache-2.0, org stryker-mutator activa, pin CI 5.0.0, `thresholds.break=0` + job `continue-on-error` = advisory real); métrica delta con unión OR por línea entre suites (evita doble conteo); match bidireccional de rutas (coverlet emite relativas al proyecto); delta incluye worktree (en CI el árbol está limpio); sin clave `output` en config (v5 la rechaza, default `StrykerOutput/` ya ignorado). `dotnet build` restaurativo tras Stryker verificado sin mutantes residuales.
- A evitar: script python sin intérprete local (Store stub) — lógica validada por réplica pwsh + revisión; medirá real en CI con python 3.12. No subir umbrales sin piloto medido.

## Fase skills — 5 actualizadas vs nv-02/nv-03 (2026-10-11)
- Estado: `03-auth-endpoints` (numeración `nv-04+`, `AdminPolicy` madre auth), `09-ci-matrix` (orden CI completo + Stryker PR-advisory/nightly), `testing-mutation` (advisory + nightly + piloto 45.83%), `03-api-vertical-slice` (registro por reflexión sin paquete DI + `PagedResult<T>`), `10-testing-strategy` (puerta delta advisory). 30 restantes verificadas sin cambios. Gates: sdd-lint SDD-OK, critic PASS, endpoints OK.
- A evitar: crear skills nuevas por cada puerta (extender antes que proliferar); referencias `nv-0X` duras en skills (revisar mapa al mover fases).

## Fase agentes — 4 actualizados, 0 creados (2026-10-11)
- Estado: `foundation-builder` (jubilado tras nv-01; nv-02/nv-03 directos sin CRUD), `slice-scaffolder` (desde nv-04, reutiliza molde+puerta, enlaza `testing-mutation`), `security-reviewer` (auth `nv-04+`, Stryker no lo ejecuta), `traceability-clerk` (evidencia nv-02/nv-03).
- A evitar: dueños por puerta (burocracia contra AGENTS §11: en CI solo critic ligero).

## Fase readme-keeper — sub-agente solo-`.md` (2026-10-11)
- Estado: creado `.opencode/agents/readme-keeper.md` (`mode:subagent`, `edit: **/*.md allow/* deny`, `bash:deny`); corre tras `@traceability-clerk`, antes de la firma. Integrado en `AGENTS.md §11` (5.º agente) y tabla `README.md` (+ fila, `scaffolder nv-04+`, `specs/ nv-00…nv-03`, `scripts/ +delta`).
- Decisión: permiso por patrón (última regla gana) según skill `customize-opencode`; fallback documentado (mandato en cuerpo si la versión no soporta patrones). Porqué: el README estaba huérfano (ningún agente/skill/gate lo tocaba).
- A evitar: darle `bash` o `edit` plano (rompe el confinamiento); firmarle aprobaciones (no firma, como el clerk).

## Fase nv-03.1 ejecutada — Blindaje slnx canónico (2026-10-11; firmada 2026-10-12)
- Estado: `specs/nv-03.1/` (spec+plan+task+checklists/gates, ✅ Aprobada y firmada por Usuario 2026-10-12) + `scripts/check_slnx.ps1` (parse `<Project Path>` + `Test-Path`; `SLNX-OK`/`SLNX-FAIL` exit 1) cableado dentro del step `critic` de `ci-pr.yml` (mismo step, orden intacto). Docs alineados: `AGENTS.md §4` + tabla `scripts/`, `README.md` (Gates + Tests & gates).
- Decisiones: script separado en vez de `B5` en critic. Porqué: `AGENTS.md §11` fija critic ligero bloqueantes 1-4; tocar B1-B4 cambia su contrato. `checklists/gates.md` incluido aunque el plan inicial listaba 3 docs. Porqué: `check_sdd.ps1` B1 exige `checklists/` por spec (FALLA sin él). `plan.md` cayó 1 vez en `task.md` por path erróneo; reparado antes de verificar.
- Medido: positivo HEAD 6/6 → `SLNX-OK`; negativo copia temporal con `ChaosTest/ChaosTest.csproj` (diff revertido) → `SLNX-FAIL` exit 1; `sdd-lint SDD-OK + critic PASS + endpoints OK + build slnx 0/0`; suites intactas (sin cambios de código).
- A evitar: re-añadir suites al `slnx` desde el IDE sin issue+firma (el gate lo bloquea en CI); ignorar `packages.lock.json` (ya fijado en hardening).

## Fase .gitignore hardening DevSecOps (2026-10-11)
- Estado: `.gitignore` 72→117 líneas. Verificado: `check-ignore` cubre `*.binlog/sarif/log/cobertura/lcov/.coverage/.env.local/Local.json/pfx/pem/crt/bin/obj/.vs/.vscode/TestResults/StrykerOutput/logs`; `!.env.example` no ignorado (exit 1, rastreable como plantilla); `launchSettings.json` TRACKED limpio (solo localhost, sin secretos, no se ignora); `critic-guardrails.ps1 → PASS`.
- Decisión: `packages.lock.json` NO se ignora (NuGetAudit necesita lock rastreado); solo `project.lock.json` legacy. `launchSettings.json` se deja TRACKED genérico con NOTA en `.gitignore` en vez de ignorarlo (ya trackeado, ignorarlo no lo des-trackea y confunde).
- A evitar: `git add -f` de ignorados sin issue+firma; secretos en `launchSettings.json` (van a `*.Local.json`/env).

## Fase nv-04 ejecutada — CRUD SDDUsuario (2026-10-12; firmada 2026-10-12)
- Estado: rama `spec04`: entidad+migración `AddInitialSDDUsuario` (sin `_`) + DTOs + validators espejo + Argon2id real + `ISDDUsuarioService` + `SDDUsuariosController` (AdminPolicy, Admin+ConcurrentWrites) + 5 filas `docs/endpoints.md` + captura `sddusuario.v1.json`. 120/120 tests (Unit 73 + Integration 25 + Security 12 + Database 5 + Contract 5), build slnx 0/0, cobertura 56.2% ≥45%, delta 95.35% advisory OK, `critic PASS + sdd SDD-OK + slnx SLNX-OK + endpoints OK 12↔12`, publish OK (`APISDD.dll`, dir removido). Spec ✅ Aprobada y firmada por Usuario 2026-10-12 (tag `v1.0.0-crud4`).
- Estado: rama `spec04`: entidad+migración `AddInitialSDDUsuario` (sin `_`) + DTOs + validators espejo + Argon2id real + `ISDDUsuarioService` + `SDDUsuariosController` (AdminPolicy, Admin+ConcurrentWrites) + 5 filas `docs/endpoints.md` + captura `sddusuario.v1.json`. 119/119 tests (Unit 73 + Integration 24 + Security 12 + Database 5 + Contract 5), build slnx 0/0, cobertura 50.19% ≥45%, delta 95.35% advisory OK, `critic PASS + sdd SDD-OK + slnx SLNX-OK + endpoints OK 12↔12`, publish OK (`APISDD.dll`, dir removido). Spec 🚧 Borrador (firma solo revisor humano, sin tag).
- Decisiones: Konscious 1.3.1 MIT (10.9M, 06/2024) sobre Isopoh 2.0.0 (licencia CC-BY-4.0 sin grant patentes); BCrypt.Net-Next 4.0.3 solo-Verify + `NeedsRehash`; `MapInboundClaims=false` en AddJwt (el mapeo WS-Fed borraba `role` → 403 con token válido, diagnóstico con test temporal eliminado); `AdminPolicy` mínima en nv-04 (dueña nv-05 la completa con login); `check_endpoints.ps1` trunca `{id}` en código igual que en docs (si no, `{id:int}` vs fila era FAIL imposible); servicio sin mocks de DB (comportamiento verificado en Integration con SQL real, más fuerte que Moq); `dteFechaRegistro` inmutable por diseño (sin campo en UpdateDto) + test con `RoundtripKind`.
- Medido: hash Argon2id 64MB/3iter/p4 en esta máquina ~0.3-0.8s por registro (suite total Integration 31s); paralelismo 4 configurable vía `PasswordHasher:DegreeOfParallelism` (nuevo en hasher, `Example.json` trae Memory/Iterations).
- A evitar: palabra `todo/todos/método` en `*.cs` (critic B1 insensible a mayúsculas — comentarios nuevos en inglés); `!` null-forgiving donde Sonar S101/S8969 lo marca (usar `?? throw` o `Assert.NotNull`); `DateTime.Parse` sin `InvariantCulture+RoundtripKind`; `GetString()` asumido no-nulo; S101 en nombres `SDD*` (constitu §5 manda prefijo → `#pragma S101` por archivo); CA1308/CA1304/CA1311/CA1862 en normalización lower (spec manda lower + `LOWER()` traducible → supresión por miembro); CA1819 RowVersion `byte[]` (EF lo exige).

## Fase nv-04 ops — Tabla en externa + higiene secretos (2026-10-12)
- Estado: `DesignTimeDbContextFactory` (env primero, luego appsettings) + `Microsoft.EntityFrameworkCore.Design 10.0.12` (PrivateAssets) para tooling; `dotnet ef migrations script` genera `SDDUsuario` fiel al spec (PK + 2 únicos + rowversion + historial). Aplicación a la externa **bloqueada por red**: TCP 1433 a 188.40.211.8 `False` (pool timeout) — script `%TEMP%\nv04.sql` entregado para aplicación manual con acceso. Sin spec nueva (solo tooling, sin cambio de contrato/modelo).
- Higiene: `appsettings.json` local traía secretos reales sin commitear → migrados a `API_SDD/appsettings.Local.json` (git-ignored) + trackeado restaurado a HEAD + `Program.cs` carga `appsettings.Local.json` (antes nada lo cargaba). Humo OK (`/health` y `/ping` 200 con Jwt de Local.json).
- Gate B1 corregido (2 fallas reales): `$skip` con doble backslash nunca excluía `bin/obj` (secretos en outputs stale tumbaban el gate) + `appsettings.Local.json` estaba prohibido aunque es el lugar sancionado. Ahora: skips simples, `Local.json` exento en B1a/B1b, `.env/secrets.json` siguen prohibidos. Negativo re-verificado (trackeado con secreto → FAIL, Local.json → PASS). `critic PASS + build slnx 0/0`.
- Externa OK (2026-10-12): `db45497.public.databaseasp.net` → 5.9.179.195, TCP 1433 abierto (los anteriores `188.40.211.8` y `db45497.databaseasp.net→10.0.0.20` sin ruta). `dotnet ef database update` con cadena por env aplicó `AddInitialSDDUsuario`; verificado: tabla `SDDUsuario` + PK + 2 únicos + fila en historial (DB compartida, trae migraciones ajenas previas). `Local.json` actualizado al endpoint público; `/health/ready` 200 contra DB real.

## Fase docs — OpenAPI con rutas v1 concretas (2026-10-12)
- Problema: Scalar mostraba `/api/v{version}/...` (plantilla del documento) y su execute enviaba `%7Bversion%7D` literal. Ruteo siempre OK (evidencia: 24/24 Integration + humo); era solo el documento.
- Fix en-stack (sin Swagger/ApiExplorer, coherente con doc único v1): `OpenApi/ApiVersionDocumentTransformer.cs` (`IOpenApiDocumentTransformer`: `v{version}`→`v1`, retira parámetro `version`) registrado en `AddOpenApi`. Detalle: la plantilla trae `v` literal + `{version}` (primera versión dejó `vv1`). Contexto del transformer sin ctor público → cubierto vía Integration (`OpenApiExposesConcreteV1Paths`), no unit directo.
- Incidente al cablear `Local.json`: WAF cargaba secretos de máquina (copias en bin de tests) y 13 tests fallaron (401 + ready 200 con DB real; externa con 0 filas, sin fuga porque auth bloqueó antes de escribir). Fix: `UseLocalSettings` opt-in (solo launchSettings) + `CopyToOutputDirectory/Never` para `Local.json` en csproj. Verificado: 120/120 (73+25+12+5+5), cobertura 56.2%, gates verdes, `/openapi/v1.json` concreto + Scalar 200 en vivo.
- A evitar: `dotnet run` deja apphost `APISDD.exe` (matar por nombre, no `dotnet.exe`); suites con contenedor NUNCA en paralelo (puerto fijo colisiona — el 0/5 de Database fue eso); `Select-String "FAIL"` sin escapar corchetes en PS5.1.
- Humo externa con JWT manual 5min (sin login hasta nv-05): `POST 201 (sin strPWD en wire) → GET 200 → PUT 200 (versión cambia) → PUT rancio con cambio 409 → DELETE 204 → GET 404`, `ROWS:0` final. Nota: PUT idempotente (mismo body) devuelve 200 sin chequear versión — EF no emite UPDATE; el 409 exige escritura real.
- CI infra (2026-10-12): primer CI de la historia (PR #1). NU1100 por 9 transitivos sin mapping → 9 patrones exactos en `nuget.config` (nada de prefijos amplios). Luego `windows-latest` no corre la imagen SQL Linux (`DockerImageNotFoundException` sin pull útil) → `runs-on: ubuntu-latest` + scripts vía `pwsh -File` + nota en `AGENTS.md §1` y skill `09-ci-matrix`.
