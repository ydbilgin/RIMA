# RIMA — Durum İncelemesi İçin ChatGPT Brief'i (2026-06-08)

> **Kullanıcı için not:** Bu dosyayı ChatGPT'ye (repo erişimi olan web ChatGPT) yapıştır. ChatGPT private repo'yu okuyabiliyor; dosya yollarını verdim. Cevabı bana geri getir → council'den (cx + ax-3.1-Pro + Opus) geçirip stale/yanlış olanları eleyeceğiz.

---

## Sen kimsin, ne istiyorum

Sen kıdemli bir oyun yapımcısı / teknik direktör gözüyle bakacaksın. RIMA adlı **2D top-down roguelite ARPG** (Unity, URP 2D) bir **bitirme projesi demosu** hedefliyor. Repo'ya erişimin var — kod, doküman, commit geçmişi okuyabilirsin.

**İstediğim 3 şey:**
1. **Öncelik kontrolü** — Aşağıdaki "yapılanlar" ve "bekleyenler" listesine bak. Demo için **doğru sırada** mı ilerliyoruz? Yanlış yere efor harcanan, erken yapılan ya da atlanmaması gereken bir şey var mı?
2. **Kör nokta avı** — Bu kadar otonom iş yapıldı; gözden kaçan risk, biriken teknik borç, ya da "demo günü patlar" diyeceğin bir şey var mı?
3. **Düzenleme/öneri** — Backlog'u sadeleştirir/yeniden sıralarsan nasıl olurdu? Kısa, eyleme dönük madde madde.

⚠️ Spekülasyon yapma; emin değilsen "repo'da X'e bakılmalı" de. Tahmin yerine dosya işaret et.

---

## Proje özeti (bağlam)

- **Tür:** 2D top-down roguelite ARPG, Hades/Children of Morta hissi (high 3/4 kamera, pixel art).
- **Çekirdek döngü:** Attunement Chamber (sınıf seçimi odası) → Rift kapısı → prosedürel oda zinciri (combat → clear → 3-kart reward draft → sonraki oda) → boss → death/victory.
- **Sınıf modeli:** 10 sınıflık veri modeli mevcut; **4'ü uçtan uca oynanabilir** (demo dürüstlüğü böyle anlatılıyor).
- **Pipeline:** PixelLab (sprite/anim) + imagegen (environment) + Unity MCP otomasyonu. Çok-ajan orkestra (Claude orchestrator + Codex/cx + Gemini/ax).
- **Canonical tasarım kaynağı:** NotebookLM notebook (kod-dışı kararlar orada).

İlgili dosyalar:
- `CURRENT_STATUS.md` (en üstteki 2026-06-08 bloğu = güncel)
- `.claude/PROJECT_RULES.md` (mimari kurallar: 8-yön sprite, PPU64, top-down 3/4, asset canon)
- `STAGING/MASTER_PLAN_FINAL_2026-06-06.md` (T1-T9 demo planı)

---

## Bu oturumda YAPILANLAR (2026-06-08, hepsi commit'li + push'lu)

| # | İş | Commit | Durum |
|---|---|---|---|
| 1 | **Interaction-prompt fix** — `[G] [G] Ödülü Al` çift-prepend bug'ı; token-aware `HUDController.SetInteractionPrompt`. 7 EditMode test yeşil. | `d45a3780` | ✅ |
| 2 | **Playtest round-1 fix'leri** — ödül scale (1.5→0.55), kamera zoom (1.0→1.25), chamber "bleed" temizliği (iki seçim sistemi + temizlenmeyen chamber root → rebuild + combat-load'da stale destroy), `_Arena` kapısı yürü-trigger yerine **[G]-interact + nabız ring**, chamber çıkışı **siyah portal silüeti**, boş odaların prop/mob ile doldurulması. | `0f77996` | ✅ |
| 3 | **Weapon pipeline audit** — her bulgu dosya:satır ile CONFIRMED/REFUTED. Verdict: Warblade Level1 demo-hazır (PARTIAL PASS), 10-class FAIL (minimal "mount profili" kodu gerek). Mob boyut çelişkisi COMBAT_ROSTER ile çözüldü. | `daeb2402` | ✅ |
| 4 | **Doc canon-drift cleanup** — silah doc'larındaki yanlış canon (staff/whip/gauntlet vb. FORBIDDEN) düzeltildi, stale status notları temizlendi. | `dfe68c6b` | ✅ |
| 5 | **Chamber redesign (5+5)** — oda 28×20 büyütüldü, 10 pedestal iki yay, statik dummy, bürünme çalışıyor, kilitli=siyah+cost prompt. | `260cf159` | ✅ |
| 6 | **Chamber REWORK-2** (playtest sonrası, EN GÜNCEL fonksiyonel) — (a) **CRASH FIX** `ChamberSelectBootstrap.cs:785` (destroyed-SpriteRenderer null-guard), (b) **yürüme açıldı** (pedestal disk prop'ları kaldırıldı, walkable tam serbest), (c) **10 silüet 5+5 front-facing idle_south** sol iki kenarda, (d) **dummy** = ölümsüz (100k HP, ölmez, ölümcül sonrası dolar), G→sınıf-seç popup→`PlayerClassManager` uygular, (e) **🔑 dual-system çözüldü** (çoklu başlama ekranı bug'ı: MainMenuScreen duplicate bootstrap kaldırıldı, `_IsoGame`→`_Arena` route). 13 EditMode + 3 PlayMode test. | `d96e86f9` | ✅ |
| 7 | **PixelLab silah üretim yöntemi LOCK** — 3 kaynak (cx + ax-3.1-Pro + ChatGPT) oybirliği = B-hibrit: tam-boyut object generation production için, Create-Image-Pro fallback; hedef-boyut native (büyük-canvas-küçültme YASAK), kare canvas + transparent CROP (SCALE yasak), aspect-ailesine göre 3 batch. Üretim öncesi tek gate = VERIFY-LIVE testi. | `7c4baafb` | ✅ |

