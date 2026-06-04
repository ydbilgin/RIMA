ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Ölüm ekranını ("berbat") NLM-canon "Vivid Vulnerability / ink-on-paper" tonuna göre yeniden tasarla + build-seed'i tamamen kaldır.

READ FIRST: STAGING/UI_REDESIGN_SCREENS_DECISION_2026-06-04.md (§1 Death + cross-cutting) ve STAGING/UI_REDESIGN_BRIEF_2026-06-04.md.

## Dosyalar (sadece bunlar)
- Assets/Scripts/Core/DeathScreenManager.cs (ana iş)
- Assets/Scripts/Core/RunStats.cs (dead seed temizliği)
- Assets/Scripts/**/DemoCompleteOverlay.cs ("Build:" satırını kaldır)

## Yapılacaklar (DeathScreenManager.cs)
1. **KALDIR (tamamen):** CopyBuildSeedButton + CopyBuildSeed() + listener wiring + stats'taki "Build: {BuildName}" satırı; **WISHLIST butonu + OpenWishlist()** (kullanıcı: tamamen kaldır); **NextClassTeaser** paneli (clutter).
2. **Blackout = yumuşak vignette gradient** (opak blok DEĞİL) — donmuş combat hafif görünsün altında. Mevcut OverlayDark + death_screen_bg backdrop kullan ama panel'i opak kutu yapma.
3. **Layout (ortada dikey, ferah):**
   - (a) Rastgele canon ölüm satırı (mevcut DeathLines dizisi — context-aware DEĞİL), pixel-serif ~28pt, RimaUITheme.TextPrimary, ortalı.
   - (b) İnce cyan hairline ayraç (1-2px Image, RimaUITheme.Cyan düşük alpha).
   - (c) Run stats: küçük, dikey, sadece `ODA · KILLS · SÜRE` (Build YOK). RimaUITheme.TextMuted, ~14-16pt.
   - (d) İki EŞİT buton yatay: "TEKRAR DENE [R]" (RestartRun) + "ANA MENÜ" (LoadMainMenu). Aynı stil: Pack button_9slice (RimaUITheme) translucent + cyan hover. İnce cyan hairline border, opak panel YOK. 'R' kısayolu korunsun.
4. Tüm buton renkleri RimaUITheme'den (hardcoded #D8FFFF vs YOK). Tutarlı hover/pressed.

## RunStats.cs
- DeathScreenManager seed kullanımı kalkınca dead olan: BuildSeed, GetBuildSeed(), SkillToken() — başka AKTİF kullanım yoksa kaldır. BuildName: DemoCompleteOverlay artık kullanmayacaksa onu da kaldırabilirsin; referans kalırsa bırak. Compile clean ŞART.

## DemoCompleteOverlay.cs
- "Build: {RunStats.BuildName}" satırını kaldır (tutarlılık — build seed/summary hiçbir yerde yok).

## Doğrulama
- dotnet build ya da derleme temiz olmalı (0 hata). Play-mode'a GİRME (D3D12 crash riski — kullanıcı D3D11 restart edecek, verify ben yapacağım).
- Sonucu CODEX_DONE.md'ye yaz: hangi satırlar değişti, kaldırılan member listesi, compile durumu.
