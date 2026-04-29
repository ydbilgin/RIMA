# Gemini — RIMA S43 Anchor Roster Visual Review

**Bu dosya self-contained.** Başka dosya açmana gerek yok — gerekli tüm context aşağıda.

---

## Görev

10 character anchor PNG'sini incele. Sprite'lar 128×128 dark fantasy ARPG roguelite roster'ı (oyun adı: RIMA).

İki ana soruya cevap ver:
1. **Aynı artist'ten çıkmış gibi mi?** (style cohesion)
2. **Her class kendi kimliğini ilk bakışta veriyor mu?** (identity recognition)

Sonuç dosyasını şu yola yaz: `_STAGING/anchors/_GEMINI_REVIEW_S43.md`

---

## Inputs

10 PNG dosyası (her biri 128×128, transparent BG):

```
_STAGING/anchors/elementalist/elementalist_anchor.png   ← STYLE ANCHOR (referans)
_STAGING/anchors/warblade/warblade_anchor.png
_STAGING/anchors/shadowblade/shadowblade_anchor.png
_STAGING/anchors/ranger/ranger_anchor.png
_STAGING/anchors/ravager/ravager_anchor.png
_STAGING/anchors/ronin/ronin_anchor.png
_STAGING/anchors/brawler/brawler_anchor.png
_STAGING/anchors/gunslinger/gunslinger_anchor.png
_STAGING/anchors/hexer/hexer_anchor.png
_STAGING/anchors/summoner/summoner_anchor.png
```

Eğer summoner_anchor.png yoksa: 9 karakterle devam et, raporda not et.

---

## RIMA Art Direction (referans)

- **Tür:** Dark fantasy action roguelite
- **Perspektif:** 30-35° 3/4 ARPG view (Cursemark / Last Epoch / Hero Siege tarzı — yüz okunaklı, vertical face dominant)
- **Vibe ref:** Cursemark (kamera + netlik) + Hades (ton) + Diablo 2 (atmosfer)
- **PixelLab view:** "low top-down" (25-40° native)
- **Yön:** 4-cardinal (S/E/N/W) + diagonal cheat
- **PPU:** 128
- **Style description:** "Stylized ARPG fantasy, slight dark atmosphere, thick single-color outline, chunky pixel clusters, painterly weathered shading, muted desaturated palette with class accent"
- **YASAK:** high top-down, side view, anime/cute, full grimdark/black-blob

---

## Class Identity (her sprite'a karşılaştır)

**ÖNEMLİ:** Sprite'a önce bak, ne gördüğünü yaz, SONRA aşağıdaki description'ı oku ve karşılaştır. Bias kaçır.

