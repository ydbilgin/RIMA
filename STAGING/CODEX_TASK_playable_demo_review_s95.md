# Codex Görev: RIMA Playable Demo Architecture Review (S95)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Bağlam

User direktifi (2026-05-19 S95):
> "gerçek bi oyun hissi arıyorum anladın mı beni bu konuda senden daha yardım edebilecek bir şeyim yok. codexe de sorarak ilerle ki mantıklı ilerleyelim"

User playable demo aşamasına geçmek istiyor — "real game feel" hedefi. Bana plan review için second-opinion lazım. Bu görev sadece analiz, kod yazma YOK.

## Mevcut Durum (S95)

**Karar mimari (LIVE):**
- Karar #150: Fake isometric + dungeon-inside, 32×22 default sub-room, irregular layout, internal-arch primary
- Karar #149: Combat/Elite macro node = 1 EncounterTemplateSO × 3-5 sub-room sequence, fade-to-black transitions
- Karar #144: Weaponless body + WeaponSR child SR (weapon attach to hand)
- Karar #100/#114: 8-dir (5 sprite + 3 mirror), 35° tilt baked in art
- S94 Transform Squash: Tilemap parent localScale.y = cos(35°) = 0.819 — sadece floor için
- S95 LOCK: Wall decoration = pure attachment only, NO wall background in sprite

**Envanter (Act 1 Shattered Keep) - 2026-05-19 S95:**
- 46 PNG temiz pack (27 cloud + 19 fresh gen)
- + 24 PNG yeni gen (5 props + 16 decals tile_pro + 3 L4 patches)
- + 16 PNG in-flight mounting apparatus batch (cf69a2f9, 64px × 16 items via n_frames=16)
- Cloud library: 147 obje + 52 tiles_pro + 25 wang tileset + 4 iso + 17 character (10 anchor class)
- Folder yapı: arches/, pillars/, props/, wall_decoration/, rift_accents/, scatter/, decals/, patches/, walls/, floor_tiles/{granite_base, iso}

**Character anchors LIVE (PixelLab cloud):**
- Warblade 2656075d-d113-4f18-a6c1-94b5a6b8bf65 (8dir 120x120)
- Elementalist 4c83c0be-e856-48f1-b8b5-9626e041a082 (8dir 120x120)
- Ranger d5b1cf71-0158-4347-97b9-a34a5ac0d98a (8dir 128x128)
- Shadowblade deee34b5-7796-4c8f-9262-b8a83f907240 (8dir 124x124)
- + 6 diğer class anchor

**Scene durumu (RoomPipelineTest.unity):**
- Spawn_01_NewTileSystem (32×22 painted, perimeter wall stubs 39 pillar_broken/intact, 26 props L5b, 16 scatter L5)
- Spawn_02_NewTileSystem (32×22 painted boş, kompozisyon yok)
- Warblade_Player (16, 8, 0) — sadece SpriteRenderer, hareket script YOK, collider YOK
- Lighting_Setup × 2 (15 Light2D toplam)
- Main Camera: ortho 11.5, position (16, 11, -10), rot (6,0,0)

## Demo Hedefi (user'ın "gerçek oyun hissi")

