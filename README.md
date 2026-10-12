# API_SDD — Spec-Driven REST API (.NET 10)

> Built with **Spec-Driven Development (SDD)**. Secured with **DevSecOps best practices** from day one.
> Constitution `v1.0.0` (`.specify/memory/constitution.md`). Active specs: `nv-00` + `nv-01` (both Draft).

## What this is

Stateless, single-instance REST API on **ASP.NET Core 10 + EF Core + external SQL Server**. Every CRUD ships as a releasable slice: `spec → migration → DTO → service → controller → tests → gates → tag`, `dotnet publish`-ready, not deployed yet.

## Out of scope (at this stage)

AWS, saga / MassTransit / SQS, distributed Redis, full OTel / Prometheus / Grafana, NBomber / Chaos, real Pact, hosting / CD. Tracked as documented debt, never as false green.

## Tech stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 / ASP.NET Core 10 |
| Persistence | EF Core + external SQL Server, migrations |
| Future auth | JWT HS256 + refresh rotation + TOTP (Otp.NET) + DataProtection |
| Validation | FluentValidation |
| Tests | xUnit + FluentAssertions + Moq + deterministic FsCheck + WebApplicationFactory + Testcontainers.MsSql (fixed port) |
| Logs | Serilog JSON rolling + console, secret filter, CorrelationId |
| SAST | Semgrep + SonarAnalyzer.CSharp + NuGetAudit |
| CI | GitHub Actions, `ci-pr.yml` only, no deploy |

No new libraries without license / maintenance / security evaluation (exception rule in `NV-00`).

## SDD components

| Component | Path | Role |
|---|---|---|
| Constitution (source of truth) | `.specify/memory/constitution.md` (+ mirror `./constitution.md`) | Non-negotiables; wins on conflict |
| How-to manual | `AGENTS.md` | Operating manual (11 sections + solution structure) |
| Specs | `specs/nv-00/`, `specs/nv-01/` (`spec.md` + `plan.md` + `task.md`) | `nv-00` constitution MVP; `nv-01` foundation + pipeline + external DB; next is `nv-02` = auth |
| Templates | `.specify/templates/` (+ alias `specs/_template.md`) | 7-section spec template + Approval, plan and tasks templates |
| Living contracts | `docs/endpoints.md`, `docs/rate-limit-matrix.md`, `docs/asvs-l2-checklist.md` | Specs link, never duplicate |
| Gates | `scripts/critic-guardrails.ps1`, `scripts/check_slnx.ps1`, `scripts/check_endpoints.ps1`, `scripts/check_coverage.py`, `.semgrep/semgrep.yaml` | Pre-push quality gates |
| PR checklist | `CHECKLIST_PR.md` | Mandatory per PR + 1 reviewer |
| Journal | `Memoria.md` | State, decisions with rationale, lessons per phase (max ~10000 lines, no secrets) |

## Sub-agents (`.opencode/agents/`, 35 skills in `.opencode/skills/`)

| Agent | Permissions | Job |
|---|---|---|
| `@foundation-builder` | `edit: allow, bash: allow` | Owns `nv-00 → nv-01` (no CRUD): props/config, pipeline, middlewares, `Ping/Probe`, health, base tests. Retires after `nv-01`. |
| `@slice-scaffolder` | `edit: allow, bash: allow` | Owns `nv-04+` (auth first, then each CRUD): compilable skeleton (Phase A) + full slice (Phase B), real-first JWT/Argon2id/TOTP. Reuses `nv-02` mold + `nv-03` gate. |
| `@security-reviewer` | `edit: deny` | Phase-matrix audit before each tag (foundation mode skips future auth FAILs). Table `PASS/FAIL`, blocks tag. |
| `@traceability-clerk` | `edit: deny` | Drift reporter `spec ↔ code ↔ test` before signing Draft-with-evidence. Never edits. |
| `@readme-keeper` | `edit: **/*.md allow, * deny; bash: deny` | Keeps `.md` living (README, docs, specs): CI order, counts, links. Never touches code. |

Skills are linked, not copied (e.g. `00-constitution`, `01-foundation`, `04-security-core`, `critic-guardrails`, `drift-guards`); all 35 load on demand via the `skill` tool and most are also `/slash` commands.

## Repository layout

```text
API_SDD/            Web API (Program.cs)
UnitTest/           xUnit
IntegrationTest/    WAF + Testcontainers
SecurityTest/       WAF
DatabaseTest/       Testcontainers migrations/seed
ContractTest/       capture + convention (Fixtures)
specs/              nv-00…nv-03 (spec/plan/task + checklists)
docs/               endpoints, rate-matrix, asvs (living)
scripts/            critic, endpoints, coverage, delta
deploy/             optional local SQL compose (future)
.github/workflows/  ci-pr.yml only
.opencode/          agents + skills
.specify/           memory (constitution) + templates
```

