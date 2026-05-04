# RIMA Skill Sheet Codex Degerlendirmesi

**Date:** 2026-05-03
**Reviewer:** Codex
**Scope:** `RIMA_skill_sheets/*.png`
**Status:** REVIEW FOR CLAUDE

Bu dosya `RIMA_skill_sheets` klasorundeki 10 sinif skill sheet'inin RIMA'nin
mevcut karanlik izometrik roguelite tarzi, sinif ayrimlari ve PixelLab uretim
pipeline'i acisindan degerlendirmesidir.

Not: Bu sheet'ler production sprite kaynagi olarak degil, konsept ve prompt
referansi olarak okunmali.

---

## Kisa Hukum

Genel karar: **Gorsel yon olarak guclu, production kaynagi olarak kismi uygun.**

Sheet'ler sinif kimligini hizli anlatmada basarili. Karanlik UI, buyuk action
pozlari, renk ayrimlari ve skill fantezileri RIMA icin kullanilabilir. Ancak
dogrudan animasyon veya canonical skill listesi olarak kilitlenmemeli.

Ana sebep: Cogu panelde karakter, VFX, impact, dusman ve sahne ayni gorsel
icinde pismis durumda. Bu, PixelLab karakter animasyon pipeline'i icin riskli.
Bizim pipeline'da karakter frame'i, VFX frame'i, projectile ve ground decal ayri
uretilmeli.

---

## Global Kararlar

- **Konsept referansi olarak kullan:** Evet. Sinif tonu, renk, poz ve skill
  hissi icin degerli.
- **Dogru gameplay sprite kaynagi olarak kullan:** Hayir. Tek tek frame, temiz
  transparency, 8-way anchor ve 252px frame standardi yok.
- **Skill isimleri canonical sayilsin:** Hayir. Isimler mevcut karar belgeleri
  ve contract dosyalari ile map edilmeli.
- **Warblade icin once run pipeline:** Evet. Sheet, Warblade agir zirhi ve iki
  elle kilic kimligini destekliyor; ancak run/idle/attack frame'leri yine
  8-way anchorlardan uretilmeli.

---

## Genel Riskler

1. **VFX karaktere gomulu.**
   Sheet'lerde slash trail, patlama, impact spark, enemy hit ve aura tek panelde
   beraber. Bu PixelLab'da karakter sheet'ine kopyalanirsa Unity importu kirlenir.

2. **Skill isimleri mevcut sozlesmelerle birebir ayni degil.**
   Ornek: Warblade sheet'inde `Sunder Mark`, `Death Blow`, `Iron Counter` gibi
   dogru hisseden isimler var; fakat mevcut S43 rehberlerinde `Iron Crush`,
   `Earthsplitter`, `Gravity Cleave`, `Iron Charge` gibi kilit isimler de var.
   Claude kararindan once isimler canonical yapilmamali.

3. **Siniflar arasi renk ve state ownership karisabilir.**
   Shadowblade, Hexer ve Summoner mor/yesil/olum/lanet alaninda birbirine
   yaklasiyor. Warblade ve Brawler/Ravager da agir fiziksel siddet alaninda
   ayrismali.

4. **Illustration pozlari 64px okunurluk garantisi vermez.**
   Sheet'ler guzel ama cok detayli. Unity'deki 64 PPU ve kucuk ekranda sadece
   siluet, silah ve ana hareket okunur.

5. **Dusmanli paneller referans olarak tehlikeli.**
   Dusman, hasar efekti ve karakter ayni promptta kullanilirsa PixelLab karakter
   sheet'ine ekstra figurer uretmeye meyledebilir.

---

## Sinif Bazli Degerlendirme

