# COUNCIL — ChatGPT REV2 Review Analizi (cx lens: KOD-GERÇEKLİĞİ + FİZİBİLİTE)

ACTIVE RULES: (1) think before answering (2) min — no speculation (3) read-only ANALYSIS (4) BLOCKED if unclear.

## ⛔ MUTLAK KISIT — READ-ONLY
**Hiçbir dosyayı DEĞİŞTİRME. Kod yazma. `git add/commit/push` YOK. Unity'de mutasyon YOK.** Sadece OKU + ANALİZ ET + tek bir RESP dosyası yaz. (Geçen oturum bir advisor rogue gidip `git add .` yaptı — tekrarı YASAK.)

## Sen kimsin
Sen council'in **kod-gerçekliği + fizibilite** lens'isin. Diğer iki advisor (design/UX + lean-skeptic) ayrı bakıyor; sen ÇAKIŞMA ARAMA, kendi açından en sert/dürüst analizi ver.

## Bağlam
- RIMA = Unity 2D top-down ARPG roguelite, bitirme **demosu 19 Haziran = 2 GÜN**. Tez: "oyun değil environment + tooling showcase" (%20 oyun / %60 mimari / %20 graphify).
- ChatGPT bir REV2 review paketi gönderdi (aşağıda). Biz bunu council ile bağımsız analiz edip ORTAK karar çıkaracağız. ChatGPT akıllı ama repo'yu derinlemesine bilmiyor.
- Ground truth: `CURRENT_STATUS.md` (oku). Kod: `Assets/Scripts/**` (Grep/Read).

## ChatGPT paketi (OKU — hepsi kısa .md)
`STAGING/_process/2026-06/chatgpt_review_rev2/RIMA_ChatGPT_Review_2026-06-17_REV2/`
- `01_EXECUTIVE_DECISION.md` · `03_SCREEN_BY_SCREEN_REVIEW.md` · `04_DIRECTOR_MODE_REDESIGN.md` · `05_BUILD_MODE_UX_POLISH.md` · `06_GAME_UI_REDESIGN.md` · `07_TWO_DAY_DEMO_PLAN.md` · `09_PROMPT_FOR_CLAUDE.md` · `DECISIONS.json` · `02_CAPTURE_QA.md`

## ⚠️ ChatGPT'nin BİLMEDİĞİ kör nokta (doğrula + değerlendir)
`DirectorMode.GameEntryScenes = {"MainMenu","CharacterSelect"}` guard'ı yüzünden **menüden başlayınca Director bootstrap ATLANIYOR** → backquote ölü + F2 (Build Mode, Director'a muhtaç) ölü full-flow'da. Demo şu an **dev-direct `_Arena`** sahnesinden koşuyor. ax'in `sceneLoaded`-hook fix denemesi VERIFY'da çalışmadı.
→ ChatGPT'nin TÜM Director redesign planı, erişilebilir bir Director varsayıyor. **Bootstrap-fix, redesign'dan ÖNCE mi gelmeli? Redesign, demoda erişilemiyorsa boşa mı? Yoksa demo zaten _Arena'dan koşacağı için sorun değil mi?** Kodu incele, net söyle.

## SENİN SORULARIN (kod + fizibilite)
1. **Director shared-prefab-shell yaklaşımı (`04`/`09`):** `DirectorMode.cs` + ilgili UI prefab/script yapısı GERÇEKTE buna uygun mu? "Mevcut callback'leri koru, decorative frame'leri disable et, yeni parent shell altında re-anchor + DirectorPanel/Button/Card/Input shared prefab" — bu 2 günde RİSKSİZ mi, yoksa gizli refactor tuzağı mı? Hangi dosyalar etkilenir?
2. **Build Mode "polish değil redesign yapma" kararı:** Mevcut `InPlayMapPaintOverlay` / Build Mode kodu, ChatGPT'nin istediği grid-görünürlük-seviyesi + hover-cell + footprint + valid/invalid-reason + status-bar eklemelerini KALDIRACAK yapıda mı? IMGUI mi uGUI mi? 2-4 saat gerçekçi mi?
3. **HUD ölçü override (`06`):** `HUD_DESIGN_SPEC.md`'deki 72×4px HP bar'ı demo için override — kodda HUD nasıl kurulu (sabit prefab mı, data-driven mı)? Ölçü değişimi kaç dosya?
4. **Boss P0 (`03`/`06`):** sprite scale/pivot/PPU/anchor + health-bar + shop-residue-cleanup + subtitle — bunlar GERÇEKTEN P0 blocker mı, kaç dosya, 3-5 saat gerçekçi mi?
5. **Capture-QA (`02`):** ChatGPT byte-byte duplicate screenshot yakalamış (09=08, 21=20, 19/20 panel görünmüyor) — bu metodoloji hatasını doğrula. SHA-256-FAIL + `activeInHierarchy` kontrollü bir capture-harness 2 günde kurulabilir mi, nasıl?
6. **2-gün planının (`07`) fizibilite sıralaması:** ChatGPT'nin P0 sırası kodla çelişiyor mu? Sence gerçek en-yüksek-kaldıraç sıra ne (bootstrap-bug dahil)?

## GRAPHIFY
Cross-file/mimari soruda önce graphify query: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json` (bulk-read'den ~71× ucuz).

## ÇIKTI (E1: dönüş ≤10 satır)
Tüm analizi şu dosyaya YAZ: `STAGING/_process/2026-06/chatgpt_review_council/RESP_cx.md`
Format: her soru için **AGREE / PARTIAL / DISAGREE + kanıt (dosya:satır)** + risk + 2-gün-gerçekçi-mi. Sonunda **"cx'in tek-cümle kararı"** + **"ChatGPT'nin en tehlikeli/yanlış 1 önerisi"** + **"ChatGPT'nin kaçırdığı 1 şey"**.
Dönüşte SADECE: RESP yolu + 8 satır en kritik bulgu. Rapor içeriğini dönüşe gömme.
