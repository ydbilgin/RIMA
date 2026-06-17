# Q1

Kisa cevap: Hipotez 1 DOGRUYA YAKIN. Mevcut promptlar silahi isimle cagiriyor, ama her pose-state ayni silah geometri kilidine bagli degil. Bu yuzden PixelLab her state/yone "sword/blade/weapon identity" kavramini yeniden sentezleyebilir. Risk: ayni Warblade zirhi korunur, fakat kilic uzunlugu, guard, aci, kalinlik ve hatta hangi tarafta okundugu state'ler arasinda kayabilir.

Mekanik:
- `create_character_state(character_id, edit_description, seed, use_color_palette_from_reference)` kaynak karakterin kimligini ve rotasyon grubunu referans alir; fakat endpoint'te ayri `reference_image`, `init_image`, `ai_freedom`, geometry-lock parametresi yok.
- `use_color_palette_from_reference=true` renk tutarliligina yardim eder. Silah geometrisini garanti etmez. Yeni silah eklenirken `true` kullanmak yeni silah rengini de kisitlayabilir; armed-anchor uretirken `false`, anchor'dan pose-state turetirken `true` daha dogru.
- `animate_character(mode=v3)` state karakteri uzerinden hareket uretir; action text'teki "stable sword silhouette" sadece yonlendirme. Geometri kilidi degil.
- Web UI'daki custom start/end frame veya init-image benzeri slotlar frame pozunu daha iyi sabitler; fakat bu brief kapsamindaki MCP `create_character_state` icin boyle bir parametre yok. BELIRSIZ: PixelLab Web UI'da "AI freedom" benzeri ayar varsa geometrik drift'i azaltabilir, ama bu MCP schema'da gorunmuyor.

Dogru tutarlilik yolu:
1. Once tek bir `armed anchor` uret: base Warblade + net greatsword. `use_color_palette_from_reference=false`, cunku silah yeni ogedir.
2. UI'da anchor'i gorsel onayla: kilic okunur, 5 native yon icin ayni siluet ailesinde, armor bozulmamis.
3. Mid-run / strike-windup / breathing-idle / flinch state'lerini base karakterden degil, bu armed anchor'dan uret. `use_color_palette_from_reference=true`.
4. Her pose prompt'unda "same exact greatsword from the reference, unchanged blade length, crossguard, grip, and silhouette" gibi geometri-kilit dili kullan.
5. Animate'ten once state preview QC yap. Kotu state'i animate'e sokma; reroll et.
6. Mutlak garanti isteniyorsa demo sonrasi silahsiz body + runtime weapon mount gerekir. Baked-silah pipeline AI tutarliligini yukseltir ama deterministik garanti vermez.

# Q2

## A) SILAHSIZ state recetesi

Amac: weapon-mount pipeline icin body silahsiz kalsin; eller bos, ama kol/omuz/torso hareketi daha sonra takilacak silaha uygun olsun.

Kopyala-yapistir state prompt sablonu:

```text
same [CHARACTER_NAME], high top-down 3/4 2D game sprite, [POSE_NAME] pose,
[TORSO_MECHANICS],
[RIGHT_ARM_MECHANICS],
[LEFT_ARM_MECHANICS],
hands empty, fists loosely clenched, fingers curved as if ready to grip, no held items,
preserve armor, proportions, palette, and silhouette, no redesign, no VFX,
transparent background, readable in 8 directions,
NO weapons, NO sword, NO blade, NO shield, NO staff, NO bow, NO gun, NO dagger, nothing in either hand
```

Silahsiz LMB windup ornegi:

```text
same warblade, high top-down 3/4 2D game sprite, heavy attack windup pose,
torso twisted to the right, feet braced wide, weight loaded on rear foot,
right arm drawn back across the body above the right shoulder, elbow bent, fist clenched, wrist aligned for a later hand-mounted object,
left arm forward for balance, shoulder low and guarded,
hands empty, fists loosely clenched, fingers curved as if ready to grip, no held items,
preserve armor, proportions, palette, and silhouette, no redesign, no VFX,
transparent background, readable in 8 directions,
NO weapons, NO sword, NO blade, NO shield, NO staff, NO bow, NO gun, NO dagger, nothing in either hand
```

Silahsiz horizontal swing apex ornegi:

```text
same warblade, high top-down 3/4 2D game sprite, heavy swing apex pose,
torso fully rotated left, feet planted, hips following the rotation,
right arm fully extended across the body in a wide horizontal arc, fist clenched, wrist straight, shoulder committed,
left arm counterbalances behind the torso,
hands empty, fists loosely clenched, fingers curved as if ready to grip, no held items,
preserve armor, proportions, palette, and silhouette, no redesign, no VFX,
transparent background, readable in 8 directions,
NO weapons, NO sword, NO blade, NO shield, NO staff, NO bow, NO gun, NO dagger, nothing in either hand
```

