# KARAR — ChatGPT playtest paketi asset/Egg/UI + PixelLab üretim önceliklendirme (2026-06-16)

**Soru:** Demo (**19 Haz = 3 gün**, golden-path-first, scope-lock HOLD, REWARD-02 demo-kritik) için PixelLab'dan NE üretmeli; ChatGPT paketinden ne benimsemeli; canon-çatışması var mı?
**Advisor'lar:** council = cx (feasibility/pipeline) + ax 3.1 Pro (deep design + VISION) + ax 3.5 Flash (lean) · + opus subagent (cross-system) · sentez+karar = Opus 4.8 orchestrator. Ham çıktılar: `STAGING/_process/2026-06/chatgpt_playtest/COUNCIL_{cx,axpro,axflash,opus}.md`. Triyaj: `STAGING/CHATGPT_PLAYTEST_EVAL_REWARD02_2026-06-16.md`.

## Yakınsama (4/4 oybirliği)
- **warblade anim (idle/run/LMB) = demo'nun TEK art-zorunlu PixelLab işi** (armed-anchor greatsword, V3, 120×120, 8-dir, transparent).
- **REWARD-02 fix KODDUR, asset'ten ÖNCE.** cx guardrail: fix `RewardPickup.cs` trigger/G-input'a dokunuyor → asset (sprite/prompt) üretimi **prefab collider/trigger veya G-input akışını değiştirmesin.**
- **4-cardinal/flip-yok karakter modeli = DROP** (RIMA 8-yön LOCKED, Karar #114).
- **"35° top-down" = prompt'ta düzelt** → "high top-down 3/4 ~70-80°"; UI flat olduğu için etkilenmez, sadece prop/karakterde.
- **Egg sistemi/incubation/ChoiceSet = post-demo** (paketin kendisi de "bug'lar çözülmeden yapma" diyor).
- **PRO mode = DROP** (maliyet); V3/Create Image Pro yeterli.
- **Benimse:** polish-first altın kural (PNG-mezarlığı sigortası); UI-01 reward-kartı footer layout fix yönü + DATA-01 "trigger→outcome" etiketleme (⚠️ AYRI bug'lar — aşağıdaki bug-ayrımına bak, "combo-box" RIMA terimi DEĞİL).

## Ayrışma + KARARIM
ax_pro + cx golden-path-görünür UI alt-kümesini (reward-card/combo/chrome) demo'ya alabiliriz; opus + flash "warblade only, gerisi scope-creep" dedi.
**KARAR = opus/flash yönünde, kritik içgörüyle:** reward ekranının ihtiyacı **yeni PNG değil, KOD fix'i** — reward kartı `SkillOfferUI.BuildSkillCard`'da tamamen KODDA inşa (sabit sizeDelta, prefab/Instantiate YOK, **ContentSizeFitter YOK** → paketin spekülatif "TMP/ContentSizeFitter çatışması" teşhisi bizim kodda GEÇERSİZ → ÖNCE repro). Yeni sprite footer collapse'ini çözmez. **REWARD-02 (RewardPickup.cs) + UI-01 (kart footer, SS-04) = eş demo-kritik KOD fix'leri** reward beat'ini düzeltir. ax_pro/cx'in haklı yanı (reward ekranı videoda görünür) **kod tarafında** karşılanıyor; yeni UI art = post-demo.

## Bug-ayrımı (critic fix — 3 AYRI bug, birleştirme!)
- **UI-01** (paket): reward kartı footer/synergy alanı dikey cyan sütuna çöküyor (SS-04). Yer: `SkillOfferUI.BuildSkillCard` (kodda). Cause BİLİNMİYOR → repro-first. **Demo-kritik** (golden-path reward beat'inde görünür).
- **backlog-U1** (RIMA): tooltip dikey-şerit → `TooltipSystem` preferredWidth/ContentSizeFitter. AYRI widget. Post-demo (golden-path'te değilse).
- **DATA-01** (paket): "eşleşir" → "trigger→outcome / KOMBO AÇAR" etiketleme = string/data, layout değil. Post-demo.
- ⚠️ "combo-box" RIMA terimi DEĞİL (paket manifest'inin `reward_combo_box` asset adından + opus'tan sızmıştı) — kullanma.

## FINAL — Üretim listeleri
**DEMO PixelLab (tek):**
1. **warblade idle/run/LMB anim** — armed-anchor greatsword; Create Image Pro master sheet → Create Character → Custom Anim V3; 120×120; 5 üret (S/SE/E/NE/N) + 3 mirror; transparent+alfa-doğrula. (Kullanıcı koşturur; `STAGING/ANIM_PRODUCTION_HANDOFF_2026-06-15.md`.)
- **DEMO reward-beat kalitesi = KOD** (asset değil, eş demo-kritik):
  - **REWARD-02** fix: `RewardPickup.cs` OnTriggerStay2D + Awake OverlapCircle.
  - **UI-01** reward-kartı footer collapse (SS-04): `SkillOfferUI.BuildSkillCard` — ⚠️ ÖNCE repro (cause bilinmiyor, ContentSizeFitter teşhisi geçersiz), sonra footer min-genişlik/anchor fix.
  - **HUD readability** = Canvas Scaler/scale (kod).
- **Opsiyonel (yalnız warblade BİTTİ + REWARD-02/UI-01 kod fix audit-PASS + zaman kaldıysa):** tek ucuz dokunuş — RewardPickup 96px shell sprite (⚠️ Awake sprite-fallback satır 36-51 ile çakışabilir → AYRI commit, REWARD-02 audit-PASS sonrası) VEYA `UI_Chrome.spriteatlas`'ta eksik/zayıf birkaç parça. Planlı/blocking DEĞİL.

**POST-DEMO:** modüler UI pack (~50 parça, 8-atlas) · UI polish art-pass (Pause/Settings/Codex) · Rift-Forged Egg sistem+art (skin mevcut RewardPickup'a bağlanabilir ama RewardDefinition/ChoiceSet açma) · Elementalist anim (idle/run/cast) · hatch/reject anim · Q/E/R/F = reuse+SkillVfx (bespoke YOK).

**DROP:** 4-cardinal/flip-yok karakter modeli · PRO mode · Egg incubation/pet companion · UI-içine-gömülü text/icon (TMP+mevcut icon kullan) · boss-reward-için-egg.

## Canon-flag tablosu
| Madde | Çatışma | Öneri |
|---|---|---|
| Yön: 4-cardinal | EVET | REDDET; 8-yön LOCKED (karakter konseptlerini drop) |
| View: 35° | EVET (prop/char) | prompt = "high top-down 3/4 ~70-80°"; UI flat → etkisiz |
| Renk: purple neon overload (Egg/VFX, OV-02) | KISMEN | mor'u kıs; cyan ≤%15; UI ekranları (VP-01..05) zaten canon-uyumlu |
| Stil: slate/dark-iron/cyan/amber | YOK | uyumlu |
| UI scale 1080/1440/ultrawide | KOD riski | Canvas Scaler 1920×1080 + safe-area/max-width (asset değil) |
| Atlas (Chrome≠FX) | YOK | doğru; Point, mipmap off, compression none |

## Benimsenecek ekstra (post-demo UI işine taşı)
- Buton state ayrımı (Idle/Hover/Pressed/Disabled — VP-06) + iç-içe rarity-strip/overlay = PNG-israfı önleyen best-practice.
- Reward lifecycle atomikliği (CombatCleared && RewardResolved → kapı; ya da açık Reddet/Atla) = REWARD-03 cevabı (post-demo, kod).

## Riskler / guardrail
- En yüksek risk: PixelLab'ı UI/Egg için şimdi açmak → REWARD-02'yi geciktirir. **warblade anim (kullanıcı) ‖ REWARD-02 kod fix (crafter+audit) paralel; UI/Egg açma.**
- 4-cardinal'i yanlışlıkla anim'e taşıma. "35°"yi literal prompt'a koyma. Transparent/alfa doğrulanmazsa halo artefaktı.

## Adversarial critic — UYGULANDI
VERDICT: **NEEDS-FIX** (çekirdek yön SAĞLAM, kodla doğrulandı: U1=kod değil-art, REWARD-02 asset-bağımsız, 8-yön/V3/Egg-post-demo canon-uyumlu). Binding fix'ler bu doc'a işlendi: (1) "combo-box" uydurma terim kaldırıldı + UI-01/backlog-U1/DATA-01 ayrıldı; (2) ContentSizeFitter teşhisi geçersiz→repro-first; (3) UI-01 footer = eş demo-kritik; (4) opsiyonel shell-sprite ayrı-commit guardrail. Tam: `STAGING/_process/2026-06/chatgpt_playtest/_council_critic.md`.
