ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
RESPOND INLINE — dosya yazma.

# Amaç (FİKİR + FİZİBİLİTE)
RIMA geliştirme/demo iş akışını kolaylaştıracak işlevsel UI/UX tool fikirleri üret. Kullanıcı tek kişilik dev; oda boyama + live-edit + demo playtest döngüsünü hızlandıran araçlar istiyor. Sadece fikir + fizibilite — KOD YAZMA.

# Bağlam
- Mevcut: RimaRoomPainterWindow (4-mode painter, L1-L6 layer filter, collider drag-handle, cliff brush), LiveToolPaletteWindow (Editor palette), LiveRoomReloader (file-watch reload, %58 kurulu), RoomLoader (5-oda demo loop), CombatJuice, RuntimeAssetRegistryBaker.
- Pipeline: Unity Editor + MCP + PixelLab + cx/agy dispatch.
- Hedef: oynanabilir demo (combat verified). "Sadece animasyon kalsın" — sistem/tool tarafı bu gece kurulsun.

# Sorular
1. Bu dev iş akışını HIZLANDIRACAK en değerli 6-8 tool/convenience fikri ne? (örn: one-click test-room launcher, asset-pool inspector/health-check, brush preset save/load, room snapshot/diff, encounter quick-edit, palette-lock checker, mob-spawn previsualizer, "play-from-here" room jump, hotkey overlay). Her fikir: NE çözer + kim kullanır.
2. Her fikir için FİZİBİLİTE: efor (XS/S/M/L), Unity Editor-only mu yoksa live-tool mu, mevcut koda eklenir mi yoksa yeni mi, risk.
3. EN YÜKSEK değer/düşük efor 2-3 tool hangisi (gece-yapılabilir, animasyon-bağımsız)?
4. Saçmalık tespiti: önerdiğin fikirlerden over-engineered / düşük-ROI / mevcut araçla çakışan var mı?

# Çıktı
Numaralı fikir listesi (NE çözer + efor + nerede) + sonda "GECE YAPILACAK TOP 3 (değer/efor)".
