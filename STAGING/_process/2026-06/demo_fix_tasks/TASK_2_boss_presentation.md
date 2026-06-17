# TASK 2 — Boss Presentation P0 (3-5h)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / kod / STAGING / memory.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`).

## Bağlam
Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md` (Boss = P0 sunum). ChatGPT spec detayı: `STAGING/_process/2026-06/chatgpt_review_rev2/RIMA_ChatGPT_Review_2026-06-17_REV2/06_GAME_UI_REDESIGN.md` (Boss bölümü) + `03_SCREEN_BY_SCREEN_REVIEW.md` (23/24). Council kök-neden: `_process/2026-06/chatgpt_review_council/RESP_cx.md` Q4.

**Sorun:** Boss demonun klimaksı ama "öğrenci işi" gösteriyor: boss alt-kenardan taşıyor/zemine oturmuyor, HUD'u kapatıyor, neon-yeşil health-bar (palet-dışı), **merchant standları boss odasında kalıyor** (shop-residue), subtitle boss gövdesine biniyor.

## Sub-fix'ler (her birini ayrı doğrula)

### 2A — Shop-residue cleanup (KÖK-NEDEN, code)
`RoomRunDirector` `ShopRoomController`'ı retained-reference tutmadan spawn ediyor → sonraki room'a (boss dahil) geçişte temizlenmiyor. Cleanup sadece controller destroy edilirse çalışıyor.
- Dosyalar: `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:320-322,942-946` + `Assets/Scripts/Shop/ShopRoomController.cs:107-117`.
- Fix: spawn edilen ShopRoomController'a retained-ref tut; boss/sonraki room state başlarken **explicit destroy/cleanup** çağır. Boss odasında HİÇ merchant artıfaktı kalmamalı.

### 2B — Boss sprite scale/pivot/PPU/anchor
Boss tamamen arena içinde görünür olmalı, zemine otursun, HUD'u kapatmasın, alt-kenardan taşmasın.
- İlgili: `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs:132-169` (scale/intro hook'ları). Sprite pivot/PPU prefab/asset tarafında olabilir — `25 + extra Scene View` ChatGPT notu: prefab root + sprite pivot + PPU + SortingGroup incele.
- Runtime'da doğrula: boss bounds arena içinde, HUD ile overlap yok.

### 2C — Health-bar (palet + faz)
Neon-yeşil bar RIMA paletine aykırı. ChatGPT spec: 720-900px genişlik, 16-22px fill, **stone/slate frame + crimson (veya desature amber/red) fill**, **%66/%33 phase notch**, name + phase label.
- Dosya: `Assets/Scripts/UI/BossHealthBar.cs:54-55,80-128`.

### 2D — Subtitle/monolog güvenli-alan
Subtitle boss gövdesine binmesin → bottom-center, skill-bar'ın ÜSTÜnde güvenli alan.
- Dosya: `Assets/Scripts/UI/RoomMonologController.cs:123-236`.

## Kısıt
- Cerrahi: yukarıdaki dosyalar + ilgili boss prefab/asset. Combat/skill/spawn mantığına DOKUNMA (sadece sunum).
- Boss faz/skill DEĞİŞTİRME (yeni mekanik yok — sadece görsel sunum).
- Palet canon: crimson/amber/stone; cyan ≤%15; neon-yeşil YASAK.
- git'e DOKUNMA.

## VERIFY (runtime, compile yetmez)
- Boss odasına gir (mümkünse deterministik). Assert/screenshot ile kanıtla: (1) merchant-residue YOK, (2) boss tam-görünür + HUD-overlap yok, (3) health-bar crimson/stone + phase-notch, (4) subtitle güvenli-alanda.
- `read_console` 0-error.
- scene_view screenshot al (overlay UI çıkmaz ama boss/bar/dünya görünür).

## ÇIKTI (E1: dönüş ≤10 satır)
Evidence + detay → `STAGING/_process/2026-06/demo_fix_tasks/DONE_2_boss.md` (+ screenshot yolu). Dönüşte: değişen dosyalar + her sub-fix (2A-2D) PASS/durumu + runtime-verify + console durumu + kalan risk. Rapor içeriğini gömme.
