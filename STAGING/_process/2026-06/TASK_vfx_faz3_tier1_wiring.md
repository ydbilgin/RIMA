ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
VFX Faz 3 — Tier 1 skill wiring: Fireball · Warblade basic · Gravity Cleave'e `SkillVfx` çağrıları ekle (additive, ENGINE juice). Combat DEĞİŞMEZ (DamagePacket/hitbox/cooldown/DealDamage dokunulmaz). SkillVfx API ZATEN VAR: `Assets/Scripts/VFX/SkillVfx.cs` (commit 72b27aca).

## SkillVfx API (mevcut — kullan)
- `SkillVfx.CastFlash(caster, VfxElement)` · `ImpactBurst(pos, VfxElement)` · `ProjectileTrail(go, VfxElement)` · `MeleeArc(pos, dir, VfxElement)` · `GroundCrack(pos, VfxElement)` · `SpawnTinted(prefab, pos, element, dir)` · `PlayBurst(spritePrefab, pos, element, ...)`
- Enum: `VfxElement { Physical, Fire, Frost, Lightning, Void, Arcane }`

## YENİ asset'ler (import settings AYARLA)
`Assets/Resources/VFX/Skills/` altına 4 yeni PNG indi: `vfx_explosion_a/b.png` (128²), `vfx_shatter_a/b.png` (128²).
**İLK İŞ:** bu 4 sprite'ın texture import'unu ayarla: **Texture Type=Sprite (2D and UI), Filter Mode=Point, Pixels Per Unit=64, Compression=None, Mipmaps=off, Wrap=Clamp.** (manage_texture set_import_settings veya .meta).

## İŞ — Tier 1 wiring (cx önceki audit hook noktaları)
1. **Fireball** (`Assets/Scripts/Skills/Elementalist/Fireball.cs`):
   - `Execute()` başında (`:42-49`) → `SkillVfx.CastFlash(player/gameObject, VfxElement.Fire)`.
   - Projectile yaratıldıktan sonra (`:64-68`) → `SkillVfx.ProjectileTrail(go, VfxElement.Fire)`.
   - Impact: `PlayerProjectile.SetOnHit` callback ile → `SkillVfx.PlayBurst(Resources.Load<GameObject>?/vfx_explosion sprite, hitPos, VfxElement.Fire)` VEYA `SkillVfx.ImpactBurst(hitPos, VfxElement.Fire)` + `vfx_explosion_a` sprite'ı (Resources.Load "VFX/Skills/vfx_explosion_a") `PlayBurst` ile. Patlama = explosion sprite + tint fire + scale/fade.
   - DİKKAT: SetOnHit imzası `Action<Collider2D>` — sadece pozisyon kullan, hasar mantığına DOKUNMA.
2. **Warblade basic attack** (`Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs` — swing/hit hook'unu OKU/bul):
   - Swing anında → `SkillVfx.MeleeArc(hitOrigin, facingDir, VfxElement.Physical)` (mevcut slash_arc sprite + sweep + HitSpark, SkillVfx içinde hazır).
   - Eğer hook noktası belirsizse → BLOCKED yaz, uydurma.
3. **Gravity Cleave** (`Assets/Scripts/Skills/Warblade/GravityCleave.cs`):
   - Cast/merkez (`:30-37`) → `SkillVfx.MeleeArc(center, facingDir, VfxElement.Void)` (geniş arc, void tint) + `SkillVfx.GroundCrack(center, VfxElement.Physical)`.
   - Her hedefte hit (`:37-52`, DealDamage SONRASI) → `SkillVfx.ImpactBurst(target.pos, VfxElement.Void)`.
   - Pull/OverlapCircle/DealDamage/rage satırlarına DOKUNMA.

## YAPMA
- DamagePacket/Health/DealDamage/hitbox/cooldown/pull-force DEĞİŞTİRME.
- Yeni prefab/SO/pooling YAZMA. SkillVfx'i kullan.
- Tier 2/3 (Chain Lightning/Iron Charge/Earthsplitter/Glacial Spike/Elem basic) bu task DEĞİL.
- Skill'in mevcut görsel kodunu (varsa) silme — additive EKLE; çakışırsa eski prosedürel'i koru, not düş.

## Doğrulama (commit ÖNCESİ)
1. `read_console` → 0 compile error. (Unity AÇIK → batchmode test çalışmaz; compile-clean yeterli, combat dosyası mantığı değişmedi.)
2. Combat doğruluğu: DamagePacket/Health/hitbox satırları diff'te DEĞİŞMEMİŞ olmalı (sadece SkillVfx çağrı satırları eklenmiş).
3. Yeşilse commit: `feat(vfx): wire Tier 1 skills (Fireball/Warblade-basic/Gravity-Cleave) to SkillVfx + explosion sprite import [visual unverified]`.
4. Belirsizlik/hook bulunamadı → BLOCKED, uydurma.

## CODEX_DONE'a yaz
- Hangi 3 skill'e hangi SkillVfx çağrıları eklendi (file:line).
- Import settings ayarlanan 4 sprite.
- Combat dokunulmadığı teyidi (diff özeti).
- Compile sonucu. SKIPPED/BLOCKED varsa neden.
