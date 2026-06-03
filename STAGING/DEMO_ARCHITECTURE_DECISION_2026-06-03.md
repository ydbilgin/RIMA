# RIMA PLAYABLE DEMO — MİMARİ KARARI (Council sentezi, 2026-06-03)

Council: cx (feasibility) + ax 3.1 Pro (deep arch) + ax 3.5 Flash (lean) → Opus sentez.
Advisor dosyaları: `STAGING/_council_{cx,q_31pro,q_35flash}_demo.md` + brief `_council_demo_brief.md`.

## KARAR: **Path B-lite** (data-driven IsoRoomBuilder + lightweight RoomRunDirector, tek `_Arena`)

### Neden (fork çözümü)
- 3.1 Pro "Path B forced" (preview-adaları next-room DATA'sı ister → IsoRoomBuilder doğal engine). 3.5 Flash "Path A, çalışan loop'u bozma, baked thumbnail". cx **B-lite** ile ortayı buldu: IsoRoomBuilder ZATEN çalışıyor + en yüksek reuse; previews gerçek-geometri data-driven olunca thumbnail-yönetimi derdi yok; ama *lite* (DAG yok, orb yok, drag-slot yok) → 3.5 Flash'ın rewrite-riskini küçültür.
- **Üç advisor oybirliği:** preview-adaları = kullanıcının ayırt edici isteği, KORU. drag-to-slot = DEFER. Full StS-DAG = gereksiz, lightweight typed-route yeter.

### Demo SCOPE (vertical slice)
- **Rota:** Start(Combat) → Choice(2-3 typed kapı) → Combat/Elite → Choice → Boss. ~4-5 oda. Tek `_Arena` sahnesi, oda geçişi = RoomTemplateSO swap + IsoRoomBuilder.Build (sahne-load YOK).
- **IN:** büyük odalar (Combat_Large/Medium templates), wave mob (EncounterController, placeholder kare), reward→draft (mevcut, auto-assign), typed kapılar (RoomBankSO.Pick + GateBehavior typed-sprite), **preview-adaları** (cliff-altı/bg-üstü/uzak, gerçek-geometri mini IsoRoomBuilder), statik karanlık void bg.
- **DEFER (demo SONRASI):** drag-to-slot (auto-assign yeter; gerekirse click-to-assign), orb-travel (şimdilik fade), full StS-DAG, full parallax bg, painter tool, iso-collider refine, gate gameplay lock state machine.

### Preview-adaları (CORE — net spec)
- `IsoRoomBuilder`'a **`BuildPreview(RoomTemplateSO, parent, scale~0.5)`** modu: SADECE floor+cliff+props renderer; collider/enemy/spawner/gate YOK.
- ExitChoice {index, targetTemplate, RoomType, direction} → HEM portal HEM preview-ada aynı kaynaktan (drift yok).
- Yerleşim: `_Arena` void'inde cliff siluetinin ALTINDA, bg ÜSTÜNDE, geniş gap; ayrı sorting (Decor_Floor/Decor_Cliff veya yeni Preview layer, bg ile gameplay arası). Rest'te karanlık (alpha<25% + tint), portal-hover'da matching ada tint→normal.
- Uzaklık = gerçek parallax DEĞİL: scale 0.45-0.65 + brightness düşür + statik (az kod, "uzak" hissi).

### Typed procedural path (lightweight, DAG değil)
- `RoomRunDirector`: route-slot tablosu (Start/Combat/Choice/Reward/Boss). Her clear'da typed tablodan 1-3 ExitChoice üret → `RoomBankSO.Pick(roomType, seed)` ile target RoomTemplateSO çöz. Portal+preview sayısı = ExitChoice sayısı (RoomTypeData.PickPortalCount AUTHORITY değil, sadece sayı-policy fallback).
- Seçim → RoomRunDirector `_Arena`'yı seçilen template ile rebuild + route-index++.

### EN BÜYÜK RİSK (cx) — yönetilecek
Oda-lifecycle'ı (teardown/teleport/clear/fragment/draft-wait/gate-unlock/boss-complete) `RoomLoader`'dan `_Arena`/`RoomRunDirector`'a taşırken **static-event leak / stale-gate / duplicate-reward / softlock**. → Temiz event unsubscribe + state reset + tek reward/draft tetik. rima-qc son adımda bunu denetler.

## BUILD SEQUENCE (cx kod / Opus Unity / cx-imagegen asset)
1. **cx:** `RoomRunDirector` skeleton in `_Arena` — seed, route-index, current-template, choice-list, `IsoRoomBuilder.Build(template)` + player teleport (markers'tan). [M]
2. **cx:** `RoomBankSO` entegrasyon — `Pick(RoomType,seed)`; eksikse start/reward list ekle. [S-M]
3. **cx:** Encounter wire — RoomTemplateSO.enemySpawnSockets → spawn transform → `EncounterController` start, room-enter sonrası. [S-M]
4. **cx:** Reward/draft/gate flow — clear → reward → `DraftManager.ShowDraft` → exit unlock; RoomLoader'dan çıkar, leak-safe. [M]
5. **cx:** Typed portal/door spawn — GateBehavior typed-sprite reuse VEYA basit portal prefab + ExitChoice binding. [M]
6. **cx:** `BuildPreview` modu + preview-ada yerleşim (visual-only). [M]
7. **Opus/Unity:** `_Arena` ref wiring + sorting layer + sample RoomTemplateSO library + full-route playtest. [M]
8. **cx-imagegen:** statik void bg + portal/rune art (gameplay loop çalışınca). [S-M]
9. **rima-qc:** compile + scene-ref + event-unsubscribe/state-reset + stale MapFlowManager/RoomLoader leak denetimi. [S]
- BAŞLAMA: painter / full-bg-art / orb-travel / drag-drop ile başlama (polish, ana riski azaltmaz).

## Reuse (cx)
RoomBankSO.Pick = yüksek (typed source). IsoRoomBuilder = en yüksek (ana yeni renderer hazır). GateBehavior typed-sprite = yüksek kısa-vade. FanLayoutSolver = orta (portal yerleşim, choice-count'u BESLE). RoomTypeData.PickPortalCount = sınırlı (count heuristic, authority değil).
