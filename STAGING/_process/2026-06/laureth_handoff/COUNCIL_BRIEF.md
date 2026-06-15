# COUNCIL — LaurethStudio mekanik/polish handoff: demoya ne girer? (BAĞIMSIZ KARAR)

ACTIVE RULES: (1) think before answering (2) min recommendation, no speculation (3) surgical — judge the listed items only (4) say BLOCKED if context insufficient.
NLM ACCESS: RIMA tasarım bağlamı gerekirse önce NLM sorgula:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"
Direkt-oku: bu brief + handoff dosyaları (yollar aşağıda) + (gerekirse) Assets/Scripts.

GRAPHIFY: cross-file/mimari soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.

UNITY YOK: Bu görev SALT ANALİZ. Unity'ye BAĞLANMA, play mode AÇMA, execute_code ÇALIŞTIRMA. Sadece dosya oku + yargı ver.

---

## GÖREVİN
LaurethStudio (stüdyo meta-katmanı, AYNI dev) RIMA'ya bir mekanik+polish öneri paketi gönderdi. Sen bu paketi **bağımsız** değerlendireceksin. NE LaurethStudio'nun çerçevesini, NE orchestrator'ın eğilimini onaylamak zorundasın — kendi yargını ver. Paket dosyaları:
- `STAGING/_process/2026-06/laureth_handoff/00_README.md`
- `STAGING/_process/2026-06/laureth_handoff/01_MECHANIC_OPTIONS.md` (Grup A: A1-A6 combat-feel · Grup B: B1-B10 derinlik/meta)
- `STAGING/_process/2026-06/laureth_handoff/02_DEMO_POLISH.md` (Tier-1/2/3 juice)

## RIMA'NIN KİLİTLİ DURUMU (öneren bunu BİLMİYORDU — sen biliyorsun, karar buna göre)
1. **Demo ~20 Haziran = ~5 gün sonra.** Tek sınıf (Warblade). Solo dev.
2. **Sunum tezi KİLİTLİ (4× council + 2×2 deney):** RIMA = "oyun DEĞİL, **environment + ilk vertical slice**". Eksen = **%20 oyun / %60 mimari / %20 graphify-audit**. Centerpiece = **Edit-to-Play video** (in-game seviye editörü → oyna). Demonun işi "savaş derin/eğlenceli" demek DEĞİL; "bu stüdyonun TOOLING/MİMARİSİ ciddi" demek. → `STAGING/PRESENTATION_VISION_DECISION_2026-06-14.md`
3. **Mevcut #1 iş = 6 cerrahi fix'lik NO-REFACTOR batch** (council + ChatGPT bağımsız review hemfikir, scope KİLİTLİ). Golden-path (videodaki akış) az önce **GREEN doğrulandı** (0 kod fix gerekti). → `STAGING/DEMO_BATCH_FIX_SPEC_2026-06-15.md`, `STAGING/GOLDEN_PATH_VERIFICATION_DECISION_2026-06-15.md`
4. **Strateji direktifi:** "golden-path-first — 'tüm oyunu bug'sız yap' tuzağına DÜŞME; sadece videodaki akış kusursuz olsun."
5. **KOD GERÇEĞİ (orchestrator doğruladı):** Tier-1 juice'ların ÇOĞU ZATEN VAR → `Assets/Scripts/Core/HitStop.cs`, `Combat/Juice/ScreenShakeDriver.cs` + `VFX/ScreenShake.cs` + `Core/CameraShake.cs`, `Combat/Juice/CameraPunchController.cs`, `Combat/Juice/DamageNumberDriver.cs` + `UI/DamagePopup.cs`, `Enemy/Telegraph/EnemyTelegraph.cs`, Posture sistemi (x3.0 cap→Posture mevcut). Yani polish paketi büyük ölçüde mevcut kodla örtüşüyor.
6. **Canon kısıtı:** Karar#25 "kalıcı hub-upgrade/run-arası kalıcı meta → Faz 4-5". Grup B'nin bir kısmı (B2/B5/B10 kalıcı kısımları) bununla çakışıyor (öneren de işaretledi).
7. **Modüler-tasarım disiplini:** "modülerliği hak ediyor mu?" refleksi; prototipte erken modülerleştirme YOK; signature/boss/Echo bespoke kalır.

## CEVAPLAYACAĞIN SORULAR (net IN/OUT + 1 satır neden)
- **S1 — Grup A (A1-A6):** 20 Haz demosuna giren VAR mı? Her ilgili madde için IN(demo) / POST(sonra) / DROP + 1 satır. (Hatırlatma: 5 gün, golden-path GREEN, scope-lock var.)
- **S2 — Grup B (B1-B10):** Demoya giren VAR mı? Canon (Karar#25) çakışanları işaretle. POST-demo için en değerli 1-3 tanesi hangisi?
- **S3 — Polish (Tier-1/2/3):** Zaten-VAR olanları çıkar. GERÇEKTEN eksik + golden-path VİDEOSUNU profesyonelleştiren + düşük-risk olan bir alt-küme var mı? Yoksa hepsi POST mu? Eğer "IN" diyorsan, batch-fix + runtime-verify SONRASINA mı, yoksa golden-path'i destabilize etme riski yüzünden tamamen sonraya mı?
- **S4 — META:** Bu paketin demo penceresindeki NET değeri ne? "Bağımsız" yargın: orchestrator'ın eğilimi (demo scope-lock HOLD, hepsi post-demo backlog; sadece A2 Posture-Crack+SHATTERED glyph mevcut Posture'a yaslandığı için en güçlü post-demo adayı) — bu eğilim DOĞRU mu, EKSİK mi, YANLIŞ mı? Katılmıyorsan gerekçeli itiraz et.

## ÇIKTI FORMATI (DOSYAYA yaz, dönüş ≤10 satır özet)
Çıktını şu dosyaya yaz: `STAGING/_process/2026-06/laureth_handoff/COUNCIL_<senin-adın>.md` (cx → COUNCIL_cx.md, ax_pro → COUNCIL_axpro.md, ax_flash → COUNCIL_axflash.md).
Format: S1/S2/S3/S4 başlıkları + her madde tek satır IN/POST/DROP + neden + son "TEK CÜMLE TAVSİYE". Orchestrator'a dönüşte sadece ≤10 satır özet + dosya yolu.
