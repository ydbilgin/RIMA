ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Modüler-props kararı adım 1 [S]: prop mirror desteği — `flipX` ile tek asset'ten iki görünüm (council kararı `STAGING/MODULAR_PROPS_DECISION_2026-06-05.md` K1/K4; 3/3 oybirliği).

# İş (cerrahi, küçük)
1. `PropPlacementData`'ya (RoomTemplateSO'daki prop yerleşim kaydı — bul: `Assets/Scripts/MapDesigner/Room/`) `public bool flipX;` ekle.
2. `IsoRoomBuilder.BuildProps` (~L542): prop instantiate edilirken `spriteRenderer.flipX = placement.flipX` uygula. Collider/footprint MANTIĞINA DOKUNMA (simetrik footprint varsayımı yeterli; asimetrik footprint'li prop'larda flipX'i auto-placer atlamalı — aşağıda).
3. `BridsonPoissonAutoPlacer` (`Assets/Scripts/MapDesigner/Props/Auto/`): yerleştirme sırasında %50 şansla `flipX=true` (deterministik — mevcut seed/random akışını kullan, yeni RNG yaratma). Footprint'i kare/simetrik OLMAYAN prop'larda flipX atlanır (footprint width==height check ya da PropDefinitionSO'ya `allowMirror` default-true alanı — hangisi daha az kod ise onu seç, gerekçele).
4. EditMode test (1 dosya, 2 test): flipX serialize round-trip + auto-placer deterministik-seed'de aynı flipX dizilimi.

# Doğrulama
- `dotnet build RIMA.Runtime.csproj` PASS + read_console 0 error + testler yeşil (çalıştır: Unity Test Runner MCP).
- CODEX_DONE.md: değişen dosya+satırlar, allowMirror/width-height seçimi gerekçesi, test sonucu.

# YASAK
.unity düzenleme · RoomTemplateSO asset'lerini elle değiştirme · IsoRoomBuilder'da başka refactor.
