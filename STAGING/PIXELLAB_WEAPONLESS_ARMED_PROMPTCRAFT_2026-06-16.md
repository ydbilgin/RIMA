# PixelLab — Silahsız vs Tutarlı-Silahlı State Prompt-Craft (KALICI BİLGİ)
> Kaynak: council oybirliği (cx + ax Gemini 3.1 Pro + ax Gemini 3.5 Flash), 2026-06-16. Brief+ham yanıtlar: `STAGING/_process/2026-06/pixellab_anim_council/`. **Gelecek oyunlarda da kullanılacak reusable bilgi** → özet `~/.claude/PIXELLAB_REFERENCE.md`'e işlendi.

## 1. KÖK NEDEN — "neyi yanlış yazdık da silah tutarsız çıktı" (3/3 advisor onayı)
warblade state promptları silahı **her state'te bağımsız ve ismen** çağırdı:
- mid-run: *"sword kept readable at side"* · strike-windup: *"blade pulled back... preserve weapon identity"* · breathing-idle: *"sword ready"* · flinch: *"sword still in hand"*

Her `create_character_state` çağrısı base karakterden BAĞIMSIZ bir varyant türetir. Silah ismen geçince **difüzyon modeli her state için latent space'ten SIFIRDAN bir kılıç geometrisi uydurur** → state/yön arası kabza-balçak-namlu oranları değişir → animasyonda **"şekil değiştiren kılıç" (shape-shifting blade) titremesi**.
- `palette-snap=true` sadece RENGİ kilitler, **geometriyi DEĞİL**.
- `AI-freedom`/`strength` > 0.6 → silahı + pozu yeniden tasarlar (sapma artar). 0.3–0.4 → ana hatları korur.
- Model'in state'ler arası silah takibi yapacak 3D-rig/bone farkındalığı **YOK**.

→ **Kural:** Tutarlı silah istiyorsan silahı her state'te tekrar tarif ETME; bir kez **armed-anchor**'da kilitle, gerisini ondan türet. Silahsız istiyorsan silahı **hiç** isimlendirme; uzuv mekaniğini tarif et.

---

## 2. REÇETE (A) — SİLAHSIZ STATE (weapon-mount pipeline / post-demo)
**Felsefe:** "kılıcını savuruyor" DEME → "**kolunu/elini şöyle hareket ettiriyor**" DE. Eylemi nesneyle değil **eklem+kinetik** ile tarif et; el boş kalsın, silah sonradan Unity'de monte edilsin.

### Şablon (kopyala-yapıştır)
```
[CHARACTER]: same <char>, high top-down 2D game sprite, <ACTION_POSE — sadece uzuv mekaniği>, EMPTY HANDS, fists loosely clenched with no held items, hand in weapon-ready grip posture (curved open palm as if gripping an invisible haft), preserve armor details and silhouette, no weapons, no shield, no redesign
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: <karakter paleti>
```
`<ACTION_POSE>` örneği (strike-windup): *"right arm raised high back over right shoulder as if wound up for a heavy strike, torso twisted, weight on back foot"* — **silah kelimesi YOK.**

### Negatif prompt
```
sword, blade, weapon, dagger, staff, whip, gun, shield, held objects, items in hands, blurry edges, anti-aliasing, soft gradients, 3d render, isometric, watermark
```

### DO / DON'T kelime listesi
- **DO:** `empty hands` · `fists loosely clenched` · `no held items` · `weapon-ready grip posture` · `open curved palm` · `hands empty throughout all frames`
- **DON'T:** `sword` · `blade` · `greatsword` · `weapon` · `wielding` · `slash` · `grip hilt` · `weapon identity` · `shield`

> RIMA mount-infra ZATEN VAR: `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` + `Data/SpriteHandData.cs` + `Combat/WeaponDatabaseSO.cs` → post-demo silahsız+mount sıfırdan değil.

---

## 3. REÇETE (B) — TUTARLI-SİLAHLI BAKED STATE (demo)
**Pipeline:** armed-anchor-FIRST. Önce kılıcı tek anchor'da kilitle, TÜM aksiyon state'lerini O anchor ID'sinden türet.

### Adım 1 — Armed Anchor (kılıç tasarım kilidi) · `palette-snap=FALSE` (yeni metal/renk tanıt)
```
[CHARACTER]: same warblade, high top-down 2D game sprite, guarded breathing idle stance, WIELDING A TWO-HANDED GREATSWORD, metal blade kept readable pointing down-right, brass crossguard, dark leather grip, preserve armor and silhouette, no redesign, no VFX
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: dark slate gray armor, brass highlights, messy black hair, metallic gray steel blade
```

### Adım 2 — Aksiyon state'lerini ANCHOR'dan türet · `palette-snap=TRUE`, `AI-freedom=0.35`
```
[CHARACTER]: same warblade, high top-down 2D game sprite, <ACTION_POSE> with the same greatsword, preserving the exact blade design and crossguard shape, weapon held firmly in hands, preserve armor and silhouette, no redesign, no VFX
[STYLE]: <yukarıdaki ile birebir aynı>
[PALETTE]: snaps to parent state palette
```
`<ACTION_POSE>` = `mid-run pose, forward lean, sword at side` / `strike-windup, blade pulled back` / `flinch, upper body recoiling`. Hepsi AYNI anchor'dan → aynı kılıç.

---

## 4. GENELLENEBİLİR PROMPT-CRAFT KURALLARI (10 — gelecek oyunlar için)
1. **State-first:** animasyondan önce kritik poz state'lerini (`create_character_state`) sabitle, sonra `animate_character` ile türet.
2. **Armed-anchor hiyerarşisi:** baked silahta ASLA base'den bağımsız aksiyon state'i türetme; önce armed-anchor → ondan türet.
3. **Silahsız = anatomik tarif:** hareketi silahla değil kol/eklem mekaniğiyle yaz (`right arm in wide horizontal arc`), eli boş bırak + negatif filtre.
4. **Kamera kilidi:** her promptta `top-down 35 degree ARPG angle`; negatife `isometric, side-view`.
5. **Palette-snap:** türetilen state'lerde `palette-snap=true` → renk kayması yok (ama geometriyi kilitlemez!).
6. **Düşük AI-freedom:** türetimde `AI-freedom`/`strength` = 0.3–0.4; >0.6 silahı/zırhı tanınmaz yapar.
7. **No-redesign guard:** her prompt sonuna `preserve armor and silhouette, no redesign, no VFX`.
8. **Matte pixel clusters:** `crisp hard edges, matte hand-pixeled clusters, no anti-aliasing` → difüzyon bulanıklığını engeller. "highly detailed/epic 4k" gibi boş buzzword YASAK.
9. **Headroom:** `don't fill canvas, leave wide transparent headroom` → lunge/vuruşta kırpılma yok (~%40 boşluk).
10. **5+3 yön + horizontal-right silah:** S/SE/E/NE/N üret, W/SW/NW = Unity flipX mirror. Bağımsız silah objeleri (`create_object`) yatay-sağ (kabza sol, uç sağ) çiz → pivot standardı.

---
## 5. RIMA AKSİYONU
- **Demo:** warblade = Reçete (B). Mevcut 4 silahsız-base state TUTARSIZ kılıç riski → **armed-anchor'dan REDO** (kullanıcı PixelLab Web UI).
- **Post-demo:** Reçete (A) ile silahsız base + mevcut `HandAnchorAttach` mount infra.
- Elementalist (caster, silahsız) state'leri = sorunsuz, dokunma.
