# CX ANALYSIS TASK — Demo Map Build: Technical Audit + Architecture

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — analysis only, DO NOT write code this round (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
Opus otonom olarak "güzel seamless izometrik demo map" yapacak. Sen TEKNİK analiz yap, Opus karar verecek. **Bu round KOD YAZMA — sadece analiz + öneri raporu.** Çıktını `CODEX_DONE.md`'ye yaz, başlık `## CX ANALYSIS — DEMO MAP`.

## Bağlam (locked, re-litigate etme)
- Perspektif = İZOMETRİK. Floor = seamless tile. Cliff = floor'un ALTINA matematiksel oturacak (derinlik).
- Asset pack olacak: floor / cliff / object kategorileri, portable ScriptableObject, designer palette'i besler.
- Karakter/anim/mob HARİÇ (kullanıcı yapacak). Biz map + demo iskeleti yapıyoruz.
- UNIFIED DESIGNER zaten kuruldu (RIMA/Map Designer, 7 tab, ortak RoomData, 363 test geçti). Bu round onun ÜSTÜNE inşa.

## Mevcut asset envanteri (Opus doğruladı, yeniden üretim YOK)
- Floor flat: `Assets/Sprites/Environment/PixelLabFloorFlat/flat_0-15.png` (16, düz, PPU64)
- Floor iso: `Assets/Sprites/Environment/PixelLabFloor/pl_floor_0-15.png` (16, iso)
- Cliff: `Assets/Sprites/Environment/KitB_Cliff/` (cliff_N/E/S/W/NE/NW/SE/SW + cliff_S_new1-4 + cliff_cyan_glow + corner_fade + edge_ao_rim)
- Decor/obje: `Assets/Sprites/Environment/IsoKit/decor/` (16: banner, brazier_lit/unlit, rift_a/b, rubble_a/b, rune, sarcophagus, seal_circle, slab_crack, bones, blocks, bricks, moss, foot_debris)
- Kit: `Assets/Sprites/Environment/PixelLabKit/` (brazier, pillar_broken, rubble, wall_tower)

## Analiz soruları (her birine net, kanıtlı cevap — dosya:satır referansı ver)

### 1. MENÜ KONSOLİDASYONU (EXEC 1 — ilk yapılacak)
- Tüm LIVE (`_archive~` HARİÇ) `[MenuItem("RIMA/...")]` girişlerini envanterle: dosya yolu + tam menü string + ne yaptığı.
- CURRENT_STATUS worklist#0'daki taşıma planını doğrula: hangileri `RIMA/Legacy/`, hangileri `RIMA/Utilities/`, hangileri üstte kalmalı (Map Designer + Play F5 + Stop F6).
- Her MenuItem string değişikliğinin yan etkisi var mı? (örn. başka kod o menüyü execute_menu_item ile çağırıyor mu, kısayol çakışması, validate fonksiyonu).
- RİSK: taşıma sırasında compile kırılır mı? Hangi dosyalar.

### 2. ASSET PACK MİMARİSİ (EXEC 2)
- Mevcut `RuntimeAssetRegistry` + `AssetPackV3Importer` + tag sistemi nasıl çalışıyor? (dosya:satır)
- Designer palette şu an asset'leri nereden çekiyor? (floor/cliff/object/prop tag).
- "Portable AssetPack ScriptableObject" için EN AZ kod ne? Mevcut registry'yi mi genişletmeli yoksa yeni SO mu? Mevcut sistemle çakışmadan.
- Cliff/floor/object kategorileri palette'e nasıl bağlanır (mevcut DesignerCategory/DesignerCategoryMap'i kullan).

### 3. CLIFF-UNDER-FLOOR MATEMATİĞİ (EXEC 3) — EN KRİTİK
- `RoomCliffSolver.cs` + `RoomDepthStack.cs` + `CliffGenerateAction.cs` şu an cliff'i NASIL yerleştiriyor? (algoritma + dosya:satır)
- Cliff'in floor kenarının ALTINA "seamless" oturması için doğru matematik ne? (iso offset: floor tile altına cliff tile'ı kaç px aşağı + sorting order). KitB_Cliff sprite'larının boyut/pivot'unu kontrol et.
- 8-yön cliff (N/E/S/W + köşeler) hangi floor-edge durumuna karşılık gelir? Bitmask tablosu öner.
- Mevcut solver bunu yapıyor mu, yoksa eksik mi? Eksikse EN AZ ekleme ne.

### 4. SEAMLESS MAP ASSEMBLY (EXEC 4)
- Floor tile'ları seamless döşemek için doğru import ayarı + tilemap/grid setup ne? (flat vs iso hangisi seamless?).
- Demo room'u programatik kurmak için mevcut hangi API var (RoomData + Composer + execute_code)?

### 5. EKSİK ASSET (EXEC 5 — imagegen)
- Gerçekten eksik ne? (kapı/gate, map-fragment, reward pickup). Mevcut kit'te kapı var mı kontrol et.
- imagegen için on-brand spec (iso, charcoal/iron + cyan-sparing, PPU64, transparent).

## Çıktı formatı
`CODEX_DONE.md` → `## CX ANALYSIS — DEMO MAP` başlığı altında, her soru numarasına göre. Kanıt = dosya:satır. Öneri = EN AZ kod. Belirsizlik = "BLOCKED: <soru>" olarak işaretle, Opus karar versin.
