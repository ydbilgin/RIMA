ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA + gelecek oyunlar için Godot'a geçiş kararına FEASIBILITY / REUSE / "ne zaten var" lensinden danışmanlık. ANALİZ ONLY — kod değişikliği YOK. Cevabı CODEX_DONE.md'ye yaz.

# Görev
Aşağıdaki bağlam + 4 alt-soruya FEASIBILITY/REUSE lensinden cevap ver. Önceki hiçbir audit'i tekrar üretme. Kısa, kanıta dayalı, file:line ver-ebildiğin yerde.

## BAĞLAM (web-doğrulanmış güncel veri — ben/orchestrator çektim, transcript'ler dahil)

**RIMA = Unity** (2022 URP 2D, C#, Pixel Perfect Camera, 640×360 ref res, 549 test) + **Unity-MCP-tabanlı çok-ajan pipeline** (Claude+Codex+Gemini; mcp__UnityMCP__* ~39+ tool: manage_gameobject/scene/script/prefabs/animation, run_tests, execute_code...; C# native; ÇOK olgun). PixelLab/imagegen asset pipeline. 10-sınıf veri modeli (4 oynanabilir). Bitirme demosu. **RIMA Unity'de KALACAK (sunk cost); soru GELECEK oyunlar için.**

### Godot MCP GÜNCEL DURUM (2026, web-doğrulanmış):
- **godot-ai (hi-godot/godot-ai):** ~39 MCP tool / 120+ ops; scene/node, GDScript edit, signal wiring, UI/material/anim/particle/camera/env, file search, test exec. SADECE GDScript (C# yok). Godot 4.3+ (4.4+ önerili). 503★, 66 release, v2.6.1 (2026-06-03). AssetLib 1-tık. Claude Code: `claude mcp add --scope user --transport http godot-ai http://127.0.0.1:8000/mcp`.
- **Coding-Solo/godot-mcp:** editor launch, run/debug, console+error capture, version, list/analyze projects, scene create, node add, sprite→Sprite2D, 3D→MeshLibrary, UID(4.4+). JS%59.6+GDScript%40.4 (C# yok). 4.1k★, 390 fork.
- **StraySpark (TİCARİ):** 131 tool, "production-safe", SceneTree/node/property, scene+GDScript, signals, settings, run+capture.
- Dil: GDScript tam destek; "C# entegrasyonu bazı server'larda VAR ama GDScript'ten belirgin daha az olgun."
- Olgunluk: Godot MCP 2025 shipped, Unreal 2024. 3 gotcha: auto-reload davranışı, GDScript dinamik-tipleme→daha çok başarısız call, SceneTree/dosya state ıraksaması.
- Unity MCP'ye kıyasla **tool SAYISI yakın/eşdeğer** (39 vs 39; StraySpark 131). C# tarafı zayıf nokta.

### Godot vs Unity 2D (2026): Godot amaca-özel 2D (3D-Z=0 değil), native pixel-perfect, ücretsiz/MIT royalty-yok, 150MB anında açılır, recompile-yok. Unity: dev asset store, olgun URP 2D lighting (RIMA kullanıyor), Spine, console export daha sorunsuz, $200k royalty eşiği.
### TileMapDual: MIT Godot dual-grid (15 tile vs 47); **Unity versiyonu DA var** (jess-hammer) → tek başına geçiş sebebi değil.

### 3 VİDEO (transcript-doğrulanmış):
1. **"Make Systems Not Games":** Dream game'i direkt yapma; ayrı projelerde bağımsız/reusable SİSTEMLER yap (envanter/AI/combat/controller). Refactor-bırakma en büyük risk. Sistemler=portföy+reuse+kolay-test+esneklik. Sistemi DREAM-GAME'e değil GEREKSİNİME göre kur. Export'u erken öğren.
2. **"Yūgen Terrain Toolkit":** Godot 3D-pixel-art terrain (Marching Squares/chunk/brush, MIT). 3D-pixel-art = gelecek oyun stili, RIMA değil. Godot açık-ekosistem hızına örnek.
3. **"You Don't Need to Be an Artist":** Godot pixel ayarları (filter=Nearest; 640×360/320×180; stretch=canvas_items/keep/INTEGER). Sanat felsefesi: basit başla, Krita/Aseprite, karakter-dışı için fırça-boya, tileset opsiyonel, particle/shader ile "pop".

## ALT-SORULAR (FEASIBILITY/REUSE lensi)
1. **"Make Systems Not Games" → RIMA'nın olgun C# sistemleri** (combat, draft/reward, room/IsoRoomBuilder, knockdown, gate-slot, localization) engine-agnostic "reusable system library"ye çıkarılabilir mi? Hangileri taşınabilir (saf C# / mantık), hangileri Unity'ye kilitli (MonoBehaviour/SO/URP)? Kabaca efor.
2. Bu sistemler GODOT'a port edilseydi (C# Godot mu, GDScript'e re-yazım mı?) — gerçekçi maliyet? GDScript'e yeniden-yazım mı yoksa Godot-C# mi reuse'u korur?
3. Agent-pipeline pratikte: Unity-MCP iş akışımız (test-runner, execute_code, scene probe) Godot MCP'de ne kadar 1:1 karşılanır? Eksik kalan somut yetenek var mı?
4. Saf feasibility: "RIMA'yı bitir + gelecek oyunda Godot'u GDScript ile dene" mi, "Unity'de kal + sistem kütüphanesi çıkar" mı daha düşük-risk? Kısa gerekçe.

Çıktı: madde madde, net. BLOCKED yaz belirsizse.
