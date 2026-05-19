# ANIMATION SPLIT TECHNIQUE
> Karar #120 LOCKED 2026-05-13 -- bkz. MASTER_KARAR_BELGESI.md

---

## TRIGGER (MANDATORY -- her ikisi gerekli)

**Uygula:**
- A: Frame budget >= 12f
- B: Net tanimlanmis tek apex frame var (gameplay/visual kimlik olan: impact, death collapse beat, dash peak)

Her ikisi de saglaniyorsa → Split-Animation pipeline ZORUNLU.

**Uygulama:**
- A veya B eksikse → single-pass Custom V3 (Karar #108 standart) yeterli.

---

## TRIGGER (FORBIDDEN -- bu durumlarda split YASAK)

- Loop / cycle animasyonlar (idle, run-loop) -- apex kavrami gecersiz
- Apex-yok animasyonlar (smear, motion-blur, generic recovery)
- Multi-peak animasyonlar (2+ distinct apex) -- ayri anim spec veya single-pass
- Apex belirsizse → bkz. Apex Disambiguation Fallback (asagida)

---

## APEX DISAMBIGUATION FALLBACK

Apex belirsiz gorunuyorsa, bu sirayla karar ver:

1. Gameplay anchor var mi? (hit frame, SFX trigger, camera shake, cancel window) → o frame apex.
2. Visual drama zirvesi hangisi? (en genis silhouet, max weapon extension, max weight shift) → o frame apex.
3. Hala belirsizse → single-pass Custom V3 VEYA animasyonu mekanik beat'lere bol (her beat'in net apex'i olabilir).
4. Sentetik apex YASAK -- "ortada bir frame seceyim" kabul edilmez.

---

## 3-STAGE PIPELINE

### Stage 1 -- Apex State (Create State)

- Hedef: apex pose'u 8 yon icin kilitle (Karar #114: N/NE/E/SE/S + 3 mirror).
- Tool: PixelLab Create State
- Gen count: 20-40 gen (Karar #108)
- Canvas / pivot / palette: Karar #100 (124x124, 35 derece, chibi)
- Cikti: apex_state_id per direction + apex_pixel_hash per direction (her yonun son pixel hash'i)
- Kaydet: STAGING/ANIM_OUTPUT/{CharName}_{AnimName}_apex_state.json

### Stage 2 -- Part 1 (Anticipation → Apex)

- Tool: PixelLab Custom V3
- Frame range: frames 1..APEX (APEX dahil)
- End Frame: Stage 1 Apex State (apex_state_id)
- Canvas / pivot / row-order / direction naming: Apex State ile ayni
- Prompt: motion description + Karar #71 weapon lock bloku (asagida) + body-part lock list (Karar S67 drift rules)
- Gen count: Karar #108 (3 gen/dir minimum)

### Stage 3 -- Part 2 (Apex → Recovery)

- Tool: PixelLab Custom V3
- Frame range: frames APEX..N (APEX dahil, N = son frame)
- First Frame: Stage 1 Apex State (apex_state_id)
- Canvas / pivot / row-order / direction naming: Part 1 ile ayni (fark YASAK)
- Prompt: ayni style constraints + drift kit + palette + body-part lock. Part 1 ile prompt tutarli.
- Gen count: Karar #108 (3 gen/dir minimum)

---

## KARAR #71 WEAPON LOCK (POZITIF-ONLY)

Weapon animasyonu iceren her iki parca icin zorunlu prompt bloku:

```
weapon gripped and attached every frame, weapon hand fingers and wrist fixed on haft/hilt every frame,
during body collapse weapon follows collapsed hand as one rigid unit, grip contact point fixed on weapon haft/hilt every frame
```

**YASAK kelimeler (prompt'ta HICBIR YERDE kullanma):**
drop, release, slip, fall (of weapon), throw, separation, let go, weapon falls, weapon flies, weapon detaches, weapon on ground

---

## ASEPRITE ASSEMBLY

1. Part 1 full acil: frames 1..APEX (APEX frame dahil)
2. Part 2'den frame 2..N al (frame 1 = apex, zaten Part 1'den mevcut -- tekrarlama YASAK)
3. Birlestir: [Part 1 full] + [Part 2 frames 2..N]
4. Toplam = N (apex bir kez sayilir)
5. Label: fNN_APEX (NN = apex frame numarasi, ornek: f07_APEX)

---

## EXPORT CHECKLIST (5 MADDE -- PASS OLMADAN COMMIT YOK)

- [ ] 1. Part 1 son frame hash == Part 2 ilk frame hash (per direction) -- pixel-level match
- [ ] 2. Final sheet frame count per row == N (toplam anim frame sayisi)
- [ ] 3. Exactly 1 fNN_APEX label per direction (NN = locked apex frame numarasi)
- [ ] 4. Frame order: Part 1 full + Part 2 frames 2..N
- [ ] 5. Apex frame hash == apex_pixel_hash from Stage 1 kayit

---

## EK KURALLAR

**Gameplay timing:**
- Apex frame = hit frame / SFX trigger / camera shake / cancel window anchor.
- Apex frame numarasi kod tarafinda referenced olmali (AnimEventData veya benzeri).

**Pivot / canvas lock:**
- Part 1 + Part 2 + Apex State: ayni canvas boyutu, ayni origin, ayni pivot, ayni row order, ayni direction naming.
- Herhangi birinde fark = QC FAIL.

**Prompt / version lock:**
- Part 1 ve Part 2 ayni style constraints, drift kit, palette, body-part lock list kullanmali.
- Prompts STAGING/'e kaydedilmeli (Karar feedback: PixelLab Prompts → Dosya).

**Regeneration kuralı:**
- Part 2 yeniden uretilirse → yeni Part 2 frame 1, apex_pixel_hash ile match etmeli (Stage 1 hash referansi).
- Part 1 yeniden uretilirse → Stage 1 Apex State degismez, Part 1 End Frame yeniden apex_state_id ile kilit.

**Mirror QC (Karar #114):**
- 5 gen yonu + 3 mirror: her mirror icin apex-once kontrol et + chain-diff pass.

**Multi-hit veya combo chain:**
- Birden fazla peak olan combo'lar (LMB1 → LMB2 → LMB3): her anim ayri spec.
- LMB2/LMB3 gibi chained animlar: chain transition explicit (Beat N son frame = Beat N+1 frame 1, pixel match).

---

## FRAME MATH -- WARBLADE ORNEGI

| Anim | N | Apex | Part 1 | Part 2 | Sheet |
|------|---|------|--------|--------|-------|
| LMB2 | 13 | 7  | 1..7 (7f)  | 7..13 (7f)  | 13 |
| LMB3 | 14 | 8  | 1..8 (8f)  | 8..14 (7f)  | 14 |
| RMB  | 12 | 6  | 1..6 (6f)  | 6..12 (7f)  | 12 |
| Death| 16 | 10 | 1..10 (10f)| 10..16 (7f) | 16 |

Sheet = N cunku apex bir kez sayilir.
Part 1 (N_p1 frames) + Part 2 (N_p2 frames) = N_p1 + N_p2 - 1 = N (overlap = 1).

---

## ILGILI KARARLAR

- Karar #71: Weapon lock pozitif-only (STRENGTHEN -- bu teknik #71 enforcement'i zorunlu kilar)
- Karar #108: Custom V3 frame budget, gen count, Create State gen count (INHERIT)
- Karar #109: Ambient idle anim sistemi (UNAFFECTED -- idle loop bu teknikten muaf)
- Karar #114: 8 yOn animasyon (INHERIT -- her stage 8 yon uygular, 5 gen + 3 mirror)
- Karar #119: AI ASCII Matrix Parser -- animation pipeline ile baglantisiz, scope ayrimi korunur

---

*Son guncelleme: 2026-05-13 S67*
