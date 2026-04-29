# S41 Class Pipeline Guide
> RIMA | 2026-04-25 | ChatGPT Image 2 -> PixelLab Pixelize | 10 class icin guncel uretim kaynagi

Bu guide S41 kararlarini tek yerde toplar. Base karakter, 8 yon konsept, skill sheet yenileme ve cross-class isim uyumu icin once bu dosya okunur.

## Kaynak Onceligi

1. Bu dosya: S41 sonrasi class kimligi, poz, palette, skill revizyonu.
2. `STAGING/PIPELINE_8DIR_CHATGPT.md`: ayrintili kamera ve 8 yon prompt template.
3. `STAGING/SKILL_REVIZYON_PLANI.md`: skill degisimlerinin gerekcesi.
4. `STAGING/PROMPTS_RANGER_SHADOWBLADE.md`: Ranger ve Shadowblade icin hazir 8 yon prompt.
5. `TASARIM/STYLE_BIBLE.md`: genel RIMA tonu, boyut, PPU, import kurallari.

`GUIDES/CHATGPT_CHARACTER_PIPELINE.md` ve `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md` S41 oncesi Create Character agirlikli pipeline icerir. Kamera, palette veya class identity celisirse bu S41 guide kazanir.

## Sabit Pipeline

1. ChatGPT Image 2'de tek class icin tek conversation ac.
2. Ilk mesaja kamera/style referansi olarak `C:/Users/ydbil/Downloads/warblade_pro/warbladeNEW7.png` yukle.
3. Once S yonu uret. S yonu PASS almadan diger 7 yoni uretme.
4. Sonraki yonlerde "same character, same outfit, same proportions, same camera, only direction changes" kuralini kullan.
5. Her yonu PixelLab Edit Image ile 128x128 pixelize et.
6. Aseprite final pass: pivot, canvas, palette, silhouette ve yon tutarliligi.
7. Unity import: `Assets/Sprites/Characters/[Class]/base/[class]_base_[DIR].png`, PPU 64, Point filter, uncompressed.

## Kamera Kilidi

ChatGPT her firsatta eye-level heroic poster'a kayar. Kabul kriteri:

- Bas tepesi net gorunur.
- Omuzlarin ust yuzeyi gorunur.
- Yuz foreshortened okunur.
- Ayaklar bas bolgesinden daha kucuk ve kadrajda daha asagidadir.
- Gozler yuzun ust uc bolgesindedir, merkezde degildir.
- Siyah arka plan, tam vucut, yaklasik %10 margin.

Bu kriterlerden biri fail ise devam edilmez; ayni yon regenerate edilir.

## Yon Standardi

| Yon | Kural |
|---|---|
| S | Strict frontal. Govde, kalca, ayak ve bas kameraya bakar. Asimetri sadece kol/silah pozundan gelir. |
| SE | 45 derece clockwise 3/4 front. Sag omuz onde, sag taraf okunur. |
| E | Pure right profile. Tam yan siluet. |
| NE | 45 derece clockwise 3/4 back. Sag sirt/omuz lider. |
| N | Full back. Silah/accessory vucut disinda okunur. |
| NW | 45 derece counter-clockwise 3/4 back. Sol sirt/omuz lider. |
| W | Pure left profile. E'nin mirror'i ama eldeki silah saklanmaz. |
| SW | 45 derece counter-clockwise 3/4 front. Sol omuz onde. |

## Global QC

- SE ve SW, S ile ayni gorunuyorsa set fail.
- E ve W pure side degilse set fail.
- Far-side silah tamamen torso arkasinda kayboluyorsa set fail.
- Class accent rengi diger class accent'iyle karisiyorsa palette pass yap.
- 128x128 downsample sonrasi yuz detayindan cok silhouette okunurlugu onceliklidir.
- Her class icin S, SE, E, NE, N, NW, W, SW dosya adlari ayni pattern'i kullanir.

## Class Rehberi

### 01 Warblade

**Kimlik:** Disiplinli agir savasci; full knight degil, battle-worn warrior.

**Poz:** Greatsword sag omuz ustunde dik resting. Sag el ust grip, sol kol asagida rahat yumruk. Sag ayak yarim adim onde. S yonunde torso frontal kalir; yuze silaha dogru donus ekleme.

**Palette:** Dark steel gray, warm brown leather, ember orange/silver accent. Eski mavi VFX artik ana karar degil; yeni S41 karari silver + ember.

**Skill guide:** Kucuk revizyon. War Stomp -> Earthsplitter. Iron Crush, Iron Counter ve Battle Surge gorsel ayrimi guclendirilecek. Cross-class matrix isim etkisi dusuk; War Stomp geciyorsa Earthsplitter'a tasinmali.

**Uretim notu:** Mevcut Warblade 6 yon korunabilir ama SE/SW hatasi yuzunden uzun vadede shoulder-rest pose ile komple yeniden uretim daha temizdir. Patch sadece hiz kazandirir.