### 01 — WARBLADE
Male heavy fighter, late 20s. Light-tan weathered skin, short dark hair, trimmed dark beard, diagonal scar on left cheek. Dark cracked steel chestplate, worn brown leather straps crossing torso, split battle-tabard at waist. Asymmetric right shoulder pauldron only. No helmet, no cape. Massive greatsword in RIGHT HAND — blade angled down at 45°, tip near right foot. Cold blue (#66AAFF) hairline crack glow on sword fuller and one chest plate crack only. AVOID: raised sword, full armor, helmet, dark skin.

### 02 — ELEMENTALIST (STYLE ANCHOR)
Female arcanist, mid-20s, lean build. Fair skin. Warm honey-blonde hair in low rounded bun at nape of neck. Dusty-indigo sleeveless crop top, entire midriff bare. Short dusty-indigo miniskirt with antique gold filigree hem edge. Black tights, brown leather boots, fingerless leather gloves. Fist-sized gold-cream orb hovering above LEFT PALM at chest height. Right arm relaxed at side. NO STAFF. Warm gold-cream glow on orb only. AVOID: staff, robe, covered midriff, top bun.

### 03 — SHADOWBLADE
Male rogue, late 20s, wiry lean build. Olive Mediterranean skin. Messy short black hair. Dark cloth veil covering nose and mouth — dark eyes visible above veil. Dark brown leather jacket over charcoal cloth undershirt. Cloth forearm wraps, leather bracers, dark navy trousers. RIGHT HAND: reverse grip dagger, blade pointing DOWN behind wrist. LEFT HAND: forward grip dagger, blade angled down-forward. Void purple (#9933CC) hairline glow on dagger edges. AVOID: full hood, all-black silhouette.

### 04 — RANGER
Female hunter, mid-20s, lean athletic. Tanned skin, freckles. Auburn (reddish-brown) hair in tight practical braid. Sleeveless leather vest over bone-white combat wrap, leather bracers. Side-hip quiver on RIGHT HIP — NOT a back quiver. Bone-recurve bow held vertically in LEFT HAND. RIGHT HAND reaching to side-hip quiver. Bow in LEFT hand — mandatory. Gold (#FFCC00) on bow limb tips and arrow fletching only. AVOID: bow in right hand, back quiver, hood.

### 05 — RAVAGER
Male berserker, late 20s, very muscular. Sunburned tan skin, pale wild eyes. HAIR MANDATORY: wild blond-red hair, matted past shoulders. Short blond-red beard. NOT dark, NOT black. Bare scarified chest, one large geometric rune scar at sternum. Leather harness straps. Fur mantle on LEFT shoulder only. Dark kilt, leather bracers. No shirt. Dual chipped cleaver-axes, one each hand, blades angled outward-down. Blood red (#FF3322) in scar channel lines and axe edge stains only. AVOID: dark/black/brown hair, shirt, flames.

### 06 — RONIN
Male swordsman, late 20s, lean athletic. Medium skin, East Asian features. Calm narrow eyes, small chin scar. Black hair in loose low knot at nape. THREE dark cloth layers: charcoal gi top, dark navy-grey underlayer, dark cloth trousers. Tabi boots. Broken lamellar shoulder plate remnant on right shoulder. SINGLE katana SHEATHED at LEFT HIP. Right hand on hilt. Left hand steadying scabbard. Blade NOT drawn. Pure white (#FFFFFF) hairline crack on scabbard edge only. AVOID: drawn sword, second sword, heavy armor, all-black blob.

### 07 — BRAWLER
Male fighter, late 20s, compact extremely muscular. **DARK BROWN or DEEP BRONZE SKIN — unique to this character in the roster, mandatory.** Tight close-cropped black hair, strong jaw, broken nose. Bare chest with charcoal-purple geometric tattoo lines — flat ink, NOT glowing. Loose charcoal trousers, cloth knee wraps, leather boots. Bone-and-steel knuckle gauntlets — NOT boxing gloves. Bone-white knuckle plates, dark steel backing. Boxing guard: LEFT fist near chin, RIGHT fist near sternum. Amber (#FF8800) subtle glow at knuckle contact points only. AVOID: light/tan skin — must be dark bronze. Boxing gloves, shirt.

### 08 — GUNSLINGER
Female duelist, mid-20s, lean athletic. **BROWN SKIN — medium to dark brown, mandatory.** Small scar on upper lip, confident smirk. Deep auburn hair, braid over one shoulder. Dark burgundy short half-coat open over black sleeveless vest, visible midriff. Diagonal bandolier strap. Dual thigh holsters. No hat, no goggles. Two pistols — one in EACH hand, both lowered at sides. Brass-yellow (#FFB800) on pistol cylinder and barrel mechanisms only. AVOID: fair/pale skin — must be brown. Hat, goggles, pistols raised.

### 09 — HEXER
Female curse-binder, mid-20s, gaunt thin. Near-grey pale skin, sharp cheekbones, pale yellow irises. Haunted expression. Dark hair below cropped hood — hood frames face but does NOT cover it. Asymmetric ritual tunic, exposed collarbone and forearms. Charcoal curse-script tattoo marks on forearms. Dark cloth trousers. NOT a full robe. **LARGE PHYSICAL LANTERN in LEFT HAND at chest height** — real lantern object, NOT an orb. Cursed yellow-green (#CCFF00) light from lantern. NO STAFF. Right hand at side, faint curse tendrils. AVOID: staff — LANTERN only. Full robe. Face mask. Hood covering face.

### 10 — SUMMONER
Female death commander, mid-20s, tall willowy. Near-white pale skin, large dark eyes with faint necro green inner glow. Silver-white flowing hair. Hood fully pushed back — face completely visible. Layered black funeral mantle, narrow fitted waist. Bone fetishes at belt. **Tall staff in LEFT HAND** — tip near ground, staff head above her head. RIGHT HAND slightly raised, open palm forward — faint necro green (#22FF88) summoning light in palm. Necro green (#22FF88) on staff orb, palm light, and robe hem only. AVOID: staff in right hand, lantern (that's Hexer), hood covering face.

---

## Inceleme Soruları

### Bölüm A — Style Cohesion (her metrik 1-5 puan, 1=tutarlı, 5=kopuk)

1. **Pixel density** — Tüm 10 karakter aynı pixel grid'de mi yoksa bazıları daha "yüksek çözünürlük" gibi mi?
2. **Outline** — Outline kalınlığı/rengi/dağılımı tutarlı mı?
3. **Shading dili** — Aynı painterly weathered shading mı, yoksa biri "düz cel-shaded", diğeri "yumuşak gradient" mi?
4. **Renk paleti** — Muted desaturated mı, yoksa biri aşırı saturated/parlak mı?
5. **Proportions** — Kafa-vücut oranı (~7-head) tutarlı mı? Kim daha "chibi", kim daha "realistic"?
6. **Camera angle** — Hepsi 30-35° low top-down mı? Side-view veya düz front var mı?
7. **"Aynı artist" hissi (1-10)** — Roster bütün baktığında bir game studio'nun art direction'ı oturmuş hissi var mı?

### Bölüm B — Per-Character Identity (her 10 karakter için ayrı ayrı)

Description'ı önce SAKLAYIP sprite'a bak, sonra description ile kıyasla:

1. Class'ı bilmeden sprite'a baksan: tank/melee/range/caster hangisi?
2. Cinsiyet açık mı?
3. Silah/tematik element ilk bakışta okunuyor mu?
4. Accent renk görünür mü? Hangi kısımda?
5. Description'a karşı: ne uyuyor, ne eksik/yanlış?

### Bölüm C — Roster İçi Karışma (kritik)

1. Hangi 2 karakter siluet olarak çok benziyor? (örn. Hexer + Summoner ikisi de hooded female robe riski)
2. Kim siluetinden tanınamıyor?
3. Renk paletinde hangileri "aynı palette"?

### Bölüm D — RIMA Vibe (Dark Fantasy ARPG)

1. Her sprite "Cursemark / Last Epoch / Hades" referans hissi veriyor mu?
2. Aşırı bright/anime/cute olan var mı?
3. Aşırı grimdark/black-blob (kimliksiz) olan var mı?
4. Genel ton "stylized ARPG fantasy slight dark atmosphere" mı?

### Bölüm E — V3 Önerileri

1. **Universal:** Tüm 10'a uygulanabilir 1-2 küçük değişiklik var mı?
2. **Per-class:** En zayıf 3 karakter hangileri? Her birine 1 cümle fix önerisi.
3. **Skill fantasy match:** Hangi karakter "fantezisini ilk bakışta vermiyor"? Pose/prop önerisi.

---

## Özellikle Dikkat Edilecekler

Claude (orchestrator) zaten ön analiz yaptı, şu noktalarda ikinci görüş istiyor:

- **Brawler skin tone:** Description "DARK BROWN/DEEP BRONZE" zorunlu diyor. Sprite gerçekten dark bronze mı yoksa light/tan mı görünüyor? Roster'daki en koyu cilt mi?
- **Gunslinger skin tone:** "Medium to dark brown, mandatory". Yeterince koyu mu?
- **Hexer fener hangi elde?** Description LEFT, sprite muhtemelen RIGHT. Aseprite flip lazım mı?
- **Shadowblade stil drift:** Roster içinde stil olarak diğerlerinden farklı render gibi mi?
- **Hexer ↔ Summoner siluet karışması:** İkisi de female robed/hooded, ayrı tanınıyor mu?

---

## Output Format

Şu dosyayı yaz: `_STAGING/anchors/_GEMINI_REVIEW_S43.md`

```markdown
# Gemini Visual Review — RIMA S43 Roster
**Date:** <YYYY-MM-DD>
**Reviewer:** Gemini

## A — Style Cohesion
- Pixel density: X/5 — <1 cümle gerekçe>
- Outline: X/5 — <gerekçe>
- Shading: X/5 — <gerekçe>
- Palette: X/5 — <gerekçe>
- Proportions: X/5 — <gerekçe>
- Camera: X/5 — <gerekçe>
- "Aynı artist" hissi: X/10

## B — Per-Character Identity
### 01 Warblade
- Class izlenimi: tank/melee
- Cinsiyet: clear
- Silah/element: greatsword visible
- Accent: cold blue on sword fuller — visible/faint/missing
- Description match: <hangi noktalar>
- Eksiklik: <varsa>

### 02 Elementalist
(...)

(10 karakter)

## C — Roster Collision
- Karışan ikili: <class1> ↔ <class2> — neden
- Tanınmaz silüet: <class>
- Palette overlap: <list>

## D — RIMA Vibe
- Cursemark hissi veren: <list>
- Aşırı bright/cute: <list veya none>
- Aşırı black-blob: <list veya none>
- Genel verdict: PASS / FAIL

## E — V3 Recommendations

### Universal (tüm 10'a)
- <öneri 1>
- <öneri 2>

### Per-Class (en zayıf 3)
- <class>: <fix>
- <class>: <fix>
- <class>: <fix>

### Skill Fantasy Match
- <class>: <pose/prop önerisi>

## Cevaplar — Claude'un Sorduğu Spesifik Noktalar
- Brawler skin: <değerlendirme>
- Gunslinger skin: <değerlendirme>
- Hexer fener eli: LEFT / RIGHT
- Shadowblade stil drift: var/yok, neden
- Hexer-Summoner karışma riski: var/yok
```

---

## Done When

- [ ] 10 karakter için Bölüm B doldu
- [ ] Bölüm A'da 7 metrik puanlı
- [ ] Bölüm C, D, E doldu
- [ ] Spesifik sorular cevaplandı
- [ ] Çıktı dosyası `_STAGING/anchors/_GEMINI_REVIEW_S43.md` olarak yazıldı
