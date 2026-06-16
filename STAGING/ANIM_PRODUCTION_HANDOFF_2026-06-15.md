# Animasyon Üretim Handoff — warblade + Elementalist (2026-06-15)

> Council (cx+ax Pro+ax Flash) kararı → `WARBLADE_ANIM_DEMO_DECISION_2026-06-15.md` + `_process/2026-06/anim_council/ANIM2_*.md`. Bu dosya = **aksiyon-edilebilir üretim referansı**: state'ler MCP'den üretildi (aşağıdaki ID'ler), animate + VFX promptları hazır.
> **Sıra:** warblade P1 (demo) ÖNCE → Elementalist (post-demo) sonra. Yön: **S,SE,E,NE,N üret + SW,W,NW = Unity flipX mirror.** 120x120 high-top-down, 10-12fps, PPU64.
> ⚠️ State'ler ~90s'de biter → animate'ten ÖNCE PixelLab UI'da **görsel teyit** et (off-model çıkanı reroll). Üretim sonrası **Unity animator/sprite-swap wiring doğrula** (cx caveat).

## ⚔️ SİLAH KARARI (council oybirliği): (A) SİLAHLI-BAKE — greatsword
Demo = silah animasyona GÖMÜLÜ (bone/mount altyapısı yok → silahsız+mount post-demo, `WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md`). **AMA warblade state'leri silahsız base'den üretildi → her biri farklı kılıç icat etmiş olabilir (tutarsızlık riski).**
→ **DOĞRU BAKED PIPELINE (warblade için redo gerekebilir):**
1. **Armed anchor** üret (palette-snap=FALSE, silah yeni öğe): `create_character_state(2656075d…, "same warblade, high top-down 2D game sprite, wielding a greatsword held in hand, weapon clearly visible, preserve armor and silhouette, no redesign")` → tutarlı silahlı kimlik.
2. Pose-state'leri (mid-run/strike-windup/breathing-idle/flinch) **bu armed anchor'dan** üret (palette-snap=TRUE) → hepsi aynı greatsword.
3. Animate (önce visual-teyit). **Mevcut 4 warblade state'i**: PixelLab UI'da kılıç tutarlı mı bak; değilse anchor'dan redo. **Elementalist (caster, silahsız) state'leri = SORUNSUZ, dokunma.**

## 1) ÜRETİLEN STATE'LER (create_character_state — DONE, processing)
| Karakter | Poz (state) | State char ID (animate'te bunu kullan) |
|---|---|---|
| warblade | mid-run | `e0a13068-4926-46a5-b815-5e5657ba71df` |
| warblade | strike-windup | `b135a79f-3a96-4dd3-8b59-3024a381181c` |
| warblade | breathing-idle | `b11e4a9c-5d48-4ce4-9c11-9f76f365aba7` |
| warblade | flinch (P2) | `9f46d1c6-b3e8-4a61-80f4-a6ba0bdb3138` |
| Elementalist | mid-run | `d8b6f98b-cf62-4824-ae17-12a048df48f8` |
| Elementalist | cast-charge-windup | `7e2b6175-0c89-42cf-95cb-22707cbb7d21` |
| Elementalist | breathing-idle | `de53752f-d6c3-4ccb-bd27-a7d5a9d81e40` |
| Elementalist | flinch (P2) | `ce2ee7d4-b7f1-4ab0-97fa-c41cf2a1a274` |

## 2) ANIMATE PROMPTLARI (animate_character, mode=v3, directions=[south,south-east,east,north-east,north])
**WARBLADE — P1 (demo, ÖNCE):**
- idle ← `b11e4a9c…` · action: `guarded breathing idle loop, subtle chest and shoulder motion, sword steady, no foot sliding` · frame_count 8
- run ← `e0a13068…` · action: `run loop, grounded top-down ARPG movement, stable sword silhouette, readable foot cycle` · frame_count 8
- LMB ← `b135a79f…` · action: `basic sword slash attack, fast anticipation into heavy slash and short recovery, no extra magic VFX` · frame_count 6
- (P2, sadece P1 OBS-temizse) flinch ← `9f46d1c6…` · action: `short hit reaction, recoil then recover toward guarded stance, no spin, no knockdown` · frame_count 4

**ELEMENTALIST — P1 (post-demo prep, SONRA):**
- idle ← `de53752f…` · action: `calm caster breathing idle loop, subtle robe movement, hands steady, no large spell effects` · frame_count 8
- run ← `d8b6f98b…` · action: `run loop for a robed caster, clean foot cycle, upper body controlled, hands and orb readable` · frame_count 8
- basic cast ← `7e2b6175…` · action: `basic spell cast, gather energy then release forward, quick recovery to caster stance, small hand glow only` · frame_count 6
- (P2) flinch ← `ce2ee7d4…` · action: `short interrupted cast hit reaction, recoil then regain balance, no fall, no large VFX` · frame_count 4

> Q/E/R/F skilleri: **bespoke animasyon YOK** → basic-cast/LMB anim REUSE + `SkillVfx.cs` (tint/additive/trail/scale-fade) ile farklılaştır.

## 3) SKILL VFX — HYBRID (PixelLab core asset; gerisi engine)
- **★ Basic energy bolt projectile (SARI TOP REPLACEMENT)** — `create_8_direction_object`, 64px, transparent: `top-down pixel art magic energy bolt projectile, compact glowing white-cyan core with a short tapering trail, crisp readable directional silhouette, 64x64, transparent background, no character, no ground shadow, NOT a flat circle, NOT a plain ball`
  > Elementalist'in şu anki dümdüz sarı topunu DEĞİŞTİRİR. **Beyaz-cyan nötr core** üret → `SkillVfx.cs` ile element-başına TINT (fire=turuncu, frost=cyan, light=altın). Tek sprite, çok element = hybrid yaklaşımın kalbi. Unity'de eski sarı-top sprite'ını bununla swap et.
- **Fireball projectile** — `create_8_direction_object`, 64px, transparent: `top-down pixel art fireball projectile, compact bright orange core with small ember tail, readable in 8 directions, 64x64, transparent background, no character, no ground shadow`
- **Fire impact burst** — `create_map_object`, 80px: `top-down pixel art fire impact burst, circular ember explosion with bright core and short sparks, 80x80, transparent background, no character, no ground tile`
- **Glacial spike cluster** — `create_map_object`, 96x64: `top-down pixel art glacial spike line cluster, sharp cyan ice shards rising in a narrow forward line, readable gameplay silhouette, 96x64 canvas, transparent background, no character`
- **Frozen orb core** — `create_map_object`, 64px: `top-down pixel art frozen orb, rotating cyan-blue ice sphere with small frost shards around it, 64x64, transparent background, no character, no ground tile`
- **Light beam flare** — `create_map_object`, 96x32: `top-down pixel art radiant light beam flare, narrow golden-white horizontal strip with bright center and crisp edges, 96x32, transparent background, no character, no rainbow spray`
> Meteor/Blizzard/Frost Wall/Living Bomb/Lightbreak = engine telegraph + mevcut placeholder (PixelLab YOK şimdilik).

## 4) STATE PROMPTLARI (üretimde kullanıldı — reroll/yeni karakter için referans)
warblade: mid-run / strike-windup / breathing-idle / flinch · Elementalist: mid-run / cast-charge-windup / breathing-idle / flinch — tam metinler: `_process/2026-06/anim_council/ANIM2_cx.md` §E3.

## BUDGET / SIRA
P1 anim ≈ 3×5=15 gen/karakter · VFX core ≈ 20-30 gen · budget 874 = bol. Darboğaz = per-yön cleanup. warblade-P1 demo'yu geciktirmez; Elementalist staged post-demo.