### 02 Elementalist

**Kimlik:** Prime force scholar; aristokrat/noble mage degil, pratik seyyah bilgin.

**Poz:** Staff sol elde dikey, sag elde chest hizasinda acik avuc rune/orb. Staff ve casting hand zit taraflarda okunmali.

**Palette:** Dusty indigo, cream cloth, element accent. Void purple yok.

**Skill guide:** Buyuk revizyon. Prism Lance -> Prism Beam, Halo Fracture -> Frost Wall, Sunshard Torrent -> Solar Flare, Luminary Surge -> Radiant Pillar, Combustion -> Element Charge, V Burst Inferno -> Trinity Storm. Fire/Frost/Radiance anim pozlari ayrismali.

**Uretim notu:** Hexer ile staff karismasini onlemek icin Elementalist staff sol elde, Hexer staff sag elde kalmali. Elementalist'in acik avuc runesi sicak/elemental; Hexer curse-weave yesil-violet.

### 03 Shadowblade

**Kimlik:** Veil Walker; phase assassin, generic hooded rogue degil.

**Poz:** Low predator crouch. Sag el reverse-grip dagger pelvis hizasinda, sol el forward-grip dagger tehdit gesture olarak onde. Veil mask alt yuzu kapatir.

**Palette:** Pure void black, hot magenta, worn brown leather. Cold purple, blue ve gold yok.

**Skill guide:** Tam redesign. Ana mekanik "phase + mark + execute". 12 skill: Veil Strike, Phase Step, Backstab Mark, Shadow Clone, Death Mark, Veil Burst, Sever, Smoke Veil, Chain Cull, Shadow Pin, Twin Carve, Wraith Form.

**Uretim notu:** Ranger da mark kullandigi icin Shadowblade mark'lari yakin mesafe infaz ve phase cikisi ile baglanmali. "Mark" kelimesi ayni olabilir ama davranis ranged precision degil assassination window olmali.

### 04 Ranger

**Kimlik:** Rift Stalker; Tolkien green hood archer degil.

**Poz:** Bone-recurve bow sol elde dikey, sag el side-hip quiver'dan ok secer. Hood ve cape yok. Half-shaved braid, war-paint ve side-quiver kimlik kilidi.

**Palette:** Bone-white, worn brown leather, rift-purple accent. Forest green yok.

**Skill guide:** Tam redesign. 12 skill: Rift Arrow, Pinning Shot, Marked Detonate, Hunter's Step, Bone Trap, Sweep Volley, Skirmish Shot, Predator's Mark, Multi-Mark, Final Strike, Rift Step, Spirit Bow.

**Uretim notu:** Mark mekanigi monotonlasmasin diye mark sadece hedef etiketi olmamali; trap, detonate, execute ve movement kararlarini degistiren av ritmi olmali.

### 05 Ravager

**Kimlik:** HP dustukce tehlikeli brutal berserker; Brawler'dan daha iri ve vahsi.

**Poz:** Two-handed great axe sag omuz ustunde resting. Fur mantle sol omuzda net gorunur. Genis stance, govde agresif forward lean.

**Palette:** Dirty bronze skin, dark fur, crimson blood accent. Mor ve mavi yok.

**Skill guide:** Orta revizyon. Whirlwind -> Carnage Spin. Intimidating Shout/Battle Cry ailesi -> Bloodied Roar ve RMB Blood Pact. Shout filler olmamali; HP/Fury trade okumali.

**Uretim notu:** Brawler ile ciplak ust vucut overlap riski var. Ravager'da axe + fur mantle + beard + genis kitle zorunlu; Brawler'da gauntlet + boxing guard + sportif footwork zorunlu.

### 06 Ronin

**Kimlik:** Iaido draw-master; sakin, kompozisyonu temiz, samurai discipline.

**Poz:** Katana sol kalcada kininda, sol el kin bogazinda, sag el kabzada draw-ready. Cekilmis savas pozu degil, "before strike".

**Palette:** Muted indigo, black, silver edge accent. Alev ve mor yok.

**Skill guide:** Kucuk revizyon. Mille Feuille Cut -> Soken-giri. Blade Veil -> Sakura Veil. Wind Step duz line, Phantom Step arc/afterimage olarak ayrilmali.

**Uretim notu:** Shadowblade ile close-range overlap'i, Ronin'in sheathed iaido calm stance'i ve tek temiz blade vector'u ile ayrilir. Shadowblade crouch + twin dagger + magenta phase'dir.

### 07 Gunslinger

**Kimlik:** Ritualistic worn duelist; temiz western saloon veya puffy sleeve yok.

**Poz:** Sag pistol asagida relaxed, sol pistol holster'a yari yerlestirilmis. Hat egik, sag omuz hafif onde. Dueling calm.

**Palette:** Deep auburn hair, dark leather, brass, dusty red accent. Mor ve el glow yok.

