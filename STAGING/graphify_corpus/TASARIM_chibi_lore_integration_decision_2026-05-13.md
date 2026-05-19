---
status: LOCKED
faz: 1
tarih: 2026-05-13
ozet: "Kararlar #77-80 chibi+lore entegrasyon kararları"
---
# Chibi Visual + RIMA Lore Integration -- Final Karar (2026-05-13)

**Status:** CANDIDATE -- 4 LOCKED proposals + 1 contradiction fix + 2 NEW decisions to be added to MASTER_KARAR_BELGESI as #77-#80.
**Authors:** rima-design (Opus) + Codex (gpt-5.5, xhigh reasoning) review.
**Locked rules respected:** Karar #30 Fractured Epic, #71 weapon single-state, #72 Pure 2D Top-Down, #73-76 S59 pivot family.

---

## Ozet

RIMA chibi 64x64 (S59) ile "Fractured Epic" dramatik+canli tonu (Karar #30) uzlasi NOT via "HLD dread" but via **"Vivid Vulnerability"** -- kucuk canli karakter / monumental dusman sistem kontrastı. Boss intro, Architect reveal ve 3 ending icin **premium static portrait tier** v1'de SADECE Architect (+ optional ending illustration); diger bosslar icin **title card / boss sigil** sistemi. NPC hub diyalog portrait modal v1 SCOPE CUT (Codex flag). Tone surfaces (death/run/codex/class-select) bir standarda baglanir; 10 sinif chibi diferansiyasyon "Class Silhouette Bible" ile korunur. Final Boss scale **2.5x player (not 4x)** -- Karar #73-76 ile uyumlu.

---

## 7 Soruya Yanit

| # | Soru | Karar | Sebep | Action item |
|---|------|-------|-------|-------------|
| 1 | Chibi + Fractured Epic tonal model? | **Vivid Vulnerability** (HLD dread DEGIL). Kucuk canli kahraman / vast hostile sistem. Hades-style theatrical mythic framing temel referans. | Codex feedback: HLD restraint = Karar #30 "dramatic+vivid" ile celisir. Vulnerability dramayi tasiyabilir ama silence/distance tasimaz. | TASARIM/VISUAL_TONE_BIBLE.md guncelle: "Vivid Vulnerability" terim, Hades + Salt and Sanctuary (chibi-but-serious) ikili referans. HLD ref CAUTION listesine. |
| 2 | Boss intro / Architect / ending cinematic tier? | **Premium static portrait tier**, v1'de SADECE Architect reveal + 1 ending illustration. Diger 3 boss = title card + sigil + 1-frame silhouette reveal. | Codex: 8-12 portrait v1 imkansiz (10 sinif + 6 mob + 4 boss zaten asagi yukari yuklu). Portrait = scope multiplier, sadece premium moment. | Cinematic Layer D revize: "portrait cut-in" sadece Architect; F1-F3 bosslar = title-card+sigil sistem (asset budget: 4 sigil + 4 title font lockup). |
| 3 | UI/Tone language? | Serif lore + sans HUD + neon-on-mat gameplay + parchment/glass UI **ONAYLI** ama "terse archaic/dread prose" REVIZE: dramatic, character-voiced, vivid metafor (Hades style monologue), arkaik DEGIL. | Codex: terse/archaic prose dark fantasy/grimdark dogru, Karar #30 forbid. | Prose writer'a brief: "vivid first-person character voice", ornek Hades Achilles/Nyx dialogue, NOT HLD codex fragment. |
| 4 | 32x32 tile env storytelling? | **Decal pass MANDATORY** ama icerik "blood/horror" DEGIL -> **ritual catastrophe**: void cracks, broken seals, fading sigils, ash drift (renkli), engine sparks, statue shards, ancient script. Lighting: rim + flicker point + low-fog. | Codex: blood/ash/low-fog grimdark drift riski. "Vivid fracture/ritual catastrophe" framing icinde tutulmali. | Decal art brief: 16-24 decal v1, "rift-aftermath" tematic (blood DEGIL, void crack + sigil break + engine residue), neon glow OK. |
| 5 | NPC + Hub atmosphere? | **Portrait modal NPC sistem v1 CUT** (Codex). Hub diyalog = chibi sprite + bubble text + UI namecard. Sadece Architect + 1 ending NPC portrait modal. Atmosfer = env decay + cold light + scattered NPC blocking. | Codex: 8-12 portrait imkansiz, en buyuk scope multiplier; chibi+bubble yeterli. | Portrait budget v1 = 2 (Architect + 1 ending). Hub UI namecard sistem yeterli. Post-v1: portrait expansion. |
| 6 | Architect reveal? | **Sprite 256x256 = ~2.5x player (Karar #73-76 ile uyumlu, 4x DEGIL)**. Reveal stack = 1 static portrait reveal + gameplay freeze/darken + 1 screen crack VFX + 1 boss title line. **Parallax + letterbox + phase cut-in cut for v1.** | Codex LOCKED RULE FLAG: 4x player Karar #73-76'yi ihlal. + parallax+letterbox+phase cut-in v1 scope asar. Tek excellent reveal > tekrarli kompleks reveal. | Karar metni duzelt: "256 canvas, PPU=64, 2.5x player". v1 reveal: 1 portrait + freeze + crack VFX + title line. Phase 2-3-4 transition = title card only (no portrait re-cut-in). Parallax post-v1. |
| 7 | Risk listesi? | Cute-read mitigation onayli (mat palette + grounded anim + readable silhouette). + Tone surface standardi (Decision 8) ekle. + Class silhouette bible (Decision 9) ekle. | Codex Decision 7 LOW risk ama incomplete -- tone leakage death/run/codex'te. | Decision 8 ve 9 yazilacak (asagida). |

---

## Yeni Tasarim Direktif

### Karar Onerileri (MASTER_KARAR_BELGESI'ne #77-#80 olarak)

**KARAR #77 (CANDIDATE -- LOCKED'a tasinmaya hazir): Vivid Vulnerability Tonal Model**
Chibi 64x64 + Fractured Epic uzlasi = **Vivid Vulnerability**. Kucuk canli kahraman / vast hostile sistem kontrastı. Tone reference TOP: Hades theatrical mythic + Salt and Sanctuary chibi-but-serious. HLD dread, Hammerwatch ironic-cute, Don't Starve macabre-whimsical CAUTION listesinde (NOT primary). Drama scale-contrast ve color-vividness'tan gelir, restraint/silence'tan DEGIL.

**KARAR #78 (CANDIDATE): Premium Cinematic Portrait Tier -- v1 Scope**
Portrait illustration tier (~512x768, PixelLab Pro base + manual paint-over) v1'de SADECE:
- 1 Architect reveal portrait
- 1 ending illustration (final decision moment)

Diger boss intro = title card + boss sigil (256x256) + 1-frame silhouette reveal. NPC dialogue v1 = chibi sprite + bubble text + namecard. Portrait modal NPC sistem POST-V1.

**KARAR #79 (CANDIDATE -- NEW from Codex): Tone Surfaces Standard**
Tum non-combat text surface'leri Fractured Epic + Vivid Vulnerability tonu takip eder: death screen, run summary, class select, codex unlock, boss title, achievement, ending choice UI, loading tip. **YASAK:** joke names, cozy tavern language, generic grimdark despair, ironic-cute. **MANDATE:** dramatic, character-voiced, vivid metafor, ritual gravity. Owner: rima-doc + rima-design joint review.

**KARAR #80 (CANDIDATE -- NEW from Codex): Class Silhouette Bible**
10 sinif paylasilan 64x64 chibi proportion altinda diferansiyasyon zorunlu. Her sinif icin LOCKED 64x64 identity profile:
- Weapon silhouette (single-state, Karar #71'e bagli)
- Head/shoulder shape (helm, hood, mantle gibi)
- Accent color (palette icinde 1-2 signature hex)
- Idle posture (combat-ready, casting, stalking gibi)
- Animation energy (heavy/light/fluid/jagged)
- VFX motif (1 ait skill VFX karakter)

v1 4 ana class (Warblade/Ranger/Shadowblade/Elementalist) icin LOCKED; kalan 6 sinif v2 portfolio. Owner: rima-design + rima-asset joint, dokuman TASARIM/CLASS_SILHOUETTE_BIBLE.md.

### Karar Duzeltme (Locked Rule Fix)

**Karar #73-76 reference fix:** Final Boss = 256 canvas, PPU=64, **~2.5x player size** (4x DEGIL). Tum boss documents bu siralamayi reflect etmeli. Daha buyuk hissi composition'dan (portrait reveal + freeze + VFX + title), sprite scale'inden gelmez.

---

## Riskler ve Mitigasyon

| Risk | Olasilik | Etki | Mitigasyon |
|------|----------|------|------------|
| "Vivid Vulnerability" producer/codex/asset agent'lara karistirilir, HLD dread'e geri donulur | ORTA | HIGH (ton kayar) | TASARIM/VISUAL_TONE_BIBLE.md acik referans + her asset brief'inde tone keyword |
| Title card + sigil sistemi "ucuz" durur, boss intro impact eksik | ORTA | MED | Title card animasyon brief: 2-3 sec dramatic reveal, sigil glow + screen shake + audio cue. Asset budget: 4 sigil + audio. |
| Decal pass grimdark'a kayar (Codex R6 endisesi) | ORTA | HIGH | Decal brief: "ritual catastrophe" tematic mandate, blood/horror YASAK, palette neon glow OK. Review gate: rima-qc + rima-design. |
| Architect reveal "ucuz" durur (parallax/letterbox cut'tan sonra) | DUSUK | MED | 1 reveal excellence: PixelLab Pro portrait + manual cleanup 30-60dk + screen-crack shader iyi yap (TimeWarp + chromatic split). |
| 10 sinif chibi'de class-fantasy okumuyor (Codex R6c) | ORTA | HIGH | Decision 9 Class Silhouette Bible v1 4 sinif icin LOCKED gate. Playtest gate: "blind silhouette test" -- siluet only sinif tahmin %80+. |
| Tone leakage death/run/codex screen'inde (Codex R6b) | YUKSEK (default risk) | HIGH | Decision 8 Tone Surfaces Standard zorunlu. rima-doc tum text surface'i review eder, joke/cozy/grimdark filter passes. |
| Portrait pipeline (PixelLab Pro + manual paint) drift -- inconsistent style across 2 portrait | DUSUK (sadece 2 v1) | MED | Style guide: head proportion, palette, brush style, light direction LOCKED single brief. Same artist hand on both. |

---

## Codex Review Notlari

**Codex ONAYLADI:**
- Portrait tier kavrami (premium boss/ending layer olarak)
- 2D portrait Karar #72'yi ihlal etmiyor (3D/billboard/2.5D degil)
- Decal pass kavrami (ama icerik framing'i revize ile)
- Decision 7 risk mitigations (eksik ama sensible)

**Codex ITIRAZ ETTI / DUZELTTI:**
- LOCKED RULE FLAG: Decision 6 Final Boss "4x player" YANLIS -- Karar #73-76 ile 2.5x player olmali. DUZELTILDI.
- "HLD-leaning dread" tonal framing Karar #30 ile celisir (HLD restraint vs Hades vivid). YENIDEN ADLANDIRMA: Vivid Vulnerability.
- Decision 5 (8-12 portrait NPC system) v1 SCOPE OVERBLOWN. CUT TO 2 portrait (Architect + ending).
- Decision 6 cinematic stack (parallax + letterbox + phase cut-in) v1 SCOPE OVERBLOWN. SIMPLIFIED TO 1 excellent reveal.
- Decision 4 decal/light/prose drift grimdark riski. FRAMING REVISED: ritual catastrophe (blood/horror DEGIL).

**Codex EKLEDI:**
- Decision 8 Tone Surfaces Standard (kabul edildi, #79)
- Decision 9 Class Silhouette Bible (kabul edildi, #80)
- R6 missed systems: death screen, run summary, codex unlock, boss defeat, class select, achievement, loading tip (Decision 8 kapsamina alindi)

---

## Sonraki Adimlar

1. **Orchestrator -> rima-doc**: 4 yeni CANDIDATE kararı MASTER_KARAR_BELGESI'ne #77-#80 olarak ekle. Karar #73-76 referans satirini Final Boss "2.5x player" olarak duzelt (eger 4x yazan satir varsa).
2. **Orchestrator -> rima-doc**: TASARIM/VISUAL_TONE_BIBLE.md yeni dosya, "Vivid Vulnerability" tonal model, Hades + Salt and Sanctuary primary ref, HLD/Hammerwatch/Don't Starve caution.
3. **Orchestrator -> rima-doc**: TASARIM/CINEMATIC_LAYER_v1.md guncelle, Layer D "boss intro frames" -> "premium portrait (Architect+ending) + title-card/sigil (other bosses)".
4. **Orchestrator -> rima-design + rima-asset** (joint): TASARIM/CLASS_SILHOUETTE_BIBLE.md taslak, v1 4 class profile (Warblade/Ranger/Shadowblade/Elementalist) LOCKED, 6 class v2.
5. **Orchestrator -> rima-doc**: TASARIM/TONE_SURFACES_STANDARD.md, death/run/codex/class-select/boss-title/achievement/ending/loading-tip text rules.
6. **Orchestrator -> rima-asset**: Decal pack brief (16-24 decal, "ritual catastrophe" tematic, palette uyumlu). PixelLab MCP create_isometric_tile veya manuel pixel art.
7. **Orchestrator -> rima-asset**: Architect portrait brief (PixelLab Pro 1024px base + manual paint 1-2 hour, 512x768 final). Style guide LOCKED single brief.
8. **Orchestrator -> rima-asset**: 4 boss sigil + title card brief (256x256 sigil + font lockup).
9. **Playtest gate (v1 alpha)**: "Blind silhouette test" -- 4 sinif siluet only, tester %80+ class tahmin etmeli. Class Silhouette Bible validation.
10. **Playtest gate (v1 alpha)**: Tone surface walkthrough -- her text surface'i (death/run/codex/class-select/achievement) Fractured Epic + Vivid Vulnerability tonu tasiyor mu? rima-qc gate.

---

## Karar Sayisi Sonrasi

MASTER_KARAR_BELGESI su an son #76. Bu dokuman 4 yeni karar onerir:
- #77 Vivid Vulnerability Tonal Model
- #78 Premium Cinematic Portrait Tier
- #79 Tone Surfaces Standard
- #80 Class Silhouette Bible

Tumu CANDIDATE; kullanici onayindan sonra LOCKED'a tasinir.

