# HUD LAYOUT (EDIT MODE) — KARAR (2026-06-12)

> Kullanıcı isteği: "skill barı istediğim yere çekebileyim, o bilgisayarda öyle kaydolsun" + grid sistemi.
> Kapsam onayı: **Gerçek oyuncu-facing özellik** (Director değil), **tüm widget'lar taşınabilir + scale**, **grid-tabanlı occupancy (bloklama engelli)**.
> Sıralama: DEMO_TOOLS plan Faz B/C ile (Director uGUI altyapısını paylaşır). Faz A önünü KESMEZ.

## Karar özeti
Oyuncu "HUD Düzenle" moduna girer → tüm HUD widget'ları tutamak kazanır, grid'e snap'lenerek taşınır + boyutlanır, üst üste binemez, **PlayerPrefs**'e (o makine/kullanıcı) kaydolur, "Sıfırla" defaultlara döner.

## Tasarım kararları
1. **Anchor + grid-hücre saklama (mutlak piksel DEĞİL).** Ekran kaba grid'e bölünür (24×14 öneri). Her widget = `{anchorCorner, col, row, wCells, hCells}`. Çözünürlük/aspect değişse de doğru durur — PlayerPrefs farklı makine/çözünürlükte bozulmaz.
2. **Occupancy map = bloklama engeli.** Hücreler dolu/boş; widget dolu hücreye düşemez → ghost kırmızıya döner. (Director C1 Spawn ghost+collision motorunu REUSE.)
3. **Widget başına boyut kısıtı.** Her widget min/max hücre footprint (skill bar geniş-kısa, minimap kare, barlar yatay). Scale sadece izinli hücre boyutlarına snap.
4. **Hibrit his:** serbest sürükle → bırakınca en yakın geçerli hücreye snap. Grid overlay yalnız Edit Mode'da görünür.
5. **Reset + preset.** Varsayılana dön + 2-3 hazır şablon (solak / minimal / streamer).

## Taşınabilir widget'lar (onaylı: hepsi + scale)
- Skill bar (asıl istek) · Minimap · HP/kaynak/XP barları · (varsa) buff bar, minimap, para/sayaç.
- Hepsi taşınır + scale. Grid occupancy hepsine uygulanır.

## Teknik temel (reuse-öncelikli)
- `HUDController.cs` ZATEN VAR → üstüne `HudLayoutManager` (grid + occupancy + persist) kurulur, sıfırdan değil.
- Persist = **PlayerPrefs** (JSON serialize edilmiş layout map). Yeni altyapı yok.
- `Loc.cs` hazır → "HUD Düzenle / Sıfırla / Şablon" metinleri `Loc.T()` key'leriyle.
- Director C1 Spawn'ın ghost+grid-snap+collision kodu paylaşılır.

## Sıralama (DEMO_TOOLS plan)
- **Faz B'den sonra** (Director uGUI Canvas + chrome + grid altyapısı hazır olunca) **Faz C/D arası** HUD Edit Mode eklenir.
- Faz A (stat çekirdeği) ÖNÜNÜ KESMEZ. Bağımsız.
- Görsel-ağır → otonom build edilir ama görsel doğrulama **sabah toplu kontrole** bırakılır (blind-commit yok).

## ERTELENEN
- Preset şablon içerikleri (solak/minimal) — temel grid+persist çalışınca.
- Controller/gamepad ile HUD edit — şimdilik fare.
