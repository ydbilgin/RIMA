# Skill Sistemi Analiz Notlari

Durum: Yeni session'da tekrar analiz ettirilecek soru/not dosyasi.
Amac: RIMA skill sistemi icin Hades-style upgrade modeli, PixelLab skill sheet uretilebilirligi ve mevcut skill set tutarliligi hakkinda karar sorularini toplamak.

## Ana Soru

RIMA'da skill gelisimi skill agaci gibi mi olmali, yoksa Hades gibi random gelen pasif/upgrade teklifleriyle mi ilerlemeli?

Mevcut oneri:

- Skill agaci ekrani yapma.
- Hades'e yakin oda sonrasi teklif sistemi kullan.
- Teklifler yeni aktif skill, mevcut skill upgrade'i, pasif/echo veya tag sinerjisi olabilir.
- Last Epoch'tan alinacak fikir "skill tree UI" degil, skill upgrade'lerinin bazen skill davranisini anlamli degistirmesi.

## Onerilen Model

Skill'in cekirdek kimligi korunur. Upgrade'ler bu kimligi yonlendirir.

Ornek upgrade tipleri:

- `Cleave` hedeflere Bleed ekler.
- `Fireball` yanma suresini patlatabilir.
- `Trap` tetiklenince hedef `Marked` olur.
- `Death Blow` normal dusmanda execute, boss'ta yuksek burst olur.
- Projectile bir kez seker.
- Zone daha buyuk olur ama cooldown artar.

Bu sistem Hades boon mantigina yakin kalir. Arka planda tag/upgrade havuzu olabilir; oyuncuya Last Epoch gibi skill tree ekrani gosterilmesi sart degildir.

## Skill Ana Etki vs Ekstra Effect

Kural:

- Common/Rare upgrade'ler cogunlukla effect/modifier eklemeli.
- Epic upgrade'ler skill'e yeni yan mekanik ekleyebilir.
- Legendary upgrade'ler nadiren skill'in davranisini ciddi degistirebilir.

Yani her skill surekli baska bir skille donusmemeli. Ama yuksek kademe secimlerde skill'in calisma sekli degisebilir.

## PixelLab Skill Sheet Degerlendirmesi

`RIMA_skill_sheets/*.png` dosyalari guclu konsept referansi olarak gorulmeli.

Dogru kullanim:

- Class moodboard.
- Renk dili.
- Poz dili.
- Skill fantezisi.
- Prompt referansi.

Yanlis kullanim:

- Dogrudan production sprite kaynagi.
- Sheet'ten panel kirpip Unity'ye koymak.
- Karakter, VFX, dusman ve arka plani tek animasyon sheet'i gibi kullanmak.
- Sheet'teki skill isimlerini otomatik canonical kabul etmek.

PixelLab ile skill uretilebilir mi?

Evet, ama parcalara ayrilarak:

- Karakter cast/release animasyonu ayri.
- Slash / projectile / spell VFX ayri.
- Ground decal / impact ayri.
- Enemy hit reaction ozel animasyon degil; Unity'de flash, knockback, slide, hit-stop ile.
- Boss/dusman icin her skill'e ozel reaction animasyonu yapilmamali.

## Skill Animasyon Gercekligi

Mevcut skill anlatimlari yer yer fazla sinematik olabilir.

RIMA icin pratik production standardi:

- 1 kisa caster animasyonu.
- 1 VFX/projectile spritesheet.
- Hit-stop.
- Camera shake.
- Enemy flash.
- Code-driven knockback/slide.
- Kisa ground decal veya aura.

Riskli / ertelenmesi gerekenler:

- Her skill icin ozel dusman animasyonu.
- Grapple/ragdoll/struggle animasyonu.
- Boss'a ozel reaction animasyonu.
- Cok sahneli sinematik skill panelleri.

`TASARIM/GLOBAL_REPEAT_RULES.md` bu yonde dogru kilit koyuyor: bespoke mob animation yasak, VFX + slide + hit-react yeterli.

## Skill Set Tutarlilik Degerlendirmesi

Genel temel guclu:

- Warblade: `engage / break / execute`
- Ranger: `mark / trap / detonate`
- Shadowblade: `phase / scar / collapse`
- Hexer: `stack / spread / blast`
- Elementalist: `switch / shape / detonate`
- Brawler: `weave / combo / launch`
- Ravager: `suffer / trade / frenzy`
- Ronin: `wait / draw / punish`
- Gunslinger: `slide / shoot / reload`
- Summoner: `command / sacrifice / raise`

Bu verb sistemi RIMA icin iyi bir temel.

## Ana Riskler

1. Kapsam cok buyuk.
   - 10 class x 12 skill + LMB/RMB/dash + ulti + pasif upgrade agir.
   - Phase 1 Warblade ve temel sistemlerle sinirli kalmali.

2. Fiziksel siniflar cakisabilir.
   - Warblade = armor break / sunder / disciplined heavy sword.
   - Ravager = self-risk / blood / frenzy.
   - Brawler = body weight / punch / off-balance / wall-ground impact.

3. Karanlik buyu siniflari cakisabilir.
   - Shadowblade = phase + scar.
   - Hexer = curse + stack + spread.
   - Summoner = minion + sacrifice + corpse economy.

4. Bazi aktif skiller pasife donusmeli.
   - Sadece hasar artisi veren skill aktif slot hak etmez.
   - Sadece buff veren skill aktif slot hak etmez.
   - Bunlar Hades tarzi pasif/boon havuzuna cekilmeli.

5. Gorsel anlatim sadelesmeli.
   - Skill sheet'ler havali ama production icin fazla illustratif.
   - Oyunda okunacak sey: siluet, yon, ana renk, impact.

## Analizde Sorulacak Kritik Test

Her aktif skill icin sor:

1. Bu skill tek fiille ne yapar?
2. Hangi state'i uretir veya tuketir?
3. Aktif slot hak ediyor mu?
4. Baska class kimligiyle cakisiyor mu?
5. Abuse riski nedir?
6. Hangi encounter sorusuna cevap veriyor?

Kisa test:

`Bu skill aktif slotu hak ediyor mu, yoksa Hades tarzi pasif upgrade mi olmali?`

Aktif skill kalacaksa mutlaka bir state uretmeli veya tuketmeli:

- `Broken`
- `Sundered`
- `Marked`
- `Trapped`
- `Scar`
- `Hex`
- `Off-Balance`
- `Suppressed`
- `Opened`

Sadece hasar veya sadece buff ise pasif/upgrade havuzuna cekilmeli.

## Yeni Session Icin Istenen Analiz

Yeni session'da bu dosyaya bakarak su analizleri yaptir:

1. Mevcut 10 class skill set'i RIMA icin tutarli mi?
2. Hangi aktif skiller pasif/upgrade olmalidir?
3. Hangi skiller PixelLab ile pratik uretilebilir?
4. Hangi skill anlatimlari production icin fazla sinematik?
5. Hangi siniflar birbirine fazla yaklasiyor?
6. Hades-style random offer modeli icin upgrade havuzu nasil olmali?
7. Tag sistemi hangi minimum tag set'iyle baslamali?

