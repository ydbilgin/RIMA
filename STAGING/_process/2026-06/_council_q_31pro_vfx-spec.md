# RIMA Skill VFX Üretim Spec — DEEP ART/TECH lens (Gemini 3.1 Pro High)

Sen RIMA için derin VFX/teknik-sanat danışmanısın. RIMA = 2D top-down pixel-art roguelite ARPG, Unity, C#, PPU 64, URP 2D Renderer + Pixel Perfect Camera + 2D Lights, 8-yön sprite, high top-down 3/4 açı.

Gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
READ:
- STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md (denetlenecek spec — ANA)
- STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md (renk kaynağı)
- Assets/Scripts/Skills/Elementalist/Fireball.cs · ElementalistRuntimeVisuals.cs · PlayerProjectile.cs

DEĞERLENDİR (derin, somut):
1. **Pixel-art VFX yöntemi:** PPU-64 pixel-art bir oyunda "güzel ama tutarlı" skill VFX için en doğru teknik nedir? Spec'in önerisi (pixel-snapped chunky ParticleSystem + seçici flipbook sprite-sheet) sağlam mı? Soft default particle'dan kaçınma stratejisi yeterli mi? Additive glow + 2D light kombinasyonu pixel okumayı bozar mı? Concrete Unity ayarları ver (Point filter, Pixel Snap shader, particle texture boyutu, max particle, simulation space).
2. **SkillVfx mimarisi:** static API (`SkillVfx.Play(archetype, element, pos, dir)`) mi, yoksa SO-tabanlı `VfxProfile` (skill başına asset, inspector-tunable) mi? Demo hızı vs uzun-vade tunability trade-off. Pooling mimarisi (GC-free, skill spam). Mevcut Fireball 8-dir sprite + PlayerProjectile hook ile nasıl köprülenir?
3. **Arketip tasarımı:** Cast / Travel / Impact 3-parça doğru ayrım mı? Chain Lightning (LineRenderer zikzak vs particle trail), Gravity Cleave (pull vortex), Earthsplitter (ground crack) için pixel-art'ta en iyi teknik hangisi? Melee swing arc: prosedürel trail mesh mi, elle-çizili arc sprite mi?
4. **§8'deki 5 açık soruyu** mimari gerekçeyle yanıtla.
5. **Telegraph/okunabilirlik:** player VFX'i düşman telegraph'larıyla (EnemyTelegraph) karışmadan nasıl ayrışır (renk/parlaklık/şekil dili)?

Somut Unity tip/ayar/shader isimleri ver. Türkçe karakter serbest.
