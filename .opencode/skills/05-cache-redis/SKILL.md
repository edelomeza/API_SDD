---
name: 05-cache-redis
description: Solo blacklist/2FA IMemory, 0 caché entidades (lite)
metadata:
  scope: lite
  variant: lite
  opencode/slash: "true"
---
# 05-cache-redis MVP (lite)

## Incluye
- Prefijos `blacklist:{jti},attempts:,lockout:` TTL 60-120s + nunca password/secret/token.

## Excluye
- `cache:{entidad}`, write-through, fallback Redis, `CacheFallbackTests` entidades.

## Verify
- Solo keys IMemory 2FA/blacklist; 0 `CacheService` entidades.
