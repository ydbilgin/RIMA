# RIMA — CharacterSelect final polish + demo unlock economy — DESIGN lens (Gemini 3.1 Pro)

READ first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\CHARSELECT_FINAL_BRIEF_2026-06-04.md
RIMA = dark pixel-art roguelite, cyan #00FFCC, void-purple, Echo meta-currency, 4 classes free / 6 locked (Echo cost: 120/180/250). Roster-room CharSelect (chars in a generated keep, left identity popup, right vertical skills, bottom SEÇ/GERİ).

Concrete design (numbers + recipes; feeds implementation):
1. **Demo Echo amount:** how much STARTING Echo so the dev can unlock 1 class live for a professor demo, without trivializing the whole roster? (cheapest locked = 120). Give a number + reasoning (e.g. ~150-200 = unlock one, feel the cost). Where industry seeds demo currency.
2. **Unlock flow UX:** click locked char → it does NOT become the playable selection; instead the left/right panels show locked identity + the right/bottom shows "KİLİDİ AÇ — {cost} Echo" (enabled if affordable, muted if not). Click unlock → spend Echo → **reveal** (silhouette → full sprite, a quick cyan flash/particle) → now selectable. Spec the exact states + transitions + the affordable/not-affordable visual.
3. **Locked = full BLACK silhouette:** recipe — tint the idle_south Image to near-black (e.g. #0A0510) keeping alpha (shape only), maybe a thin cyan rim + floating lock glyph + cost chip. On unlock → tween color black→white (reveal). Confirm this reads as "mystery/locked" cleanly.
4. **Bigger side panels + no-occlusion:** left identity + right skills are currently too small/narrow. Give bigger-but-balanced widths (e.g. identity x.01-.22, skills x.80-.99?) + the resulting character center band + char sizes so all 10 stay fully visible, NONE behind a panel/column. Numbers.
5. **Echo HUD:** should the current Echo balance be shown on the CharSelect screen (so the demo shows "I have X Echo → unlock costs Y")? Where + how (small top/corner chip)?

Tight, numbers + hex + transition spec. Demo-appropriate, not a full meta-economy.
