# NLM Conflict Sweep + Resolution (S114 overnight, N1)

**Amaç:** NLM canonical kaynaktan tüm açık tasarım çelişkilerini + saçmalıkları çıkar, Opus 4.8 ön-karar ver, Codex+agy review'dan geçir, final lock. Kayıp olmasın diye MEMORY.md index pointer ata.

**Yöntem:** NLM query (conversation 1fa77821) → Opus ön-analiz (bu doc) → Codex+agy review (profile-fallback) → Opus final (aşağı "FINAL" bölümü review sonrası).

---

## A. Doğrulanan 6 çelişki — hepsi MEVCUT CANON ile çözülü (NLM teyit etti)

NLM 6 alanı taradı; hepsi 2026-05-28 "Çözülen çelişkiler" 7-tablosuyla ZATEN çözülmüş. NLM canon'u doğruladı, **DEĞİŞİKLİK YOK** — sadece regresyon kontrolü:

| # | Konu | CANON (değişmedi) | Eski (superseded) |
|---|---|---|---|
| 1 | Duvar/ambiyans | **Wall-less floating Hades Elysium** — eski 119 duvar = L5 Wall Blocker dekoru (sınır DEĞİL) | wall_production_pipeline_s99 devasa N/E/W duvar |
| 2 | Cliff yerleşim | **2-Stage hibrit** — CliffAutoPlacer (kenar oto) + DecorCliffPainter (manuel dokunuş). Oto İPTAL DEĞİL | CLIFF_MANUAL_BRUSH "oto DEPRECATED" (yanlış) |
| 3 | Silah/anim | **VFX-first + weaponless body, PPU64, HandAnchor mount** | "PEAK frame silahla üret" WEAPON LOCK (iptal) |
| 4 | Kamera | **High Top-Down 3/4 ~70-80°, iso-art / iso-MATH değil, 640×360 PPC** | diamond-iso 45° / pure 85-90° top-down |
| 5 | Asset katman | **6-layer (L1-L6)** | 4/5-layer / 10-layer painter enum |
| 6 | Live tool | **T3 full standalone (kullanıcı kilidi)** | T2 (AI tavsiyesi, reddedildi) |

→ **Aksiyon: yok.** Canon sağlam. (Not: #2'de CURRENT_STATUS'ta "auto cliff DEPRECATED, manuel brush" diyen S110 memory'leri var — NLM 2-stage hibrit diyor. Bu küçük drift Codex+agy'ye sorulacak: cliff oto-placer demo'da AKTİF mi yoksa tamamen manuel mi? Demo-loop'u etkiler.)

---

## B. YENİ tespit — 2 tasarım SAÇMALIĞI (kullanıcı "dizaynda saçmalık varsa tespit et" dedi)

### SAÇMA-1 — Mixel Boss (252px @ PPU32 vs oyuncu PPU64)
- **Kaynak çelişki:** `PIXELLAB_PRODUCTION_GUIDE_v2.md` Adım 22a → Final Boss'u 252×252 tuvalde çiz, Unity'de **PPU=32'ye düşürerek** dev yap. `LAURETH_2D_ILLUSION_LIBRARY.md` "Common Mistakes" → *"Mixels (16/32/64 PPU yan yana) → illüzyon kırılır, tek PPU lock zorunlu."*
- **Neden saçma:** PixelPerfect 640×360 @ assetsPPU **64** kilidi var. PPU32 boss = ekranda 2× büyük pikseller = literal mixel = brand ihlali. İkisi aynı sahnede olamaz.
- **OPUS ÖN-KARAR:** Boss = büyük TUVAL (128-192px), **PPU 64 SABİT**. Eski PPU32 talimatı SUPERSEDED. 128px@PPU64 = 2 birim, 192px@PPU64 = 3 birim — oyuncuya (64px=1 birim) göre 2-3× dev, mixel YOK. (PenitentSovereign zaten "128-192px üret" notu var → tutarlı.) Üretilebilirlik notu: boss create_character size=128 max; 192 için Create Image Pro master sheet → Create Character. PPU import = 64.

### SAÇMA-2 — 1-dir silah rotasyonu vs saldırıda silah gizleme (ÖLÜ EFOR iddiası)
- **Kaynak çelişki:** `weapon_master_spec_10_class` → silah 1-dir çiz, Unity `Transform.rotation` ile döndür (maliyet kısma). Bir diğer doc → 70-80° tepeden 2D kılıcı 360° döndürmek "karton" görünür (foreshortening kaybı) → "düzeltme" = saldırı swing'inde silahı **tamamen gizle**, yerine slash-arc VFX oynat. NLM: madem saldırıda silah yok ediliyor, AnimationCurve rotasyon altyapısı "ölü efor."
- **Çelişki noktası (KRİTİK):** Az önce review+fix ettiğim S114 weapon mount kodu (`OrientationSync.BeginSwing` procedural swing) silahı GİZLEMİYOR, SALLIYOR. NLM "gizle+VFX" diyen eski doc'a dayanıyor. Hangisi canon?
- **OPUS ÖN-KARAR (provisional — silah PAUSED, A5 playtest bekler):** İkisi de uç. **Hibrit:** (a) idle/walk + AĞIR/yavaş saldırı = silah GÖRÜNÜR + procedural swing (arc okunur, mount kodu DEĞERLİ, ölü efor değil); (b) HIZLI flurry = VFX-overlay baskın, silah dim/blur olabilir ama tam gizlenmez. Yani mount kodunun swing'i ağır saldırı için gerçek işlev taşır. NLM'in "ölü efor" iddiası fazla güçlü AMA "her saldırıda full rotasyon = karton" uyarısı geçerli → swing arc'ı KÜÇÜK + juice (hitstop) ile maskele. **Final = weapon-resumption + A5 combat-feel playtest'te kilitlenecek.** Şimdilik mount kodu KALIR (silinmez).

---

## C. Codex + agy'ye sorulan sorular (review hedefi)
1. SAÇMA-1 çözümü doğru mu? Boss PPU64 + büyük tuval mixel'i çözer mi, kaçırdığım bir şey var mı?
2. SAÇMA-2: mount kodunun procedural swing'i KORUNMALI mı (ağır saldırı işlevi) yoksa NLM haklı mı (gizle+VFX, swing sil)? Demo combat-feel açısından hangisi?
3. Cliff oto-placer demo'da AKTİF mi (2-stage hibrit) yoksa tamamen manuel mi? (drift — demo-loop'u etkiler)
4. Gözden kaçan başka çelişki/saçmalık var mı?

