# COUNCIL BRIEF — Code-Only Animation Stratejisi: Knockback ✓, Knockdown?, Sınır Nerede? (2026-06-05)

## Amaç
Kullanıcı: "Karakterlere/moblara HİÇ animasyon üretmesek olur mu? Knockback tamam — knock DOWN'ı da
(yere serilme) sağlayabilir miyiz? Ayarlanabilir olsun. Hepsini council'le düşün."
Karar: kod-only hareket TIER LİSTESİ + knockdown reçetesi + tunable mimari + üretim-gereken istisnalar.

## RIMA mevcut durum (kıyas tabanı — advisorlar kod doğrulayabilir)
- Karakterler: 8-yön idle+walk ÜRETİLMİŞ (PixelLab, 64px görünür/120px canvas). Başka anim YOK ve üretim GATED.
- **Mimari zaten kod-anim bahsinde:** silah = AYRI el sprite'ı, 8-yön KODLA savruluyor + swing-fade
  (`project-weapon-hand-separate-lock`) · slash = `SlashArcVFX` kod çizimi · 13 mobda Knock+HitFlash KODLA
  çalışıyor · JuiceManager (screen-shake vb.) var · mob ölümü "kod squash/fade" olarak planlı (A-polish).
- Kamera: HIGH top-down 3/4 (~70-80°), Hades/CoM/D3 ref. PPU64, Pixel Perfect.
- Moblar şu an çoğunlukla placeholder kare; gerçek mob sprite'ları gelecek (üretim kullanıcıyla).

## SORULAR
### K1 — Kod-only hareket TIER tablosu (ana iş)
Şu hareketlerin her biri için: kod-only YAPILABİLİR Mİ, teknik reçete (1-3 satır), bunu yapan pixel-art
örnek oyun: knockback (✓ baz) · **KNOCKDOWN (yere serilme + yatma + kalkma)** · stagger/sendeleme · ölüm ·
spawn/doğma · dash (ghost-trail) · melee saldırı (weapon-hand+arc zaten lock) · cast/büyü · zıplama/lunge ·
uyanma/uyuma (elite intro?). Knockdown DETAYLI: parabol arc + sprite rotasyonu (yatay) + 1-2 sekme + ayrı
gölge elipsi (havada gölge yerde kalır) + toz puff + get-up (geri rotasyon + mini hop + i-frame). Top-down
3/4'te yatan sprite'ın perspektif sorunu ve çözümleri (rotasyon mu, tek "yatık" frame'i runtime'da mı
üretmek — örn. 90° döndürülmüş sprite'ın okunabilirliği).
### K2 — Üretim KAÇINILMAZ olan ne kalıyor?
idle/walk zaten var. Boss-stagger gibi büyük okunabilirlik anları? Cast-pose tek-frame? Liste + gerekçe —
"asla üretme" değil "ne zaman değer" çizgisi.
### K3 — Tunable mimari (ayarlanabilirlik — kullanıcı şartı)
Mevcut Knock/HitFlash/JuiceManager üstüne minimal sistem: per-mob/per-skill parametreler (knockback force,
knockdown threshold/poise, süreler, bounce sayısı). ScriptableObject "JuiceProfile" mi, mob tanımına alanlar
mı? Knockdown MEKANİK tasarımı: hangi vuruş knockdown atar (ağır skill / poise-kırılınca / şarjlı)?
Yerdeyken hasar? Get-up i-frame? Over-engineering'den kaçın — demo ölçeği.
### K4 — Riskler + mitigasyon
64px'te okunabilirlik · rotasyonun pixel-grid kırması (step-rotation 15°? outline? RotSprite?) · çok mob
aynı anda yerdeyken görsel kaos · "her şey kod" hissinin ucuzlaşma riski (nerede durmalı).

## Çıktı formatı
K1 tier tablosu (hareket → kod-only? → reçete → örnek oyun) · K2 kısa liste · K3 net mimari önerisi ·
K4 risk/mitigasyon · TL;DR pozisyon.
