# Weaponless Animation + Weapon Production + Mount System Plan

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## ROL
Sen Sonnet orchestrator-subagent (NEW HARD `feedback_sonnet_default_opus_exception` — Sonnet default, plan/analiz/orchestration yapar). Triple-AI flow SENİN içinde:
- Sen Sonnet: design + sentez
- agy dispatch (research)
- Codex dispatch (codebase reality, profile race riski memory `feedback_codex_agy_profile_race`)

## Amaç (kullanıcı verbatim 2026-05-28)
> "silahları karakterlerin eline koyacağımız için plan yapmamız laızm. silahsız aniamsyonları yapacaz ona göre detaylı güzel anlatmak lazım. eline oturtacaz silahı. bunlardan önce bi silahı eline tutturmak için silahlarını üretelim. bunu da çalıştık galiba silahları kaç piksel olacağı nasıl bağlanacağını bunları tekrar netleştirt 3 agentla ona göre son rapor ver. yapılacak listesine doğru şekilde koy ki yapalım"

3 birbirine bağlı karar:
1. **Silahsız animasyon production** — karakter prompt'larından silah çıkar, eller boş (idle pose, attack swing, etc.)
2. **Silah asset üretimi** — Warblade greatsword + diğer 9 class silahları. Piksel boyutu + canvas + sprite spec
3. **Eklem noktası (HandAnchor) bağlama sistemi** — silahı eline nasıl mount eder, kaç piksel offset, 8-dir rotation

## Bağlam — MEVCUT LIVE sistem

### Weapon decouple pipeline (Karar #144 + #123 + #146 LOCK — memory ref)
- `MEMORY: project_weapon_pipeline_lock.md` — decouple LOCKED 2026-05-13
- `MEMORY: project_weapon_system_8dir_lock.md` — S100 LOCK: 1 sprite/weapon, HandAnchor + OrientationSync + WeaponSorter
- Body sprite **weaponless** (silahsız)
- Child SpriteRenderer + HandAnchor 8-dir bağlama
- Silah TEK sprite, 8 yön Unity SpriteRenderer + rotation/flipX
- LIVE asset: Warblade longsword PixelLab id `441bccf0`, Ronin katana `692f43ce`