**Toplam: 8 commit, hepsi push'lu.** Önceki gün (~32 commit) demo iskeleti kuruldu: portal wiring, boss intro momenti, çift-dilli (TR/EN) UI, walkable fizik, UI↔JSON oda editörü, screenshot modu, juice+SFX+execute prompt.

---

## BEKLEYENLER / Açık kararlar

**Kullanıcı playtest GATE'i (en kritik):** Chamber rework-2 + playtest-1 fix'leri henüz kullanıcı tarafından oyunda doğrulanmadı. Kontrol listesi: crash gitti mi · yürüme serbest mi · 10 FARKLI sınıf görünüyor mu (eskiden 3-Warblade bug'ı vardı) · kapı [G] ile mi geçiyor · dummy popup'la sınıf değiştiriyor + ölmüyor mu · TEK başlama ekranı mı.

**GATED (kullanıcı hazır olunca, birlikte):**
- **PixelLab 3-silah seansı** — ilk adım VERIFY-LIVE testi, sonra B1/B2/B3 batch (Elementalist disk / Ranger yay / Shadowblade hançer). Yöntem: `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md`.
- **Minimal mount-profil patch** (~205-300 LOC) — silahların oyunda elde görünmesi için (`WeaponDatabaseSO` profili + `WeaponMountView` + flip-profile). Audit §5'te. Silahlar üretilince mantıklı.

**Opsiyonel workstream'ler:**
- Oda-layout tasarımı (ChatGPT sketch'leri `STAGING/_inbox/character_room_sketches_2026-06-08/` — walkable boyut, pedestal küçültme, asset dili birleştirme).
- Dual-system consolidation (`_IsoGame` bypass yolunun tamamen sökülmesi).
- Demo cila kuyruğu: T5 chamber atmosfer görsel QC, T8 void wiring, Sunum Çekim Rehberi.

**Açık tasarım soruları:**
- Chamber'daki 10 silüet: hepsi tıkla-seç mi olmalı, yoksa sadece dummy-popup üzerinden mi sınıf seçilsin? (Şu an silüetler display-only.)
- Currency adı "Echo" — meta-isim vs mekanik çakışması var, kilitlenmedi.

---

## Sana özel sorular (cevaplaman istenenler)

1. Demo için kritik yol doğru mu? **Playtest → (fix VEYA PixelLab silah) → ...** sıralaması mantıklı mı, yoksa önce yapılması gereken başka bir şey mi var?
2. "10 sınıf veri modeli ama 4'ü oynanabilir" — bir bitirme demosu için bu yeterli mi, yoksa demo anlatısı açısından zayıf nokta mı?
3. Silah pipeline'ı henüz hiç sprite üretmedi (sadece yöntem locked). Demo'da silahların elde görünmemesi kabul edilebilir mi, yoksa bu "must-have" mi?
4. Bu kadar otonom commit + bu kadar doküman → senin gördüğün en büyük **risk** ne? (teknik borç / scope creep / doğrulanmamış varsayım / başka)
5. Backlog'u sen yeniden sıralasan ilk 5 madde ne olurdu?

> Cevabın repo'daki gerçek dosyalara dayansın. Emin olmadığın yerde "X dosyasına bakılmalı" de.