## D. FINAL — Opus 4.8 final karar (triple-AI CONVERGE)

**Sonuç: 4/4 soruda Opus + Codex (yasinderyabilgin) + agy (ydbilgin) AGREE.** Fallback doğrulandı (Codex quota-aware→yasinderyabilgin profili, agy priority-chain→ydbilgin).

- **SAÇMA-1 LOCK (Mixel boss):** Boss = 128-192px tuval, **PPU 64 SABİT**. Eski 252px@PPU32 SUPERSEDED.
  - Codex katkı: kaynak çizimde pixel-density / outline / VFX kalınlığı karakter grid'iyle AYNI olmalı (yoksa PPU doğru olsa bile "farklı oyundan" durur).
  - agy katkı: 128-192px sprite için pivot (ayak tabanı) + collider offset 64-PPU koordinat sisteminde MANUEL yapılandırılmalı.
  - Üretilebilirlik: create_character size=128 max; 192 için Create Image Pro master sheet → Create Character; import PPU=64, point filter, no-compress.
- **SAÇMA-2 LOCK (provisional, A5 combat-feel kapatır):** Procedural swing **KORUNUR** (mount kodu silinmez). Hibrit: ağır/yavaş saldırı = silah görünür + okunur arc; hızlı flurry = küçük arc + VFX baskın + hitstop mask.
  - Codex kritik: tehlike swing'in VARLIĞI değil — her saldırıya aynı büyük 360° "karton" rotasyonu uygulamak. Arc saldırı-tipine göre ÖLÇEKLENSİN. (Mount kodu zaten `strikeFraction`/`swingBackswing`/`swingFollowThrough` knob'larına sahip → A5'te tune.)
- **CLIFF LOCK:** **2-stage hibrit** (CliffAutoPlacer kenar-otomatik + DecorCliffPainter manuel dokunuş). S110 "full manuel cliff" drift'i demo üretim hattında GERİ ALINIR. Demo odaları auto-cliff ile rebuild edilir = SİSTEM işi (cliff sprite art deferred). NLM 2-stage doğru, S110 memory'ler stale.
- **YENİ DEMO-BLOCKER'lar (Codex+agy ortak tespit → N2/N3'e besle):**
  1. Cliff'ler kullanıcı tarafından silinmiş → oda rebuild gerek (sistem).
  2. Decor_Cliff Light2D black-cliff: S114-S2 "16/16 hedefliyor" fix iddiası var → DOĞRULA (Codex hâlâ flag'liyor).
  3. Parallax L4 toggle runtime-etkisiz (pre-existing critical) → demo combat'ı bloklamaz, DEFER.
  4. 11 boş AssetPoolSO → statue kategorizasyon tutarsızlığı (FILL).

**Canon teyidi:** 6 NLM çelişkisinin hepsi mevcut canon ile çözülü, regresyon yok. DEĞİŞİKLİK YOK.

---
**Index pointer:** MEMORY.md → `reference_nlm_conflict_resolution_s114` (6 çelişki canon-teyit + 2 saçmalık triple-AI LOCK + 4 demo-blocker).
