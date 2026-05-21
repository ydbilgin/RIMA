# Antigravity Prompt — Rift Threshold Original Design Brainstorm

**Kullanım:** Aşağıdaki code-fenced bloğu Antigravity'e yapıştır. Project intro YOK (UnityMCP zaten okuyor).

---

```
RIMA dungeon room threshold (oda-arası geçiş) için orijinal görsel kurgu istiyorum.
"Hades-style framed door" clone'u DEĞİL — daha özgün, lore-driven, 8 yön uygulanabilir.

KONTEKST (kısa):
- RIMA = 35° isometric ARPG, 2D pixel art (Hades + Diablo karışımı)
- Lore signature: "Echo Imprint Cascade" — die, room remembers, each death writes the arena
- Visual signature: cyan rift (floor cracks, wall accents, energy)
- Karakterler 8-yön hareket eder (sprite + flipX = 8 view coverage)
- Renderer: URP 2D + Pixel Perfect Camera, PPU=64, sprite 128×128 standard
- Production budget kıt: PixelLab 1 gen = 20-40 credit, mümkün olduğunca minimal sprite çeşidi

ROOM TYPE TAKSONOMİSİ (oda başına farklı threshold visual istiyoruz):
1. Combat — düz savaş arenası, threshold "açık geçit"
2. Elite — challenge encounter, threshold "ağır kapı / işaret"
3. Boss — final showdown, threshold "ritualistic / kapalı / sembol"
4. Chest — loot room, threshold "ödül habercisi / parlak"
5. Merchant — NPC trade, threshold "dost / işaretli / işlek"
6. Forge — upgrade station, threshold "endüstriyel / metal / kor ateş"
7. Event — narrative encounter, threshold "gizemli / cazip"
8. Curse — risk room, threshold "uyarı / kanlı / kararmış"
9. Corridor — basit geçiş, threshold "minimum / sıradan"

DESIGN PROBLEMS TO SOLVE:
A) Stone arch + cyan portal = Hades clone hissi → orijinal alternatif gerek
B) Sprite 1-yönlü olursa player farklı yönden gelirken yanlış görünür → 8-dir uygulanabilirlik
   (idealde direction-invariant veya minimal flipX coverage)
C) Room type başına UNIQUE visual ama TUTARLI lore (cyan rift signature korunsun)
D) Production minimal: 1 base form × room type variant (palette/symbol swap) > 9 unique sprite

İSTEDİĞİM ÇIKTI:
1. 3-5 orijinal threshold KONSEPT (sketch + 100-200 kelime açıklama her biri):
   - Form (geometri/strüktür/silüet)
   - Lore framing (neden böyle görünüyor)
   - Room type adaptation stratejisi (palette swap mı, symbol swap mı, geometry swap mı)
   - 8-dir uygulanabilirlik (sprite kaç yönlü, flipX yeterli mi)
   - Production maliyet tahmini (1 form × 9 variant = X gen)

2. HER konsept için Hades'ten farkı net belirtilmeli:
   - "Hades'te X'tir, biz Y yapıyoruz çünkü Z"
   - Diablo/Hyper Light/Bastion gibi diğer ARPG referanslarından alıntı OK

3. Final tavsiye: bu 3-5'in hangisi RIMA için en güçlü? Neden?

4. Bonus: 8-dir applicability için pragmatic engineering öneri
   (sprite shader trick, billboard, depth-aware overlay, vs.)

OUTPUT FORMAT: Markdown, her konsept ayrı section. Code/diagram OK. Sahnede MCP ile prefab kurma YOK — sadece KONSEPT/TASARIM.

UnityMCP'ye sahne dosyalarına erişimin var — STAGING/concepts/rift_threshold_*.png'lere bak (Codex'in mevcut çıktısı = stone arch + cyan rift, ki bu Hades clone problemi). Mevcut çıktıyı eleştir + alternatif sun.
```

---

## Antigravity Çıktısını Aldıktan Sonra

Output'u şuraya yaz/yapıştır: `STAGING/ANTIGRAVITY_DONE_door_brainstorm.md`

Sonra:
1. Orchestrator (ben) Codex çıktısı ile karşılaştırır
2. 1-2 konsepti seçeriz
3. PixelLab/imagegen ile prototip gen
