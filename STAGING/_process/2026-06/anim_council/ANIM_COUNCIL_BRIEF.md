# COUNCIL — Demo için warblade animasyon önceliklendirme (state-first workflow)

ACTIVE RULES: (1) think (2) min — sadece karar ver, üretme (3) surgical — sadece sorulan (4) BLOCKED if unclear.
NLM ACCESS: RIMA tasarım context gerekirse: NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>".
UNITY YOK: Salt analiz. Unity'ye BAĞLANMA. Sadece dosya oku + yargı.

## GÖREVİN
Demo (~20 Haz, ~5 gün) için **warblade** (tek oynanabilir sınıf) hangi animasyonları üretmeli — **bağımsız** önceliklendir + budget'la. Ne kullanıcının ne orchestrator'ın eğilimini onaylamak zorundasın; kendi yargın.

## DURUM (öneren bunu bilmeli)
- **warblade durumu:** 8-yön 120x120 high-top-down hazır AMA `animations: none` → idle/koşma/vuruş HEPSİ sıfırdan.
- **PixelLab budget:** Tier 2 aktif, **874 generation kaldı** → BOL. **Asıl kısıt = ZAMAN + per-yön cleanup eforu** (Pixelorama elle düzeltme), generation değil.
- **Sunum tezi KİLİTLİ:** RIMA = "environment + vertical slice", eksen **%60 mimari / %20 oyun**; centerpiece = **Edit-to-Play video**. Demoda warblade ŞU AKIŞTA görünür: idle → koşma → **LMB temel vuruş (stat→damage beat = combat centerpiece)** → (Q/E/R/F basılır ama bunlar `bypassStatScaling` = stat-deaf KOREOGRAFİ; combat matematiği SADECE LMB'de). → `CURRENT_STATUS.md` golden-path.
- **Asset canon (DEĞİŞTİRME):** 8-yön LOCKED = 5 üret (S,SE,E,NE,N) + 3 mirror (flipX); 120x120; high-top-down 3/4; 10-12 fps; PPU 64. Workflow = Create Image Pro → Create Character (VAR) → **Custom Animation V3**.
- **Claude 4.8 kotası ~%3 (~21s reset):** üretim PixelLab+kullanıcı; bu yüzden KARAR şimdi, execution sonra.

## VİDEO STRATEJİSİ (kullanıcı "buna göre karar ver" dedi) — tam transcript: `pixellab_states_video_transcript.txt`
PixelLab'in YENİ **States** workflow'u: idle'dan brute-force animasyon ÜRETME. Önce hedef-aksiyona yakın **STATE (poz)** üret (ör. mid-walk, fighting-pose, flinch), sonra **o state'i ilk-frame tutarak** Custom Animation V3 ile animate et → temiz başlangıç. **Interpolation:** start+end frame ver → ara hareket (poz-geçişleri: windup→strike, hit→recover). **Mirror:** SE/E/NE üret → W-yüz mirror; sonra sadece S+N → 8-yön hızlı. Per-yön: kimi mükemmel, kimi cleanup, kimi reroll.

## CEVAPLA (net karar)
- **S1:** Demoya hangi warblade animasyonları girmeli? **P1 (must, golden-path videoda GÖRÜNEN)** / **P2 (yüksek değer, vakit varsa)** / **P3 (demo-dışı/reuse, post-demo)** olarak sırala. Garanti: **idle + koşma**. Vuruşlar? hit-react? death?
- **S2:** Q/E/R/F skill'leri (stat-deaf koreografi) — BESPOKE animasyon mu, yoksa LMB/generic-cast REUSE mu? (tez %20-oyun + 5 gün + cleanup-eforu ışığında)
- **S3:** Her P1/P2 için **state-first plan:** hangi STATE önce üretilir → hangi animasyon? interpolation gereken var mı? Kaç yön üret+mirror?
- **S4:** Kaba budget (generation + cleanup-zaman riski) + "5 günde gerçekçi mi?" — golden-path-first: videoyu güzelleştiren MİNİMUM set ne?

## ORCHESTRATOR LEAN (bağlayıcı DEĞİL — itiraz et)
P1: idle, koşma, LMB vuruş (videoda görünen 3). P2: hit-react (flinch — stat→damage beat'i okutur). P3/reuse: Q/E/R/F bespoke = post-demo (demoda LMB/generic-cast reuse); warblade death = düşük öncelik. State-first: breathing→idle, mid-run→koşma, strike-windup→LMB, flinch→hit-react. 5 üret+3 mirror.

## ÇIKTI (E1)
Dosyaya yaz: cx → `ANIM_cx.md` · ax_pro → `ANIM_axpro.md` · ax_flash → `ANIM_axflash.md` (bu klasörde). Format: S1-S4 + son "TEK CÜMLE TAVSİYE". Orchestrator'a dönüş ≤10 satır + yol.
