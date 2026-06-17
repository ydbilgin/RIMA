# COUNCIL BRIEF — Playtest polish batch (9 konu) — 2026-06-17

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## ⛔ HARD KISITLAR (ihlal = RED)
- **SALT-OKUNUR (READ-ONLY). KOD/GIT/UNITY'YE DOKUNMA.** Hiçbir dosyayı düzenleme, `git add/commit` YAPMA, Unity'de değişiklik yapma. Sadece OKU + ANALİZ + ÖNER.
- **Tam Türkçe karakter ZORUNLU** (ç ş ğ ü ö ı İ) — çıktı dokümanı Türkçe.
- **GRAPHIFY query-first:** cross-file/mimari soruda önce `graphify query` (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`), bulk-read'den ~71× ucuz.
- Çıktıyı **dosyaya** yaz: `STAGING/_process/2026-06/director_hud_council/AX_PRO_VERDICT.md` (Flash isen `AX_FLASH_VERDICT.md`). Dönüşte sadece ≤10 satır özet + dosya yolu.

## Bağlam
RIMA = top-down ARPG (Unity URP 2D, Pixel Perfect, 8-yön sprite, PPU 64). **Demo ~19 Haziran (2 gün)**. Sınıflar: **Warblade** (silahlı greatsword bake — demo ana sınıfı) + **Elementalist** (mage). Kullanıcı canlı playtest'te 9 görsel/mekanik sorun buldu. Görseller: bu klasörde `01..06_*.png` (AÇ VE İNCELE — özellikle ax_pro vision).

Senin işin: her sorun için (a) **kök neden** (benim ön-teşhisimi onayla/çürüt), (b) **en cerrahi fix kaldıracı** (DATA mı KOD mu — hangi dosya/alan), (c) **risk + 2-gün demo'da öncelik** (P0 ship / P1 / post-demo / BLOCKED). Ayrıca HUD ve Director Mode için **somut yeniden-tasarım** öner.

---

## SORUN 1 — Prop Y-sıralama (görsel 01, 02)
**Belirti:** Yere konan proplar (yarık, Vitality Crystal, iskelet decal) karakterin ÜSTÜNDE çiziliyor; karakterin altında olmalı. Dik proplar (sandık, fıçı, sütun) Y-pozisyona göre sıralanmalı (karakter yukarıdaysa prop önde, aşağıdaysa arkada).
**İlgili kod:** `Core/IsoSorter.cs` (oyuncu/düşman Y-sort), `MapDesigner/Props/Runtime/PropSorterRuntime.cs` (prop Y-sort, **"Props" sorting layer**), `MapDesigner/Props/PropDefinitionSO.cs` (sortingMode/sortingLayerOverride/sortingOrder alanları).
**Ön-teşhisim:** Sorting **LAYER** karşılaştırması `sortingOrder`'dan ÖNCE gelir. Proplar "Props" layer'ında, oyuncu kendi layer'ında (IsoSorter). Eğer "Props" layer'ı Project Settings → Sorting Layers'da oyuncunun layer'ının ÜSTÜNDEyse, Y-math ne olursa olsun TÜM proplar karakterin üstünde çizilir. → **Çözüm yönü:** Yer-decal'leri (yarık/kristal/iskelet) entity'lerin ALTINDA bir layer'a (örn. Floor/Decal) + `FixedOrder`; dik proplar oyuncuyla AYNI sorting layer'da + `YPosition`. **DOĞRULA:** TagManager sorting layer sırası + oyuncunun hangi layer'da olduğu.

## SORUN 2 — Prop collider/hitbox (görsel 01)
**Belirti:** Sandık/fıçı katı olmalı (içinden geçilememeli); yer-decal'leri yürünebilir.
**İlgili kod:** `MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs` (sadece `propDef.blocksWalkable` ise BoxCollider2D ekler; footprint `propDef.footprintSize`), `PropDefinitionSO.cs` (DİKKAT: İKİ collider yolu var — `blocksWalkable`+footprintSize VE Phase B-2 `blocksMovement`+`colliderShape`+`colliderFootprintRatio`+`colliderLayer`. Hangisi otorite? Çakışıyor mu?).
**Soru:** Sandık/fıçı için doğru alan kombinasyonu ne? `footprintSize` iso-grid hücresi mi dünya birimi mi? Collider offset `box.size*0.5` doğru mu (pivot taban-sol köşede)?

## SORUN 3+5 — Silah mount (sağ el + facing'e göre ön/arka) (görsel 03, 01)
**Belirti:** Silah karakterin ÖNÜNDE/yanında saçma duruyor; sağ ele oturmalı, facing'e göre body'nin önüne/arkasına geçmeli.
**İlgili kod:** `Systems/Combat/HandAnchorAttach.cs` (handAnchor'a mount, `entry.anchorOffset`; `UpdateWeaponSortOrder` zaten N/NE/NW'de behindBody yapıyor), `WeaponDatabaseSO` (anchorOffset/gripOffset), `Systems/Combat/OrientationSync.cs` (per-dir flip/offset/rotation).
**Ön-teşhisim:** Sort-flip mantığı VAR. Sorun POZİSYON: `handAnchor` local pos + WeaponDatabaseSO `anchorOffset` yanlış → silah elin dışında. Lever = DATA (handAnchor transform + anchorOffset per-dir). Ayrıca yerdeki **dropped sword item** (görsel 01 sarı/turkuaz item drop) collider'ı ayrı — `RewardPickup`/item pickup hitbox. Hangisi "saçma hitbox"? Netleştir.

## SORUN 4 — Yer yarığı: yürünebilir ama orta blok + karakter üstte (görsel 01)
SORUN 1 + 2'nin özel hali. Yarık: footprint küçük merkez-collider (ortasından geçilemez) + decal layer (karakter üstte). Öneri?

## SORUN 6 — HUD yeniden-tasarım (görsel 01 sol-üst)
**Belirti:** Can/enerji barı şu an sol-ÜSTTE, segmentli yeşil bar; kullanıcı SOL-ALTA + daha akıllı ARPG tasarımı istiyor (ChatGPT örnekleri vermiş).
**İlgili kod:** `UI/HUDController.cs`, `UI/HUDElement.cs`, `UI/HUDEditorManager.cs`.
**İstenen:** Somut layout spec — sol-alt yerleşim, can+enerji+Echo+ODA sayacı düzeni, ARPG referansları (Hades/Diablo/CoM). Anchor/boyut/renk önerisi.

## SORUN 7 — Boss can barı düşmüyor (görsel 04)
**Belirti:** "THE PENITENT SOVEREIGN" barı dolu kalıyor, hasara göre azalmıyor. AMA phaseText "PHASE II" gösteriyor.
**İlgili kod:** `UI/BossHealthBar.cs` (`UpdateFill`: `hpFill.fillAmount = ratio`; hpFill = renkli Image, **type=Filled, fillMethod=Horizontal, ama SPRITE ATANMAMIŞ**), `Enemies/BossAI_PenitentSovereign.cs:231` + `Enemies/Boss/PenitentSovereign.cs:163` (ikisi de UpdateHP çağırıyor — hangisi canlı?).
**Ön-teşhisim (güçlü):** `hpFill`'e sprite atanmamış. Unity'de `Image.type=Filled` + sprite=null → fillAmount geometrisi çalışmaz, bar HEP dolu render eder. phaseText doğru güncelleniyor (sadece metin) → "PHASE II + dolu bar" çelişkisini bu açıklar. **Fix:** hpFill'e built-in UISprite ata VEYA fillAmount yerine RectTransform width/scale tabanlı doldurma. DOĞRULA.

## SORUN 8 — Director Mode: güzelleştir + scroll + skill netliği (görsel 05) ⭐ council odağı
**Belirti:** (a) Skill listesi AŞAĞI KAYMIYOR → son skill "Gravity Cleave" kesik. (b) Daha güzel olmalı. (c) Q/E/R/F ATA + LMB/RMB BASIC butonları + skill'lerin ne yaptığı net değil.
**İlgili kod:** `UI/DirectorMode.cs` (skill paneli, sekmeler: Spawn/Class&Skill/Stats/Build/Map/Telemetry; BASLAT/STEP/RESET; Inspector).
**İstenen:** Somut UI redesign — skill listesine ScrollRect, görsel hiyerarşi/spacing/renk iyileştirme, skill kartlarının okunabilirliği, atama akışı netliği. Vision ile mevcut ekranı eleştir + somut öneri (Hades dev-tool / Diablo cheat-panel estetiği).

## SORUN 9 — Elementalist VFX + 8-yön (görsel 06)
**Belirti:** (a) Fireball'un VFX'i güzel, ama Elementalist'in DİĞER skilleri + basic attack VFX göstermiyor. (b) Elementalist'te 8 yön sprite YOK.
**İlgili kod:** `Skills/Elementalist/*.cs` (Fireball/GlacialSpike/Meteor/FrozenOrb), `VFX/SkillVfx.cs`, `Skills/SkillRuntime.cs`, oyuncu animator/sprite 8-dir setup.
**Ön-teşhisim + BLOCKER:** VFX = skill başına prefab/SkillVfx wiring eksik (sadece Fireball bağlı). 8-yön = Elementalist sprite seti eksik → **PixelLab balance=0** (yeni gen BLOKED). **TRİYAJ SORUSU:** Demo tek-sınıf Warblade mı? Öyleyse Elementalist 8-dir post-demo; sadece VFX-wiring + mevcut-yön reuse demo'ya girer mi?

---

## ⛔ DOKUNMA listesi (locked — bunlara fix önerme): GATE / Boss-presentation akışı / HUD-reward-bleed / Build-placement çekirdeği / Director-skin temel mimari / weaponless-anim state'leri / branching-seed.
(HUD görsel-redesign ve Director güzelleştirme İSTENİYOR — ama mimariyi kırmadan, cerrahi.)

## Çıktı formatı (VERDICT dosyasına)
1. **Triyaj tablosu:** 9 sorun × [kök neden onay/red · fix kaldıracı (dosya/alan) · DATA/KOD · efor (S/M/L) · öncelik (P0/P1/post/BLOCKED)].
2. **HUD redesign:** somut layout (anchor/boyut/öğeler/renk).
3. **Director redesign:** somut UI iyileştirme + scroll fix yaklaşımı.
4. **2-gün demo sıralaması:** ne ship, ne CUT, ne blocked.
5. Belirsizlik/varsayım → açıkça FLAG.
