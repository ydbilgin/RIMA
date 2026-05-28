# Task: Asset Cleanup Faz 1+2 — Archive Move

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Audit'te identified zero-risk + low-risk klasörleri `Assets/_Archive_2026-05-23/`'e taşımak. ~1,114 dosya. .meta dosyaları da birlikte taşınacak (GUID continuity için).

## YAPILACAK (sırayla)

### Phase 1 — Kenney IsoMiniDungeon (zero risk)
**Kaynak:** `Assets/Art/_TempReferencePacks/Kenney_IsoMiniDungeon/`
**Hedef:** `Assets/_Archive_2026-05-23/Kenney_IsoMiniDungeon/`
**Komut:**
```bash
mkdir -p "Assets/_Archive_2026-05-23/"
git mv "Assets/Art/_TempReferencePacks/Kenney_IsoMiniDungeon" "Assets/_Archive_2026-05-23/Kenney_IsoMiniDungeon"
```

### Phase 2A — F1 Generated Tiles (PNG + .asset birlikte)
**Kaynak:** `Assets/Art/Tiles/F1/Generated/`
**Hedef:** `Assets/_Archive_2026-05-23/Tiles_F1_Wang16_Generated/`
```bash
git mv "Assets/Art/Tiles/F1/Generated" "Assets/_Archive_2026-05-23/Tiles_F1_Wang16_Generated"
```

### Phase 2B — F1 Tilesets (spritesheets)
**Kaynak:** `Assets/Art/Tiles/F1/Tilesets/`
**Hedef:** `Assets/_Archive_2026-05-23/Tiles_F1_Tilesets/`
```bash
git mv "Assets/Art/Tiles/F1/Tilesets" "Assets/_Archive_2026-05-23/Tiles_F1_Tilesets"
```

### Phase 2C — Keep Tiles
**Kaynak:** `Assets/Art/Tiles/Keep/`
**Hedef:** `Assets/_Archive_2026-05-23/Tiles_Keep/`
```bash
git mv "Assets/Art/Tiles/Keep" "Assets/_Archive_2026-05-23/Tiles_Keep"
```

## Doğrulama

Taşıma sonrası kontrol et:
1. `Assets/_Archive_2026-05-23/` mevcut ve 4 alt klasör var
2. Original konumlar artık yok (Kenney_IsoMiniDungeon, F1/Generated, F1/Tilesets, Keep)
3. `.meta` dosyaları da taşındı (her .png yanında karşılığı olmalı)
4. Git status: rename'ler tespit etmiş olmalı

## ⚠ DİKKAT
- **Karakter klasörüne dokunma:** `Assets/Art/Characters/` HARD PRESERVE
- **Resources/ klasörüne dokunma:** runtime path-loaded, kritik
- **modular_kit_v1/ klasörüne dokunma:** active production
- **v2 wall files dokunma:** active registry
- **AssetParts_v3, v4, v5 dokunma:** Faz 3'te ele alınacak

## Output → CODEX_DONE.md
- Taşınan klasör listesi
- Dosya count'u (her klasörde kaç dosya taşındı)
- Hata varsa hangi step failed
- Git status snapshot

## Sonra (manuel)
- Unity'yi aç (kullanıcı yapacak)
- Console kontrolü — missing reference uyarısı var mı?
- Yoksa PASS → ileri Faz 3'e geç
- Varsa FAIL → hangi prefab/scene, revert et
