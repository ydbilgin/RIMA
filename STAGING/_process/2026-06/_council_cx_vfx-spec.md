ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ANALYSIS ONLY — kod yok, commit yok. VFX üretim spec'ini FEASIBILITY / REUSE / mevcut-altyapı açısından denetle.

READ:
- STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md (denetlenecek spec)
- Assets/Scripts/Skills/Elementalist/Fireball.cs · ElementalistRuntimeVisuals.cs
- Assets/Scripts/Skills/PlayerProjectile.cs (impact hook)
- Assets/Prefabs/VFX/ (HitSpark/DeathBurst/HandGlowVFX/RiftGlowVFX)
- Assets/Scripts/Skills/Warblade/IronCharge.cs, GravityCleave.cs, Earthsplitter.cs · Elementalist/GlacialSpike.cs, ChainLightning.cs

DEĞERLENDİR (somut, dosya:satır):
1. Mevcut altyapı spec'in iddia ettiği kadar VAR mı? PlayerProjectile impact hook gerçekten reuse'a uygun mu? HandGlowVFX/HitSpark prefab'ları cast/impact için temel olabilir mi yoksa sıfırdan mı gerekir?
2. Reusable `SkillVfx` static API + pooling: en az kodla nasıl kurulur? SO-VfxProfile gerekli mi yoksa overkill mi (modüler ders: SO sadece gerçek varyasyon varsa)?
3. 6 kit skill + 2 basic'in mevcut Execute() akışına VFX kancası eklemek combat'ı bozar mı? DamagePacket/hitbox'a dokunmadan additive eklenebilir mi (her skill için kanca noktası neresi)?
4. Chain Lightning'in mevcut zincir mantığı (ChainLightning.cs) LineRenderer görselini beslemeye uygun mu? Gravity Cleave pull-force görselleştirmesi mevcut koda nasıl bağlanır?
5. §8'deki 5 açık soruya feasibility cevabı. Demo süresinde 8 VFX gerçekçi mi, yoksa öncelik sırası ne?
6. RED FLAG: spec'te yapılamaz/aşırı-riskli bir şey var mı?

Sonucu CODEX_DONE.md'ye yaz. Belirsizlik → BLOCKED.
