# RIMA — Stratejik Sentez + Temiz İş Listesi (S114 S6, 2026-05-29)

> **Yazar:** Opus (karar). **Kaynaklar:** NLM canon (notebook 30ddffa5) + agy endüstri/oyuncu araştırması + kod haritası (Explore: skill/UI/room) + CURRENT_STATUS + memory.
> **Durum:** Codex + agy review BEKLİYOR (yapmadan-önce-kararlaştır modeli). Review sonrası bu doc canon olur.

---

## 1. OYUN NEREDE — Vizyon vs Demo

**Tam vizyon (canon, NLM):** "Hades oda yapısı × MMORPG dual-class build-crafting × Slay the Spire skill-acquisition." Run ortasında oyun **ikiye kırılır** (Act 1 boss → secondary class), 10 sınıf, cross-class pasifler (56 kombo), 3 act, 55-70 dk run, meta-hub (Ferryman/Vrel/Sister Mourne/Cartographer). Lore: **The Fracturing** (kasıtlı tercih), **Rift March**, oyuncu = kararın hem faili hem son kalıntısı. Ton: **Vivid Vulnerability** (kırık dünyada cyan #00FFCC umut kıvılcımı). Boss: **Penitent Sovereign** (zincirli, öz-ceza, 3 faz).

**Demo (Faz 1) — bu sürümün scope'u:** TEK sınıf (Warblade), Act 1, ~5 oda, skill draft (Common/Rare), 1 boss, map-fragment gate. ~10 dk oynanabilir loop → **Steam demo / wishlist**.

**Çekirdek gerilim:** Vizyon devasa, demo küçük olmalı. **Tüm S6 kararları "demo'yu en kısa yoldan wishlist-çeken bir dikey-dilime götür" pusulasına göre.** Vizyon ertelenir, silinmez.

---

## 2. ÇELİŞKİLER + ÇÖZÜM (Opus kararı — "çelişkileri atalım")

| # | Çelişki | CANONICAL (kalan) | ESKİ (atılan/banner) |
|---|---|---|---|
| C1 | **Scope:** demo vs tam vizyon | Faz 1 = 1 sınıf, Act 1, 5 oda, draft, 1 boss | cross-class/10 sınıf/3 act = **DEFER (post-demo)** |
| C2 | Karakter canvas | **120px canvas / 64px içerik / PPU 64** | 128×128, 252→128, boss 256 (boss demo=128-192) |
| C3 | Yön sayısı | **8 yön** (5 bake + 3 mirror flipX) | "4 yön MVP" (eski) |
| C4 | Kamera | **Kamera 70-80° + sprite 35° çizim** (uzlaştı), PixelPerfect 640×360 | "35° iso math" / diamond terminoloji |
| C5 | Parallax | **0.05–1.10, 6 katman, Y=0.5×X** | 0.03–0.14 |
| C6 | Live tool T3 | **İç geliştirme aracı** (dev-only) | "Demo player feature" = DEĞİL, defer |
| C7 | İki skill sistemi (kod) | **SkillDatabase + DraftManager + SkillOfferUI** canonical | `SkillDraftSystem` + `ActiveSkillData` stub = SİL/ignore |
| C8 | Ekonomi/para | **KARAR: minimal "Rift Shards"** ekle (reroll + heal) | (canon'da kilitli değildi → boşluk dolduruldu) |
| C9 | Oda akış sırası | **Clear → skill draft (3 kart) → seç → map-fragment/gate → next** | kod şu an bozuk (draft çağrılmıyor, ikon placeholder) |
| C10 | Boss HP | demo boss HP tutarlı set (canon 3-faz, demo = 1-2 faz graybox) | "100 vs 800" çelişkisi → tek değer |

**Kod-seviyesi düzeltilecek çelişkiler:** C7 (stub sil), C9 (vertical-slice wiring), 2× CameraFollow + 2× PlayerController duplicate merge, legacy `_IsoGame` test triage.

---

## 3. ENDÜSTRİ + OYUNCU (agy + Opus) — neyin ilgi çektiği

- **Multiplicative synergy** (Balatro/Brotato): oyuncular lineer +%10 değil, **çarpan sinerji** sever → draft'ta "tag" etkileşimleri.
- **"Bir run daha" hook'u** = kırık build yakalama arzusu (Hades duo-boon), risk-reward odaları (Dead Cells cursed), mikro meta-progression (run sonrası kalıcı +1).
- **Game-feel ilk 60 sn:** hitstop + slash-arc VFX + shake = wishlist'in %50'si. Ses (thwack/shatter) game-feel'i ikiye katlar.
- **Death/Victory ekranı** = wishlist CTA + sonraki sınıf teaser silüeti.
- **Narrative = ucuz atmosfer:** Hades run-içi monolog (cyan kutu + daktilo), boss intro, environmental rune-lore.

---

## 4. NE EKLENMELİ / NE ÇIKARILMALI

**EKLE (yüksek-ROI, demo-ölçekli):**
1. **Skill draft'ı çalışır kıl** (ikon + DraftManager + her oda sonrası → seç → gate) — *temel, olmazsa demo yok.*
2. **Audio skeleton** (hit/shatter/UI SFX placeholder) — game-feel ROI #1.
3. **İlk-60-sn juice** + combat-feel freeze (A5).
4. **Skill tag sinerjisi** (Heavy/Rift/Bleed/Swift) + 2-3 "duo" sinerji skill (Hades-boon hissi).
5. **Rift Shards ekonomisi** (pickup → reroll draft + heal).
6. **Corrupted elite** varyant (mevcut mob recolor + cyan aura + 1 saldırı tweak) — ucuz çeşitlilik.
7. **Death/Victory ekranı** (run-stat + Wishlist CTA + sonraki-sınıf teaser).
8. **Ucuz narrative** (run-içi diyalog + Penitent Sovereign intro + 3 rune-lore parçası).

**ÇIKAR / ERTELE (overscope):** cross-class · secondary class · sınıf 2-10 · act 2-3 · meta-hub NPC'ler · T3 live tool'u "demo feature" yapmak · derin diyalog ağacı · base-building.

---

## 5. HİKAYE ENTEGRASYONU (lore → mekanik, ucuz)

- **Run-içi monolog:** Warblade odaya girince cyan-çerçeve daktilo kutusu ("Sovereign'ın nefesi burada daha soğuk..."). Seslendirme yok.
- **Boss intro:** Penitent Sovereign cyan yarıktan yükselir, zemin çatlar, gotik font "The Rift Keeper". Lore = öz-ceza/zincir.
- **Environmental rune-lore:** 3 etkileşimli cyan rün → 1-2 cümle "Geçmişin Yankısı" (The Fracturing/corruption). 
- **Mekanik↔lore:** ölüm = ceza değil "mühür zorunluluğu"; map-fragment = "parçalanmış dünyanın görsel anlatısı" (zaten canon).

---

## 6. ★ SON TEMİZ İŞ LİSTESİ (öncelik sırası, demo-first)

**Routing:** her item = **Opus karar/yazar (mekanik=Sonnet) → Codex+agy review**. Gated = kullanıcı.

### TRACK 0 — Dikey dilim (demo'nun çekirdeği, ÖNCE)
- **T0.1 Skill draft fonksiyonel** — ikon registry + DraftManager ensure + oda-clear→3 kart draft→seç→gate. (C9) [Opus]
- **T0.2 Audio skeleton** — AudioManager + combat/UI SFX placeholder (thwack/shatter/select). [Sonnet]
- **T0.3 İlk-60-sn juice pass** + **A5 combat-feel playtest (GATED)**.

### TRACK 1 — Roguelite derinlik (demo)
- **T1.1 Skill tag sinerji** (4 tag draft'ta görünür) + 2-3 duo-synergy skill.
- **T1.2 Rift Shards ekonomi** (pickup + reroll + heal, reward odası).
- **T1.3 Corrupted elite** (2 mob recolor+aura+tweak).
- **T1.4 Death/Victory ekranı** + Wishlist CTA + sonraki-sınıf teaser.

### TRACK 2 — Narrative (ucuz)
- **T2.1 Run-içi diyalog + boss intro + 3 rune-lore.**

### TRACK 3 — Görsel/dünya (çoğu GATED — PixelLab)
- **T3.1 Cliff/depth final pass + backdrop** (PixelLab gen GATED).
- **T3.2 Boss sprite (PenitentSovereign)** (PixelLab gen GATED).

### TRACK 4 — Temizlik / çelişki (paralel, otonom-güvenli)
- **T4.1** SkillDraftSystem stub sil (C7) + 2× CameraFollow/PlayerController merge + legacy `_IsoGame` test triage.
- **T4.2** /lint + MEMORY.md slim (29.6KB→<24KB) + CURRENT_STATUS sadeleştir.

### GATED (kullanıcı, akışı bekletir)
A5 combat-feel · PixelLab gen (cliff/backdrop/boss/weapon) · git-push (remote divergence).

### DEFER (post-demo, vizyon)
cross-class · sınıf 2-10 · act 2-3 · meta-hub · T3 tool feature.

---

## 7. ÖNERİLEN İLK HAMLE (orijinal — Bölüm 8 ile güncellendi)
**T0.1 (skill draft fonksiyonel)** = en yüksek kaldıraç.

---

## 8. ★ REVIEW KONSOLİDASYONU + FINAL TEMİZ LİSTE (Codex + agy, 2026-05-29)

**Sürpriz bulgu (Codex grep):** Durdurulmaya çalışılan ilk cx işi BİTMİŞ → T0.1'in çoğu zaten ağaçta (UNCOMMITTED): `Assets/Resources/SkillIconRegistry.asset` baked (ironcharge/riftstrike entry'leri), `SkillDatabase.Add()` ikon atıyor (`SkillDatabase.cs:327`), `RoomLoader.UnlockGateAfterDraft()` (`RoomLoader.cs:293`), `DraftManager.DraftDrivenByRoomLoader` flag (`DraftManager.cs:105`). → **Önce compile+scene-verify, sonra tamamla.**

**agy düzeltmeleri (design):** (1) Rift Shards ekonomi = overscope → **Rift Altar** (HP↔power, tek UI). (2) Tek sınıf retention zayıf → **2 silah/stance** (zero-sprite, ömür 3×). (3) **Death/Victory + Wishlist CTA → Track 0** (+ steam://openurl overlay + Share-Build seed; roguelite demo→wishlist %16-24, wishlist'in %70'i streamer).

**Codex bulguları (code):** (1) **#1 BLOCKER: `SkillDatabase` playable scene'de YOK** — sadece recovery/archive'da; `EnsureDependencies` onu auto-create ETMİYOR (sadece UI+Generator) → generator serialized `allSkills` fallback'e düşer (scene'de wired değil). (2) **Çok fazla draft-driver** (RoomLoader/RuntimeRoomManager/RewardPickup/MapFragmentBridge/HandleRoomCleared) → tek canonical'a indir. (3) **Tag synergy hidden-expensive** (enum'da Heavy/Rift/Bleed/Swift yok, Add() tag atamıyor, effect-hook gerek) → display-only yap, gerçek synergy DEFER. (4) `EliteAffix` speed/Shielded multiplier'ları runtime'da OKUNMUYOR + Shielded integer-bug → elite "ucuz" sadece 1 affix doğru wire edilirse. (5) `ActiveSkillData` silinirse `SkillSlot` da migrate edilmeli. (6) 2× CameraFollow = farklı namespace (compile-dup DEĞİL, seçim-debt). (7) `_IsoGame` test triage ELEVATE (verification güvenilirliği).

### FINAL LİSTE (bu CANON — Bölüm 6 supersede)

**TRACK 0 — Çekirdek dikey dilim + CTA**
- `T0.0` **Bootstrap dep pass** (Codex #1): SkillDatabase'i playable scene'e koy VEYA deterministik auto-create + cx'in uncommitted T0.1 kodunu **compile+verify**.
- `T0.1` **Tek-driver skill draft**: rakip entry-point'leri tek canonical driver'a indir; clear→draft→seç→gate; AssignActive için skill component'leri player'da garanti; play-verify.
- `T0.2` **Audio skeleton** (combat/UI SFX placeholder).
- `T0.3` **Death/Victory + Wishlist CTA** (PROMOTED): steam://openurl + Share-Build seed + run-stat + next-class teaser.
- `T0.4` İlk-60sn juice + **A5 combat-feel playtest** 🔒gated.

**TRACK 1 — Retention + derinlik**
- `T1.1` **2 silah/stance** (run başı: Sovereign's Will / Rift Carver — attack variant + VFX renk + start passive, zero-sprite).
- `T1.2` **Tag display + combo pop-up** ("Rift Burst x5!" screenshot value). *Gerçek multiplicative synergy = DEFER (hidden-expensive).*
- `T1.3` **Rift Altar** (ekonomi yerine: HP↔power reroll/heal, currency YOK).
- `T1.4` **Corrupted elite** (1 affix DOĞRU wire — EliteAffix multiplier bug'larını düzelt).

**TRACK 2 — Narrative (ucuz):** `T2.1` run-içi diyalog (boss + 3. oda fısıltı) + boss intro + 3 rune-lore.
**TRACK 3 — Görsel (🔒gated PixelLab):** `T3.1` cliff/depth+backdrop · `T3.2` boss sprite.
**TRACK 4 — Temizlik:** `T4.1` SkillDraftSystem stub sil (ActiveSkillData→SkillSlot bağı dikkat) · `_IsoGame` test triage (ELEVATED) · CameraFollow seçim-debt (düşük) · /lint + MEMORY.md slim.

🔒**Gated:** A5 · PixelLab gen · git-push · weapon batch.
⏸**Defer:** cross-class · sınıf 2-10 · act 2-3 · meta-hub · gerçek tag-synergy mechanics · T3 tool-as-feature.

**Sıra:** T0.0 → T0.1 → (T0.2 ∥ T0.3) → T0.4🔒 → T1.x. Codex: T1.2 gerçek-synergy'yi T0.1+T1.3 sonrası, sadece display ise erken OK.
