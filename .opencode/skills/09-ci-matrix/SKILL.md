---
name: 09-ci-matrix
description: Solo ci-pr sin nightly/deploy (lite)
metadata:
  scope: lite
  variant: lite
  opencode/slash: "true"
---
# 09-ci-matrix MVP (lite)

## Incluye
- `restore→build→unit→integration→security→critic→endpoints→contract→semgrep, timeouts 5/15/30min, --no-build`.

## Excluye
- mutation/perf/chaos/database nightly, deploy AWS.
> Nota: suite `Database` sí es obligatoria por CRUD (constitución §7); lo excluido es solo el *nightly*, no la suite.

## Verify
- `.github/workflows/ci-pr.yml` orden §8 + `yaml.safe_load` válido.
