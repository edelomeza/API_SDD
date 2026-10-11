---
name: 04-totp-provisioning
description: TOTP Otp.NET + DataProtection desde día 1
metadata:
  scope: full
  mvp: hosting-ready-sin-saga
  opencode/slash: "true"
---
# 04-totp-provisioning MVP

## What I do
- `Otp.NET ±1 ventana + secreto protegido DataProtection KeysPath persistente + enrollment con waiver + recovery codes`.
- `attempts/lockout 2FA` IMemory TTL corto. Sin fake determinista.
- Real-first desde CRUD1.

## Verify
- TOTP real ±1 + `DataProtection:KeysPath` fuera wwwroot probados.
