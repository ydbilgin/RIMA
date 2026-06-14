ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

RESPOND INLINE — do NOT write files. Return your design as reply text.

# Amaç (DÜŞÜN, ÜRETME)
RIMA'nın "duvarsız havada-asılı tile + altında cliff + void" ambiyansı için AYDINLATMA METODU tasarla. Bu bir TASARIM görevi — sprite/asset ÜRETME, sadece metod + Unity Light2D config reçetesi + üretilebilirlik notu.

# Bağlam (LOCKED canon)
- Görsel: wall-less floating-island Hades Elysium. Zemin void üstünde asılı ada, cliff kenarları karanlığa düşer. Sınır = cliff + sparse column + cyan rune circle + brazier.
- Palet: slate #3A3D42 base / cyan rift #00FFCC accent / warm orange #E89020 secondary / deep purple #3A1A4A bg depth.
- Kamera: High Top-Down 3/4 ~70-80°, PixelPerfect 640×360, assetsPPU 64, pixelSnapping OFF (jitter önler).
- Render: URP 2D Renderer + Light2D + 6-layer sorting (L1 Floor / L2 Cliff base / L3 Cliff face decor / L4 walkable decor / L5 wall blocker / L6 gameplay; Decor_Cliff=12, Decor_Floor=13).
- Pixel-art kuralı: dinamik 2D ışık kullanılıyorsa sprite İÇİNE gölge bake ETME (nötr çiz) — baked+dynamic overlap illüzyon kırar.

# Bilinen sorun (oku: STAGING/CLIFF_BLACK_LAYER_DIAGNOSIS.md)
Cliff'ler SİYAH çıkıyordu çünkü `Decor_Cliff` sorting layer hiçbir Light2D `m_ApplyToSortingLayers`'ında yoktu → 0 ışık → Lit material siyah. S114-S2'de "16/16 Light2D Decor_Cliff hedefliyor" fix iddiası var. Inactive `RimLight_*_Cyan` + `Brazier_*_WarmLight` GO'lar mevcut.

# Sorular (gerekçeli yanıtla)
1. **Metod:** Bu floating ambiyansı için ışık katmanlama nasıl olmalı? (Global ambient seviyesi + cliff-edge cyan rim-light + void-altı gradient/karanlık + rune pulse + brazier focal pool). Her ışık tipi: Light2D türü (Global/Freeform/Spot/Sprite), hedef sorting layer'lar, intensity/color/falloff aralığı öner.
2. **Derinlik/void:** Tile'ların "havada asılı" hissi ışıkla nasıl güçlenir? (alt-cliff'e daha az ışık = düşüş hissi, drop-shadow, void gradient bg). Parallax bg ile etkileşim.
3. **Cliff-rim (cyan):** Cliff kenarına cyan rim nasıl verilir — sprite-içi mi (bake, ama kural yasaklıyor) yoksa ayrı rim Light2D / additive rim sprite katmanı mı? Üretilebilir mi?
4. **Saçmalık tespiti:** Bu ambiyans/ışık planında mantıksız, kendi içinde çelişen veya uygulanamaz bir şey var mı? (örn. pixelSnapping OFF + Light2D shadow combo, baked-vs-dynamic çakışması, performans)
5. **Üretilebilirlik:** Eğer ışık için yeni asset gerekiyorsa (rim sprite, glow mask, void gradient) — hangi boyutta, hangi tool ile üretilir (create_object/create_map_object/Codex imagegen)? Spec ver ki hazır olsun. ÜRETME.

# Çıktı formatı
Numaralı yanıt + sonda "ÖNERİLEN IŞIK REÇETESİ" (madde madde Light2D setup) + "ÜRETİLECEKLER (spec, üretilmedi)".
