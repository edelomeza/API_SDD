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
- `restore→build→unit→integration→security→database→contract→critic→endpoints→sdd→coverage-merge→coverage→delta-check→stryker→semgrep, timeouts 5/15/30min, --no-build`.

## Excluye
- Stryker bloqueante, perf/chaos, deploy AWS. (Stryker corre PR-advisory `since:main` + nightly.)
> Nota: suite `Database` sí es obligatoria por CRUD (constitución §7); lo excluido es solo el *nightly*, no la suite.

## Verify
- `.github/workflows/ci-pr.yml` orden §8 + `yaml.safe_load` válido.
