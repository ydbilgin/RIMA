# SKILL VFX — IMPLEMENTATION PLAN (2026-06-12, BAĞLAYICI)

Konsolide: spec v3 (`SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md`) + batch limits + council (cx/3.1Pro/Flash) + endüstri araştırması (Dead Cells/Unity/asset-pack pratiği). **Ana ilke: AZ sprite, ÇOK engine katmanı** (Dead Cells: "VFX layer >> animation frames").

---

## A. PixelLab BATCH'leri (minimal — signature-only)

### Önce VERIFY-REUSE (üretmeden önce)
cx repo'da şunları buldu (64×64), önce bunları gör → yeterse ÜRETME:
- `slash_arc_main/crescent` · `glacial_spike_cluster/spear` · `frozen_orb_main/alt` · `meteor_main/comet` · `floor_riftcrack`+`cracks_bones_01..12` · Fireball 8-dir (48²).

### Üretilecekler = sadece authored signature şekiller (yok olanlar)
Tool: `create_1_direction_object`, KARE. Tematik bölmeli (cx: mixed batch = concept bleed). Her çağrı = aynı şeklin N varyantı → en iyiyi seç. **Gri/nötr çiz, rengi Unity verir.**

| Batch | Tool çağrısı | İçerik | Not |
|---|---|---|---|
| **B1 (ops)** | size=64, 16 variant | slash arc YÜKSELTME | sadece mevcut placeholder zayıfsa; 16 varyant→en iyi 1 |
| **B2** | size=128, 4 variant | Gravity Cleave void arc | signature, ember+void |
| **B3** | size=128, 4 variant | Fireball patlama | fire (veya particle ile fake → opsiyonel) |
| **B4** | size=128, 4 variant | Frost shatter | frost (veya particle shard) |
| **B5** | reuse | Earthsplitter fissür | `cracks_bones` reuse → muhtemelen ÜRETME |

→ Gerçek üretim: **2-4 çağrı** (~80-160 gen / 959). Geri kalan = reuse/particle.
**Animasyon:** çoğu YOK → Unity scale/fade/additive. `animate_object` sadece şekil-morph (shatter) gerekirse.
**8-yön:** Unity rotation+flipX (1 sprite → tüm yönler).

---

## B. UNITY IMPLEMENTATION (asıl iş — Dead Cells engine-layer modeli)

### Faz 1 — SkillVfx çekirdeği (cx dispatch)
- `Assets/Scripts/VFX/SkillVfx.cs` (static), `VfxElement` enum, taksonomi `Palette` (Physical #E89020·Fire #FF6A1F·Frost #7FE0FF·Lightning #FFE600·Void #7B3FA8·Arcane #00FFCC).
- `SpawnTinted(prefab, pos, element, dir)` → instantiate + tint + **sorting "VFX"/order≥20 ZORLA** (cx red-flag: prefab'lar 0 geliyor) + stopAction=Destroy. Pooling YOK (Flash: demo'da gerekmez).
- Engine-anim helper: `PlayBurst` (scale-up + alpha-fade curve), `PlaySweep` (arc süpürme+fade). **Additive material** (core), alpha-blend ana (whiteout önlemi).

### Faz 2 — 5 archetype (reuse-first)
| Archetype | Kaynak | Teknik |
|---|---|---|
| CastFlash | `HandGlowVFX` tint | el parıltısı |
| ImpactBurst | `HitSpark`/`DeathBurst` tint + scale/fade | vuruş |
| ProjectileTrail | TrailRenderer/particle | mermi izi |
| MeleeArc | mevcut `slash_arc` sprite + sweep+fade+additive | kesme |
| GroundCrack | mevcut decal + reveal | yer çatlağı |
| ChainBolt | LineRenderer, **cached shared material** (cx fix: ChainLightning.cs:79 per-arc `new Material` SİL) | zincir |

### Faz 3 — Tier 1 skill wiring (additive, DamagePacket/hitbox DOKUNULMAZ)
cx hook noktaları (file:line):
- **Fireball** `Fireball.cs:42-49/64-68` + `SetOnHit` (cast+trail+impact)
- **Warblade basic** (`Combat/BasicAttack/MeleeChainBehavior`) — slash arc sweep + HitSpark
- **Gravity Cleave** `GravityCleave.cs:30-52` — void arc + pull vortex (particle) + crack
→ sonra Tier 2 (Chain Lightning cleanup, Iron Charge, Earthsplitter), Tier 3 (Glacial Spike, Elem basic).

### Faz 4 — Juice bağı
Screen-shake / hit-stop: VFX TETİKLEMEZ, mevcut sistemle senkron (Iron Charge/Earthsplitter impact — sistem varsa, onaylıysa). Ses-sync: opsiyonel `sfxId` hook (stub). Combat timing DEĞİŞMEZ.

---

## C. INDICATOR LAYER (WoW-addon / WeakAuras-style — combat feedback)
> Ayrı track, spell VFX değil — HUD/UI. **Data RIMA'da ZATEN VAR, görünmüyor.** Yüksek demo-getirisi (sistem-derinliği algısı).

- **Proc belirteci:** Fireball 3-ardışık→Living Bomb (`consecutiveCasts`) → "2/3" dolan gösterge + hazır olunca glow.
- **Stack belirteci:** Elementalist element stack (`RegisterElementCast`), `SkillStateTracker.GetStacks`, `StatusEffectSystem` stack'leri → düşman/oyuncu üstü sayaç.
- **Reusable component:** `StackIndicator` + `ProcGlow` (tek component, tüm skill'lerde) — modülerliğin DOĞRU yeri.
- HUD Layout kararıyla (`HUD_LAYOUT_DECISION_2026-06-12.md`) birleşir. **Demo önceliği: signature VFX'ten YÜKSEK olabilir.**
- ⚠️ ScreenSpaceOverlay screenshot'ta görünmez ([[feedback_screenspaceoverlay_not_in_screenshot]]) → world-space veya Camera canvas tercih, ya da Game-view capture.

---

## D. Sınır (üretme/yapma)
- 8 placeholder sınıf (Ravager/Gunslinger/Brawler/Summoner/Hexer `isImplemented=false`) signature VFX = GELECEK, demo değil (cx).
- Generic VFX (spark/dust/aura) = particle/kod, PixelLab sprite DEĞİL (endüstri+spec).
- Tam WeakAuras-configurable sistem = overkill, demo'da basit indicator yeter.
- Tek çağrıda 13 random item = concept bleed → YASAK.

## E. Doğrulama
Play-mode görsel (Unity AÇIK, VFX world-space → screenshot yakalar), **kör commit YOK** ("visual unverified" notu), combat doğruluğu (DamagePacket/hitbox) additive katmanla DEĞİŞMEZ. QA recipe: boş oda + combat oda, her skill, basic ×20 spam (GC test), demo zoom, karanlık zemin, yakında enemy VFX (telegraph ayrımı).

## F. Routing / sıra
1. **Faz 1 + Faz 2** → cx dispatch (SkillVfx çekirdeği + archetype'lar, reuse-first, compile gate).
2. **PixelLab B2-B4** (signature şekiller) → paralel (kullanıcı onayı + verify-reuse sonrası).
3. **Faz 3 Tier 1 wiring** → cx, Faz 1 bitince.
4. **Indicator layer** → ayrı (HUD Layout ile); demo-önceliği yüksek, sıralamayı kullanıcı seçer.
5. Play-mode QA → görsel onay → commit.
