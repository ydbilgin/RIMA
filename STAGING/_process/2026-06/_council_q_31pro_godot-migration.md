# Council danışma — DERİN MİMARİ + WEB-DOĞRULAMA lensi (Gemini 3.1 Pro High)

Sen kıdemli oyun-teknoloji mimarisi danışmanısın. **Web erişimin varsa KULLAN** — aşağıdaki Godot MCP verisini 2026 itibarıyla DOĞRULA/güncelle (yanlışsa düzelt, eksikse ekle, URL ver). Çıktını SADECE bu cevap metni olarak ver — DOSYA YAZMA, dosya yazdım deme. Bilmediğin şeyi uydurma; "doğrulanamadı" de.

## BAĞLAM (orchestrator çekti — web-doğrulanmış + transcript'ler)

**RIMA = Unity** (URP 2D, C#, Pixel Perfect Cam, 640×360, 549 test) + **Unity-MCP çok-ajan pipeline** (Claude+Codex+Gemini, ~39+ tool, C# native, çok olgun). Bitirme demosu. **RIMA Unity'de KALACAK; soru GELECEK oyunlar.**

**Godot MCP güncel (doğrula):** godot-ai (~39 tool/120+ ops, GDScript-only, Godot 4.3+, 503★/66 release/v2.6.1 2026-06-03, Claude Code 1-satır kurulum) · Coding-Solo/godot-mcp (4.1k★, JS+GDScript, editor/run/debug/scene/node) · StraySpark (ticari, 131 tool, production-safe) · GDAI alt. Dil: GDScript tam, C# daha az olgun. Olgunluk: Godot-MCP 2025, Unreal-MCP 2024. Gotcha'lar: auto-reload, GDScript dinamik-tip→başarısız call, SceneTree/dosya ıraksaması.

**Godot vs Unity 2D (2026):** Godot amaca-özel 2D + native pixel-perfect + ücretsiz/MIT + 150MB/anında + recompile-yok. Unity: asset store + olgun URP 2D lighting + Spine + console-export + $200k royalty.

**3 video (transcript):** (1) "Make Systems Not Games" = dream-game'i parçalama, ayrı projelerde reusable SİSTEM yap, refactor-bırakma riski, gereksinime göre tasarla. (2) "Yūgen Terrain Toolkit" = Godot 3D-pixel-art terrain (Marching Squares, MIT) — gelecek-oyun stili. (3) "You Don't Need to Be an Artist" = Godot pixel ayarları (Nearest/640×360/integer-scale) + sanat felsefesi (basit başla, Krita, tileset opsiyonel, shader/particle ile pop).

## ALT-SORULAR (DERİN + WEB)
1. **[ÖNCELİK] Godot MCP "kötü/zayıf" demek doğru mu?** Yukarıdaki veriyi web'le doğrula. Unity MCP'ye kıyasla gerçek olgunluk farkı ne? Agent-pipeline gücü (test-runner, execute_code/eval, scene introspection, screenshot) ne kadar korunur? Kullanıcının "Godot daha az karışık" sezgisi (Godot+GDScript+MCP < Unity+C#+UnityMCP karmaşıklığı) HAKLI mı?
2. **C# vs GDScript stratejik kararı:** MCP en çok GDScript'te güçlü. GDScript öğrenmek pipeline'ı maksimize eder mi? Yoksa Godot-C# ile RIMA reuse'u korumak mı? Hangisi 2-3 yıllık ufukta daha sağlam?
3. **Geçiş riski:** Kullanıcının asıl varlığı motor DEĞİL, agent-orkestra. Bu varlık Godot'a ne kadar taşınır? Geçişin gizli maliyetleri (öğrenme + pipeline yeniden-kurma + ekosistem)?
4. **"Make Systems Not Games" + engine-agnostik tasarım:** RIMA sistemlerini reusable kütüphaneye çıkarmak, motor kararını ertelemenin/sigortalamanın yolu olabilir mi? Mimari öneri.
5. **3 videodan RIMA'ya ŞİMDİ + gelecek oyunlara somut, eyleme-dönük dersler.**

Net, kanıtlı, URL'li (web kullandıysan). Önerini gerekçelendir.
