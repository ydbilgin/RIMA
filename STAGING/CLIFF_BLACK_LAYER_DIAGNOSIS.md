# Cliff SİMSİYAH + Layer Teşhisi (triple-AI, S114 2026-05-28)

Kullanıcı: floating-platform mantığı; cliff'ler simsiyah görünüyor, doğru layer'da olmalı. Teşhis empirik (`execute_code`, PlayableArena_Test01 canlı sahne). **FIX uygulanmadı — kullanıcı onayı sonrası ayrı task.**

## KÖK NEDEN (kesin)
Cliff'ler `Decor_Cliff` sorting layer + `Sprite-Lit-Default` material kullanıyor AMA sahnedeki **hiçbir Light2D'nin Target Sorting Layers listesinde `Decor_Cliff` YOK** → cliff 0 ışık alıyor → Lit shader siyah render ediyor. Sprite null DEĞİL (doluyor, sadece ışıksız).

**Kanıt:**
- `CliffTilemap` → `Decor_Cliff`/-1/`Sprite-Lit-Default`. `DecorCliffTilemap` → `Decor_Cliff`/50. `CliffDropShadowTilemap` → `Decor_Cliff`/-20. Floor → `Floor`/0 (aydınlık).
- 17 Light2D tarandı; target listeleri `...BackwallLandmark, Characters, Props` ile bitiyor. **`Decor_Cliff`(12) + `Decor_Floor`(13) hiçbirinde yok.** Aktif ışık: 4 (Global 0.55 + Player/Gate/Fragment auto). Decor_Cliff hedefleyen aktif ışık = **0**.
- `DirectionalCliffTile_Hades` sprite'ları dolu (spritesS=5, generatedCount=311).

**Regresyon nedeni:** `Decor_Cliff`/`Decor_Floor` layer'ları D2'de (2026-05-27) eklendi; tüm Light2D target listeleri ondan ÖNCE `Props`'a kadar yapılandırılmıştı. URP'de target liste explicit — yeni layer otomatik katılmaz. Floor `Floor` layer'ında kaldığı için aydınlık; cliff yeni layer'a taşınınca karardı.

## DOĞRU LAYER KARARI: değişiklik GEREKMEZ
6-layer arch'a (D2 LOCK) uyuyor: L2 base `Decor_Cliff`/-1 ✓, L3 face `Decor_Cliff`/50 ✓, shadow `Decor_Cliff`/-20 ✓. **Sorun layer ataması değil, o layer'a IŞIK gitmemesi.** Layer'lara dokunma.

## FIX PLANI (kullanıcı onayı sonrası, ayrı Sonnet task)
**P0 — siyahlıktan çıkar (~2 dk):** İlgili Light2D'lerin `m_ApplyToSortingLayers`'ına `Decor_Cliff`(12) + `Decor_Floor`(13) ekle. En kritik `Global Light 2D` (ambient fill). (Seçenek B = Unlit material → hızlı ama cyan-rim brand'i öldürür, reddedildi.)
**P1 — floating-platform LOOK (brand):** Sahnede zaten VAR ama **inactive**: `RimLight_West/East/South_Cyan` (#00FFCC, 0.8) + `Brazier_*_WarmLight` (amber). Aktive et + target'larına `Decor_Cliff` ekle → cliff kenarı cyan-rim + amber fill = "ışıklı yüzen ada kenarı" (düz siyah silüet DEĞİL, brand doğru).
**P2 — ikincil runtime bug (lighting'den bağımsız):** `DirectionalCliffTile.GetTileData` yön çözümü `#if UNITY_EDITOR` içinde (satır ~37-78) → Play/build'de hep güney yüz. `#if` dışına taşı. Sonnet write + Codex review.

**Doğrulama:** fix sonrası `execute_code` "active lights targeting Decor_Cliff" > 0 + Play mode görsel.

Dosyalar: `Assets/Scenes/Test/PlayableArena_Test01.unity` (Light2D+CliffTilemap), `Assets/Scripts/Environment/DirectionalCliffTile.cs` (P2), `ProjectSettings/TagManager.asset` (Decor_Cliff=12/Decor_Floor=13).
