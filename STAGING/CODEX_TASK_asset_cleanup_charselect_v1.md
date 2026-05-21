# Codex Task — Asset Cleanup + Character Select Update

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

UnityMCP REQUIRED.

---

## Görev — 2 Paralel İş

### İş A — Asset Pack Cleanup (mantıksız/saçma assetleri archive et)
### İş B — Character Select Screen Update (eski class image'leri canonical sprite ile değiştir)

---

## İş A — Asset Cleanup

User feedback: "dust patch falan çok manasız kalıyor", "bir sürü mantıksız duran şey var", "RIMA asset packinden çıkaralım".

### Confirmed Removal (user explicit)

5 dust asset → **archive et** (silme, `_archive/dust_removed_2026_05_21/` taşı):
1. `Assets/Art/AssetPacks/Act1_ShatteredKeep/decals/act1_decal_dust_var0_08.png`
2. `act1_decal_dust_var1_09.png`
3. `act1_decal_dust_var2_10.png`
4. `act1_decal_dust_var3_11.png`
5. `Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/act1_patch_dust_drift_v01.png`

Meta dosyalarını da archive'a taşı (GUID preserve).

### Additional Audit (sen değerlendir)

`Assets/Art/AssetPacks/Act1_ShatteredKeep/` içindeki tüm asset'leri inceleple ve **atmosphere/combat room context'inde mantıksız** olabilecek ek aday belirle. Atmospheric dungeon mood (Hades/Diablo) için DEĞMEZ olanları flag et.

**Kriter:**
- Random scatter / placeholder hissi
- Combat room mood'a uymayan (örn. çiftçilik aletleri olsaydı — atmosphere bozar)
- Aynı functional kategoride birden fazla (örn. 4 dust = aşırı redundant)
- Visual quality kötü (PixelLab drift, placeholder render)

**Atmosphere-fit assets (KEEP — koruyoruz):**
- Floor tiles (granite variants)
- Walls (Pilot A 7-piece)
- Pillars (3)
- Braziers (cyan + orange)
- Statues (3)
- Ritual altar + obelisk + bench + marker + headstone
- Wall decorations (banners, chains, lantern, cage, skeleton_shackled, grate, ivy_vine)
- Rift accents (cyan crack overlay)
- Scatter (bone offering pile, skull pile)
- Decals: crack + bone chip variants (dust YOK, archived)
- Props: brazier, urn, rubble debris, rubble heap skulls, treasure pile, barrel, crate

**Possible additional removal candidates (sen audit et):**
- Aynı functional group içinde fazlalık variant
- Style drift gösteren (Pilot A wall set'inden style farklı)
- Combat room context'inde "neden burada?" hissi

Audit raporu yaz: `STAGING/_research/ASSET_PACK_CLEANUP_AUDIT.md`
- Her removal candidate için: path + sebep + KEEP/ARCHIVE/REGEN-LATER verdict
- User onay bekleyen şüpheli case'ler ayrı section

### Cleanup Process

1. Confirmed 5 dust asset → `_archive/dust_removed_2026_05_21/` taşı (mv, silme)
2. Audit candidates → audit raporuna yaz, **user onayı olmadan taşıma**
3. Unity asset reference check: dust asset'leri kullanan prefab/scene var mı? Varsa missing reference olur, raporla.
4. Asset Database refresh

---

## İş B — Character Select Screen Update

User feedback: "ana ekrandaki class resimleri hala eski güncelle"

### Inspect

- `Assets/Scenes/UI/CharacterSelect.unity` aç
- `Assets/Scripts/UI/CharacterSelectController.cs` + `CharacterSelectScreen.cs` oku
- Mevcut class card'larda kullanılan sprite reference'ları bul
- Hangi sprite path'ler atanmış raporla

### Canonical Sprite Replacement

**Yeni sprite source:** `Assets/Art/Characters/{Class}/Rotations/{class}_south.png`

10 class:
1. Warblade → `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
2. Ronin
3. Gunslinger
4. Ranger
5. Elementalist
6. Shadowblade
7. Ravager
8. Hexer
9. Brawler
10. Summoner

**Mapping:**
- Phase 1 unlocked 4: Warblade, Elementalist, Ranger, Shadowblade — full color sprite
- Locked 6: Ronin, Gunslinger, Ravager, Hexer, Brawler, Summoner — silhouette (black tint or 30% saturation)

### UI Layout (v2.3 spec)

CharacterSelectScreen card layout:
- 4 active class cards: portrait + name + "Click to Start"
- 6 locked class cards: silhouette + name + "Unlock for X Echoes"
  - Ronin: 120 Echoes
  - Ravager: 120
  - Gunslinger: 180
  - Brawler: 180
  - Summoner: 180
  - Hexer: 250 + "Elementalist ile 1 run yap"

Mevcut CharacterSelectScreen.cs **lock UI** logic varsa extend, yoksa add.

### Asset Path Setting

UnityMCP ile:
1. `CharacterSelect.unity` scene aç
2. Per-card GameObject bul (10 card, naming pattern muhtemelen "Card_Warblade" vs)
3. Image component sprite field'ını canonical sprite path'e set et
4. Locked cards: silhouette material (gri/dark tint) apply
5. Cost text + precondition text güncelle

### Asset Reference Check

Eğer `CharacterSelectScreen.cs` runtime sprite assign ediyorsa (kod içinde Resources.Load veya direct asset ref):
- Class enum → sprite path mapping güncelle
- ScriptableObject `ClassUnlockData` varsa sprite field'ı oraya da ekle

### Test

1. PlayMode → CharacterSelect scene aç
2. 10 card render edildiğini doğrula
3. 4 active, 6 silhouette görsel kontrol
4. Cost text 6 locked card'da doğru
5. Screenshot `STAGING/screenshots/character_select_updated.png`

---

## Output

### Asset Cleanup
1. `Assets/Art/AssetPacks/Act1_ShatteredKeep/_archive/dust_removed_2026_05_21/` — 5 dust asset + meta
2. `STAGING/_research/ASSET_PACK_CLEANUP_AUDIT.md` — additional removal candidates user onayına

### Character Select
3. `Assets/Scenes/UI/CharacterSelect.unity` — updated sprite assignments
4. `Assets/Scripts/UI/CharacterSelectScreen.cs` — extended lock UI (varsa)
5. `STAGING/screenshots/character_select_updated.png`

### Reports
6. `CODEX_DONE_*.md`: cleanup count + char select status + compile result

---

## Compile Check (HARD RULE — 2026-05-21 LOCK)

- Console hatalarını OTOMATIK fix et: dispatch sonrası `read_console` çağır
- Error/warning varsa çöz + recheck
- Hâlâ hata varsa BLOCKED + raporla
- Bu task scope'a embed

## Kısıt

- Dust assets sadece **archive**, **delete YASAK** (GUID preserve)
- Audit additional candidates: **user onayı olmadan taşıma yok** — sadece raporla
- Character Select: canonical chibi sprite kullan (yeni portrait gen YASAK)
- Locked class lock condition güncelleme: v2.3 LOCK Section 1.1 economy doğru
- Mevcut UI layout'a saygı — sadece sprite + text update, layout dokunma

## Effort
high
