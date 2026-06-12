# 03 — RIMA Tasarım Canon (NotebookLM)

## Magic Attack AYRI stat (canon)
2 ayrı ana hasar statı: **Physical Damage (Phys)** + **Ability Power (AP)**.
- Item bileşenleri: Iron Shard = +%6 Phys · Void Fragment = +%6 AP.
- **Class → hasar tipi:**
  - **Physical:** Warblade, Shadowblade, Ronin, Gunslinger, Ravager, Brawler
  - **Ability Power:** Elementalist, Summoner, Hexer
  - (Ranger = physical/ranged)

## Damage scaling formülü (canon — 3 kategori + cap)
1. **Identity** (class pasif/keystone)
2. **Build** (item/aktif modifier) — aynı kategoride en yüksek tam, 2.'si %50 katkı
   - **Cap:** (Identity × Build) tabanı **max ×3.0**. Aşan hasar boşa gitmez → **Posture (denge çubuğu)** hasarına döner (sersemletme).
3. **Situational** (weak-point / break-window) — kendi içinde toplamsal, max ×2.0, cap'ten SONRA final'e çarpılır.

## Arketipler (savaş ritmi + dash-cancel penceresi)
| Class | Kimlik | Dash-cancel penceresi |
|---|---|---|
| Warblade | ağır infazcı (engage/break/execute), hit-stop yüksek | %60-75 (geniş=ağır hissi) |
| Ravager | berserker (suffer/trade/frenzy), hasar alıp Fury doldurur, az can | geniş |
| Shadowblade | en hızlı melee, çift hançer reverse-grip, vur-kaç | %15-25 (en kısa=keskin) |
| Brawler | çıplak el, ritim/kombo, en hızlı body-combat | %60-75 |
| Gunslinger | run-and-gun kinetik ranged | — |
| Ranger | taktiksel mesafe + tuzak | — |
| Ronin | iaido (wait/draw/punish), spam değil | — |
| Elementalist | element-switch fluid cast, wind-up iptal EDİLEMEZ | — |
| Hexer | lanet biriktir/yay (stack/spread/blast) | yavaş birikim |
| Summoner | minion feda, orkestra şefi | — |

## HP / hız felsefesi (KRİTİK)
- Canon'da **sabit per-class HP/speed sayı tablosu KİLİTLİ DEĞİL.**
- **UI temsili:** karakter seçimde **5-bar:** Hasar · Dayanıklılık · Hız · Kontrol · Zorluk.
- **Ağırlık/hızın gerçek kaynağı:** rakamsal moveSpeed DEĞİL → **animasyon frame data + dash-cancel pencereleri + hit-stop + recovery kareleri.**
  - Warblade/Ravager ağır = uzun wind-up + uzun hit-stop + geniş iptal penceresi.
  - Shadowblade hafif = kısa recovery = keskin/hızlı.

## Demo için çıkarım
Animasyonlar henüz yok → sayısal stat modeli = **placeholder altyapı** (kabul edilebilir, "animasyon sonra" çerçevesi). Ama: **Phys/AP split korunmalı** (canon), HP/speed sayıları geçici (final ağırlık frame'den gelecek), 5-bar UI temsili canon. Stat tool bu placeholder'ları tweak eder.
