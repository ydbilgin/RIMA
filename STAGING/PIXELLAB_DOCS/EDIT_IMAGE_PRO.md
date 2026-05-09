# Edit Image Pro
*Kaynak: https://www.pixellab.ai/docs/tools/edit-image-pro*

## Özet
Mevcut pixel art sprite'ı AI ile değiştirir. Text veya referans image ile.

## İki Yöntem

### Edit with Text
Değişikliği text ile describe et:
- "add a sword"
- "change shirt to red"
- Opsiyonel style referans image

### Edit with Reference
Referans image → o görünümü hedef image'a uygular.

## Workflow
1. Image yükle
2. Text veya reference yöntemini seç
3. Description veya referans image ver
4. Output boyutu ayarla
5. Üret (tek seferde 1 image)

## Maliyet Tablosu

| Output Boyutu | Maliyet |
|---|---|
| 1–256px | 20 gen |
| 257–314px | 25 gen |
| 315–512px | 40 gen |

## Parametreler
- **Method:** Text / Reference
- **Description:** Değişiklik açıklaması (text mode)
- **Reference image:** Style hedefi veya ipucu
- **Output size:** Min 32×32, Tier 1 max 256×256
- **No background:** Transparent
- **Seed:** Reproducibility

## Kısıtlamalar
- Tier 1+ gerekli
- Tek seferde 1 image
- Tier 2+ → daha büyük output boyutu

## RIMA Kullanım Senaryosu
Production Playbook Adım 25/34/43 — Weapon Pass:
Warblade/Ranger/Shadowblade body-only sprite'lara silah eklemek için bu tool kullanılır.
