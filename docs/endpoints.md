# Endpoints — vivo (nv-01)

> Fuente contratos. Los specs solo enlazan, no duplican. Constitución §6.
> Filas login/2FA/CRUD vuelven en `nv-02` con su spec.

| Método | Ruta | Auth | Rate | Códigos | Spec |
|---|---|---|---|---|---|
| GET | `/health` | no | global | 200 | nv-01 |
| GET | `/health/ready` (check SQL externo) | no | global | 200/503 | nv-01 |
| GET | `/api/v1/ping` | no | global | 200 | nv-01 |
| GET | `/api/v1/probe/timeout` (no-prod) | no | global | 408/404 | nv-01 |
| GET | `/api/v1/probe/error` (no-prod) | no | global | 500/404 | nv-01 |
| GET | `/api/v1/probe/forbidden` (no-prod) | no | global | 403/404 | nv-01 |
| GET | `/api/v1/probe/conflict` (no-prod) | no | global | 409/404 | nv-01 |
| GET | `/api/v1/probe/invalid` (no-prod) | no | global | 422/404 | nv-01 |
| GET | `/openapi/v1.json` (solo Dev) | no | — | 200/404 | nv-01 |
| GET | `/scalar/v1` (solo Dev) | no | — | 200/404 | nv-01 |

Convención JSON legacy: `prefijos minúsculos + resto PascalCase + id/sufijo`, `PropertyNamingPolicy=null`.
`PagedResult{Items,TotalCount,Page,PageSize}` (desde nv-02). OpenAPI + Scalar solo Dev.