**Skill guide:** Orta revizyon. Bullet Rain -> Cursor Storm, Critical Shot -> Deadshot, Dead Eye -> Rift Grenade. Range skill'ler cursor-line veya cursor-zone kararlarini net vermeli.

**Uretim notu:** Hat korunabilir ama yuz/sac okunurlugunu olduruyorsa hat bandi ve siluet aksesuarina indir. Gun silhouette 128px'te okunmali.

### 08 Brawler

**Kimlik:** Footwork boxer; "silahsiz Ravager" degil.

**Poz:** Boxing guard. Sol omuz onde, sol yumruk cene hizasinda, sag yumruk gogus hizasinda chambered. Gauntlet'ler iki elde net.

**Palette:** Bronze skin, dark cloth, arcane purple tattoo glow, dark steel gauntlets. Mor sadece arcane fury/proc okumali.

**Skill guide:** Orta revizyon. Rush Combo -> Combo Chain. Momentum Strike -> Pivot Hook. Motion silhouette yumruk rotasyonundan okunmali; mor patlama spam'i azalt.

**Uretim notu:** En kritik ayrim ayak pozudur. Brawler her yonde stance/footwork ile okunmali; sadece "iki yumruk onde" yeterli degil.

### 09 Summoner

**Kimlik:** Death commander; minyon lideri, generic necro cloak degil.

**Poz:** Soul lantern sol elde omuz hizasinda. Sag el asagida acik avuc command gesture. Skeleton-mask helm gorunur; hood tamamen yuzu kapatmaz.

**Palette:** Void black, bone white, cyan soul-glow. Yesil/mor yok.

**Skill guide:** Kucuk revizyon. Rally Cry -> Command Beacon. Command line master-to-minion net gorunmeli. Soul Siphon Totem ve sacrifice mekanikleri korunur.

**Uretim notu:** Hexer ile robe/staff overlap riski yuksek. Summoner staff yerine lantern + skeleton helm + cyan glow ile ayrilmali.

### 10 Hexer

**Kimlik:** Curse-binder; sabirli stack/curse fantasy.

**Poz:** Curse staff sag elde dikey, sol el gogus hizasinda curse-weave gesture. Hood up ama yuz gorunur; skeleton mask yok.

**Palette:** Deep violet, cursed green, dark robe black. Cursed green + violet birlikte okunmali.

**Skill guide:** Kucuk revizyon. Blight Sigil eklenmeli. Tercih: Soul Bargain yerine Blight Sigil; Cursed Mirror daha karakterli oldugu icin korunabilir.

**Uretim notu:** Summoner'dan ayrim: Hexer staff + floating sigils + green/violet curse. Summoner lantern + skeleton helm + cyan soul.

## Cross-Class Guncelleme Checklist

Bu guide'dan sonra asagidaki belgeler ayrica guncellenmeli:

- `TASARIM/CROSS_CLASS_SKILL_MATRIX.md`
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
- `Assets/Scripts/CrossClass/CrossClassSkillData.cs`
- `Assets/Data/CrossClass/`

Ozellikle kontrol:

- WB+Shadow Iron Phantom: Hemorrhage/Kidney/Evasion yerine Death Mark, Shadow Pin, Smoke Veil.
- Shadow+Ranger Hunter's Mark: Vanish/Explosive Trap/Aimed Shot yerine Smoke Veil, Bone Trap, Rift Arrow.
- Ranger+Shadow Ghost Arrow: Vanish + Aimed Shot yerine Smoke Veil + Rift Arrow charge.
- WB+Ranger Predator's Advance: Aimed Shot/Concussive/Barbed yerine Rift Arrow, Pinning Shot, Bone Trap.
- Gunslinger Faz 3 taslak: Bullet Rain/Critical Shot/Dead Eye isimleri Cursor Storm/Deadshot/Rift Grenade olarak tasinmali.

## Uretim Sirasi

1. Ranger S yeniden denenir; mevcut eye-level ornek fail olarak kullanilir.
2. Ranger 8 yon tamamlanir ve PixelLab/Aseprite pass alir.
3. Shadowblade 8 yon tamamlanir.
4. Warblade komple yeni shoulder-rest pose ile yeniden uretim karari verilir veya sadece SE/SW patch yapilir.
5. Elementalist ve Summoner/Hexer arka arkaya uretilmez; silhouette overlap yorgunlugu icin araya Ravager/Ronin gibi farkli class konur.
6. Kalan class'lar: Ravager, Brawler, Ronin, Gunslinger, Elementalist, Summoner, Hexer.

## Stop Conditions

- ChatGPT 3 denemeden sonra eye-level vermeye devam ederse prompt degil referans zinciri degistirilir.
- 128px pixelize sonrasi class kimligi sadece renk ile okunuyorsa concept basarisizdir.
- Cross-class belgeleri guncellenmeden skill sheet final uretimine gecilmez.
- SE/SW hala S gibi okunuyorsa Create Character veya tek sprite patch ile zaman harcanmaz; 8 yon ChatGPT concept pipeline'a donulur.
