# Rate Limit Matrix — vivo

> Constitución §3, §6. `UseRateLimiter` antes de Auth.

| Política | Límite | Ventana | Ámbito | Spec |
|---|---|---|---|---|
| Global | 1000 | 1 min | todo | NV-02 |
| Login | 5 | 5 min | `POST login` | NV-01 |
| Login2faVerify | 10 | 5 min | `POST login/2fa-verify` | NV-01 |
| Admin | 200 | 1 min | endpoints admin | NV-02 |
| ConcurrentWrites | 10 | concurrente | POST/PUT/DELETE | NV-02 |

429 uniforme con `Retry-After`. Medir antes de fijar timeouts (principio empírico §2.5).