Playable demo MVP:
- 4 oynanabilir class (Warblade + Elementalist + cross-class + opsiyonel Ranger/Shadowblade)
- Weapons attach to hand (Karar #144)
- Walking + 8-dir movement
- Combat: temel saldırı, 1 mob ile fight
- Sub-room door-through transition (Karar #149 fade-to-black)
- HP/HUD minimal
- Hit feedback (hitstop, camera shake, slow-mo) — "juice"

## Önerilen 2-track plan

**TRACK A — Visual gen (paralel, in-flight)**
1. Mounting apparatus batch (cf69a2f9, 16 items @ 64px) → review + select_object_frames
2. Wall pieces gen (4-5 single calls @ 128px): wall straight section, wall corner NE/NW, wall arch opening, wall with cyan rift integrated
3. Statue + ritual batch (1 call, 16 items @ 64px): statue intact, statue toppled, sarcophagus, altar, bench, obelisk, pedestal, ladder, cart, lever, grate, treasure pile, ... + Karar #150 spec
4. Mob batch (1 call, 16 items @ 64px): bone walker, imp, slime, rat, spider, wraith, wisp, bat, crawler, goblin, skull animated, husk, archer, ratking...
5. Character states (per anchor, ~3 each = 12 calls): attack pose, cast pose, dash pose, hit pose
6. Multi-room compose (Spawn_01 + Spawn_02 + Spawn_03)

**TRACK B — Mechanics (paralel)**
1. Player movement: WASD 8-dir, speed tune (Hades feel ~5 unit/s)
2. Box collider 2D: walls + arches + pillars (composite collider mantıklı mı?)
3. Sub-room transition: arch trigger → fade quad → teleport Warblade
4. Enemy AI: 1 basic mob, idle + chase + attack
5. Combat: basic attack hit detection, hitstop 50-100ms, screen shake
6. HUD: HP bar üst-sol, gen text
7. Hit feedback (juice): hitstop + shake + slow-mo combo

## Sorular Codex'e

**Sor 1:** "Real game feel" için Track A vs Track B sırası — paralel mi yoksa Track B önce (visual yeterli artık) mi? Visual perfection için art tracking düşmek riskli mi?

**Sor 2:** Top-3 game-feel blocker'ı sırala (öncelik order'ı ne olmalı). Örnekler: tight movement responsiveness, hitstop timing, camera follow lag, attack feedback, enemy AI feel, transition smoothness.

**Sor 3:** Transform squash architecture risk:
- Tilemap parent localScale.y = 0.819 (squash floor)
- Vertical prop'lar parent içinde olduğu için inherit squash → biz `localScale.y *= 1/0.819 = ×1.22` counter-squash uygulayık
- Soru: Warblade_Player ŞU AN sahnenin root'unda (parent değil), squash uygulanmıyor. Spawn_01 içine taşırsak counter-squash gerekir mi? 8-dir sprite-based karakter, baked perspective var.
- Plus collider 2D: squash parent altındaki collider'lar otomatik counter scale-able mi yoksa world-space collider mi gerek?

**Sor 4:** Karar #149 sub-room transition mekaniği için minimal kod path:
- Arch trigger collider (IsTrigger) → OnTriggerEnter2D → fade canvas alpha 0→1 → teleport player → fade 1→0
- EncounterTemplateSO + SubRoomSequenceController zaten mevcut (Karar #149 LIVE per memory). Bu sahnede entegre mi yoksa bypass edip basit teleport mı yapayım demo için?

**Sor 5:** Camera follow Hades-style — tight lock vs lag. Cinemachine kullanmalı mı yoksa TestCameraFollow (mevcut script) yeter mi?

**Sor 6:** Multi-class playable — Warblade aktif + Elementalist switching mi yoksa cross-class skill slot mu (Karar #65 cross-class system memory)? Switch UX nasıl olur?

**Sor 7:** Hit feedback "juice" — hitstop ms, screen shake amplitude, slow-mo recommended values. Combat Feel memory referansı var ([[combat-feel-research-combined]]).

**Sor 8:** Risk inventory — şu mimaride neyi atlamış olabilirim? Demo için kritik blocker'lar.

## Çıktı Format

```
## Plan Verdict
APPROVE_AS_IS | APPROVE_WITH_REVISIONS | BLOCK

## Top-3 Game-Feel Blockers (priority order)
1. ...
2. ...
3. ...

## Track Order Önerisi
[paralel / sequential / mix justification]

## Soru-Soru Cevapları
[Sor 1-8 cevaplar, kısa + actionable]

## Risk Inventory
[3-5 risk + mitigation]

## Sonraki 3 Concrete Action
[1, 2, 3]
```

Cevap **max 2000 token**, conservative. Code önermeyecek (planlama only). NLM gerekirse query yap (RIMA bağlamı için), local memory yeterse direkt referansla.
