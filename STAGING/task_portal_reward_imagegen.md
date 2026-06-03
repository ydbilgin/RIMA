# TASK: Portal + reward pixel-art varlıkları ($imagegen ile)

ACTIVE RULES: (1) think before drawing (2) min asset, no speculation (3) surgical — sadece listelenen sprite'lar + manifest (4) BLOCKED yaz eğer $imagegen yoksa.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: bu task + STAGING.

Amaç: RIMA'nın katmanlı izometrik sahnesi için EKSİK iki görsel varlığı PIXEL-ART olarak üret: (1) portal "rift" sprite + tip rune ikonları, (2) oda-sonu ödül görseli. Bunlar PixelLab varlıklarının (koyu granit floor + slate cliff) YANINDA duracak → AYNI pixel-art dil + PPU64-uyumlu boyut ZORUNLU. Painterly/HD/realistik DEĞİL.

## TOOL
- `$imagegen` skill'ini kullan (senin yerleşik image-generation yeteneğin; generate2dsprite iş akışı geçerli). Solid background (magenta/yeşil) üstüne çiz → chroma-key ile transparan yap → kırp/boyutla.

## STYLE LOCK (hepsi için)
- PIXEL ART, RIMA on-brand: koyu slate/granit taş + demir + cyan #00FFCC "seal energy" (cyan AZ ve vurgu olarak). Mor void aksanı OK.
- Realism YOK, painterly YOK, pixel dithering dışında yumuşak gradyan YOK.
- Çıktı: transparent PNG. PPU 64 mantığı (1 dünya birimi = 64px).

## DELIVERABLE 1 — PORTAL
- `portal_rift.png` — ~128×128 px. Radyal-simetrik (YÖNSÜZ) dikey badem/oval yarık; etrafında kırık slate taş halka/çerçeve; merkezde cyan girdap enerji. (Unity'de 8 yöne döndürülmeyecek; tek yönsüz sprite.)
- Tip rune ikonları (AYRI dosyalar, ~48×48 veya 64×64, cyan line-art, transparent) — RoomType başına:
  - `rune_combat.png` (çapraz kılıçlar)
  - `rune_elite.png` (boynuzlu kafatası)
  - `rune_chest.png` (kupa/chalice)
  - `rune_boss.png` (büyük kafatası / taç)
  - `rune_rest.png` (ocak/alev)
  - `rune_event.png` (göz / soru işareti)
- Portal animasyonu YOK — tek still kare. (Swirl animasyonu sonra.)

## DELIVERABLE 2 — REWARD
- `reward_relic.png` — ~96×96 veya 128×128 px. Oda temizlenince beliren ödül görüntüsü: cyan parlayan rün-relik / kristal, küçük slate kaide üstünde havada asılı, toplanabilir pickup hissi. Transparent.
- Opsiyonel `reward_relic_glow.png` — daha parlak hover/seçili varyant.

## OUTPUT
- Hepsi → `STAGING/imagegen/portal_reward/` klasörüne transparent PNG.
- `STAGING/imagegen/portal_reward/MANIFEST.md` yaz: her dosya için amaç + px boyut + kullanım (portal / rune / reward) + PPU64 import notu.
- Assets/'e KOPYALAMA, Unity import YAPMA. Orchestrator (Unity bağlı) QC edip doğru import ayarlarıyla (PPU64, point filter, transparent) içeri alacak.

## SUCCESS CRITERIA
- En az: `portal_rift` + 4 rune (combat/elite/chest/boss) + `reward_relic`. Hepsi transparent, doğru boyut, pixel-art, on-brand.
- `$imagegen` yoksa/başarısızsa: çıktıya **BLOCKED** yaz + neyin eksik olduğunu açıkla (orchestrator ax→agy/Imagen fallback yapar).
