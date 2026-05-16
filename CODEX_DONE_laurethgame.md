Sprint 13 implementation review completed.

Verdict: FAIL

Required report written to STAGING/codex_review_sprint13_impl_DONE.md.

Key result: EditMode passed through the live Unity editor (321/321 progress, 0 failed), and the dependency report was generated. The review still fails because PropRuntimeSpawner is not wired into the live runtime room-load path; it is only a callable service covered by direct tests. Additional gaps: v1.1 spec text still contains the old "seed-based or random" variant wording, dotnet build is blocked by a missing local Microsoft.Unity.Analyzers.dll reference for RoomDesigner editor projects, and full PlayMode is blocked by _IsoGame missing from the active build profile/shared scene list.