DO kelime/ifade listesi:
- `hands empty`
- `nothing in either hand`
- `no held items`
- `fists loosely clenched`
- `fingers curved as if ready to grip`
- `wrist aligned`
- `arm drawn back across the body`
- `arm fully extended in a wide horizontal arc`
- `two hands close together in grip posture`
- `preserve armor, proportions, palette, and silhouette`
- `transparent background`
- `readable in 8 directions`

DON'T / yasak kelime listesi:
- `sword`
- `blade`
- `greatsword`
- `slash`
- `cleave`
- `stab`
- `weapon`
- `wield`
- `grip the sword`
- `weapon identity`
- `hilt`
- `crossguard`
- `staff`
- `bow`
- `gun`
- `dagger`
- `shield`

Not: "as if gripping a two-handed haft" bile silah cagirabilir. Daha guvenli ifade: `fingers curved as if ready to grip, but hands empty`.

## B) TUTARLI-SILAHLI state seti recetesi

Amac: demo bake icin silah sprite-sheet'e gomulu kalsin, ama state'ler arasinda ayni greatsword ailesi korunsun.

Pipeline:
1. Armed anchor uret.
2. Anchor'i gorsel onayla.
3. Tum pose-state'leri armed anchor'dan turet.
4. Palette snap'i pose-state'lerde ac.
5. Animate'e sadece QC'den gecen state'leri sok.

Armed anchor prompt:

```text
same warblade, high top-down 3/4 2D game sprite, guarded neutral stance,
wielding one large two-handed greatsword clearly visible in both hand area,
greatsword has dark steel straight blade, simple brass crossguard, dark leather grip,
blade length, blade width, crossguard, grip, and silhouette must stay readable across all 8 directions,
preserve armor, proportions, palette, and body silhouette, no redesign, no VFX,
transparent background, clean pixel clusters
```

Pose-state prompt sablonu, armed anchor kaynak alinacak:

```text
same warblade from the armed anchor, high top-down 3/4 2D game sprite, [POSE_NAME] pose,
[TORSO_AND_LEG_MECHANICS],
[ARM_AND_WEAPON_POSITION],
same exact greatsword from the reference, unchanged blade length, blade width, crossguard, grip, color, and silhouette,
weapon stays in hand and remains readable, no new weapon, no alternate weapon,
preserve armor, proportions, palette, and body silhouette, no redesign, no VFX,
transparent background, readable in 8 directions
```

Armed idle prompt:

```text
same warblade from the armed anchor, high top-down 3/4 2D game sprite, guarded breathing idle stance,
feet planted, shoulders relaxed, torso upright, weight balanced,
greatsword held ready but relaxed near the body,
same exact greatsword from the reference, unchanged blade length, blade width, crossguard, grip, color, and silhouette,
weapon stays in hand and remains readable, no new weapon, no alternate weapon,
preserve armor, proportions, palette, and body silhouette, no redesign, no VFX,
transparent background, readable in 8 directions
```

Armed strike windup prompt:

```text
same warblade from the armed anchor, high top-down 3/4 2D game sprite, heavy strike windup pose,
feet braced wide, torso twisted, shoulders loaded before a heavy attack,
greatsword pulled back with the body, blade readable and not cropped,
same exact greatsword from the reference, unchanged blade length, blade width, crossguard, grip, color, and silhouette,
weapon stays in hand and remains readable, no new weapon, no alternate weapon,
preserve armor, proportions, palette, and body silhouette, no redesign, no VFX,
transparent background, readable in 8 directions
```

Animate action prompt sablonu:

```text
[ACTION_NAME], top-down ARPG motion, stable body silhouette, same exact greatsword silhouette throughout, weapon stays in hand, no new weapon, no magic VFX, no redesign
```

