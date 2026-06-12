ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Skill VFX engine katmanı Faz 1+2: reusable `SkillVfx` static helper + 5 archetype, MEVCUT prefab/asset reuse ederek. Combat'a DOKUNMA (additive katman). Plan: `STAGING/SKILL_VFX_IMPLEMENTATION_PLAN_2026-06-12.md` (B bölümü) + `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md` (v3 COUNCIL SYNTHESIS bağlayıcı). Model = Dead Cells "tek statik sprite + engine juice".

## İLKE
- Sprite = tek statik form. Juice = engine: tint + scale-up + alpha-fade + additive + rotation + particle. animate_object/flipbook YOK.
- Combat DEĞİŞMEZ: DamagePacket/hitbox/cooldown/DealDamage'a DOKUNMA. VFX additive, event sonrası çağrılır.
- Pooling YOK (Instantiate/Destroy, stopAction=Destroy). SO-VfxProfile YOK.

## İŞ 1 — SkillVfx çekirdeği
`Assets/Scripts/VFX/SkillVfx.cs` (static) + `VfxElement` enum.
- `enum VfxElement { Physical, Fire, Frost, Lightning, Void, Arcane }`
- Palette (taksonomi — DAMAGE_TYPE_TAXONOMY_DECISION): Physical `#E89020` · Fire `#FF6A1F` · Frost `#7FE0FF` · Lightning `#FFE600` · Void `#7B3FA8` · Arcane `#00FFCC`. (Crit `#FFD24A` element DEĞİL, kullanma.)
- `static void SpawnTinted(GameObject prefab, Vector3 pos, VfxElement element, Vector2 dir = default)`:
  instantiate → SpriteRenderer/ParticleSystem rengini Palette[element]'e tint → **sorting layer "VFX" + order≥20 ZORLA** (cx önceki audit: mevcut prefab'lar sorting layer 0 geliyor) → ömür sonunda kendini Destroy.
- Engine-anim helper'ları:
  - `static void PlayBurst(GameObject sprite, Vector3 pos, VfxElement element, float scaleFrom=0.5f, float scaleTo=1.3f, float life=0.25f)` — scale-up + alpha-fade coroutine (patlama/impact). Additive material kullan (core), whiteout için ana katman alpha-blend.
  - `static void PlaySweep(GameObject arcSprite, Vector3 pos, Vector2 dir, VfxElement element, float life=0.2f)` — arc süpürme (rotate to dir + scale + fade).
- Bir `SkillVfxRunner` MonoBehaviour (coroutine host) gerekebilir (static'ten coroutine için). Minimal tut.

## İŞ 2 — 5 archetype (MEVCUT asset reuse — sıfırdan PREFAB YAPMA)
Mevcut (repo'da VAR, cx audit doğruladı):
- `Assets/Prefabs/VFX/HitSpark.prefab`, `DeathBurst.prefab` (one-shot particle, order 30) → ImpactBurst bazı
- `Assets/Prefabs/VFX/HandGlowVFX.prefab` (looping) → CastFlash bazı (attach, tint)
- `Assets/Resources/VFX/Skills/slash_arc_main.png` / `slash_arc_crescent.png` (64×64) → MeleeArc sprite
- `Assets/Sprites/Environment/Decals/floor_riftcrack.png` + `cracks_bones_01..12` → GroundCrack decal
- `Assets/Resources/Prefabs/VFX/SlashArcVFX.prefab` + `Assets/Scripts/VFX/SlashArcVFX.cs` (LineRenderer arc) → mevcut, reuse veya referans

Archetype API'leri (SkillVfx üstünde ince wrapper):
- `CastFlash(caster, element)` → HandGlow tint, kısa
- `ImpactBurst(pos, element)` → HitSpark/DeathBurst tint + PlayBurst
- `ProjectileTrail(go, element)` → TrailRenderer ekle/tint (mevcut yoksa runtime ekle)
- `MeleeArc(pos, dir, element)` → slash_arc sprite + PlaySweep + HitSpark
- `GroundCrack(pos, element)` → decal sprite reveal (scale/fade)
- `ChainBolt(from, to, element)` → LineRenderer, **cached shared Material** (cx red-flag fix referansı: ChainLightning.cs:79 her arc `new Material` — burada paylaşılan material kullan, ileride o satır da düzeltilecek)

## YAPMA
- Skill dosyalarına henüz DOKUNMA (wiring ayrı task A4).
- DamagePacket/Health/DealDamage/hitbox DEĞİŞTİRME.
- Yeni ScriptableObject / pooling / VfxProfile YAZMA.
- animate_object/flipbook/Animator controller — engine scale-fade kullan.

## Doğrulama (commit ÖNCESİ)
1. Derlenmeli (compile error YOK). `read_console` ile kontrol.
2. **Unity AÇIK** — batchmode test çalışmaz; sadece compile-clean yeterli (bu task VFX katmanı, mevcut test yok). Compile temizse commit et.
3. Commit: `feat(vfx): SkillVfx engine core + 5 reusable archetypes (tint/additive/scale-fade, reuse existing prefabs) [visual unverified]`.

## CODEX_DONE'a yaz
- Eklenen dosyalar + SkillVfx API imzaları.
- Hangi mevcut prefab/asset reuse edildi.
- Compile sonucu.
- Combat dosyalarına dokunulmadığının teyidi.
