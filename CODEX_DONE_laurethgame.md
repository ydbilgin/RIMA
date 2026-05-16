Sprint 13 spec review executed.

Output written:
- STAGING/codex_review_sprint13_spec_DONE.md

Verdict:
- FAIL

Primary blockers:
- PropRegistrySO runtime lookup is empty in player builds as specified.
- propId GUID auto-population requires an AssetPostprocessor or equivalent file missing from the locked file list.
- Rotation-aware placement cannot be validated with the current PropFootprintValidator API shape.
- Runtime collider/sorter components are not wired into any prop spawning/rendering path.

Additional gaps:
- Prop sorting can remain on Default layer instead of locked Props layer.
- Variant determinism should not rely on Vector2Int.GetHashCode().
- Variant pick wording conflicts with deterministic OQ3.
- RoomBankSO_Library_v1.asset is required by acceptance but missing from §5.
- Dependency report generation lacks direct test coverage.
