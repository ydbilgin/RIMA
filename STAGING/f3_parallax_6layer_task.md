# F3: 6-Katman Parallax Setup (Sang Hendrix tarzı)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
6-katman parallax sistemi — Sang Hendrix Realtime Parallax Map Builder tarzı. ParallaxLayer.cs ZATEN LIVE (`Assets/Scripts/Background/ParallaxLayer.cs`), yeniden yazılmaz. Sadece scene rig + 6 child GameObject + factor tune.

## İş kalemleri
1. **`PlayableArena_Test01.unity` Floor Grid kökünde** veya ayrı `ParallaxRig` GameObject:
   - 6 child layer GameObject:
     - `BG_Void` (parallaxFactor=0.05, sortingOrder=-500)
     - `BG_Far` (parallaxFactor=0.15, sortingOrder=-420)
     - `BG_Mid` (parallaxFactor=0.30, sortingOrder=-350)
     - `BG_Near` (parallaxFactor=0.50, sortingOrder=-300)
     - `Mid_Ground` (parallaxFactor=0.85, sortingOrder=10) [foreground subtle]
     - `Foreground_Front` (parallaxFactor=1.10, sortingOrder=600) [yakın gölgeler]
   - Her GameObject'e `ParallaxLayer.cs` script attach + parallaxFactor set
2. **Placeholder sprite** (procedural, gece halt rule):
   - Her katmana 1024×256 procedural Texture2D (silhouette gradient renkler)
   - Cyan-glow Hades V1 brand uyumu
3. **RoomBackgroundRig** mevcut prefab S110 LATE inactive — DEACTIVATE veya integrate
   - Eğer mevcut prefab reuse edilebilirse: aktive et, factor tune
   - Yoksa yeni rig create

## Dosyalar
- `PlayableArena_Test01.unity` (6 yeni child GameObject + ParallaxLayer attach)
- `Assets/Sprites/Environment/Parallax/Placeholder/*.png` 6 procedural sprite (runtime gen yerine pre-baked)
- VEYA `Assets/Scripts/Environment/ProceduralParallaxBaker.cs` runtime gen ~80 LOC
- Toplam ~170 LOC (placeholder gen + scene wire)

## Verify
- 0 err / 0 warn
- PlayMode aç → camera move → 6 katman farklı hızda kayar (parallax efekti)
- Yakın katmanlar hızlı, uzak katmanlar yavaş
- Brand: Hades Elysium V1 floating island cyan glow korunur

## YASAK
- ParallaxLayer.cs yeniden yazma (LIVE, reuse)
- PixelLab asset gen (gece halt, procedural placeholder yeterli)
- Sang Hendrix tam clone (Faz 2 polish)
- Yeni .cs → `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU

## Code rotation
Sen Sonnet yaz. Reviewer Opus visual coherence F3 PASS sonrası.

Output: `STAGING/F3_PARALLAX_6LAYER_DONE.md`