### Mevcut Unity altyapı (codebase verify et)
- `Assets/Scripts/.../HandAnchor.cs` — eklem noktası transform
- `Assets/Scripts/.../WeaponSorter.cs` — sort order management
- `Assets/Scripts/.../OrientationSync.cs` — 8-dir rotation
- `Assets/Scripts/.../PlayerAttack.cs:142` — BasicAttackProfile null NullRef (S111 carry, scope dışı şu task'te)

### Animation catalog mevcut (S113 LIVE)
- `STAGING/ANIMATION_PROMPT_CATALOG.md` — 11 anim Warblade
- Karakter prompt'unda silah var: "two-handed greatsword, ... weapon stays firmly in hands"
- **GÜNCELLEME ZORUNLU:** Silahsız production için prompt'lar revize edilmeli
  - Eller "empty" / "open hands" / "fists clenched" pozisyonu
  - Weapon lock rule kaldırılır (weaponless'ta gereksiz)
  - Animation hand position ile silah eklem noktası uyumlu olmalı

### PixelLab V3 gen cost (S114 LIVE)
- `MEMORY: reference_pixellab_v3_gen_cost_by_frame.md`
- 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen per dir

## Triple-AI içerik (SENİN içinde)

### agy dispatch — external research
- Silahsız chibi 2D animasyon best practice (Hades/CoM/Diablo III sprite layering)
- Weapon overlay sistemi industry pattern (Spine/DragonBones bone attachment vs static sprite child)
- HandAnchor 8-dir math (per-direction pivot offset, rotation angle)
- Silah sprite spec: chibi karakter için ortak ratio (Hades sword ~%30 character body height)
- 2-hand weapon vs 1-hand weapon mount farkı (chibi greatsword vs dagger)
- "Weaponless body + weapon overlay" prompt formulation PixelLab v3'te

Dispatch (profile race önle — Codex paralel YAPMA):
```
cd 'F:/Antigravity Projeler/2d roguelite/RIMA' && python agy_dispatch.py \
  --task-file STAGING/WEAPONLESS_PLAN_agy.task.md \
  --print-timeout 800
```

### Codex dispatch — codebase reality (agy SONRA dispatch et)
- `Assets/Scripts/Player/PlayerAttack.cs` HandAnchor reference path
- `Assets/Scripts/.../HandAnchor.cs` 8-dir offset table
- `Assets/Scripts/.../OrientationSync.cs` rotation logic
- `Assets/Scripts/.../WeaponSorter.cs` sort order
- Warblade prefab structure: Body (SpriteRenderer) + HandAnchor (Transform) + Weapon (child SpriteRenderer)
- Mevcut weapon asset envanteri:
  - `Assets/Art/Weapons/Warblade_Longsword.png` veya benzer path?
  - `Assets/Art/Weapons/Ronin_Katana.png`?
  - Diğer 8 class silah var mı?
- Weapon canvas/PPU spec — chibi 120×120 body için optimal weapon canvas? (~64×96 mi, ~32×128 mi?)
- HandAnchor per-dir offset değerleri (8 yön)
- LOC estimate her component için

Dispatch:
```
python cx_dispatch.py --task-file STAGING/WEAPONLESS_PLAN_codex.task.md --effort xhigh
```

### Sen Sonnet sentez

## Output dosyası: `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md`

### Bölüm 1 — Silahsız Animation Prompt Update
ANIMATION_PROMPT_CATALOG.md'deki her anim için **weaponless variant** prompt:
- [CHARACTER] blok güncel: "two-handed greatsword" YERİNE "empty hands / fists / open palms"
- [ACTION] blok güncel: "greatsword raised high" YERİNE "right hand raised high (weapon mount point)" — silah orada olacak ama prompt'ta yok
- [CONSTRAINTS] güncel: WEAPON LOCK kaldır (silah yok zaten), "no held items, hands empty" ekle
- Hand position critical: HandAnchor'ın eline iyi yerleşmesi için her frame'de **el pozisyonu net** olsun

Örnek update (Basic Attack):
```
[CHARACTER]: 64x64 chibi top-down character, male heavy warrior, dark steel armor uniform...
EMPTY HANDS, open palms ready to grip weapon, no held items.
view 35 degree high top-down ARPG angle.

[ACTION]: Frame 1: right arm raised high above right shoulder, hand gripping invisible weapon pose. 
Frame 4 (apex): right arm fully extended in wide horizontal arc, hand open as if releasing strike.
Frame 8: right arm low at right side, hand returning to rest position.

[CONSTRAINTS]: 2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
don't fill canvas. NO weapons, NO held items, hands empty throughout.
```

Her 11 anim için weaponless variant hazırla (Tier 1 + Tier 2).

### Bölüm 2 — Silah Asset Production Spec

| Silah | Class | Pixel canvas | Pivot point | Mount type | PixelLab cost est |
|---|---|---|---|---|---|
| Greatsword | Warblade | TBD (agy ref) | grip handle midpoint | 2-hand | 1-3 gen |
| Katana | Ronin | TBD | handle bottom | 1-hand | 1-3 gen |
| Pistol/SMG | Gunslinger | TBD | grip | 1-hand | 1-3 gen |
| Bow | Ranger | TBD | grip center | 2-hand | 1-3 gen |
| Staff | Elementalist | TBD | handle bottom | 2-hand | 1-3 gen |
| Dagger | Shadowblade | TBD | grip | 1-hand (dual) | 1-3 gen × 2 |
| Greataxe | Ravager | TBD | handle midpoint | 2-hand | 1-3 gen |
| Whip | Hexer | TBD | grip | 1-hand | 1-3 gen |
| Gauntlets | Brawler | TBD | wrist | 2× 1-hand | 1-3 gen × 2 |
| Tome+Orb | Summoner | TBD | tome center + orb hover | 1-hand each | 1-3 gen × 2 |

**Production sırası:**
1. **Faz 1 Demo öncelik:** Warblade Greatsword (LIVE 441bccf0 verify) — sadece bu Faz 1 için yeter
2. **Faz 4:** 9 class silah sıra Elementalist → Ranger → Shadowblade → diğerleri

### Bölüm 3 — HandAnchor Mount System

8-dir per-direction offset table — Warblade greatsword için:
```
| Direction | Anchor offset (px) | Weapon rotation | flipX |
|---|---|---|---|
| South | (3, -2) | 0° | false |
| SE | (4, -1) | 30° | false |
| East | (5, 0) | 60° | false |
| NE | (4, 1) | 90° | false |
| North | (3, 2) | 120° | false |
| SW | (-4, -1) | -30° | true (mirror) |
| West | (-5, 0) | -60° | true (mirror) |
| NW | (-4, 1) | -90° | true (mirror) |
```
(Values from Codex codebase verify — placeholder ise revise)

### Bölüm 4 — TODO Listesi (yapılacaklar)
1. ⏳ Warblade body weaponless verify (mevcut warblade_south.png silahlı mı silahsız mı? Hand position uygun mu?)
2. ⏳ Eğer silahlı: yeni weaponless body anim üretimi GEREKLİ — ANIMATION_PROMPT_CATALOG yeniden Phase 1
3. ⏳ Warblade greatsword (441bccf0) Unity import + HandAnchor mount test
4. ⏳ 8-dir HandAnchor offset table verify (mevcut codebase'den çek)
5. ⏳ PlayMode test: weaponless body + greatsword mount + 8-dir rotation
6. ⏳ Faz 1 demo öncelik: SADECE Warblade greatsword (9 class silah Faz 4)

### Bölüm 5 — Risk + Open Question
- Mevcut warblade_south.png **zaten silahlı** olabilir — eğer öyleyse tüm animasyon production yeniden başlar (weaponless variant gerekli)
- HandAnchor offset values mevcut codebase'de mevcut mu yoksa boş mu?
- Weapon canvas spec — chibi 120×120 body için optimal weapon boyutu (Codex/agy research gerek)
- PixelLab V3 weapon-only sprite gen mi yoksa create_character_state ile mi (silahlar character değil, separate sprite)

## YASAK
- Asset gen (gece halt rule, PixelLab gen YASAK)
- Disclaimer refusal
- Mevcut WeaponSorter/HandAnchor/OrientationSync core logic değişiklik (LIVE)
- Scene mutation
- Animasyon production trigger (sadece plan, kullanıcı manuel başlatır)

## Süre
1.5-2 saat triple-AI flow. Dispatch'ler sıralı (agy önce, Codex sonra — profile race önle).

KAPADIN: `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md` + orchestrator özet (15 cümle) + TODO listesi (yapılacaklar net).
