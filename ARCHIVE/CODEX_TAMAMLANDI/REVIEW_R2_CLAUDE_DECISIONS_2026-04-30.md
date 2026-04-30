# Round 2 Review — Claude'un Skill/Class Sistem Kararlari

Date: 2026-04-30
Status: REVIEW_PROMPT — Hem ChatGPT hem Codex'e ayri ayri verilecek
Inputs:
- Round 1 ChatGPT feedback: STAGING/CHATGPT_YORUMU_2026-04-29.md
- Round 1 Codex evaluation: STAGING/CODEX_DEGERLENDIRME_CHATGPT_YORUMU_2026-04-29.md

---

## Senin Gorevin (Reviewer)

Round 1'de feedback / degerlendirme yaptin. Claude artik karar verdi. Bu Round 2'de:

1. Kararlari rubber-stamp etme. Zayif gordugun karari soyle.
2. Genel onay yerine spesifik karsi-argument ver.
3. Kor noktalari, abuse yollari, gozden kacan sistemik riskleri yakala.
4. Scope riski varsa isaretle (Faz 1 demo zamanlamasi onemli).

Cikti formati:
- Numarali madde madde.
- Her madde: KATILIYORUM / KISMEN / ITIRAZ + gerekce + alternatif (varsa).
- Yeni sistemik risk goruyorsan en sona "EKLEME RISK" basligi altinda yaz.

---

## Round 1 Ozet

### ChatGPT Ana Bulgular
- Cross-class sisteminin state tabanli olmasi gerek
- Execute skill'leri HP<%30 klisesinden cikmali
- Counter ayrimi (Warblade/Ronin/Brawler) zorunlu
- Movement skill'leri (Shadowblade/Ranger/Ronin/Gunslinger) ayrilmali
- Generic damage buff'lar redesign
- Brawler rotation-lock, Hexer linear stack loop
- Cursor AoE/zone fazlaligi (5 sinif)
- Cross-class "+%10 damage" pasif yerine state-based davranissal cevap

### Codex Ana Itirazlar
- Direkt revizyon listesine cevirme — once "Class State Contract" yaz
- 10 sinifta paralel redesign riskli
- Warblade Faz 1 demo, asiri bozma
- Phase class'ta birden fazla phase araci kabul edilebilir (farkli rol/maliyet sart)
- Implementation maliyeti tartilmali (Faz 1 / Faz 2 / later)
- Enemy design baglantisi eksik (encounter sorulari listesi gerek)

---

## CLAUDE'UN R2 KARARLARI

### A. Kabul Edilen Cekirdek Kararlar

**A1. State-based cross-class sistemi ana cati olur.**
- Her sinif 2-3 export state uretir.
- Diger siniflar bu state'lere davranissal cevap verir.
- "+%10 damage" pasif yasak.
- Yapilacak: TASARIM/CLASS_STATE_CONTRACT.md (yeni dosya).

**A2. Execute klisesi kalkar — HP<%30 yerine state-gate.**
| Class | Execute | Gate |
|---|---|---|
| Warblade | Death Blow | Broken / Sundered / Staggered hedef |
| Ranger | Final Strike | Marked + trap-triggered hedef |
| Gunslinger | Deadshot | Son mermi / perfect reload / line aim |
| Ronin | Flash Draw | Tension full + draw window |
| Shadowblade | Severance | Scar collapse sonrasi |

**A3. Counter ayrimi kilitli.**
- Warblade: absorb / break / dominate (Broken/Sundered uretir)
- Ronin: wait / draw / punish (Tension + draw window)
- Brawler: weave / evade / body-shot (perfect timing)

**A4. Movement ayrimi kilitli.**
- Shadowblade: phase + Scar
- Ranger: distance keep + trap/mark setup
- Ronin: olculu adim + draw timing
- Gunslinger: slide/reload/shoot ritmi
- Saf "dash + damage" tek basina sebep degil.

**A5. Brawler Charge tek rol secer.**
Ya skill amplifier YA hype/Overdrive motoru — ikisi birden DEGIL.

**A6. Hexer stack davranissal hale gelir.**
Sadece sayi degil, dusman behavior bozar (heal alirsa patlar, dash atarsa yayar, muttefikine zarar verir).

**A7. Summoner abuse kilitleri.**
Internal CD + minion cap + sacrifice recovery + post-sacrifice summon capacity penalty.

**A8. Gunslinger execute konsolidasyonu.**
Deadshot kalir (uzak, line aim).
Point Blank Execute Hip Shot'in yakin-mesafe finisher branch'ine kaynar.

