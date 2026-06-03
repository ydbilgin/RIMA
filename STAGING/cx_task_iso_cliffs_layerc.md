ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
`_IsoGame` sahnesinde iso yüzen-ada kompozisyonunu TAMAMLA: (a) **KAHVERENGİ cliff/edge şeylerini KALDIR**, (b) floor island'ın **altına temiz KOYU (dark-iron) cliff'ler** gelsin (extrude/karart yöntemi — kahverengi taşan sprite DEĞİL), (c) **TEK bir layer-C arka plan** ile derinlik ekle. Sonuç: F5'te ve scene-view'da koyu, net, derinlikli iso yüzen ada — kahverengi YOK.

# Önce OKU (bağlam)
- `STAGING/agy_video_analysis_iso_tilesets.md` — CLIFF ÇÖZÜMÜ buradan: AI'dan sadece düz üst-elmas üret → cliff'i Unity'de aşağı EXTRUDE + dark iron'a (#1A1E24) karart. AI'a dikey kahverengi duvar çizdirme.
- `STAGING/MODULAR_PIPELINE_MASTER.md` — genel modüler pipeline planı (7 katman, cyan ayrı decal).
- Cliff scriptleri: `CliffAutoPlacer`, `DirectionalCliffTile`, `Assets/Scripts/Editor/MapDesigner/CliffGenerateAction.cs`, `RoomCliffSolver`.

# Mevcut durum (Opus gözlemi)
- `_IsoGame` F5'te floor451_0 iso ada korunuyor (560 hücre, useAuthoredSceneRoom flag — BOZMA). Önceki cx ayrıca Warblade'i Player'a koydu (BOZMA).
- Scene-view'da granit elmas ada'nın KENARLARINDA koyu KAHVERENGİ bloklar var = istenmeyen. Aday kaynak: import edilen `Assets/Sprites/Environment/CliffKit_RefB/` (128×192 kahverengi cliff sprite'ları — hem kahverengi hem 1-hücre floor'u TAŞIRIYOR) ve/veya KitB_Cliff tilemap/objeler. ÖNCE bunları sahnede bul ve doğrula.

# Görev
1. **Kahverengi kaldır:** `_IsoGame`'de floor kenarındaki kahverengi cliff/edge objelerini/tilemap'lerini bul (ref_kit_b / KitB_Cliff / DirectionalCliff brown). Sahneden kaldır/disable et (asset'i SİLME, sadece sahneden çıkar). Floor451 ada'ya DOKUNMA.
2. **Temiz koyu cliff (altına):** Floor island'ın DIŞ kenarlarının altına koyu dark-iron cliff bandı ekle. YAKLAŞIM SEÇ (en temiz + iyi görüneni):
   a. **Tercih (extrude):** Dış kenar floor hücrelerini bir "Cliff" tilemap'e/objeye kopyala, aşağı (~2-3 hücre derinlik) uzat, rengi dark-iron `#1A1E24`'e doğru karart (tint/material/shader). Üstten alta hafif gradient. Kahverengi YOK.
   b. Eğer extrude karmaşıksa: mevcut koyu (kahverengi-olmayan) cliff sprite varsa onu kullan; yoksa basit karartılmış edge-tile bandı.
3. **Tek layer-C derinlik:** Ada'nın ARKASINA/ALTINA TEK bir arka plan katmanı koy (void/fog) — derinlik hissi için. Kaynak: `Assets/Sprites/Environment/KitC_BG/` veya `BgKit_RefC` (L0_void..L4_fog). SADECE BİR mantıklı katman (hepsini stack'leme). Sorting: floor'un arkasında.
4. **Renk/ton:** on-brand slate/iron koyu palet + void-purple; cyan #00FFCC sadece ölçülü aksan (zaten floor'da var). Kahverengi tamamen gitsin.
5. KIRMA: useAuthoredSceneRoom fix, Warblade, floor451 ada, diğer sahneler.

# Doğrulama (ZORUNLU — kanıtsız done deme)
- Unity refresh → `read_console` 0 derleme hatası.
- `_IsoGame` scene-view screenshot + F5 game-view screenshot → KAHVERENGİ YOK, koyu cliff'ler ada'nın altında, layer-C derinlik görünür, floor451 + Warblade duruyor. Screenshot yollarını CODEX_DONE'a yaz.
- Sorting/derinlik doğru mu (bg arkada, cliff floor altında, karakter üstte) kontrol et. Sort axis LOCK = Camera Custom-Axis (0,1,0) — değiştirme.

# Çıktı (CODEX_DONE_<profil>.md)
- Kahverengi neydi + nasıl kaldırıldı.
- Cliff yaklaşımı (a/b) + layer-C kaynağı + değiştirilen dosya/obje listesi.
- 2 screenshot yolu + derleme + regression.
- Belirsizlik/risk → BLOCKED yaz.
