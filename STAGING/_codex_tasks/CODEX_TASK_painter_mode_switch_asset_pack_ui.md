# CODEX TASK — Painter Mode Switch + Asset Pack Add/Remove UI

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

User 2 yeni painter feature istiyor:
1. **Top-down ↔ Isometric mode switch** painter window header'da toggle
2. **Asset pack add/remove buttons** palette panel'inde (paletten asset add/remove)

## Hedef Dosya

`Assets/Editor/RimaUnifiedPainterWindow.cs` (namespace RIMA.Editor.MapDesigner)

## Feature 1 — Mode Switch (Top-down / Isometric)

### Yeni alan
```csharp
public enum PaintMode { TopDown, Isometric }
[SerializeField] private PaintMode currentPaintMode = PaintMode.Isometric;  // Default RIMA fake-iso
```

### UI
Painter window header'a (üst kısım, kategori sekmelerinin yanına/üstüne):
- `EnumPopup` veya `Toolbar` ile **"Mod: [Top-down | Isometric]"** toggle
- Mevcut category sekmeleri (Floor / Wall / Prop / Mob) bu mod'un ALTINDA çalışır

### Davranış
- `currentPaintMode == Isometric`: mevcut iso davranış (Antigravity fix sonrası, flipX/Y-squash compensation aktif)
- `currentPaintMode == TopDown`: mevcut iso compensation'ları PASS — direct cell-to-world placement, no Y-squash, no flipX fallback
  - `GetPlacementOffset` Y-squash compensation skip
  - `ComputeCompensatedLocalScale` parent.lossyScale=1 varsay
  - Wall flipX/rotation logic: top-down mode'da face_NS YOK ise NW-SE direction wall SKIP (paint reddedilir, user uyarısı: "TopDown modunda face_NS gerekli")

### Persistence
`PlayerPrefs` ile mode kalıcı: `PlayerPrefs.SetInt("RimaPainter_PaintMode", (int)currentPaintMode)`.

## Feature 2 — Asset Pack Add/Remove UI

### Konum
Palette panel'i (Floor/Wall/Prop/Mob sekmelerinin altında — her kategori için palette listesi). Palette listesinin ALTINDA yeni 2 buton:

```
[Palette of current category — N items]
...
[ ➕ Add From Project ]   [ ➖ Remove Selected ]
```

### Add From Project flow
1. Buton tıklanır → `EditorUtility.OpenFilePanel` (veya `ObjectPicker`) açılır
   - Filter: kategoriye göre — Floor (Tile/Sprite), Wall (GameObject prefab), Prop (prefab), Mob (prefab)
2. User asset seçer → palette listesine ADD
3. SerializedField olarak persist (`paletteOverrides` per category)
4. Repaint

### Remove Selected flow
1. User palette item'a tıklar → seçili olur (mevcut selection mekanizması)
2. Remove buton tıklanır → confirmation dialog: "Bu asset palette'ten kaldırılsın mı? (asset dosyası SİLİNMEZ, sadece palette'ten çıkar)"
3. Confirm → palette listesinden çıkar, scan folder'dan da exclude ediliyorsa exclude list'e ekle
4. Repaint

### Persistence
`[SerializeField] private Dictionary<PaletteCategory, List<string>> paletteCustomAdds = new();` — kategori bazlı kullanıcı-eklenen asset path listesi
`[SerializeField] private Dictionary<PaletteCategory, List<string>> paletteExcludes = new();` — palette'ten çıkarılan asset path listesi

`OnEnable`'da rescan sırasında bu listeleri honor et: scan results + custom adds, sonra excludes filter.

## Constraint

- **Antigravity painter fix'i BOZMA** — flipX/Y-squash logic'i Isometric mode'da aynen kalır
- Mode = TopDown ise sadece compensation'ları DEVRE DIŞI bırak, Antigravity logic'i silme
- UI minimal — yeni window/panel açma, mevcut painter window içine ekle
- Mevcut hot-reload (rescan + sync assets butonu) custom adds + excludes'u sıfırlamasın

## Test

UnityMCP ile:
1. Painter window aç
2. Mode toggle dene — Isometric ↔ TopDown
3. Wall paint dene her iki mode'da — Iso mode'da flipX aktif, TopDown mode'da skip
4. Floor tile paint dene her iki mode'da — Iso mode'da Y-squash compensated, TopDown mode'da direct
5. "Add From Project" — bir test prefab seç → palette'te göründü mü?
6. "Remove Selected" — palette'ten test asset çıkar, repaint sonrası görünmüyor mu?
7. Restart Unity → mod + custom adds + excludes persist ediyor mu?

## Rapor

`STAGING/CODEX_DONE_painter_mode_asset_ui.md`:
- Modified line ranges
- Test paint screenshot path (eğer alındıysa)
- Mode toggle behavior log
- Add/Remove flow verify
- Console error count
- Antigravity fix corruption check (Iso mode flipX/Y-squash hâlâ çalışıyor mu)

## Effort

medium-high — UI scope geniş, ~60-90 dakika.
