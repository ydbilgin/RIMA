ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# Codex Task — Agent Sprite Forge Install + Sandbox Wall Test

**Amaç:** generate2dsprite + generate2dmap skill'lerini install + 1 wall piece sandbox test (mevcut $imagegen output ile yan yana karşılaştırma için).

## Install

```bash
npx skills add 0x0funky/agent-sprite-forge -g -y --copy -a claude-code codex -s generate2dsprite
npx skills add 0x0funky/agent-sprite-forge -g -y --copy -a claude-code codex -s generate2dmap
```

Test: skill SKILL.md readable her ikisi için, Claude + Codex global + 4 profile.

## Sandbox Test

generate2dsprite ile 1 wall piece üret:

- Hedef: wall_nw_mid_plain_sprite_forge.png (sprite-forge versiyonu)
- Boyut: 128×384 final
- Spec: front-facing stone wall band, charcoal granite, HIGH TOP-DOWN 3/4 styling (NOT true iso), subtle top cap + slight side face thickness illusion, no banner, no torch baked
- Reference image: STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png
- Output: STAGING/concepts/fractured_chamber/iso_assets/wall_nw_mid_plain_sprite_forge.png

Pipeline:
1. generate2dsprite raw concept
2. Built-in chroma-key cleanup (skill includes Python processors)
3. Final 128×384 PNG with alpha

## Karşılaştırma

mevcut $imagegen output ile yan yana:
- STAGING/concepts/fractured_chamber/iso_assets/raw/wall_nw_mid_plain_raw.png (eski imagegen, 1024x1024)
- STAGING/concepts/fractured_chamber/iso_assets/wall_nw_mid_plain_sprite_forge.png (yeni sprite forge)

Rapor: STAGING/sprite_forge_vs_imagegen_comparison.md
- Sprite Forge çıktı kalitesi vs $imagegen
- Cleanup otomasyonu farkı (chroma-key + frame split + alignment)
- RIMA pipeline'a entegrasyon önerisi (Sprite Forge primary mi, $imagegen primary mi, hibrit mi)

## Hard Constraints

- HIGH TOP-DOWN 3/4 projection (NOT true iso diamond) — memory: project_high_top_down_3_4_lock_2026_05_24
- $imagegen built-in tool mode (memory: feedback_codex_imagegen_skill_not_env_var)
- No banner, no torch baked (user lock)
- Transparent BG clean

## Git Commit

```bash
git add STAGING/sprite_forge_vs_imagegen_comparison.md STAGING/concepts/fractured_chamber/iso_assets/wall_nw_mid_plain_sprite_forge.png
git commit -m "[Codex] [SPRITE FORGE] install + sandbox wall test"
```
