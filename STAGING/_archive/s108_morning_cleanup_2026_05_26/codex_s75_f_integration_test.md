# S75-F — Live Integration Sanity + Final Commit

**Effort:** small
**Prereq:** S75-E merged

---

## GOAL

Tüm S75 phase'leri (A-E) entegre test, kalan rough edge'leri fix, S75 close.

---

## TEST SCENARIOS

### Scenario 1: Map Designer end-to-end
1. Unity'de `RIMA > Tools > Map Designer` aç
2. Auto-Fit ile canvas dolduğunu doğrula
3. Wall paint (3 cell drag), Path paint (5 cell drag) test → Bresenham line interpolation çalışmalı
4. Brush=3 set → 5×5 outline görünür
5. Hover farklı cell'lerde → status bar 3-line + pairing info güncellenir
6. Multi-variant Wang aktif değilse (variantsByKey empty) → legacy tiles görünür (regression yok)

### Scenario 2: Object placement
1. Map Designer'da [Objects] toolbar → ObjectsPanel açılır
2. Folder dropdown "Mobs" → list yüklenir (Knight, Goblin, ... mevcut prefab'lar)
3. Bir mob seç → [Place Mode] toggle
4. Canvas'ta 3 yere tıkla → 3 mob placement add edilir
5. Save map JSON → objects array contains 3 entries
6. Load map JSON → objects re-render
7. Apply to Scene → Hierarchy'de 3 mob GameObject visible

### Scenario 3: Class/Mob asset round-trip
1. Project: Assets/Data/Classes/Warblade.asset Inspector aç
2. idleSprite + weaponSprite assigned (S75-E placeholder)
3. Mob SeamCrawler.asset Inspector aç → 64×64 placeholder sprite assigned, role=Swarm

### Scenario 4: Demo scene playable
1. `Assets/Scenes/Demo/_FazMVP_Demo.unity` open
2. Play
3. Karakter sprite (placeholder Warblade) görünür
4. Mob spawn yoksa: SeamCrawler placeholder placement via Map Designer + Apply to Scene → playtest
5. Console error 0

---

## EXPECTED FIXES

Test sırasında karşılaşılırsa:
- **Compile warnings:** Suppress where appropriate, document remaining
- **Asset loading fail:** Check guid references, AssetDatabase.Refresh
- **Object placement coordinate mismatch:** Verify canvas-px → world conversion formula
- **Map Designer regression (S75-A artifacts):** Document and fix individually

---

## FINAL DELIVERABLES

1. `STAGING/s75_close_report.md`:
   - All S75 commits listed (A through F)
   - Screenshot paths
   - Known issues
   - User next steps (PixelLab gen, manual sprite swap)
2. `CURRENT_STATUS.md` rewrite:
   - S75 close summary
   - S76 handoff (Faz 1 MVP integration)
3. `Assets/Editor/_archive_S73/RoomDesigner.meta` cleanup if still missing (asmdef fix follow-up)

---

## COMMIT MESAJI

```
[S75-F close] Integration test + S75 wrap

- Tested: Map Designer end-to-end, object placement, class/mob asset roundtrip
- Tested: Demo scene playable with placeholder sprites
- Fixed: <any issues found during testing>
- Updated: CURRENT_STATUS, s75_close_report
- S75 phases complete: A (UX deep) + B (multi-variant) + C (object layer) + D (class/mob SO) + E (stub sprites) + F (integration)
```
