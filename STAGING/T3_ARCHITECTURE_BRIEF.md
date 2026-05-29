ACTIVE RULES: (1) think before answering (2) concrete/min-fluff (3) scope: T3 tool architecture only (4) flag uncertain.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read: STAGING/T3_TOOL_FULL_DESIGN.md, STAGING/LIVE_EDITOR_GAP_S114.md, Assets/Scripts/Live/*, Assets/Editor/RoomPainter/LiveTool/*. RESPOND INLINE.

# Amaç
RIMA Live Editor "T3" = bağımsız Tool.exe (UI Toolkit runtime) + Game.exe (LiveRoomReloader file-watch) + Editor. Şu an T2-hibrit (Editor IMGUI palette + Game file-watch). 7/12 bileşen çalışıyor; eksik: ToolBootstrap(C5), ToolMain.unity sahne, ToolMain.uxml/.uss, RuntimeCliffHoverIndicator(C8), BrushExecutorRouter, LiveToolBuildProcessor (RIMA_LIVE_TOOL define). C6/C7/C9 (palette/handles/loader) Editor namespace'inde → Player build'a giremez, port gerek. Detay: LIVE_EDITOR_GAP_S114.md.

# SORU (concrete, T3 mimarisi)
1. **IMGUI → UI Toolkit runtime port:** C6 RuntimeBrushPalette / C7 RuntimeColliderHandles / C9 RuntimeAssetLoader Editor API'ye bağlı (Handles.BeginGUI, AssetDatabase). Player-build-safe runtime'a nasıl port edilir? Hangisi yeniden yazılmalı, hangisi soyutlanıp paylaşılabilir (interface + 2 impl: Editor + Runtime)?
2. **Build config:** Tek proje, 2 build target (Tool.exe + Game.exe). RIMA_LIVE_TOOL scripting define injection + IPreprocessBuildWithReport yaklaşımı doğru mu? Sahne/asset ayrımı (ToolMain.unity sadece Tool'da) nasıl?
3. **Runtime asset erişimi:** Tool.exe'de AssetDatabase yok. RuntimeAssetRegistry (baked SO, Resources.Load) yeterli mi? Paint sırasında yeni asset eklenince nasıl?
4. **En yüksek-risk 3 nokta** ve mitigasyonu. Editor-hibrit'i kırmadan T3'e geçişin güvenli sırası.

# İSTENEN: kısa, madde-madde, impl-yönlendirici. Tam kod değil — mimari kararlar + risk.
