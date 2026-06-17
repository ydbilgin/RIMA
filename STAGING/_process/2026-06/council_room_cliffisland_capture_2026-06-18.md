# COUNCIL: Act-1 odalarını "cliff'li yüzen ada" olarak figür için nasıl yakalarız?

## ⛔ READ-ONLY — KESİN
Koda/git'e/sahneye/dosyaya DOKUNMA. **Unity'yi SÜRME** (execute_code/play/scene-load YOK — tek-Unity-ajan kuralı; eşzamanlı MCP köprüyü çökertir). Sadece **statik kod okuma + graphify query** ile araştır ve öneri yaz. `git add/commit` ASLA.

GRAPHIFY (ZORUNLU, query-first ~71× ucuz): mimari/çok-dosya sorularını ÖNCE graphify ile sor:
graph.json = `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json` → `explain <node>` / `path <A> <B>` / `query "..."`.

NLM gerekirse: NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

## Bağlam / problem
Akademik rapora "veri-güdümlü oda" görsel kanıtı gerekiyor. **Tasarım gerçeği (kullanıcı):** RIMA'nın TÜM odaları = **tile zemin + altta procedural AUTO-GENERATE edilen cliff → yüzen ada** (canonical: `_Arena` sahnesi böyle görünür: izometrik ada, void arka plan, kenar cliff'leri). Kullanıcı: "cliff'ler generate ile otomatik, kaldırılabilir/yeniden-üretilebilir".

İlk deneme YANLIŞTI: bir ajan odaları `RoomLoaderMenu.LoadRoomJsonToActiveScene` ile BARE scratch sahnede tilemap'e yükledi → **düz, moloz-zeminli, cliff'siz, çirkin** çıktı (oyunun gerçek hâli değil). 6 yanlış PNG: `STAGING/report/figures_2026-06-18/rooms/*.png`.

6 Act-1 odası: `Assets/Data/Map/Act1_ShatteredKeep/json/act1_*.json` (entry_hall/west_chamber/east_corridor/treasure_vault/north_antechamber/shattered_throne).

## Soru (kesin, kanıtlı yanıt — dosya:satır + graphify)
1. **Pipeline:** Bir oda JSON'u → "cliff'li yüzen ada" görünümüne hangi kod/akışla dönüşüyor? Entry-point(ler)? (aday düğümler: `IsoRoomBuilder`, `RuntimeRoomManager`, `RoomLoader`, `LiveRoomReloader`, `MapFragmentBridge`, `_Arena` sahne kurulumu). graphify ile bağ/çağrı zincirini çıkar.
2. **Auto-cliff-generate:** Cliff'leri otomatik üreten fonksiyon/komponent hangisi? Bir oda yüklendikten sonra bunu PROGRAMATİK (execute_code ile) tetiklemenin yolu var mı? Void arka plan + ışık + kamera çerçeveleme nereden geliyor (neden bare sahnede yoktu)?
3. **EN İYİ + GÜVENLİ capture yolu:** 6 odayı _Arena'nın gerçek ada-görünümünde yakalamak için kesin adımlar. Tercihen `_Arena`'yı KİRLETMEDEN (no-leak): ya _Arena'ya oda yükle→cliff-gen→capture→KAYDETME-revert, ya da _Arena setup'ını klonlayan geçici sahne, ya da play-mode + LiveRoomReloader (`StreamingAssets/live/room_current.json`). Hangisi en sağlam? Adım adım, riskleriyle.
4. **Alternatif/öneri:** "Düz schematic grid" (mevcut) RAPOR için yine de değerli mi (JSON→tilemap veri kanıtı), yoksa tamamen ada-görünümüne mi geçilmeli? İkisi birden (schematic + island) mı en iyi anlatı?

## ÇIKTI
Sentez kararı: `STAGING/_process/2026-06/COUNCIL_ROOM_CAPTURE_DECISION.md` — kanıtlı pipeline + auto-cliff-gen tetikleme + LOCKED capture adımları (no-leak) + figür-anlatı önerisi. Orchestrator bunu tek Unity-ajanına verip execute edecek.