| Sinif | Karar | Not |
|---|---|---|
| Warblade | PASS as concept, PARTIAL as production | Agir zirh, iki elle kilic ve mavi/celik rift hissi dogru. VFX ayrilmali. |
| Elementalist | PASS as concept, PARTIAL as production | Fire/ice/light ayrimi net. Prism Beam fazla renkli olabilir; ton kontrolu lazim. |
| Shadowblade | PASS | Mor/void stealth kimligi guclu. Scar dili guzel ama Hexer/Summoner ile karismamali. |
| Ranger | PASS | Yesil nianci/trap/mark kimligi tutarli. Bow VFX cizgileri frame'e gomulmemeli. |
| Ravager | PASS, watch overlap | Kirmizi rage/blood berserker net. Warblade/Brawler fiziksel alaniyla ayrimi korunmali. |
| Ronin | VISUALLY STRONG, TONE REVIEW | Cok guzel ama sakura/moon samurai cebi RIMA tonundan ayrilabilir. Lore gerekcesi lazim. |
| Gunslinger | STYLE RISK | Okunur ve havali. Modern pistol ancak rift-tech/black-powder olarak gerekcelenirse uyar. |
| Brawler | PASS | En tutarli sheetlerden biri. Ciplak yumruk, agir vucut, turuncu/orange kimlik net. |
| Summoner | PASS | Necro/minion kimligi iyi. Minionlar karakter animation frame'inden ayri uretilmeli. |
| Hexer | PASS, needs stricter boundary | Curse/corruption kimligi iyi. Summoner undead ve Shadowblade shadow alanindan ayrilmali. |

---

## Warblade Notu

Warblade sheet'i oyunun simdiki Hades-benzeri diagonal yon kararini destekliyor:
agir zirh, stabil iki elli kilic, sert impact ve metalik/blue rift hissi dogru.

Ancak Warblade uretiminde sheet'ten dogrudan frame alinmamali. Dogru sira:

1. `Characters/anchors/warblade/rotations/south.png`
2. `south-east.png`
3. `east.png`
4. `north-east.png`
5. `north.png`
6. `north-west.png`
7. `west.png`
8. `south-west.png`

Bu 8-way anchor ile once `run_S/SE/E/NE/N/NW/W/SW` uretilmeli. Sonra basic combo
ve skill clip'leri ayni yonden turetilmeli. Sheet sadece "poz ve sinif hissi"
referansi olarak promptu beslemeli.

Warblade icin sheet'te iyi kullanilacak unsurlar:

- agir zirhi ve genis omuz silueti
- iki elle tutulan buyuk kilic
- celik/mavi rift aksani
- Sunder / Broken / Death Blow gibi agir hasar dili

Warblade icin kacinalacak unsurlar:

- VFX trail'i karakter sprite'ina gommek
- enemy impact panellerini character animation referansi yapmak
- mavi rift'i Elementalist light/ice VFX'i kadar parlak yapmak
- tek elle kilic veya anime-hafif katana pozlari

---

## Sinif Ayrimi Icin Onerilen Kilitler

- **Warblade:** steel, armor-break, sunder, two-handed sword, blue-cold metal.
- **Brawler:** bare fist, blunt impact, body weight, orange/dirt, ground crack only.
- **Ravager:** blood debt, frenzy, red wound, self-risk, brutal axe/body violence.
- **Shadowblade:** void purple, dissolve path, scar, rogue strike, no undead minions.
- **Hexer:** curse math, hex pips, green-purple corruption, no summon army.
- **Summoner:** corpse field, minion silhouettes, teal necromancy, separate entities.
- **Ranger:** bow, trap, mark, green/grey precision, never rushes into melee.
- **Elementalist:** readable fire/ice/light split, caster hands, no weapon melee.
- **Ronin:** opened/tension/sheathe, clean cut, disciplined pose, not Shadowblade phase.
- **Gunslinger:** heat/suppression, rift-tech firearm, muzzle VFX separate.

---

## Production Kullanimi

Bu sheet seti su sekilde kullanilmali:

1. Prompt yazarken "class mood board" olarak ac.
2. Skill contract dosyasindaki canonical isim ve state ownership'i once kontrol et.
3. Karakter animasyonunu 8-way anchor'dan uret.
4. VFX, projectile, impact ve decal'i ayri PixelLab object/VFX pass'inde uret.
5. Unity importta sadece temiz karakter sheet'ini Animator clip'e bagla.
6. Sheet'teki panel kompozisyonunu degil, poz/renk/kimlik fikrini tasir.

---

## Sonuc

`RIMA_skill_sheets` klasoru silinmemeli. Sinif kimligi ve prompt dili icin
degerli bir kaynak. Fakat Claude review olmadan bu sheet'lerdeki skill isimleri
ve panel kompozisyonlari production canon sayilmamali.

Codex onerisi: **KEEP as reference, DO NOT LOCK as canon.** Warblade run
production icin asil kaynak 8-way anchors + `STAGING/WARBLADE_ANIMATION_PIPELINE.md`.
