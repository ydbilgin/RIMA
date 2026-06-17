# CURRENT_STATUS

## ⏯️ RESUME (2026-06-17 → YENİ SESSION) — demo ~19 Haziran

**Durum:** Combat = **GO (conditional)**, tüm polish bitti (juice/outline/telegraph/dressing/music/deterministik-oda), hepsi pushed (`8fabfe87`). Otonom kısım bitti. **Sıra SENDE: test → 🚩 ver → recapture → koreografi.**

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · **cx TÜM profil quota-BLOCKED** → execute=**Claude Opus subagent (builder-opus)** + ax_opus (kullanıcı tüm kanal/kota iznini verdi) · **TEK Unity-ajan (seri)** · PixelLab **balance=0** (yeni gen yok) · **builder kuralı: `git checkout/reset` YASAK** (HEAD-sonrası uncommitted işi siler).

---

## 📋 KULLANICI TODO (sırayla — bana tek tek "şunu yap" de)

### 1. 🔴 Demo'yu elle test et (GO gate — EN ÖNEMLİ)
Play'i **MainMenu'den** başlat. Bu session DEĞİŞENLER — özellikle doğrula:
- [ ] Düşmanlar sana **geliyor** mu (idle değil)? · **Ölmeden** wave temizleniyor mu? (Penitent opening'de yok)
- [ ] Düşmanlar **net görünüyor** mu (yeni outline + ışık)? · Boss ada **içinde** + sana saldırıyor mu?
- [ ] Boss telegraph: kırmızı daire + **yeşil safe-ring**; telegraph **bitince** mi hasar? · **Müzik** çalıyor mu?
- [ ] **Seam:** F2/Director aç-kapa → combat devam? Oda geçişi temiz? · **Edit-to-Play:** F2 prop koy→çık→**aynı oda** mı?
- [ ] **3 KEZ baştan** (cold) oyna → console temiz mi?
- Detaylı checklist: `STAGING/DEMO_MANUAL_TEST_CHECKLIST_2026-06-17.md`

### 2. 🚩 Bulduklarını bana SÖYLE → düzeltirim
(Combat'a güvenmeden recapture'a geçme — kanıt gerçek-state olmalı.)

### 3. "recapture başlat" de
→ in-game kareleri (combat/boss/director/merchant/elite) BEN builder ile çekerim · overlay kareleri (draft/run-map/char-sheet) **SEN OBS** ile (screenshot'a çıkmıyor) · **yedek demo videosu** kaydet.

### 4. "edit-to-play koreografi" de
→ centerpiece sunumu birlikte planlarız (combat-dışı oda gerek — combat'ta prop persist etmiyor).

### 5. Opsiyonel
- ChatGPT'ye yeni push'u (`8fabfe87`) re-review ettir · **"rapor PDF kur"** → Pandoc+Typst+Marp kurar, markdown→Türkçe PDF + Marp slide.

Tam liste ayrıca: `STAGING/USER_NEXT_SESSION_TODO_2026-06-17.md`

---

## ✅ Bu session DONE (hepsi runtime-verified + pushed, son `8fabfe87`)
Combat P0 (boss re-acquire / death-restart token-death / Penitent opening-çıkar+combo 85→42 — 3 gerçek / 2 non-repro) · CombatJuice · **enemy-readable outline + ambient 0.22→0.35** (gizli `EnsureVisibleSprite` material-clobber bug fixed) · boss 6 telegraph + refinements (Wrath yeşil-safe-ring / origin-snapshot / FlashImpact finisher-only) · arena dressing (6 prop) · music bed (JaggedStone CC0) · **deterministik ilk-oda** (`combat_large_cross_01`) · A1 prop (F2 9→19) · Director IDE-skin · ChatGPT review içselleştirildi.
Karar: `STAGING/DEMO_BITIRME_DECISION_2026-06-17.md` · Rapor içerik: `STAGING/SUNUM_RAPOR_ICERIK_2026-06-17.md` · ChatGPT review: `STAGING/_process/2026-06/chatgpt_review_rev2/CGPT_REVIEW_OUTPUT/`

## 🧠 KEY / DOKUNMA
- "yeşil-assert ≠ çalışıyor" kez kez doğrulandı (tek-arena verify yetmedi → ChatGPT-review + verify-first ile yakalandı). Çok-katmanlı doğrulama = rapor değeri.
- **DOKUNMA:** GATE / Boss-presentation / HUD / reward-bleed / Build-placement / Director-skin / spawnProps-false-gate / weaponless-anim / branching-seed.
- Minör latent: SeloutOutline.mat orphan · HitPause overlapping-pause latch (gerçekte tetiklenmez) · draft-sonrası timeScale desync · `room_current.json` _Arena için ölü yol (kullanma).

*(Detay: MEMORY.md + [[project-demo-bitirme-decision-2026-06-17]]. Önceki RESUME git'te.)*
