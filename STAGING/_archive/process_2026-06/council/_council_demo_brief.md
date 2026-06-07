# COUNCIL BRIEF — RIMA Playable Demo Mimarisi + Sıralama

## Amaç
RIMA için oynanabilir bir DEMO döngüsü inşa edeceğiz. User vizyonu (aynen):
> Büyük odalar + doğru background. Wave-wave gelen moblar (şimdilik placeholder). Oda temizlenince ödül çıksın → skill seçme ekranı → seçtiğim skill'i istediğim SLOT'a sürükleyebileyim. Sonra odanın durumuna göre (her run farklı path) KAPILAR çıksın; her kapı mantıksal olarak farklı tür (elite/shop/combat/boss). O kapıdan girince ilerleyeyim. AYRICA cliff'lerin ALTINDA ama background'un ÜSTÜNDE, yine de UZAKTA — o run'da gerçekten kaç kapı varsa o kadar, gidilecek odaların PREVIEW ADALARI aşağıda görünsün.

## Mevcut sistemler (recon DONE — bunları REUSE et, yeniden yazma)
- **Wave:** `EncounterController` + `EncounterWaveSO` + `ThreatBudget` + `EncounterBankSO`. 2-dalga, `nextWaveKillFraction` ile tetik. Düşman görseli = `EnemyPlaceholder` (sprite yoksa 48x48 renkli kare). HAZIR — odaya wave/bank wire + enemySpawnSockets gerek. Clear → `OnRoomCleared` event.
- **Reward→Draft:** `RewardPickup` → `DraftManager.ShowDraft` → `SkillOfferUI` (3-kart slide-in + juice). Reward türleri: Skill / Gold / Heal / CrossClassEcho. Seçim → otomatik slot-assign, dolu ise replace-UI. ÇALIŞIYOR + runtime-doğrulandı.
- **Skill loadout:** Sınıf controller'ında `SkillBase[6]` (4 primary 0-3 + 2 secondary 4-5). Input Q/E/R/F/Z/X (`KeyBindManager`). `SkillBarUI` = 6 hex slot GÖSTERGE (cooldown radial). **DRAG-DROP YOK** — draft otomatik sıralı slot doldurur, dolunca replace-UI.
- **Map flow:** `MapFlowManager.GoToNextMap` RASTGELE next-map (anti-immediate-repeat), oda = AYRI SAHNE (`_IsoGame_MapXX`, 6 adet, `MapList_Act1.asset`). `RunStats.AdvanceRoom`. Door: `DoorTrigger`→`GateBehavior`→MapFlowManager.
- **Typed doors:** `RoomType` enum (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor). `GateBehavior` room-type sprite VAR (manuel inspector). `RoomBankSO.Pick(RoomType,seed)` typed pool VAR. AMA next-room seçiminde KULLANILMIYOR (random).
- **Preview islands:** SADECE SPEC = `STAGING/PORTAL_PREVIEW_SYSTEM_SPEC_S6.md`. Blocker = "RoomLoader graph-aware olmalı". `Portal.cs` placeholder (OnEntered wire değil), `FanLayoutSolver` (1/2/3 portal yerleşim) + `RoomTypeData.PickPortalCount` VAR ama wire DEĞİL. Orb-travel YOK.
- **Background:** `ParallaxLayer` + `BackgroundLayerData` framework VAR (L0 void..L4 fog parallax katsayıları). Void art YOK, sahnede author DEĞİL. `RoomTemplateSO.backgroundLayers` alanı var.
- **IsoRoomBuilder (YENİ, bu session):** `RoomTemplateSO` → iso floor (walkable∪prop-mask) + yönlü cliff + Composite boundary + props + gate görselleri. `_Arena` sahnesinde Combat_Small_01 ile ÇALIŞIYOR (doğrulandı). Library: `Assets/Data/Rooms/Library/{Combat_Small/Medium/Large, Elite_01, Boss_Intro_01, Treasure_01, Shrine_01, Spawn_01, Corridor_*}`. AMA canlı oyun hâlâ ESKİ `_IsoGame_MapXX` sahnelerini kullanıyor.

## KARAR VERİLECEK MİMARİ FORK
- **Path A (reuse scenes):** Demo'yu çalışan `_IsoGame_MapXX` + MapFlowManager loop'u üzerine kur (wave/bg/typed-door/preview ekle). HIZLI ama oda=hand-authored sahne (data-driven değil); preview-adaları için her sahnenin thumbnail'ini bake etmek gerek.
- **Path B (IsoRoomBuilder + RoomRunDirector, tek _Arena data-swap):** Demo'yu yeni data-driven IsoRoomBuilder üzerine kur. Tek `_Arena` sahnesi, oda geçişi = RoomTemplateSO swap + rebuild (sahne-load yok). Preview-adaları DOĞAL (next-room RoomTemplateSO'sunu küçük ölçekte IsoRoomBuilder ile çiz → void'de uzakta). AMA RoomRunDirector + reward/wave/door entegrasyonunu _Arena'ya taşımak GEREK (daha büyük build).

## SUB-QUESTIONS (her advisor kendi lensinden yanıtla, PASS/FAIL + somut)
1. **SCOPE:** Tam vizyonu konveyen MİNİMAL vertical-slice demo nedir? Ne IN, ne DEFER? (wave / reward-draft / drag-slot / typed-door / preview-island / bg). Demo kaç oda olmalı (ör: Start→2-3 typed-door→Combat/Elite→Boss)?
2. **FORK A vs B:** Demo için hangisi? Özellikle preview-adaları next-room DATA'sı istiyor — bu B'yi mi zorunlu kılıyor, yoksa A'da thumbnail-bake yeterli mi? Risk/süre dengesi.
3. **PREVIEW ISLANDS:** "Cliff altında, bg üstünde, uzakta, run'daki kapı sayısınca" en basit DOĞRU implementasyon? (küçük-ölçek IsoRoomBuilder mini-ada / baked thumbnail / floating orb). Sorting (Decor_Cliff/Decor_Floor/BackwallLandmark layer'ları var) + parallax-uzaklık nasıl?
4. **DRAG-TO-SLOT:** Demo için ŞART mı yoksa mevcut auto-assign+replace yeterli mi? Şartsa SkillBarUI üzerine en yalın drag-drop ne?
5. **TYPED PROCEDURAL PATH:** Lightweight RoomRunDirector (lineer typed-sıra + her clear'da N-kapı branch, RoomBankSO.Pick) mi, yoksa tam StS-DAG mı? Demo-doğru scope?
6. **SEQUENCE:** Seçilen scope için SIRALI build adımları + routing (cx=kod / Unity=Opus / asset=cx-imagegen). Hangi adım hangi ajan.

## Asset üretim notu
Background + reward-çeşit görselleri `cx imagegen` skill ile üretilecek (on-brand: slate #3A3D42 + cyan #00FFCC emissive + warm-orange #E89020 + void-mor #3A1A4A). Char ölçek=64px @ PPU64.
