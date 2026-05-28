# RIMA Cleanup Execution Plan
Generated: 2026-05-23
Target project: F:/Antigravity Projeler/2d roguelite/RIMA/
Scratch destination: F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/

---

## Section 1 — Categorization Table

| Path | Size | Category | Reason | Exact Bash Command |
|------|------|----------|--------|--------------------|
| RIMA/Library/ | ~8.1 GB | DELETE | Unity regen on next open. Already in .gitignore. Zero content loss risk. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Library"` |
| RIMA/Logs/ | ~8.7 MB | DELETE | Unity runtime logs. Already in .gitignore. Ephemeral. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Logs"` |
| RIMA/Temp/ | unknown | DELETE | Unity build temp. Already in .gitignore. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Temp" -ErrorAction SilentlyContinue` |
| RIMA/tmp/ | ~33 MB | DELETE | Scratch temp folder. Not in .gitignore explicitly but matches ephemeral pattern. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/tmp"` |
| RIMA/STAGING/twitter_research/ | ~464 MB | MOVE TO SCRATCH | Downloaded Twitter media. Pure research dump. | `Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/twitter_research" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/twitter_research"` |
| RIMA/STAGING/tweet_research/ | ~83 MB | MOVE TO SCRATCH | Older tweet research batch. | `Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/tweet_research" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/tweet_research"` |
| RIMA/STAGING/boona_frames/ | ~20 MB | DELETE | .gitignore explicitly lists — transient. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/boona_frames"` |
| RIMA/STAGING/CHATGPTSPRITESHEETS/ | ~24 MB | MOVE TO SCRATCH | AI-generated sprite sheet experiments. | `Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CHATGPTSPRITESHEETS" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/CHATGPTSPRITESHEETS"` |
| RIMA/STAGING/_reference_packs_raw/ | ~15 MB | MOVE TO SCRATCH | Raw reference downloads. | `Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_reference_packs_raw" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/_reference_packs_raw"` |
| RIMA/STAGING/codex_floor_walls_v01/ | ~17 MB | REVIEW | Are tiles imported into Assets/? | N/A |
| RIMA/STAGING/RIMA_AssetParts_v2/ | ~18 MB | REVIEW | Superseded by v3? | N/A |
| RIMA/STAGING/RIMA_AssetParts_v3/ | ~16 MB | REVIEW | Already imported into Assets/? | N/A |
| RIMA/STAGING/concepts/ | ~117 MB | REVIEW | Only copies of concept art? | N/A |
| RIMA/STAGING/_archive/ | ~270 MB | REVIEW | Move PNG bulk to SCRATCH, keep MD? | N/A |
| RIMA/STAGING/concept_art/_DISCARDED_codex_v1_* | ~10 MB | DELETE | Explicitly discarded. | `Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art/_DISCARDED_codex_v1_20260510T181951Z"` |
| RIMA/STAGING/*.md / *.txt / *.ps1 | <1 MB | KEEP | Active design docs + tooling. | — |
| RIMA/STAGING/graphify_corpus/ | ~1 MB | KEEP | Active memory files. | — |
| RIMA/STAGING/refs/ | ~2 MB | KEEP | Active research docs. | — |
| RIMA/ARCHIVE/STAGING_OLD/ | ~400 MB | REVIEW | Bulk old character frame PNGs. Cross-check Assets/ first. | N/A |
| RIMA/PIXELLAB_OUTPUTS/ | ~44 MB | REVIEW | Canonical character sprites or already imported? | N/A |
| RIMA/RIMA_skill_sheets/ | ~23 MB | REVIEW | Source reference only? | N/A |
| RIMA/Tools/ | ~25 MB | KEEP | Active tooling. | — |
| RIMA/Assets/ | ~305 MB | KEEP | Real project source. | — |

---

## Section 2 — Execution Order

**Step 1 — Create SCRATCH destination**
```powershell
New-Item -ItemType Directory -Force -Path "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23"
```

**Step 2 — MOVE: twitter_research (464 MB)**
```powershell
Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/twitter_research" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/twitter_research"
```

**Step 3 — MOVE: tweet_research (83 MB)**
```powershell
Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/tweet_research" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/tweet_research"
```

**Step 4 — MOVE: CHATGPTSPRITESHEETS (24 MB)**
```powershell
Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CHATGPTSPRITESHEETS" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/CHATGPTSPRITESHEETS"
```

**Step 5 — MOVE: _reference_packs_raw (15 MB)**
```powershell
Move-Item "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_reference_packs_raw" "F:/Antigravity Projeler/_SCRATCH/RIMA_cleanup_2026-05-23/_reference_packs_raw"
```

**Step 6 — DELETE: boona_frames (20 MB)**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/boona_frames"
```

**Step 7 — DELETE: discarded concept art subfolder**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art/_DISCARDED_codex_v1_20260510T181951Z"
```

**Step 8 — DELETE: Library/ (8.1 GB) — CLOSE UNITY FIRST**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Library"
```

**Step 9 — DELETE: Logs/ (8.7 MB)**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Logs"
```

**Step 10 — DELETE: Temp/**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/Temp" -ErrorAction SilentlyContinue
```

**Step 11 — DELETE: tmp/ (33 MB)**
```powershell
Remove-Item -Recurse -Force "F:/Antigravity Projeler/2d roguelite/RIMA/tmp"
```

**Step 12 — VERIFY**
```powershell
Get-ChildItem "F:/Antigravity Projeler/2d roguelite/RIMA" -Directory | ForEach-Object { $size = (Get-ChildItem $_.FullName -Recurse -ErrorAction SilentlyContinue | Measure-Object -Property Length -Sum).Sum; [PSCustomObject]@{Name=$_.Name; SizeMB=[math]::Round($size/1MB,1)} } | Sort-Object SizeMB -Descending
```

---

## Section 3 — Pre-Checks Before Running

1. **Git status check:**
   ```powershell
   cd "F:/Antigravity Projeler/2d roguelite/RIMA"; git status; git log --oneline -5
   ```
2. **Close Unity Editor** before Step 8 (Library lock).
3. **SCRATCH free space:** `Get-PSDrive F | Select-Object Used, Free`

---

## Section 4 — Post-Cleanup Verification

- Unity reimports Library (~15-20 dk, normal)
- Console clean check
- STAGING active files still present
- SCRATCH integrity

---

## Section 5 — Risks + Rollback

| Risk | Mitigation |
|---|---|
| Library deleted with Unity open → file lock error | Close Unity first |
| Move-Item long path failure on twitter_research | Fallback: `robocopy /E /MOVE` |
| ARCHIVE/STAGING_OLD/ contains only copy of asset used in Assets/ | Cross-check filenames before move |
| REVIEW item accidentally deleted | REVIEW commands NOT in Section 2 by design |

Full rollback: Library regenerable. SCRATCH moves reversible. No truly irreversible action in confirmed phase.

---

## Expected Disk Reclaim

| Phase | Reclaim |
|---|---|
| DELETE Unity-regen | ~8.1 GB |
| DELETE ephemeral | ~63 MB |
| MOVE TO SCRATCH | ~586 MB |
| REVIEW (if approved) | ~800 MB est. |
| **Confirmed min** | **~8.75 GB** |
| **Max with REVIEW** | **~9.55 GB** |

---

## User Decision Needed (3 questions)

1. **ARCHIVE/STAGING_OLD/** (eski Ranger animation PNG'leri) — superseded by current Assets/ sprites, SCRATCH'a taşı mı? Yoksa erişilebilir kalsın mı?
2. **RIMA_AssetParts_v2/v3/** — Assets/'a import edildi mi (STAGING kopyalar redundant)? Yoksa canonical source mu?
3. **PIXELLAB_OUTPUTS/** — SCRATCH'a (üretim history) mı, projede mi kalsın (referans)?
