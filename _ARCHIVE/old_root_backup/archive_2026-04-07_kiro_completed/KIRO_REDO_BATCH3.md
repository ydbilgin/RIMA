# KIRO — REDO Batch 3
*Tarih: 2026-04-05 | Bu dosyayı oku, sırayla uygula. Başka dosya okuma.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## DURUM — Önce Oku

Bu batch Warblade animasyonlarında tespit edilen **yön hataları**nı düzeltir.

| Görev | Animasyon | Sorun | Öncelik |
|---|---|---|---|
| 1 | Warblade / quick-horizontal-slash / south-west | SE gibi görünüyor, yön ters | **KRİTİK** |
| 2 | Warblade / quick-horizontal-slash / north-west | NE gibi görünüyor, yön ters | **KRİTİK** |
| 3 | Warblade / basic_attack_2 / south-west | SE gibi görünüyor, yön ters | **KRİTİK** |
| 4 | Warblade / basic_attack_2 / north-west | NE gibi görünüyor, yön ters | **KRİTİK** |
| 5 | Warblade / basic_attack_3 / north | Garip görünüyor, overhead slam bozuk | ORTA |

**Karakter ID:** `f3465121-2282-4faa-a955-60b29fd2a628`  
**PixelLab size:** 96px | **view:** low top-down | **mode:** pro

---

## ANIMASYON ADI EŞLEŞTİRME

| RIMA klasör adı | PixelLab `animation_name` |
|---|---|
| `quick-horizontal-slash` | `quick-horizontal-slash` |
| `basic_attack_2` | (Batch 2'de üretildi — ID'yi `get_character` ile bul) |
| `basic_attack_3` | (Batch 2'de üretildi — ID'yi `get_character` ile bul) |

---

## GÖRSEL KURAL — Her Yönde Kontrol Et

**SW (south-west):** Karakter sola-aşağıya bakmalı. Kılıç sol tarafa vuruyor olmalı.  
**NW (north-west):** Karakter sola-yukarıya bakmalı. Kılıç sol tarafa vuruyor olmalı.  
**Eğer üretilen sprite SE veya NE gibi görünüyorsa → yanlış, yeniden üret.**

---

## GÖREV 1 — quick-horizontal-slash / south-west

Mevcut dosyalar `south-west/frame_000.png` ... `frame_003.png` var ama YANLIŞ.

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="quick-horizontal-slash",
  direction="south-west",
  n_frames=8,
  start_frame=0,
  end_frame=None
)
```

Bitince: var olan dosyaların **ÜZERİNE** yaz:
```
Assets/Sprites/Characters/Warblade/animations/quick-horizontal-slash/south-west/frame_000.png
...
Assets/Sprites/Characters/Warblade/animations/quick-horizontal-slash/south-west/frame_007.png
```

---

## GÖREV 2 — quick-horizontal-slash / north-west

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="quick-horizontal-slash",
  direction="north-west",
  n_frames=8,
  start_frame=0,
  end_frame=None
)
```

Bitince üzerine yaz:
```
Assets/Sprites/Characters/Warblade/animations/quick-horizontal-slash/north-west/frame_000.png
...
Assets/Sprites/Characters/Warblade/animations/quick-horizontal-slash/north-west/frame_007.png
```

---

## GÖREV 3 — basic_attack_2 / south-west

Önce animasyon ID'sini bul:
```
mcp__pixellab__get_character(character_id="f3465121-2282-4faa-a955-60b29fd2a628")
```
basic_attack_2 animasyonunu bul → o ID ile:

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="basic_attack_2",
  direction="south-west",
  n_frames=8
)
```

Kaydet → üzerine yaz:
```
Assets/Sprites/Characters/Warblade/animations/basic_attack_2/south-west/frame_000.png
...
```

---

## GÖREV 4 — basic_attack_2 / north-west

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="basic_attack_2",
  direction="north-west",
  n_frames=8
)
```

Kaydet → üzerine yaz:
```
Assets/Sprites/Characters/Warblade/animations/basic_attack_2/north-west/frame_000.png
...
```

---

## GÖREV 5 — basic_attack_3 / north (opsiyonel, önce 1-4 bitir)

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="basic_attack_3",
  direction="north",
  n_frames=8
)
```

Kaydet → üzerine yaz:
```
Assets/Sprites/Characters/Warblade/animations/basic_attack_3/north/frame_000.png
...
```

---

## TAMAMLAMA

Her görev bitince `_STAGING/DONE.txt`'ye ekle:
```
[DONE-REDO3] Warblade/quick-horizontal-slash/south-west | tarih
[DONE-REDO3] Warblade/quick-horizontal-slash/north-west | tarih
[DONE-REDO3] Warblade/basic_attack_2/south-west | tarih
[DONE-REDO3] Warblade/basic_attack_2/north-west | tarih
[DONE-REDO3] Warblade/basic_attack_3/north | tarih
```

Dosyalar Unity'e yerleştirildikten sonra Claude Code **AnimBuilder** ile clip'leri otomatik yeniden oluşturacak.

---

## NOT — HalfThrall

Sahnede HalfThrall adlı bir mob var ama bu düşman için sprite üretilmedi.  
Şu an VoidThrall sprite'larıyla çalışıyor (placeholder).  
Bu bir sonraki sprint'te karar verilecek: ya VoidThrall'un klon versiyonu olarak kalır, ya da kaldırılır.  
**Bu batch'te HalfThrall için üretim YOK.**

---

## KIRO BİTİNCE — Claude Code Ne Yapacak

Kullanıcı "kiro bunu yaptı" veya "redo batch 3 bitti" dediğinde Claude Code şunları yapacak:

1. **Sprite refresh** — `RIMA/2 → Refresh Sprites` menüsünü çalıştır (veya AssetDatabase.Refresh)
2. **Clip yeniden oluştur** — `RIMA/3 → Build Warblade` ile animation clip'leri yenile
3. **Warblade controller doğrula** — SW/NW walk + attack clip'leri doğru atanmış mı kontrol et
4. **SpriteRenderer renk sıfırla** — Sahnedeki tüm mob SR'larını tekrar beyaza çek (domain reload'dan dolayı gerekebilir)
5. **Play mode screenshot** al — sonucu görsel olarak doğrula
6. **CURRENT_STATUS güncelle** — BUG-4 ve BUG-5 kapatıldı olarak işaretle

### Mob Ekleme Kuralı (Her Zaman Geçerli)
Sahnede yeni bir mob GameObject oluşturulduğunda veya prefab instantiate edildiğinde:
- SpriteRenderer.color = Color.white (1,1,1,1) olmalı
- Bunu Claude Code otomatik kontrol eder ve sıfırlar
- Kullanıcı elle müdahale etmez
