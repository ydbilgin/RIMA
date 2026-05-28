# PixelLab Envanter — agy External Research + Weapon Spec Industry

## Amaç
243 PixelLab obje envanteri context'inde, **external research bağımsız analiz**. Opus/Codex çıktısına BAKMA, ortak karar SONRA loop sentez aşamasında.

inline respond, NOT to file (`feedback_agy_inline_response_only`).

## Bağlam
- 243 obje envanter: `STAGING/PIXELLAB_INVENTORY_MASTER.md`
- RIMA chibi 64×64 standart, 120×120 character canvas
- Hades Elysium V1 brand (wall-less, cyan glow)
- HIGH TOP-DOWN 3/4 camera angle 70-80 deg
- 10 class roster: Warblade / Ronin / Gunslinger / Ranger / Elementalist / Shadowblade / Ravager / Hexer / Brawler / Summoner

## Sorular (external research)

### 1. Chibi 2D weapon spec industry standards
- Hades / Children of Morta / Diablo III sprite weapon canvas spec
- 64×64 mob, 120×120 character için ideal weapon boyut?
- 1-hand vs 2-hand weapon canvas farkı
- Pivot/PPU norms (PPU=64 vs PPU=100 trade-off)

### 2. HandAnchor mount mechanics industry
- 2D top-down weapon attachment pattern (Spine bone vs static child SR)
- 8-direction sprite mirroring (5+3 flipX)
- Per-direction offset table baseline değerleri
- Weapon rotation per direction (45° increment)

### 3. 10 class weapon canvas tavsiyeleri (her biri için)
- Greatsword (Warblade, 2-hand) — Hades equivalent canvas?
- Katana (Ronin, 1-hand) — ?
- Pistol (Gunslinger, 1-hand small) — ?
- Bow (Ranger, 2-hand large) — ?
- Staff (Elementalist, 2-hand) — ?
- Dagger (Shadowblade, dual 1-hand small) — ?
- Greataxe (Ravager, 2-hand chunky) — ?
- Whip (Hexer, 1-hand long) — ?
- Gauntlets (Brawler, dual 1-hand fist) — ?
- Tome+Orb (Summoner, 1-hand each + floating orb) — ?

### 4. Animasyon flow karar (paralel external research)
Kullanıcı sorusu: "karakter animasyonu basit mi olacak silah + vfxle mi yapacaz?"
- Hades pattern detay (body simple + weapon attached + VFX particle separate layer)
- CoM pattern (body detailed + weapon embedded + screen shake + VFX hit flash)
- HLD pattern (body geometric flat + weapon abstract + parallel impact slash sprite)
- RIMA için indie game research: hangi pattern hızlı production + güzel hissi verir?
- VFX library tavsiyesi (URP 2D shader graph, Unity Particle System, custom sprite anim)

### 5. Skill icon UI industry standard
- 22 cloud icon var, RIMA için UI draft (SkillOfferUI LIVE)
- Standart skill icon boyut (32×32 vs 64×64 vs 128×128)
- Tutarlı sanat tarzı için tavsiye

## Output
Inline response. 10 cümle özet + spesifik weapon canvas tablo + animasyon flow öneri.

## Dispatch
```
cd 'F:/Antigravity Projeler/2d roguelite/RIMA' && python agy_dispatch.py \
  --task-file STAGING/pixellab_analysis_agy_task.md \
  --print-timeout 1200 \
  --output STAGING/PIXELLAB_ANALYSIS_AGY.md
```

(Memory `feedback_agy_print_term_env_fix`, `feedback_codex_agy_profile_race` — Codex paralel çakışma önle. agy farklı profile kullanır.)

## YASAK
- File yazma agy'nin kendisi (orchestrator yazar)
- Opus/Codex verdict bakma
- Asset gen
