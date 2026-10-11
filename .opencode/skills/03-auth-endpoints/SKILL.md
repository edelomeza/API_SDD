---
name: 03-auth-endpoints
description: Usuario + login + refresh + logout + 2FA desde CRUD1
metadata:
  scope: full
  mvp: hosting-ready-sin-saga
  fase: nv-02+
  opencode/slash: "true"
---
# 03-auth-endpoints MVP

> Fase `nv-02+` (auth). No exigible en gates `nv-01` vacío (sin emisión).

## What I do
- `POST /usuarios + /auth/login + /auth/login2fa/verify + /auth/refresh [AllowAnonymous] + /auth/logout [Authorize] + /two-factor/* [Authorize]`.
- Anti-enumeración 401 idéntico + lockout DB 5→15min + blacklist `blacklist:{jti}` IMemory 120s.
- 2FA TOTP `Otp.NET ±1 + DataProtection` desde CRUD1. `AdminPolicy` desde `nv-02` (= NV-03 madre).

## Verify
- Login 401 idéntico real + lockout fail-closed + refresh rotación.
