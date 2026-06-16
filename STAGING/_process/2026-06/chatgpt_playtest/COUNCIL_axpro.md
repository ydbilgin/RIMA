# RIMA COUNCIL VERDICT: PixelLab Production & ChatGPT Package Review
**Advisor:** Gemini 3.1 Pro (Deep Architecture & Visual Judgment Lens)

## 1. VERDICT: Üretim Önceliklendirmesi (Golden-Path Filter)
**[DEMO - ACİL (≤4 Gün)]**
- **Warblade Anim Redo:** Armed-anchor tutarlılığı için ilk öncelik (zaten beklemedeydi).
- **Modüler UI Asset Pack (A):** Sadece "Edit-to-Play" akışında görünen ana ekranlar için (HUD, Reward Selection). 9-slice paneller, butonlar ve ikon çerçeveleri. Neden: Golden-path görsel kalitesini (environment algısı) anında yükseltir.
- **Reward UI Bug Fix:** Kod bazlı demo-kritik fix (RewardPickup.cs). Görsel polish'ten önce çözülmeli.

**[POST-DEMO]**
- **Rift-Forged Egg (B):** Sadece görsel skin ve ekonomi bağımsız. Playtest bug'ları bitmeden gereksiz, paket dokümanı da post-demo diyor.
- **UI/UX Polish Spec (C) Kalanı:** Codex, detaylı Settings, vb. Demo videosunun ana odak noktası değillerse sonraya bırakılmalı.

**[DROP]**
- **Karakter/Düşman Spriteleri (Paketten):** 4-cardinal ve 35° view ile üretilmiş, RIMA canon'u (8-dir, 70-80°) ile tamamen uyumsuz.

## 2. CANON-FLAG TABLOSU (Çatışma Analizi)
| Konu / Madde | Çatışma | Açıklama & Öneri |
| :--- | :---: | :--- |
| **Kamera Açısı (View)** | **EVET** | Paket 35° (izometrik) gösteriyor. Canon 70-80° High Top-Down. *Öneri:* UI flat olduğu için etkilenmez, ancak prop üretilecekse kesinlikle 70-80° promptlanmalı. |
| **Yön (Direction)** | **EVET** | Paket 4-cardinal (S/E/N/W) diyor. Canon 8-dir. *Öneri:* ChatGPT paketindeki karakter konseptlerini drop et. |
| **Renk Paleti (Stil)** | KISMEN | UI ekranları (VP-01..05) slate/cyan/amber dengesinde MÜKEMMEL. Ancak Egg ve VFX'lerde (OV-02) "purple neon overload" var. *Öneri:* VFX ve proplarda moru kısıtla. |
| **UI Mimarisi** | YOK | Atlas ve 9-slice (A) ayrımı son derece başarılı ve scalable. Modülerlik canon'una tam uyumlu. |

## 3. PAKETTEKİ EKSTRA DEĞERLİ UNSURLAR
1. **Durum (State) Tasarımları:** Butonların Idle/Hover/Pressed/Disabled durumlarının net ayrımı (VP-06). UI kodlamasında büyük kolaylık sağlar.
2. **Hiyerarşik Çerçevelendirme:** Rarity strip'leri ve çerçevelerin (overlay) iç içe geçebilir yapısı, PNG israfını önleyecek harika bir "best-practice".

## 4. RİSKLER & SPECS DOĞRULAMASI
- **Risk 1:** "PNG Mezarlığı". UI assetleri bütün ekran (flattened) olarak değil, *kesinlikle* parçalar (9-slice) halinde PixelLab'dan çıkarılmalı.
- **Risk 2:** PixelLab bütçe aşımı. PRO kesinlikle kullanılmamalı, modüler parçalar V3 mode ile üretilip birleştirilmeli.
- **Spec Doğrulaması:** Modüler UI (Create Image Pro / init image -> V3), Transparent: Evet (decal'ler için). Renkler: Slate (#3A3D42), Cyan vurgu (≤%15), Amber (uygun).