**A9. Codex 5-line test standart.**
Her skill review icin:
1. Verb (hangi class verb'i?)
2. State (uretir/tuketir hangi state?)
3. Slot reason (4/6 slotta neden secilsin?)
4. Overlap (ayni isi yapan baska skill var mi?)
5. Abuse (resource loop / cross-class abuse riski var mi?)

### B. Warblade Faz 1 Demo Protokolu
- Core CC ve engage/break/execute feel KORUNUR.
- Iron Crush REDESIGN: duz buff yerine Broken hedefte armor shard / catlak yayma.
- Death Blow gate: HP<%30 -> Broken/Sundered/Staggered/Rift-marked.
- Iron Charge, Sunder Mark, Earthsplitter, Iron Counter, Death Blow fantasy korunur.

### C. Reddedilen / Faz 2'ye Itilen
- Prism Beam'in Frost Wall'dan kirilarak yon degistirmesi → Faz 2 (implementation maliyeti).
- Boss saldirisinin Hex ile geciktirilmesi → Faz 2 (encounter design eksik).
- Smoke Veil enemy aggro tasarimi → Faz 2 (enemy taxonomy/lock-on sistemi gerek).
- Lightbreak sistem degisikligi → YOK. Sadece UI/HUD okunabilirlik fix yeter.

### D. Oncelik Sirasi
1. TASARIM/CLASS_STATE_CONTRACT.md yazilir (her sinif: verbs / resource / 2-3 export state / 2-3 consume trigger / forbidden generic behaviors).
2. Global tekrar kurallari kilitlenir (counter / movement / execute / zone).
3. Yuksek riskli 4 sinif redesign:
   - Brawler Charge sadelestirme
   - Hexer stack davranissallastirma
   - Elementalist shape interactions
   - Gunslinger execute konsolidasyonu + Ranger mark/trap rol ayrimi
4. Warblade minimal Faz 1 revizyonu (Iron Crush + Death Blow gate).
5. Kalan siniflar (Ravager / Ronin / Shadowblade / Summoner): overlap + abuse kontrolu sonrasi cerrahi revizyon.

---

## SPESIFIK SORULAR (cevapla)

1. **State-based cross-class:** Sinif basina 2-3 export state cok mu az / cok mu fazla? Toplam 20-30 state RIMA UI/okunabilirlik icin yonetilebilir mi?

2. **Oncelik sirasi:** Brawler/Hexer/Elementalist/Gunslinger-Ranger ilk dort dogru mu? Daha riskli bir yerde landmine birakiyor muyuz?

3. **Counter ayrimi:** absorb-break / wait-draw / weave-evade ucu input/animation/feedback acisindan player'a okunaklikla farkli hissedebilir mi? Yoksa hala karisma riski var mi?

4. **Warblade:** Iron Crush'i Broken hedefe armor shard / catlak yayma yapmak yeterince RIMA mi? Yoksa daha derin redesign mi gerek?

5. **Gunslinger execute:** Deadshot kalip Point Blank Execute Hip Shot finisher branch'ine kaynamasi dogru karar mi? Yoksa Point Blank Execute kalip Deadshot mi degissin?

6. **Lightbreak:** Sistemi koruyup sadece UI fix yeterli mi? Yoksa "fazla muhasebe" elestirisi UI fix ile cozulmez mi?

7. **Faz 2'ye itilen 4 fikir:** Hangisi aslinda Faz 1'de olmali? Hangisi later/cancel olmali?

8. **Cross-class export state listesi (initial draft):**
   - Warblade: Broken / Sundered / Staggered
   - Elementalist: Burning / Frozen / Lightbroken
   - Shadowblade: Rift Scar / Collapsing
   - Ranger: Marked / Snared
   - Ravager: Bloodied / Frenzied
   - Ronin: Opened / Punish Window
   - Gunslinger: Suppressed / Exposed Line
   - Brawler: Launched / Wall-Slammed
   - Summoner: Bound / Sacrifice Mark / Corpse Field
   - Hexer: Hexed / Overloaded
   
   Bu 25 state RIMA icin dogru sayi mi? Hangileri silinmeli/birlestirilmeli? Hangi class state kontratina yeni bir state eklenmeli?

9. **Codex 5-line test:** 5 madde yeterli mi? Eksik 6. madde var mi?

10. **Implementation order pratikligi:** "State Contract once, sonra global rules, sonra sinif sinif" sirasi calisirmi yoksa state contract sinif kararlariyla beraber mi olusmali (chicken-and-egg)?

---

## Cikti

- ASCII-only.
- Direkt madde madde cevap.
- Soru atlamaya izin yok — atlanan madde icin "atliyorum, sebep: ..." yaz.
- En sonda EKLEME RISK basligi altinda yeni gordugun sistemik problemleri yaz.
- Cevap dosya olarak donulecek: ChatGPT icin STAGING/CHATGPT_R2_2026-04-30.md, Codex icin STAGING/CODEX_R2_2026-04-30.md.
