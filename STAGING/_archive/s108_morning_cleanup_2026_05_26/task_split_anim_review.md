# CODEX REVIEW TASK — SPLIT-ANIMATION TECHNIQUE DRAFT

## Görev
Aşağıdaki HARD RULE DRAFT'ını eleştirel olarak incele. Kod yazma yok. Sadece analiz ve yorum.

Şunlara odaklan:
1. **Üretim maliyet/değer dengesi** — 3 job (State + Part 1 + Part 2) vs 1 job tradeoff makul mu? Hangi animasyonlarda split kesinlikle değmez?
2. **Edge case'ler** — Apex frame belirsiz olduğunda ne olur? Death animasyonunda Karar #71 (weapon grip lock) Part 2 spec'inde nasıl güvence altına alınmalı?
3. **Aseprite workflow riskleri** — Apex frame "bir kez" kuralı kırılırsa ne olur? Frame count hatası nasıl önlenir?
4. **Frame bölme tablosundaki sayıları doğrula** — LMB2/LMB3/RMB/Death için Part 1 + (Part 2 - 1) = Total eşitliği doğru mu?
5. **Eksik edge case veya kural var mı?** — Opus'un gözden kaçırdığı bir şey görüyor musun?

Çıktın:
- PASS veya FLAG (her madde için)
- FLAG ise: ne değişmeli, alternatif öner
- Genel sonuç: taslak LOCKED'a hazır mı, yoksa revize gerektiriyor mu?

---

## DRAFT (Opus tarafından üretildi)

### SPLIT-ANIMATION TECHNIQUE (HARD RULE — DRAFT)

**Trigger:**
- **ZORUNLU:** Custom V3 üretiminde tek-pass frame budget >= 12f olan herhangi bir animasyon (Warblade: LMB2 12f, LMB3 14f, RMB 14f, Death 12f).
- **OPSİYONEL ama TAVSİYE:** 8-11f animasyonlarda apex pose net tanımlanabiliyorsa (orn. Dash 8f anticipation-launch-land yapisi). Uretici drift riskini degerlendirip secer.
- **YAPMA:** <= 8f basit animasyonlar (Idle 6f, Hurt 6f, Run 8f). Loop tabanli + apex-yok animasyonlarda split overhead fayda getirmez.

**Adımlar:**
1. **Apex pose tanımla.** Animasyonun anlamsal zirvesi (impact frame, max anticipation, peak extension). Tek bir frame olmali — eger iki "zirve" varsa animasyon tek mekanik degildir, ayri skill say.
2. **Apex State üret.** Create State ile 8 yon (5 gen + 3 mirror) apex pose, 20-40 gen Karar #108'e uygun. Pozitif-only spec, pixel amount, body part lock list zorunlu.
3. **Apex State'i lock et.** Uretici State'i onaylamadan Part 1/Part 2'ye gecilmez. State frame degisirse her iki parca yeniden.
4. **Part 1 üret (Anticipation → Apex).** Custom V3, **End Frame = Apex State**. 3 gen/dir, Karar #108.
5. **Part 2 üret (Apex → Recovery).** Custom V3, **First Frame = Apex State**. 3 gen/dir.
6. **Birleştirme.** Aseprite'da Part 1 tam dizisi + Part 2'nin apex haric (frame 2'den sonu). Apex frame **bir kez** sheet'te.
7. **Chain transition dogrula.** Part 1 son frame = Part 2 ilk frame pixel-identical olmali. State referansi kullanildiginda bu garanti, ama manuel diff zorunlu.

**Kısıtlar:**
- Apex pose belirsizse split ETME.
- Death animasyonunda Part 2 (recovery) spec: "weapon falls from grip" tetik kelimesi YASAK, "weapon grip locked through full recovery" pozitif vurgu zorunlu.
- 3+ parcaya bolme YASAK.
- Mirror yonler (W/NW/SW) State'ten degil, uretilmis Part 1/Part 2'den mirror'lanir.
- Part 1 ve Part 2 farkli drift kurallari kullanamaz.
- Apex State frame number tasarim dokumaninda lock'lanmali.

**Frame bölme tablosu (Warblade):**

| Anim  | Total | Apex # | Part 1 | Part 2 | Sheet total      |
|-------|-------|--------|--------|--------|------------------|
| LMB2  | 12f   | 6      | 6f     | 7f     | 6 + (7-1) = 12 v |
| LMB3  | 14f   | 7      | 7f     | 8f     | 7 + (8-1) = 14 v |
| RMB   | 14f   | 6      | 6f     | 9f     | 6 + (9-1) = 14 v |
| Death | 12f   | 4      | 4f     | 9f     | 4 + (9-1) = 12 v |
| LMB1  | 8f    | —      | SPLIT ETME | — | tek-pass 8f      |
| Dash  | 8f    | 3      | 3f     | 6f     | 3 + (6-1) = 8 v  |

**Sprite sheet birleştirme notu:**
- Aseprite: Part 1 (apex dahil) + Part 2 frame 2'den itibaren (apex skip).
- Naming: `f07_APEX` label zorunlu.
- Regenerate: State + Part 1 dokunulmaz, Part 2 swap edilir.
- Her yon icin Part 1 + Part 2 ayri .aseprite — final 8-row sheet sadece export'ta merge.
- Apex frame 8 yonde silüet tutarlilik manuel kontrol zorunlu.

---

## Mevcut Kilitli Bağlam

- Karar #108 LOCKED: Custom V3 4-16 frame, 3 gen/dir; Create State 20-40 gen
- Karar #114 LOCKED: 8 yon 5 gen + 3 mirror, tum animlar
- Karar #71 LOCKED: Death anim weapon grip korunur, "falls from grip" YASAK
- STATE-BASED KEYFRAMING (mevcut hard rule): karmasik anim oncesi Create State apex/anticipation pose 8 yon, First/End Frame referans
- Drift kurallari LOCKED: pozitif-only spec, pixel amount, body part lock, chain transition explicit
