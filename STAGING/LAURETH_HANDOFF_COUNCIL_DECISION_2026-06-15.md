# LaurethStudio Handoff — Council Kararı (2026-06-15)

> **Girdi:** `F:\LaurethStudio\STAGING\RIMA_HANDOFF_2026-06-15.zip` (stüdyo mekanik-bankası + HoH2 araştırması + juice pass'i damıtılmış). Paket dürüstçe "emir değil girdi, kod durumunu bilmiyorum" diye çerçevelenmiş.
> **Süreç:** Orchestrator (Opus 4.8) bağımsız ön-yargı → council (cx/Codex + ax 3.1 Pro + ax 3.5 Flash), hepsine kilitli RIMA bağlamı verildi, **bağımsız karar** istendi (ne LaurethStudio'yu ne orchestrator'ı onaylama zorunluluğu). Ham çıktılar: `_process/2026-06/laureth_handoff/COUNCIL_{cx,axpro,axflash}.md` + brief.

## KARAR (oybirliği: 3 danışman + orchestrator)
**Demo scope-lock HOLD. 20 Haziran demosuna paketten SIFIR yeni mekanik (Grup A/B) ve SIFIR yeni polish KODU girer.**

Gerekçe (4 görüş ortak):
1. **Yanlış amaç fonksiyonu.** Paket "savaş iyi hissettirsin" için optimize; RIMA'nın KİLİTLİ tezi = %60 mimari / %20 oyun, centerpiece Edit-to-Play video, "tooling ciddi" mesajı. Kardeş-projenin iyi-niyetli girdisiyle kilitli tezden sapma riski.
2. **Golden-path GREEN'i destabilize etme.** Videodaki akış az önce 0-fix doğrulandı; 5 gün kala yeni kod = yeni bug yüzeyi tam o akışta. "golden-path-first, tüm oyunu bug'sız yapma tuzağı" direktifiyle çelişir.
3. **Scope-lock zaten council+ChatGPT onaylı.** Yeni feature için yeniden açmak o kararı baltalar; #1 iş = 6 cerrahi no-refactor fix.

## S1 — Grup A (combat-feel): HEPSİ POST
A1 Flowstep · A2 Posture-Crack+SHATTERED · A3 Aggression-Heal · A4 Sweet-Spot · A5 Rolling HP · A6 Accessibility — tamamı yeni kod/yeni yüzey; demoya GİRMEZ.

## S2 — Grup B (derinlik/meta): HEPSİ POST; bir kısmı DROP
- **CANON ÇAKIŞMASI (Karar#25 kalıcı-meta yasağı) → kalıcı kısımlar DROP:** B2 (Hub Rift mutator) · B5-kalıcı (Echo bias/Heir) · B10-hub-bank.
- **DROP (demo/tez değeri yok):** B6 (görünmez Luck statı) · B8 (çok-sınıf Status/Element — Warblade-only'de yan konu).
- **POST (canon-güvenli, değerli backlog):** B1 · B3 · B4 · B7 · B9.

## S3 — Polish/juice: ZATEN VAR; yeni kod YOK
Kodda mevcut (cx doğruladı): HitStop · ScreenShake/CameraShake/ScreenShakeDriver · CameraPunchController · DamageNumberDriver/DamagePopup · EnemyTelegraph · PixelPerfectCamera/sub-pixel snap · vignette varyantları · squash/death residue · RiftGlow/RiftPulse · MapFragment · SkillOffer kart-anim · BossIntroController.
→ **Batch-fix + runtime-verify ÖNCESİ yeni polish kodu YOK.** Sonrasında yapılacak tek şey: mevcut toggle/parametrelerin sahnede/capture'da açık olduğunu doğrulamak. Yeni sistem yazılmaz. (Tier-2/3'ün çoğu da kısmen mevcut → redundant.)

## POST-DEMO BACKLOG (küratörlü, öncelik sıralı)
**Council refine'i — orchestrator'ın "A2 = tek post-demo yıldızı" lean'i EKSİK bulundu (her iki ax + cx):**
RIMA'nın gerçek tezi *environment/tooling* olduğu için, oynanışta o tezi KANITLAYAN maddeler combat-feel'den önce/eşit gelir:
1. **B9 — Fixed-Verb + Rotating Environmental Rule** (tek Warblade, Director'dan yüklenen oda-kuralı → 5 farklı-hisseden oda). Environment-tezinin oynanış kanıtı. ⭐ En uyumlu.
2. **B7 — Placement-Driven Resource** (IsoRoomBuilder geometrisini combat kaynağına bağlar). Environment↔combat köprüsü.
3. **A2 — Posture-Crack + SHATTERED glyph.** Combat-feel'in en yüksek etki/efor adayı. ⚠️ **cx uyarısı: "mevcut Posture'a bedavaya yaslanır" KODDA KANITLI DEĞİL — `postureOverflow` consumer TODO görünüyor.** A2'yi planlamadan ÖNCE consumer tamamlığı doğrulanmalı; "ucuz kazanım" varsayımı şartlı.
4. **B1 (Fragment Contract Door) · B4 (Resonance Forecast)** — canon-güvenli run-içi derinlik adayları.

## DURUM
- Bu karar **geri-dönülebilir**; RIMA'da nihai çağrı kullanıcının. Danışmanlar arası **çatışan görüş YOK** — tek refine (A2 vs B9/B7 önceliği) yukarıda çözüldü.
- Paket SİLİNMEDİ; `_process/2026-06/laureth_handoff/` altında izlenebilir backlog kaynağı olarak duruyor.
