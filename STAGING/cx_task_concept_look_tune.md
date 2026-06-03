ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
Önceki concept-look build'ini TUNE et. Şu an `_IsoGame` 3 sorunu var (Opus screenshot review): (1) ADA AŞIRI KOYU/siyah — granit dokusu okunmuyor; (2) F5'te Warblade'in sağ-üstünde KIRMIZI KARE (bug); (3) cyan çatlaklar görünmüyor/parlamıyor. Hedef: concept01_hero_room_ISO.png gibi ORTA-KOYU slate (siyah değil, beyaz değil) + görünür granit + ölçülü parlayan cyan.

# Mevcut durum (KORU)
- Önceki build: Ground tint #444450, Global Light2D #2A2840 int 0.55, Ambient #53506A int 0.32, CliffTilemap #3C3C42 (44 tile), floor451_2 cyan 28/560, Void_BG, Bloom (1.5/1.0). useAuthoredSceneRoom + Warblade + custom axis (0,1,0) — BOZMA.
- Screenshot kanıt: `Assets/Screenshots/rima_iso_concept_f5_yasinderyabilgin_final.png` (kırmızı kare burada görünüyor) + `..._scene_...final.png` (aşırı koyu).
- Hedef art: `STAGING/imagegen/concept01_hero_room_ISO.png` (orta-koyu slate granit + cyan derz).

# Görev
1. **DAHA AÇIK (siyahlıktan kurtar):** Ground Tilemap tint'i #444450 → ~**#6E6E7A** (orta-koyu slate) yap; Global Light2D intensity 0.55 → **~0.85**; Ambient intensity 0.32 → ~0.45. AMAÇ: granit doku NET okunsun ama on-brand koyu kalsın (concept01 referans — siyah DEĞİL). Gerekirse değerleri concept'e bakıp ince ayarla.
2. **KIRMIZI KARE FIX:** F5'te Warblade'in sağ-üstündeki kırmızı hücreyi bul. Muhtemel: (a) bir floor tile'ın sprite'ı null/missing → kırmızı/magenta placeholder, ya da (b) stray enemy/obje/marker. Kök-nedeni bul. Eğer missing-sprite tile ise doğru floor451 sprite'ı ata; eğer stray obje ise kaldır/disable. F5'te kırmızı kare KALMAYACAK.
3. **CYAN GÖRÜNÜR + PARLASIN:** Kullanılan floor451_2 GERÇEKTEN cyan-damarlı varyant mı doğrula (değilse cyan olan varyantı seç). Cyan derzlerin bloom ile hafif parlaması için: cyan tile rengini HDR #00FFCC (intensity ~2) yap VEYA cyan tile'ların olduğu yere ufak Point Light 2D (#00FFCC). %5 oranını koru.
4. Bloom threshold'u gerekiyorsa düşür (sadece cyan yakalansın, tüm sahne bloom olmasın).

# Doğrulama (ZORUNLU)
- Unity refresh → `read_console` 0 hata.
- F5 game-view + scene-view screenshot → ada ORTA-KOYU granit (siyah değil), doku okunur, cyan derzler görünür/parlıyor, KIRMIZI KARE YOK, Warblade + cliff + void bg duruyor. Screenshot yollarını + concept01 kıyasını CODEX_DONE'a yaz.
- Regression: useAuthoredSceneRoom, applyPrimaryOnStart/Warblade, custom axis (0,1,0), 560 floor / 44 cliff korunuyor mu?

# Çıktı (CODEX_DONE_<profil>.md)
- Kırmızı kare kök-neden + fix.
- Yeni ışık/tint değerleri.
- cyan görünürlük çözümü.
- 2 screenshot yolu + derleme + regression. Belirsizlik → BLOCKED.
