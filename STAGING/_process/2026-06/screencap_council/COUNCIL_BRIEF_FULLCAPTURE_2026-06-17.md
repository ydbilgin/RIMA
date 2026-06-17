# COUNCIL — RIMA: HER ekranın eksiksiz envanteri + fonksiyonel verify + asset UI/UX + F2/backquote (2026-06-17)

ACTIVE RULES: (1) think — EKSİKSİZ ol, hiçbir state'i atlama (2) somut (3) demo-scope (19 Haz, solo) (4) belirsizse BELİRSİZ yaz.
NLM gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`.

> **Bağlam:** RIMA = 2D top-down ARPG roguelite (Unity URP 2D). Demo 19 Haz. İlk capture turunda SADECE 13 ekran çekildi (MainMenu/Settings/CharacterSelect/draft/HUD/runmap/director/buildmode/codex/pause). Kullanıcı: "**13 değil, oyundaki HER ŞEYİN ss'ini al, gerçekten her şeyi düşünerek**." Mevcut 13: `STAGING/_process/2026-06/demo_screenshots/`. Bilinen UI sınıfları (graphify, 97 dosya): MainMenu/Settings/CharacterSelect/Chamber/HUD/SkillBar/CharacterHPBar/BossHealthBar/PassiveStatusUI/SkillOffer(draft)/SkillCodex/Pause/RunMapOverlay/DungeonMapUI/MapPanel/ChestUI/ForgeUI/ClassSelectionUI/CharacterSheetUI/RoomMonolog/Tooltip/DeathScreen/DemoCompleteOverlay/DirectorMode/BuildModeController/BuildPlacement/BuildTileBrush/InPlayMapPaint/DemoDebugPanel/HUDEditor.

## SORU A — EKSİKSİZ EKRAN/STATE/MODE ENVANTERİ (asıl iş — "her şeyi düşün")
Oyundaki **TÜM yakalanabilir görsel state'leri** listele — 13'ten ÇOK daha fazla olmalı. Her madde için: **ekran adı · nasıl ulaşılır (sahne/tuş/aksiyon/oda-tipi) · alt-state'leri**. Düşünülmesi gerekenler (ekle/çıkar, EKSİKSİZ ol):
- **Menü akışı:** MainMenu (backdrop'lu+düz), Settings (tüm sekmeler), CharacterSelect (her sınıf hover/seçili), Chamber, sınıf-seçim odası
- **Oda-tipleri (her biri AYRI):** Combat / Elite / Merchant (ChestUI mu ForgeUI mu?) / Chest / Boss / Event — her oda-tipinin combat-içi + clear-sonrası hali
- **Combat alt-state'leri:** wave-spawn anı, mid-combat, düşman telegraph, hit-stop/flash anı, low-HP (kırmızı tint), oda-temizlendi
- **Reward/draft:** opening-kit draft, reward draft, Forge draft, Echo seçimi, kart-hover (tooltip nerede çıkıyor)
- **Run-map/harita:** RunMapOverlay (M), DungeonMapUI, MapPanel — farklı mı, hangisi canlı?
- **Overlay/panel:** SkillCodex (her sınıf sekmesi), Pause, CharacterSheet, PassiveStatus, SkillBar yakın, RoomMonolog
- **Modlar + alt-state'leri:** DirectorMode (spawn/stat/telemetry/prop-light alt-panelleri), BuildMode (PROP sekmesi / TILE sekmesi / asset-seçili / yerleştirme-önizleme / grid), InPlayMapPaint, HUDEditor, DemoDebugPanel
- **Son-durumlar:** DeathScreen, DemoCompleteOverlay, boss-health-bar dolu/boş, victory
- **Geçişler:** RoomTransitionFX, kapı-açılma, portal
→ Organize bir checklist (kategori → state → ulaşım) ver. Hangileri demo-golden-path'te, hangileri kenar-durum.

## SORU B — FONKSİYONEL VERIFY (sadece görsel değil!)
Kullanıcı: "buildmode öyle görünüyor AMA **tile doğru yerleşiyor mu? asset doğru seçiliyor mu?**" Her interaktif state için **fonksiyonel doğrulama** yaklaşımı:
- **BuildMode:** asset seç → seçili-state doğru mu? tile/prop yerleştir → DOĞRU hücreye mi oturuyor (grid-snap)? geri-al? Hangi runtime-assert'ler (execute_code) bunu data-proof eder?
- **DirectorMode:** spawn butonu → düşman GERÇEKTEN spawn oluyor mu? stat slider → değer uygulanıyor mu?
- Diğer interaktif (draft-seç→grant, kapı→geçiş): nasıl data-proof.
→ "Görsel-yakala + fonksiyonel-assert" çift-kontrol reçetesi.

## SORU C — ASSET UI/UX PROFESYONEL TASARIM
Kullanıcı: "**asset'lerin UI/UX'i tam profesyonel olmalı, güzel dizayn olmalı.**" BuildMode asset-paleti + DirectorMode panelleri + draft-kartları + HUD için: profesyonel oyun-editörü/UI tasarım prensipleri (ikon+etiket, grid, hover, seçili-vurgu, gruplandırma, spacing, tipografi). Endüstri referansı (ör. Dead Cells/Hades editör/UI, genel level-editor UX). Demo'da 2 günde yapılabilir "quick-win" vs post-demo.

## SORU D — F2 / " (backquote) ÇALIŞMIYOR
Kullanıcı build mode'a (F2) DA director mode'a (backquote) DA **giremiyor**. Olası kök: full-flow play'de (MainMenu→oyun) bu modlar bootstrap OLMUYOR (sadece dev-direct `_Arena`'da kuruluyor) → tuşlar ölü. VEYA yeni eklenen backquote-guard (IsAnyBlockingUIOpen) blokluyor. Demo'da kullanıcının bunlara ERİŞMESİ şart (canlı sunum). Çözüm: full-flow'da da mı bootstrap edilmeli, yoksa demo dev-direct mı koşulmalı? En temiz yol?

## ÇIKTI
`STAGING/_process/2026-06/screencap_council/RESP_<advisor>.md` (cx/axpro/axflash). A/B/C/D başlıklı. A = organize checklist (EKSİKSİZ). Demo-öncelik işaretle. Türkçe.