Legacy `Chaos/Performance/Mutation` suites were removed (empty templates, out of scope); Stryker runs PR-advisory (`since:main`, non-blocking) plus nightly-only, NBomber/Chaos deferred per constitution.

## Quickstart

```bash
# 1. Config (generic files tracked; secrets only in *.Local.json or env)
cp API_SDD/appsettings.Example.json API_SDD/appsettings.json
# ConnectionStrings__Default -> external DB, UseInMemoryDatabase=false

# 2. Restore + build (0 errors, 0 warnings)
dotnet restore
dotnet build -c Release --no-restore

# 3. Run
dotnet run --project API_SDD/API_SDD.csproj

# 4. Health (200 with external SQL, 503 without DB)
curl http://localhost:5000/health
curl http://localhost:5000/health/ready
```

Docker is for tests only (Testcontainers).

## Tests & gates

| Command | Verifies |
|---|---|
| `dotnet test <csproj> -c Release --no-build` (Unit, Integration, Security, Contract, Database) | 100% green suites |
| `powershell -File scripts/critic-guardrails.ps1` → `PASS` | 4 blocking guardrails (secrets, tokens in logs, pipeline, endpoints) |
| `powershell -File scripts/check_slnx.ps1` → `PASS` | slnx references only existing projects |
| `powershell -File scripts/check_endpoints.ps1` → `OK` | Living docs match routes |
| `python scripts/check_coverage.py --min 45` | Coverage ≥ 45% |
| `python scripts/check_delta_coverage.py --min 80 --advisory` | Delta vs main ≥ 80% informativo (no bloquea) |
| `semgrep scan --config=auto --config=.semgrep/semgrep.yaml --error` | 0 new findings |
| `CONTRACT_CAPTURE=1 dotnet test ContractTest` | Regenerate captures (without the var: validate only) |

CI order (`ci-pr.yml`): `restore → build → unit → integration → security → database → contract → critic → endpoints → sdd → coverage-merge → coverage → delta-check → stryker → semgrep`. Stryker PR advisory (`since:main`, `break:0`) + nightly-only; always `dotnet build` after it.

## API contracts (links, not copies)

* `docs/endpoints.md` — `/health`, `/health/ready`, `auth/login`, `2fa-verify`, generic CRUD + `PagedResult{Items,TotalCount,Page,PageSize}`
* `docs/rate-limit-matrix.md` — 5 policies (Global 1000/min, Login 5/5min, Login2faVerify 10/5min, Admin 200/min, ConcurrentWrites 10), uniform 429 + `Retry-After`, `UseRateLimiter` before Auth
* `docs/asvs-l2-checklist.md` — V2–V14 traceable checklist

Conventions: `api/v{version}/[controller]`, strict v1.0 (unversioned → 404), measured legacy JSON (`lowercase prefixes + rest PascalCase + id`, `PropertyNamingPolicy=null`), OpenAPI + Scalar in Dev only, `ErrorResponse{Error,Status,TraceId,Detail non-prod only}`.

## Security highlights

* Never secrets in git (critic B1 → FAIL); local secrets live in `*.Local.json` or env, `Example.json` is the public template.
* JWT HS256 `key ≥ 32B` from env only, `ClockSkew=Zero`, `ValidAlgorithms=[HS256]`, claims `sub/jti/role/2fa_temp`, `OnTokenValidated` anti-blacklist.
* Argon2id 64MB/3iter (BCrypt verify-only for migration), DB lockout 5→15min fail-closed, identical 401 anti-enumeration, TOTP `Otp.NET ±1` + DataProtection from CRUD1.
* `blacklist:{jti}` / 2FA attempts in `IMemoryCache` TTL 120s only (single-instance debt); passwords never cached or logged.
* Headers outermost + CSP nonce 16B/request + HSTS 365d (non-Dev) + single-origin CORS + Kestrel 10MB/2min + 60s timeout; `DataProtection:KeysPath` outside wwwroot; `Detail` null in prod.

## Contributing (1 slice = 1 PR)

1. Start from an approved `specs/nv-*/spec.md` (no contract change without spec).
2. Implement `spec → migration → DTO → service → controller → tests → gates → tag vX.Y.Z-crudN`.
3. Fill `CHECKLIST_PR.md`, get 1 reviewer (`main` protected), keep `Memoria.md` current.
4. Close the phase by reconciling `spec.md`/`plan.md` with the implementation (test paths, criteria, real file names) and resolve Approval: Draft-with-evidence, or signed only if a reviewer was assigned.

## Limitations & debt

* No entities, no used `AdminPolicy`, no JWT issuance yet — issuance is `nv-02`.
* `IMemoryCache` blacklist is lost on recycle (documented, Redis deferred).
* Trivy / ZAP / RESTler deferred (documented, not required); Sonar informative, Semgrep blocking; Dependabot on.
* See `Memoria.md` for the full per-phase log.
