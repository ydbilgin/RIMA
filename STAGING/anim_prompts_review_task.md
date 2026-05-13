# Codex Task — Animation Prompts Quality Review

## Görev
STAGING/pixellab_animation_prompts_10char.md dosyasını oku ve her promptu aşağıdaki kurallara karşı review et. Sonuçları STAGING/anim_prompts_review_result.md dosyasına yaz.

## PixelLab Custom Animation V3 Hard Constraints (görsel incelemeden)

1. **Frame range: 4–16 frames (kesin limit)** — her animasyonun frame sayısı bu aralıkta olmalı
2. **"Keep first frame (idle pose)" checkbox** mevcut — loop animasyonlarda başlangıç pozu korunabilir
3. **Action Description** — "walking forward, swinging a sword, casting a spell" formatında net ve spesifik olmalı
4. **Direction**: South/East/North/West × 4 yön üretilir (W=flipX)
5. **Cost**: 3 gen/direction — her animasyon 4 yön × 3 = 12 gen

## Karakter Spec
- **Sprite**: 64×64 chibi, dark gritty tone
- **Camera**: High top-down ~30-35° overhead (Hades match)
- **Weapon-in-hand kuralı**: Tüm karakterler silahlarıyla üretildi
- **8 directions** mevcut, biz 4 yön kullanıyoruz (N/S/E + W=flip)

## Review Kriterleri (Her Karakter İçin)

### 1. Frame Count Check
Her animasyonun belirtilen frame sayısı 4–16 arasında mı? Dışarıda olanı işaretle.

### 2. Action Description Kalitesi
- Net ve spesifik mi? (belirsiz "moves" gibi ifadeler yok)
- Silah/ekipman dahil mi? (Warblade: greatsword, Ronin: katana, vb.)
- Vücut hareketi + başlangıç + bitiş pozu var mı?
- Chibi/top-down bağlamına uygun mu?

### 3. Eksik Animasyon
Her karakter için beklenen animasyon seti:
- run ✓
- attack_basic ✓
- attack_heavy VEYA skill_cast ✓
Bunlardan eksik olan var mı?

### 4. Karakter Tutarlılığı
Prompt açıklaması karakterin sınıf kimliğiyle uyuşuyor mu?
- Warblade: greatsword, two-handed, heavy
- Elementalist: rune disc, ember, no staff
- Shadowblade: twin daggers reverse grip, hooded
- Ranger: bow + quiver
- Ravager: dual short compact axes, bare shoulders
- Ronin: katana + saya, iaido discipline
- Gunslinger: dual pistols, trench coat
- Brawler: gauntlets, arcane flash, boxing guard
- Summoner: soul lantern, cyan wisp
- Hexer: green-flame skull staff, grimoire, hunched/limping

### 5. Öneri
Her prompt için varsa iyileştirme önerisi (max 1-2 satır, kısa).

## Çıktı Formatı

STAGING/anim_prompts_review_result.md dosyasına şu formatta yaz:

```
# Anim Prompts Review — 2026-05-13

## Özet
- PASS: X/30 prompt
- WARN: X prompt (minor)
- FAIL: X prompt (frame out-of-range veya ciddi tutarsızlık)

## Karakter Bazlı Review

### 01 — Warblade
**run** [6f] ✓ / ⚠ / ✗ — kısa not
**attack_basic** [5f] ✓ / ⚠ / ✗ — kısa not
**attack_heavy** [6f] ✓ / ⚠ / ✗ — kısa not

[10 karakter için tekrarla]

## Genel Bulgular
[varsa genel notlar]
```

## Dosyalar

Okuyacağın tek dosya:
- `STAGING/pixellab_animation_prompts_10char.md`

Yazacağın dosya:
- `STAGING/anim_prompts_review_result.md`

Execute every step: read the file, analyze all 30 prompts, write the result file.
