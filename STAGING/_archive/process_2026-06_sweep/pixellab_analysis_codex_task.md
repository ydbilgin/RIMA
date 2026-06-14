# PixelLab Envanter — Codex Technical + Codebase Mapping Analiz

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
243 PixelLab obje bağımsız technical analiz. Opus/agy çıktısına BAKMA, ortak karar SONRA loop sentez aşamasında.

## Bağlam (envanter)
- Master doc: `STAGING/PIXELLAB_INVENTORY_MASTER.md` (243 obje)
- 46 PNG + 29 prefab local — 197 obje cloud-only
- 3 orphan local sprite (cloud'da yok)

## İş kalemleri

### 1. Local asset audit
- `Glob: Assets/Sprites/Environment/ShatteredKeep_PixelLab/**/*.png`
- `Glob: Assets/Prefabs/Props/ShatteredKeep_PixelLab/**/*.prefab`
- Mevcut import-ready vs sahnede kullanım analizi
- 33 prefab D2 backfill (mounting/statue/obstacle) — sahnede referans var mı?

### 2. Codebase referans çapraz check
- Asset GUID → scene/prefab referans grep:
  - `PlayableArena_Test01.unity` — sahne içeriği
  - `Assets/Prefabs/**/*.prefab` — prefab referansları
- Kullanılmayan local asset'leri tespit (cloud silinse bile local kalsın mı?)

### 3. Weapon obje analiz (18 cloud weapon)
- Hangi class için?
- Dimension/canvas spec uygun mu RIMA için?
  - Memory: weapon canvas 64×96 chibi RIMA standart (weapon_master_spec_10_class.md)
- Local'de hiç weapon PNG yok (verified) — 18 cloud weapon import edilmeli mi?
- Warblade greatsword için 441bccf0 longsword cloud'da hala var mı?
- Ronin katana 692f43ce cloud'da hala var mı?

### 4. Skill icon analiz (22 cloud)
- UI için skill draft (SkillOfferUI.cs LIVE S108)
- Memory: `warblade_12_common_skills_spec.md` 12 skill — bu 22 icon hangileri?
- Local hiç import edilmemiş — neden? Boyut/format uyumsuzluk mu, henüz scope dışı mı?

### 5. Mounting + statue 64-128 boyut audit
- D2 backfill 33 prefab — sortingLayerName + AssetCategory metadata doğru mu?
- mounting top-center pivot (D2 verify): cloud orijinal pivot ne, local reimport sonrası ne?

### 6. Silahlar boyut spec (kullanıcı net direktif)
- 10 class silah için ideal pixel canvas:
  - Greatsword (Warblade) — 128×256 (plan dosyasında)
  - Katana (Ronin) — ?
  - Pistol (Gunslinger) — ?
  - Bow (Ranger) — ?
  - Staff (Elementalist) — ?
  - Dagger (Shadowblade) — ?
  - Greataxe (Ravager) — ?
  - Whip (Hexer) — ?
  - Gauntlets (Brawler) — ?
  - Tome+Orb (Summoner) — ?
- Mevcut codebase (HandAnchor offset table) ile uyumlu spec öner

## Output dosyası
`STAGING/PIXELLAB_ANALYSIS_CODEX.md`

Format:
```
## Codex Verdict — bağımsız technical analiz

### Local vs Cloud cross-check
| Kategori | Local | Cloud only | Orphan |
|---|---|---|---|

### Codebase referans
- Kullanılan local: N adet
- Kullanılmayan local: N adet (silinmeli mi?)
- Cloud weapon 18 — import scope öneri

### 10 Class silah boyut spec
| Class | Weapon | Canvas | PPU | Pivot | Mount type |
|---|---|---|---|---|---|
| Warblade | Greatsword | 128×256 | 100 | handle mid (%25 top) | 2-hand |
| ... |

### Skill icon 22 cloud
- 12 skill spec uyumu: N adet match
- Format spec: TBD
- Import scope: GEREK / GEREKSİZ

### Open technical questions
1. ...
```

## YASAK
- Codex output dosyalarını silme (read-only)
- Opus/agy verdict'lerine bakma
- Asset gen
- Cloud delete

## Süre
~30-45 dk via cx_dispatch.

KAPADIN: `STAGING/PIXELLAB_ANALYSIS_CODEX.md` + 10 cümle özet orchestrator için.
