# EXEC 3 — Cliff-Under-Floor Math (CX) + EditMode tests

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only listed files (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory.

## Amaç
Cliff'ler floor'un kenarına MATEMATİKSEL olarak doğru otursun (ada havada yüzüyor hissi). cx kendi analizinde ANA GAP'i buldu: `DirectionalCliffTile_Hades` 8 yönü de AYNI sprite'a bağlamış + transformOffset=0. KitB_Cliff'te 8 doğru yön sprite'ı VAR (128×192, PPU64, top-pivot 0.5,1) ama bağlı değil.

Çıktıyı `CODEX_DONE.md` → `## EXEC3 CLIFF MATH` başlığına yaz.

## Opus LOCK (DEMO_MAP_PLAN_OPUS_LOCK_S6.md'den)
- Cliff floor-cell ÜZERİNE konur, top-pivot aşağı sarkar. Sorting = Ground/-10 (RoomDepthStack, doğru).
- 8-yön bitmask: void-facing N=1,E=2,S=4,W=8,NE=16,NW=32,SE=64,SW=128 → S=cliff_S, SE=cliff_SE, SW=cliff_SW, E=cliff_E, W=cliff_W (kamera-önü; N/NE/NW solver zaten kesiyor).
- Offset: PPU64, floor 64px=1u, cliff 192px=3u. Top-pivot floor-cell merkezine → görsel lip ~0.5u aşağı (ax `offset.y≈1.5` floor seam'i için). DOĞRU değeri sen hesapla + raporla.

## YAPILACAKLAR

### Adım 1 — DirectionalCliffTile 8-yön bağla (ANA FIX — çoğunlukla .asset wiring)
**ÖNEMLİ (Opus kod-okuması):** `DirectionalCliffTile.cs` KODU ZATEN DOĞRU. Komşu-floor mantığıyla yön seçiyor (cliff cell'in N komşusu floor ise spritesS, NW→spritesSE, vb. :88-106), 8 ayrı `spritesX[]` array'i + transformOffset + heightVariation hang mantığı var. **KOD DEĞİŞTİRME (gerekmedikçe).** Gap = DATA: `DirectionalCliffTile_Hades.asset`'in 8 array'i boş/aynı sprite'a bağlı.
- **ANA İŞ = `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`'i düzelt:** 8 yön array'ini KitB_Cliff sprite'larına bağla. Mantık eşlemesi (DirectionalCliffTile.cs:99-106'ya GÖRE — dikkat, sprite array adı yön-İSMİYLE ters çünkü "floor şu yönde→cliff o yöne bakar"):
  - `spritesS` ← cliff_S.png (hasN durumu: kuzeyde floor var → güneye bakan cliff)
  - `spritesSE` ← cliff_SE.png (hasNW)
  - `spritesSW` ← cliff_SW.png (hasNE)
  - `spritesE` ← cliff_E.png (hasW)
  - `spritesW` ← cliff_W.png (hasE)
  - `spritesNE` ← cliff_NE.png (hasSW)
  - `spritesNW` ← cliff_NW.png (hasSE)
  - `spritesN` ← cliff_N.png (hasS)
  - GUID'leri `Assets/Sprites/Environment/KitB_Cliff/cliff_*.png.meta`'dan al. Her array'e en az o yönün sprite'ını koy (varyasyon için cliff_cyan_glow/cliff_S_new EKLEME — PPU uyumsuz; sade tut).
- transformOffset normalize: top-pivot cliff (128×192, PPU64, pivot 0.5,1) floor-cell merkezine konunca üst kenar merkeze gelir. Floor seam'ine doğru oturması için Y offset hesapla + `.asset`'e yaz. `CliffTile_Hades.asset` eski offset.y=1.21875 referans — iso-grid (cellSize 0.94) + PPU64 ile doğru değeri TÜRET, raporla. heightVariation maxLift zaten ÜSTE-doğru lift yapıyor (gap yok), onu bozma.
- Eğer .asset'i metin olarak düzeltmek riskliyse (YAML GUID), BLOCKED işaretle → Opus MCP ile Unity'de Inspector'dan bağlar.

### Adım 2 — RoomCliffSolver cluster-parity (cx CX REVIEW #5)
- `Assets/Scripts/RoomPainter/RoomCliffSolver.cs` — orijinal `CliffAutoPlacer.CollectCliffCells`'teki orphan-cluster filtresini PORT etmemiş. İki yol (sen karar ver, raporla):
  (a) cluster-filter'ı RoomCliffSolver'a port et (paramlı), VEYA
  (b) RoomCliffSolver'ı tek-kaynak ilan et, eski cluster-bağımlı parity iddiasını kaldır (yorum güncelle).
- Opus tercihi: (b) basit + tek-kaynak (RoomCliffSolver canonical). Ama küçük odalarda izole cluster sorun çıkarıyorsa (a). Sen değerlendir.

### Adım 3 — EditMode test
- `Assets/Tests/EditMode/...` altında cliff math testi ekle/genişlet (mevcut `UnifiedDesignerTests.cs` pattern'ini izle):
  - Bilinen floor şeklinde RoomCliffSolver.Solve → beklenen cliff hücreleri (S/SE/SW kenar, N kesik).
  - Bitmask → doğru cliff yön sprite seçimi (DirectionalCliffTile mantığı test edilebilirse).
- 363 mevcut test KIRILMAMALI.

## DoD
1. DirectionalCliffTile_Hades 8 yön KitB_Cliff'e bağlandı + offset normalize.
2. RoomCliffSolver cluster kararı uygulandı (a veya b, gerekçeli).
3. EditMode test eklendi, mevcut testler geçer.
4. Compile-clean (Unity AÇIK — build deneme, Opus doğrular).
5. Değişen dosyalar + offset hesabı + BLOCKED.
