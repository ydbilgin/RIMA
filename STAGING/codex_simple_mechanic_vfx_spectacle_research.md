# Codex Research — Basit Mekanik + Mantık Çerçevesi + VFX Şölen Oyun Kütüphanesi

**Tarih:** 2026-05-15 gece
**Profil:** yasinderyabilgin (smoke test PASS sonrası)
**Effort:** high
**Dispatch:** background

## Bağlam (kullanıcı yatmadan önceki son mesaj)

Solo dev (AI pipeline: Codex + Claude + PixelLab + Aseprite). RIMA projesinde identity krizi sonrası fikir araştırması yapıyor. Önceki sentez (`oyun_fikirleri/.../17_FINAL_SENTEZ_LOGIC_BUILD_ARPG.md`) "tek mantık dili derin" yaklaşımını önerdi (Circuit Breaker = sadece elektrik, Rayline = sadece geometri).

**Kullanıcı bunu REDDETTİ.** Yeni vizyon:

> "Sadece o oyuna odaklı kalmıyorum, basit mekanikleri çözebilecek mantık çerçevesinde bağlayabilecek ama neredeyse herkesin yapabileceği görsel şölen oluşturabilecek mekaniklerle oyunlar istiyorum."

**Filtre setim (HARD):**

1. **Combat fight var** (auto-attack DEĞİL, manuel aktif).
2. **32px karakter / minimal asset / uzak kamera** — görsel sade.
3. **Birden fazla basit mekanik aynı oyunda** — tek tema/dil değil, multi-system combo (Magicka pattern, Noita pattern, Risk of Rain 2 item synergy pattern).
4. **VFX-rich / "görsel şölen"** — particle storm, ekran sallama, hit pause, chromatic aberration, glow, trail, flash — ASSET değil KOD ile zenginlik.
5. **Solo dev fizibil** — "neredeyse herkes yapabilir" çerçevesinde (12-16 hf MVP).
6. **Mantık çerçevesi** — mekanikler rastgele patlama değil, BASIT ama AKILLI bir kural seti ile bağlanır (combo öğrenme, system synergy keşfi).
7. **YASAK:** Vampire Survivors / Hades / Noita klonu. Otomatik oynama. Karmaşık asset/animasyon.

## Senden İstediklerim

### 1. Pattern Haritası (~15 oyun)

2018-2025 arasında bu profilde **kanıtlanmış 10K+ satan emsalleri** çıkar. Her biri için:
- İsim, yıl, geliştirici
- Açıklanan satış (resmi kaynak, tahmin değil)
- "Basit mekanik" envanteri (kaç ana mekanik var)
- Combo/synergy sistemi nasıl çalışıyor (1 paragraf)
- VFX juice teknikleri (hit feel, screen shake, particle vs)
- Asset minimallik seviyesi (1-10 sade)
- Solo dev mi, küçük takım mı (max 3 kişi)

Adaylar (genişlet): Magicraft, Magicka 1/2, Noita, Brotato, Death Must Die, Halls of Torment, Cult of the Lamb (combat parçası), Crab Champions, Wizard of Legend, Realm of the Mad God, Furi, Children of the Sun, Risk of Rain 2, Hyper Light Drifter, ULTRAKILL, One Step From Eden, 20 Minutes Till Dawn, Backpack Battles, Spelldrifter, Tunche, Skul, Bonfire Peaks.

### 2. "Multi-System Simple Mechanic + VFX Spectacle" Tasarım Patternleri

Pattern listesinden 8-12 çekirdek mekanik tipini çıkar. Örnek:
- Element kombosu (Magicka — 2-5 element birleşimi)
- Modifier zinciri (Noita — wand modifier stack)
- Item proc synergy (Risk of Rain 2 — itemler birbirini tetikler)
- Skill ağacı multi-class (Death Must Die)
- Form/shape combo (Crab Champions — silah formları)
- Status effect cascade (yan etki zinciri — burn → freeze break → shatter)
- Damage type / resistance interplay
- Resource sharing / drain mechanic
- Spatial trigger (mermi/saldırı arenada iz bırakır, başka mekaniği tetikler)
- Time delay / fuse / countdown
- Position-based (yakın/uzak/yan dönmüş)
- Combo counter / momentum (hit chain çarpan)

Her pattern için: hangi 3 oyunda en iyi uygulanmış, solo dev maliyeti (1-10), kombinasyon zenginliği (1-10).

### 3. 10 Yeni Oyun Konsepti (Multi-Mechanic + 32px + VFX Şölen)

Yukarıdaki pattern'leri **2-4 patternlik kombolar** halinde 10 yeni oyun konseptine dökü. Her biri:

- **İsim** (capsule-friendly)
- **Hook** (1 cümle, 5 sn'de okunur)
- **Çekirdek 3-5 basit mekanik** (her biri 1 cümle)
- **Combo / synergy kuralı** (mekanikler nasıl birleşir, basit ama akıllı)
- **Combat akışı** (control scheme + tempo)
- **VFX şölen kancası** (ne ekranda patlar)
- **MVP süre** (hf)
- **AI pipeline uygunluk** (1-10, Codex algoritmik/state machine alanı vurgu)
- **En yakın 2 emsal + farkı**
- **Yasak/yiyiklilik kontrolü** (TCK 228 / klon riski)

**Yasak:**
- Aynı pattern kombosu 2 kez kullanma (her konsept farklı 3-5 mekanik karışımı)
- "Tek dilde derin" (CB sadece elektrik) konsept — bu vizyon REDDEDİLDİ
- Karmaşık asset gerektiren konsept (yumuşak anim, voice, portrait)
- Karmaşık balance gerektiren PvP konsept (solo dev için ölüm)

### 4. VFX "Juice" Tekniği Detayı (Solo Dev Tarifi)

15-20 spesifik teknik liste. Her biri için:
- Ne yapar (1 cümle)
- Hangi oyunda en iyi (referans)
- Unity'de uygulama (1 cümle: shader / particle / Cinemachine / tween)
- Maliyet (kod satırı tahmini)

Örnek başlangıç: hit pause, screen shake, chromatic aberration, hit flash, trail renderer, sprite squash/stretch, particle storm, slow-mo finishing, color palette swap, damage numbers, camera punch zoom, freeze frame on parry, bloom intensity spike, vignette flash, time dilation on dodge, kill confetti, screen tearing on bossfight, recoil shake.

### 5. Sentez Önerisi

10 konseptten **en güçlü 3**'ünü öner. Her biri için:
- Niye top 3 (USP + solo dev fizibilite + pazar emsal)
- Pazar emsali açıklanan satış (1 cümle)
- 12-16 hf MVP risk değerlendirmesi
- Klip dakikası anı (viral kanca)

## Format

Türkçe, terse, tablo bol, emoji yok. Markdown. ~600-1000 satır.

Spekülasyonu "tahmin" işaretle. Kesin satış verisinde resmi kaynak link ver.

## Output

`CODEX_DONE_yasinderyabilgin.md` dosyasına yaz (mevcut içerik üzerine değil, append). Sonuç: `STAGING/codex_simple_mechanic_vfx_research_result.md` dosyası yarat ve içeriği oraya yaz.
