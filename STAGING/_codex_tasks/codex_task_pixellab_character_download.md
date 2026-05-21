# CODEX TASK — Download 10 PixelLab Characters (8-dir each)

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

---

## Hedef

10 canonical character anchor'ın **8-dir rotation PNG**'lerini PixelLab'tan local'e indir. Skill sheets v3 + animation work için reference olarak hazır olsun.

## Bağlam

Mevcut local PNGs:
- `Assets/Art/Characters/Warblade/Rotations/*.png` — 8 direction ✓ (manuel indirilmiş)
- `Assets/Art/Characters/Brawler/brawler_south.png` — sadece south
- Diğer 8 class için local PNG YOK

User feedback (skill sheets v2): "10 karakteri pixellabdan çek". v3 sheet için actual sprite reference gerek.

## 10 Character IDs (Roster v2 LOCK)

| # | Class | PixelLab ID |
|---|---|---|
| 1 | Warblade | `2656075d-d113-4f18-a6c1-94b5a6b8bf65` (zaten var, kontrol et) |
| 2 | Ronin | `a7957352-cc57-44a1-a9fc-96f1fbd1119a` |
| 3 | Gunslinger | `a78545eb-ef10-4e1e-827e-784000e45886` |
| 4 | Ranger | `d5b1cf71-0158-4347-97b9-a34a5ac0d98a` |
| 5 | Elementalist | `4c83c0be-e856-48f1-b8b5-9626e041a082` |
| 6 | Shadowblade | `deee34b5-7796-4c8f-9262-b8a83f907240` |
| 7 | Ravager | `091e9552-7f57-44d0-8ae3-49f689304c7e` |
| 8 | Hexer | `e260a1af-930d-4e5b-9d5e-bc11abd7c92f` |
| 9 | Brawler | `d4fa3d13-35f1-4d65-849c-dfafff688593` (south var, 7 daha) |
| 10 | Summoner | `83039c80-d2fe-448a-8c15-ecf55c0f2f7c` |

## Rotation URL Pattern

PixelLab API base: `https://backblaze.pixellab.ai/file/pixellab-characters/<user-uuid>/<character-id>/rotations/<direction>.png`

User UUID `f587b47a-7c0e-4f37-a6c9-7d311a2c935f` (Warblade'den extract edildi).

Per character:
- south.png
- east.png
- north.png
- west.png
- south-east.png
- north-east.png
- north-west.png
- south-west.png

## Görev

### Adım 1: get_character verify
Her character_id için PixelLab MCP `get_character(character_id="...", include_preview=false)` çağır — completed status + rotation URLs döner. Eğer status != completed BLOCKED yaz, atla.

### Adım 2: Download
Her direction PNG'sini curl/wget ile indir:
```
curl -o "Assets/Art/Characters/<Class>/Rotations/<class_lower>_<direction>.png" "<url>"
```

Klasör yapısı:
```
Assets/Art/Characters/
├── Warblade/Rotations/         (var, 8 PNG)
├── Ronin/Rotations/
├── Gunslinger/Rotations/
├── Ranger/Rotations/
├── Elementalist/Rotations/
├── Shadowblade/Rotations/
├── Ravager/Rotations/
├── Hexer/Rotations/
├── Brawler/Rotations/
└── Summoner/Rotations/
```

Dosya isim format: `<class_lower>_<direction>.png` (örn. `ronin_south.png`, `ronin_north_east.png`)

### Adım 3: Unity Import Settings
Her PNG için Unity sprite import settings ZORUNLU (Pixel Perfect Camera + URP 2D uyumlu):
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 64
- Filter Mode: Point (no filter)
- Compression: None (Uncompressed)
- Max Size: 256

Bunu Unity asset importer .meta file write ile veya UnityMCP `manage_asset` ile yap (Codex Unity erişimi varsa).

Eğer UnityMCP yoksa: SADECE PNG indir + Unity'de import settings sonradan elle yapılır. Final Report'a not düş.

### Adım 4: Verify
Her klasör için file count = 8 doğrula:
```
ls Assets/Art/Characters/<Class>/Rotations/ | wc -l
```

### Adım 5: Git commit
```bash
git add Assets/Art/Characters/
git commit -m "[S96] Pull 10 character anchors from PixelLab (8-dir each, 80 PNG)"
```

## Kısıtlar

- Sadece 10 anchor character indir (state variants YOK)
- Sadece 8 rotation (animation YOK — ayrı task)
- Mevcut Warblade Rotations OVERWRITE etme — eğer dosyalar varsa skip
- BLOCKED if: get_character status failed, curl erişim yok, Asset folder write izni yok

## Final Report

`STAGING/_codex_done/CODEX_DONE_pixellab_character_download.md`:
- 10 character × 8 direction = 80 PNG download tablosu (PASS/FAIL per character)
- Toplam disk usage
- Unity import settings status
- Commit hash
- Eksik: animation download (sonraki task, ayrı dispatch öncelik P3)

## Dispatch

`--effort high --profile yasinderyabilgin --timeout 3600`, background.
