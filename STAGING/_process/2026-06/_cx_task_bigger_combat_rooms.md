ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Kullanıcı: "demo combat odası ÇOK KÜÇÜK, gerçek BÜYÜK odalar olmalı." Demo'da görünen oda = DemoRoomBank'in seçtiği combat template (runSeed 12345 → index 0 = Combat_Large_01; _Arena.fallbackTemplate da Combat_Large_01). Bu odayı belirgin şekilde BÜYÜT. Floating-island doktrini + walkable enforcement + completability KORUNMALI.

# Hedef boyut (ChatGPT design-pack + combat okunabilirlik)
- Combat walkable: ~20×14 (kabul aralığı 18×12 – 22×14). Dash lane + kite alanı + projectile okuması versin.
- Elite: combat'a yakın veya biraz büyük.
- Boss: ~28×16 (aralık 26×16 – 30×18).
(Bunlar walkable hücre boyutu; etrafında void/cliff kenarı korunur = ada hissi.)

# YÖNTEM — LEAN ÖNCE (reuse > resize)
1. **AUDIT:** DemoRoomBank'teki combat/elite/boss template'lerinin + tüm `Assets/Data/Rooms/**` combat template'lerinin walkable boyutlarını (bounds + walkable hücre sayısı) çıkar (file:line). Mevcut "combatlarge_*" (organic_blob, twin_basins vb.) zaten hedefe yakın/büyük mü?
2. **EĞER yeterince büyük bir template ZATEN VARSA:** en uygun büyük combat template'ini demo'ya bağla — `Assets/Scenes/_Arena.unity` fallbackTemplate'ini ve/veya `DemoRoomBank.asset` combatRooms[0]'ı o template'e çevir. (En ucuz çözüm; resize yapma.)
3. **EĞER yoksa / hepsi küçükse:** demo combat template'ini (Combat_Large_01 veya yeni bir DemoCombat_Large) hedef boyuta BÜYÜT — walkable grid'i genişlet, spawn marker(lar)ı walkable içinde + kenardan uzak tut, NW/N/NE exit soketleri arka kenarda geçerli kalsın, void/cliff kenarı (ada hissi) korunsun. Resize'ı .asset YAML elle düzenleyerek DEĞİL, güvenli bir editor utility / mevcut Rooms aracıyla yap; riskliyse BLOCKED yaz + öner.

# KORU / DOĞRULA
- WalkabilityMap.InitFromTemplate ile uyum (görsel floor == walkable invariant).
- RoomCompletionInvariantTests + WalkableEnforcementTests + gate-slot/round-trip testleri YEŞİL kalsın.
- Camera fit (RoomRunDirector.FitCameraToRoom + cameraPadding) büyük odada düzgün çerçevelesin; gerekirse cameraPadding'i kontrol et (büyütme amacını bozma).

# ÇIKTI (CODEX_DONE.md)
- AUDIT tablosu: her combat/elite/boss template'in MEVCUT walkable boyutu.
- Seçilen yol (reuse mu resize mı) + gerekçe.
- Değişen dosyalar (file:line) + demo'nun artık kullandığı template'in YENİ boyutu.
- Test sonuçları. Mümkünse _Arena'da Play→combat odası screenshot'ı (`Assets/Screenshots/`).
- BLOCKED yaz belirsiz/riskliyse. Commit ETME. Untracked dosyalara dokunma. Legacy (RoomLoader/Gate/GateBehavior/DoorTrigger/RuntimeRoomManager) DOKUNMA.