Genel prompt-craft kurallari:
1. Bir state'te olmayan objeyi her pose'ta isimle yeniden cagirirsan model onu yeniden icat edebilir.
2. Silahsiz uretimde silah kelimesi kullanma; uzuv mekanigini tarif et.
3. Negatif prompt tek basina yetmez; pozitif prompt da `hands empty` demeli.
4. "weapon-ready" ifadesi tek basina riskli; yanina `hands empty, no held items` koy.
5. Baked-silah uretimde once anchor, sonra pose-state. Base karakterden paralel state uretme.
6. Palette snap renk tutarliligi icindir; geometri kilidi sanma.
7. Yeni renk/obje eklerken palette snap kapali; anchor'dan turetirken acik.
8. High top-down 3/4 ve 8 direction disiplinini her prompt'ta tekrar et.
9. `preserve armor/proportions/silhouette, no redesign` her state'te sabit kalsin.
10. Direction drift'i azaltmak icin once S/SE/E/NE/N native uret, W/SW/NW mirror planini bozma.
11. Animate'ten once state preview QC yap; bozuk state'i animasyonla kurtarmaya calisma.
12. Deterministik silah tutarliligi gerekiyorsa baked AI degil, runtime weapon mount kullan.

# Q3

Demo tarihi 19 Haziran oldugu icin asset sirasi golden-path gorunurlugune gore dar tutulmali. NotebookLM auth expired; bu envanter brief + STAGING kanitlarina dayali.

| Oncelik | Madde | Demo-kritik mi? | Kim yapmali? | 3 gunde gercekci mi? | Golden-path videosunda gorunur mu? | Karar |
|---|---|---|---|---|---|---|
| P0 | Warblade armed-anchor REDO + idle/run/LMB | EVET | Kullanici PixelLab; orchestrator prompt/QC; Unity import/wire | EVET, scope sadece idle/run/LMB ve 5 native yon ise | EVET, oyuncu karakteri surekli gorunur | Ilk is. Mevcut state'lerde kilic drift varsa anchor'dan redo. |
| P0 | REWARD-02 / UI-01 kod fixleri | EVET ama asset degil | Unity/kod | EVET | EVET, reward beat'te gorunur | Asset uretiminden once/parallel. Yeni PNG bu bug'lari cozmez. |
| P1 | Basic energy bolt core sprite | SARTLI | Kullanici PixelLab veya orchestrator MCP; Unity `SkillVfx` tint/swap | EVET | Sadece Elementalist gosterilirse | Warblade demo bittikten sonra. Elementalist golden-path'te aktif degilse post-demo. |
| P2 | RewardPickup shell sprite | SARTLI / opsiyonel | PixelLab + Unity, ayri commit | EVET | Reward beat'te gorunur | Sadece REWARD-02/UI-01 PASS ve Warblade bittikten sonra. Awake fallback ile cakisma riski var. |
| P2 | Alt portal bar + mavi beam T5 visual | SARTLI | Unity procedural + gerekiyorsa PixelLab tek flare | KISMEN | Portal/transition sunumda on plandaysa | Existing portal yeterliyse post-demo. Beam native/procedural daha dogru. |
| P3 | Fireball / fire-impact / glacial-spike / frozen-orb / light-beam VFX core seti | HAYIR, Warblade golden-path icin degil | PixelLab core asset + Unity `SkillVfx` | KISMEN, hepsi degil | Sadece Elementalist skill showcase varsa | Post-demo. En fazla energy bolt + tek fireball sec. |
| P3 | Run-map node sembolleri/chrome | HAYIR | PixelLab iconlar + el-Aseprite chrome + Unity atlas/dict | KISMEN | Golden-path live-testte ana beat degil | Post-demo. Runmap asset karari var ama demo art-zorunlu degil. |
| P4 | Buyuk cliff map tile/backdrop | HAYIR | Unity/reuse first; PixelLab sonra | HAYIR | Edit-to-play video ozelinde olabilir, combat golden-path'te degil | Post-demo. Mevcut tile/backdrop/pack reuse; yeni uretim scope creep. |

Net siralama:
1. Warblade armed-anchor redo kararini ver: mevcut 4 state'i UI'da kontrol et; drift varsa anchor pipeline ile redo.
2. Warblade idle/run/LMB animasyonlarini bitir ve Unity'de gor.
3. REWARD-02 + UI-01 kod fix PASS olmadan yeni UI/Egg/art acma.
4. Zaman kalirsa sadece `energy bolt core` veya `RewardPickup shell` arasindan birini sec. Elementalist gosterilecekse energy bolt; reward beat daha on plandaysa shell.
5. VFX full set, run-map chrome, portal beam polish, cliff/backdrop post-demo.

BELIRSIZ:
- Golden-path videosunda Elementalist aktif oynanacak mi? STAGING karari Warblade'i tek art-zorunlu PixelLab isi yapiyor. Elementalist sadece char-select veya post-demo prep ise energy bolt demo-kritik degil.
- Mevcut warblade state'lerinin gercek PixelLab preview'lari bu task'ta gorulmedi. Bu nedenle "redo kesin" degil; "drift varsa redo" dogru karar.
