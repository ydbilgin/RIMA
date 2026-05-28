# AGY TASK — Warblade weapon + anim + VFX second-eye / trade-off review

Amac: RIMA Faz 1 demo Warblade weapon+animasyon+VFX uretim plani LOCK edilmeden once endustri/referans gozuyle ikinci bir bakis. Opus design agent senin verdict'ini Codex + kendi judgment'i ile sentezleyecek. INLINE cevap ver, ASLA dosyaya yazma.

## Baglam (kisa)
RIMA = 2D top-down (HIGH TOP-DOWN 3/4, ~70-80 derece) ARPG roguelite. Faz 1 demo = Warblade TEK class, 5 oda, 4 mob, 10 dk loop. Karakter body sprite SILAHSIZ uretiliyor; silah ayri sprite olarak Unity'de HandAnchor child SpriteRenderer ile takiliyor (weapon-decouple pipeline locked). Animasyon flow karari: SADE body + DETACHED weapon (HandAnchor) + ayri VFX layer (Hades + Children of Morta kirilim). Juice (hit pause + screen shake + impact freeze) sade body'yi telafi ediyor.

Codex teknik pass DONE. Onun teknik bulgulari (sana ozet): weapon-mount orientation sistemi (OrientationSync/WeaponSorter) su an dead code, cagrilmiyor; en az-LOC cozum HandAnchorAttach icine 4-yon facing bridge. Mevcut CombatEventBus + VFXRouter + juice driver altyapisi var ama sahneye wire edilmemis. VFX kaynak ayrimi: kucuk/sik/timing-hassas efektler Unity-side, buyuk/nadir/kimlik-tanimlayici efektler PixelLab flipbook.

## SANA SORULAR (referans + risk gozu — net cevap)
Q1. Unity 2D VFX vs PixelLab animation-VFX trade-off: Hades / Children of Morta / Colossus-Eternal-Blight / Dead Cells gibi 2D action referanslarinda hangi efekt tipi hangi yontemle yapiliyor? Her efekt icin (hitspark, slash arc, dash trail, hit-flash, crit overlay, ultimate, death) tipik endustri yaklasimi nedir? Codex'in "kucuk=Unity, buyuk/nadir=PixelLab" kriterini onayliyor musun yoksa duzeltir misin?
Q2. SADE body + detached weapon + VFX layer yaklasiminda referans oyunlardan KACIRDIGIMIZ kritik bir sey var mi? Ozellikle: detached weapon (HandAnchor child) 2D top-down'da dogru aci/sort/derinlik hissini verir mi, yoksa "yapistirma" gorunur mu? Hades/CoM bunu nasil cozuyor (embedded weapon mi, socket mi)?
Q3. Uretim sirasi riski: BLOK A (engine weapon-mount fix) -> BLOK B (weapon asset uret) -> BLOK C (anim uret) -> BLOK D (VFX wire) -> BLOK E (entegre test). Bu sirada production-risk acisindan tehlike var mi? Colossus raporu "VFX-first weapon, sonra attached sprite" diyordu — yani once goruntu-suz hitbox+VFX ile vurus hissi prove et, SONRA silah sprite ekle. Bu, bizim "once weapon asset+mount sonra VFX" sirasiyla CELISIYOR mu? Hangisi Faz 1 demo icin dogru: weapon-asset-first mi VFX-first mi?
Q4. Juice telafisi: SADE body anim'in "yumusak/cansiz" hissini hit pause + screen shake + impact freeze ne kadar telafi eder? Referanslardan somut juice recipe (hitstop kac ms, shake amplitude, freeze frame sayisi) ver.

## KISIT
- Faz 1 = Warblade TEK. Diger class Faz 4, detaya girme.
- INLINE cevap, dosyaya YAZMA. Net madde madde Q1-Q4.
