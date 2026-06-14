# RIMA Cliff Floating Feel — Codex Codebase + Scene Reality Check

ACTIVE RULES: WRITE full response to file `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CLIFF_FLOATING_FEEL_research_codex.md`. Effort xhigh. Surgical (sadece istenen dosyalar). ~600-1000 satir output. Inline ozet sondan 10 satir.

## Amac
RIMA codebase'inde cliff yerlestirme algoritmasi, cliff sprite render geometry, mevcut shadow/parallax infrastruktur ve PlayableArena_Test01.unity sahnesinde 283 cliff tile dagilimi gercegini soyle. 4 cozum opsiyonu icin LOC tahminleri ver.

## Okumacaklar (sadece bunlar)
1. `Assets/Scripts/Environment/CliffAutoPlacer.cs` (mevcut algorithm)
2. `Assets/Scripts/Environment/DirectionalCliffTile.cs` (sprite render)
3. `Assets/Scripts/Environment/CliffPlacementRules.cs` (rules SO)
4. `Assets/Scripts/Environment/CliffDynamicFade.cs` (mevcut fade infrastruktur)
5. `Assets/Scripts/Environment/GroundBlobShadow.cs` (mevcut shadow infrastruktur)
6. `Assets/Scenes/PlayableArena_Test01.unity` (cliff cell dagilim — Tilemap section'a bak)
7. `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` (inspector overhaul scope icin)
8. Find "RoomBackgroundRig" scene'de — BG_Far children active mi, inactive mi
9. `Assets/Scripts/Environment/Modular/` klasoru genel scan (parallax veya BG ile ilgili dosya var mi?)

## Output sections

### Bolum 1 — Algorithm satir satir audit
CliffAutoPlacer.cs `CollectCliffCells()` (line 212-244) ve `Regenerate()` (line 172-205). Her kritik bolge icin:
- Ne yapiyor (1 cumle)
- Cluster-aware mi? (Hayir bekleniyor — kanitla)
- Filter eklenebilecek noktalar (HashSet post-process, BFS connected component vs)

### Bolum 2 — Floor tilemap izole cluster sayim (PlayableArena_Test01.unity)
PlayableArena_Test01.unity sahnesinden floor tilemap cell list'i cikar. Connected component (4-yon iso komsu: S/N/E/W) ile BFS yap. Asagidaki sayilari ver:
- 1-cell ada (single floor cell, hicbir floor komsusu yok)
- 2-cell ada
- 3-cell ada
- 4-cell ada
- En buyuk main cluster cell count
- Toplam izole (1-3 cell) ada cell sayisi
- Toplam floor cell

Cliff cell list cikar (283 bekleniyor — kanitla):
- Kac cliff cell ana cluster'a komsu (S/SE/SW komsusu ana cluster floor cell)
- Kac cliff cell izole cluster'a komsu
- "Havada gozuken" cliff aday sayisi (cluster size < 4 olanlar)

NOT: Eger scene dosyasi YAML parse ile zor okunuyorsa UnityMCP execute_code ile runtime'da Tilemap.GetTilesBlock + connected component algorithm calistir. Sahne yuklu degilse manage_scene action=load ile yukle once.

### Bolum 3 — Cliff sprite render geometry (DirectionalCliffTile)
- 8-yon sprite branching nasil (lines 56-78)
- transformOffset.y default deger SO instance'larinda kac? (Assets klasorunde DirectionalCliffTile_*.asset ara)
- spriteScale degeri
- Sprite PNG boyut (Art/Tiles/Cliffs altinda) — kac aspect? top-pivot mu yoksa center-pivot mu?

### Bolum 4 — Mevcut shadow + parallax infrastruktur
- GroundBlobShadow.cs ne yapiyor (player altinda mi sadece?)
- CliffDynamicFade.cs ne yapiyor (sahne wire nerede?)
- RoomBackgroundRig sahnede var mi, BG_Far child active mi? (find_gameobjects ile)
- BG_Far prefab Assets'te var mi? (Asset Database search)
- URP 2D Light setup (ambient warm + cyan rim?) hangi component, parametreler

### Bolum 5 — Cozum opsiyon LOC + risk
Asagidaki opsiyonlar icin: LOC delta, risk (low/med/high), test surface (auto-test/manual), Faz 1 demo timing impact:

| Opsiyon | LOC | Risk | Test | Demo Impact |
|---|---|---|---|---|
| A. Cluster size filter (BFS + min N cell) | ~30-50 | low | T1 oncesi BFS unit test | 0 gun |
| B. Dilate/erode morphology pass | ~60-100 | med | visual playtest | 0.5 gun |
| C. Drop shadow sprite layer (yeni tilemap + sprite) | ~80-120 | med | visual playtest | 1 gun |
| D. BG_Far parallax aktivasyon + tune | ~40-80 + scene wire | low | visual playtest | 0.5 gun |
| E. transformOffset.y tune (sprite sarkma derinligi) | ~5 | XS | visual playtest | 0 gun |
| F. Hibrit (A+C+D) | ~150-250 | med | visual + auto-test | 1.5 gun |

LOC tahminleri SPEKULASYON degil — gercek dosyalardan implementation skeleton cikararak hesapla.

### Bolum 6 — Implementation skeleton (sadece A+E icin kisa C# pseudo-code, 20 satir max her biri)
- A: CollectCliffCells sonrasi BFS connected component, min N filter
- E: DirectionalCliffTile_*.asset icindeki transformOffset.y onerilen yeni deger

YAZMA — sadece pseudo-code ornek.

### Bolum 7 — Reality check soru
3-5 sorulu: "RIMA codebase'inde fark ettim ki..." formatinda. Orneklik: "RoomBackgroundRig var ama BG_Far inactive — aktive icin shader gerekli mi yoksa sprite renderer mi?"

## Format
- Inline + STAGING/CLIFF_FLOATING_FEEL_research_codex.md (her ikisi)
- ASCII body (Turkish chars OK in section names)
- Tablolar markdown
- Effort xhigh, timeout default
