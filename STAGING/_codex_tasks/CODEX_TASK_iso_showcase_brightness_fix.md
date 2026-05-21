# CODEX TASK — IsoShowcaseRoom Brightness + Framing Fix (S95)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun

İlk screenshot (`STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png`) çok karanlık — content okunabilir ama showcase görünümünden uzak. Composition PASS (her saçmalık check geçti), sadece görünürlük problemi var:
1. Global ambient intensity 0.25 düşük
2. Camera orthographic size 5 → sağ yarı tamamen boş (render 16:9, oda kare-ish)
3. Spot light intensity'leri global ambient'a yenik

## Görev

`Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` sahnesinde **sadece bu 5 değeri değiştir**, başka hiçbir şeye dokunma:

### Değişiklikler

| Hedef | Eski | Yeni | Neden |
|---|---|---|---|
| `Lights_Root` Global Light2D intensity | 0.25 | **0.55** | Oda görünür ama hâlâ atmosferik karanlık |
| `Lights_Root` Global Light2D color | `#1A1A2A` | **`#2A2238`** | Slightly bluer-warmer, less pitch-black |
| L1 (cyan rift) intensity | 1.0 | **1.7** | Primary focal pop |
| L2 + L3 (torch sconces) intensity | 0.8 | **1.4** | Warm framing visible |
| L4 (orange brazier altar) intensity | 0.6 | **1.1** | Altar approach legible |
| L5 (cyan brazier rift) intensity | 0.5 | **0.9** | Reinforce primary cyan |
| Main Camera orthographic size | 5 | **3.5** | Sahne ekrana sığsın, sağ boşluk kapansın |
| Main Camera world position X/Y | (1.5, 1.838, -10) | **(2.2, 2.3, -10)** | Center on cell (4, 4) yaklaşık, frame asymmetric |

### Adımlar

1. UnityMCP ile sahneyi aç (`manage_scene` open).
2. `find_gameobjects` ile `Lights_Root/Global Light2D` bul → component intensity + color güncelle.
3. `find_gameobjects` ile L1..L5 (anchor isim/yer çevresinde) bul → intensity güncelle.
   - L1 = N3 wall piece (cyan rift arch) child Light2D
   - L2 = WD2 torch_sconce (N2 wall) child Light2D
   - L3 = WD3 torch_sconce (N4 wall) child Light2D
   - L4 = brazier_orange prop (cell 4,4) child Light2D
   - L5 = brazier_cyan_rift prop (cell 5,7) child Light2D
4. Main Camera transform position + Camera.orthographicSize güncelle.
5. Sahneyi save.
6. `read_console` — must be 0 errors.
7. **Yeni screenshot al:** `STAGING/screenshots/IsoShowcaseRoom_S95_brightness_fix.png`
   - Camera position from step 4
   - Orthographic size 3.5
   - 1920×1080 veya 960×540 her ikisi de OK, 960×540 yeterli
8. Rapor yaz: `STAGING/CODEX_DONE_iso_showcase_brightness_fix.md`
   - Önce/sonra parametreler
   - Yeni screenshot path
   - Console durumu
   - 1-2 cümle: "oda artık görünür mü?"

## Forbidden

- Hiçbir GameObject ekleme/silme
- Sprite/material/asset değişikliği yok
- Sadece 8 değer değişimi + 1 yeni screenshot
- Sahne hierarchy'sine dokunma

## Effort

low — ~10 UnityMCP call, 5 dakika.
