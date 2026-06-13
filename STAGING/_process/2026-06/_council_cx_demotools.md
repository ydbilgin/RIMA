ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Capstone SUNUM demosu için in-game runtime tool'larının (Director Mode) MEVCUT durumunu ve "mühendislik derinliğini en iyi gösteren minimal tool seti"ni feasibility/reuse açısından ölç. ANALYSIS ONLY.

# Bağlam (kritik)
Demo = hocaya canlı SUNUM (Steam/oyuncu değil). Sunucu (öğrenci) iki şey gösterecek: (1) oynanabilirlik (dual-class 10-dk slice), (2) ALTYAPI — Unity Editor'e GİRMEDEN oyun-içi tool'larla stat/spawn/ayar değiştirebildiğini canlı kanıtlamak. Not = SİSTEMLER üzerinden.
Director Mode planı: backtick ` → overlay, 6 sekme Spawn/Class&Skill/Stats/Build(Tile+Cliff+Prop)/Map/Telemetry. Plan: `STAGING/DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md`, `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md`.

# Sorular (feasibility/reuse — KODA bak)
1. **Mevcut durum:** Director Mode'da hangi sekmeler GERÇEKTEN çalışıyor, hangileri stub? (`Assets/Scripts/UI/DirectorMode.cs` — Spawn/Stats/Class/Build/Map/Telemetry tek tek). Stat sistemi (`ClassStatProfile→ClassStatRuntime→DamagePacket→DamageCalculator`) runtime'da canlı düzenlenebiliyor mu?
2. **En ucuz "altyapı flex":** Hocaya en az kodla en çok "altyapı derinliği" gösteren tool hangisi — canlı STAT tuning mi (slider→anında DPS değişimi), live SPAWN mı, class-switch mi? Hangisi zaten %90 hazır?
3. **Build/Prop (manuel prop palette) maliyeti:** Spawn sekmesi zaten "palette→ghost→tıkla→sil" yapıyor (`DirectorMode.cs`). Bunu prop'a aynalamak (rift crystal vb. manuel yerleştirme) ne kadar iş? LiveTool (ayrı Tool.exe) zaten prop palette yapıyor — reuse mı, Director'a port mu?
4. **Sunum güvenilirliği:** timeScale=0 overlay, IMGUI/PaintCell, runtime spawn — canlı sunumda crash/NRE riski olan kırılgan noktalar neler? Hangi sekme "sunumda göster" için yeterince stabil?
5. **Necessary vs sufficient:** 6 sekmenin hangileri sunum için ZORUNLU, hangileri kesilebilir/ertelenebilir? Kalan iş kabaca ne kadar?

ANALYSIS ONLY. Sonucu CODEX_DONE.md'ye yaz.
