ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
`_IsoGame` sahnesini concept-oda art-yönüne çek: **KOYU granit zemin + mor void atmosferi + KALIN KOYU GRİ taş cliff (kahverengi YOK)**. Şu an floor SOLUK/beyazımsı, void bg yok, cliff ya yok ya kahverengi. Hedef: F5'te concept01/05/07 gibi koyu, derinlikli, yüzen iso ada.

# ÖNCE OKU/BAK (hedef + plan)
- **Build planı (ax/Gemini, ANA REHBER):** `STAGING/agy_build_decision_concept_realization.md` — 5 adımlık somut plan. UYGULA.
- **Art hedefi (concept görseller, bunlara bak/eşle):** `STAGING/imagegen/concept01_hero_room_ISO.png`, `concept05_portal_chest_ISO.png`, `concept07_boss_arena_ISO.png` — koyu slate granit + cyan derz çatlağı + kalın koyu taş cliff + mor void.

# Bağlam (KORU, BOZMA)
- `_IsoGame` F5'te çalışıyor: floor451 iso ada (useAuthoredSceneRoom flag) + Warblade oynanabilir (applyPrimaryOnStart). BUNLARI BOZMA.
- İpucu: sahnede `Global Light 2D` objesi **activeSelf=false** (kapalı), `Ambient Light 2D` açık → soluk floor sebebi büyük ihtimalle ışık/material.
- Depth-sort LOCK = Camera Custom-Axis (0,1,0). Entity sort'unu DEĞİŞTİRME, manuel YSort EKLEME. Sorting LAYER yapısı (bg/floor/cliff) custom-axis ile uyumlu — sadece katman yapısı kur.

# Görev — ÖNCELİK SIRASIYLA (çekirdek 1-3 ZORUNLU, 4-5 yapabilirsen)
1. **KOYU FLOOR (Adım 1):** `Floor`/`Ground` Tilemap Renderer Color → koyu (#60606B civarı); materyal `Sprite-Lit-Default` (URP 2D) olsun; `Global Light 2D`'yi AÇ, renk hafif mor-lacivert (#2A2840), Intensity 0.5-0.6. Ambient'i gerekiyorsa kıs. Sonuç: floor koyu granit, beyazımsı değil.
2. **KAHVERENGİ CLIFF → KOYU TAŞ (Adım 3):** Kahverengi `ref_kit_b`/KitB_Cliff cliff objelerini sahneden kaldır/disable. Yerine KOYU GRİ taş cliff: ya mevcut cliff sprite'larını Sprite Color #303035 ile koyulaştır, ya `DirectionalCliffTile` asset'ine koyu sprite ata. `RoomCliffSolver`/`CliffAutoPlacer` bunları ada dış kenarına dizsin (varsa). Asset SİLME, sahneden çıkar. Kahverengi kalmasın.
3. **MOR VOID BG (Adım 4):** Sahneye `Void_BG` adlı SpriteRenderer ekle, `KitC_BG`/`BgKit_RefC`'den mor void/fog dokusu koy. Sorting Layer en altta (Background, order -100). Kameraya child yap (sabit) VEYA hafif parallax. TEK katman (stack'leme).
4. **(İkincil) Cyan çatlak %5-8 (Adım 2):** floor451 cyan-damarlı varyantını ~%5-8 dağıt (RuleTile/random veya room-gen weight). Aşırıya kaçma.
5. **(İkincil) Bloom (Adım 5):** Global Volume + Bloom override (intensity ~1.5, threshold ~1.0) → cyan çatlaklar parlasın. Riskliyse atla, BLOCKED yaz.

# Doğrulama (ZORUNLU — kanıtsız done deme)
- Unity refresh → `read_console` 0 derleme hatası.
- `_IsoGame` scene-view + F5 game-view screenshot (manage_camera) → floor KOYU granit, mor void bg görünür, cliff'ler KOYU gri (kahverengi YOK), Warblade + iso ada duruyor. Screenshot yollarını CODEX_DONE'a yaz. concept01 ile kıyasla.
- Regression: useAuthoredSceneRoom + Warblade + custom-axis sort bozulmadı mı?
- execute_code'da `using` YASAK (tam-nitelikli namespace). AssetDatabase batch wrapper.

# Çıktı (CODEX_DONE_<profil>.md)
- Soluk floor kök-neden + her adımda ne yapıldı (1-5 hangileri).
- Değiştirilen dosya/obje/asset listesi + satır sayıları.
- 2 screenshot yolu + derleme + regression.
- İkincil adımlar atlandıysa neden. Belirsizlik → BLOCKED.
