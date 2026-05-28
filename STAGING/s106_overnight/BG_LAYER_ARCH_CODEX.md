# Codex Cross-Validation: RIMA Wall-less BG Layer Architecture

ACTIVE RULES: (1) think before answering (2) NET, no waffle (3) engineering+Unity practical (4) BLOCKED if can't be concrete.

NLM ACCESS: If needed:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING

Amaç: ChatGPT'ye user aşağıdaki briefi gönderdi. Cross-validation için Antigravity (Gemini 3 Pro) paralel cevap veriyor. Sen Codex (gpt-5.5) olarak aynı briefe cevap ver, engineering+Unity perspektifinden. Sonra Opus üçünü sentezleyecek.

# CONTEXT (kısa)
RIMA = 2D top-down pixel art ARPG, V1 wall-less Hades Elysium style LOCKED. Karakter ~64px. Floating arena above void. Cyan rift + warm brazier contrast. Unity 6 + PixelLab asset gen. Reference: 10 ChatGPT mockup images at `STAGING/Chatgpt_walless/` premium quality bar.

# RIMA technical state
- Unity 6 (URP 2D Renderer + Pixel Perfect Camera + 2D Lights)
- PPU = 64
- Karakter canvas = 120×120 chibi
- Tile = 64×64 @ PPU 64 = 1 unit
- Camera = high top-down 3/4 ~70-80°
- PixelLab Tier 2 subscription, 1369 gen kaldı
- PixelLab MCP create_object: ≤42→64 candidate, 43-85→16, 86-170→4, 171-256→1, 20-40 gen
- PixelLab create_image_pro web UI: 32→512 sizes, single image per gen, ~20-40 gen for 512

# THE BRIEF (user's exact question — answer all 10)
[Same brief as in BG_LAYER_ARCH_ANTIGRAVITY.md — paste here for Codex]

> RIMA için top-down 2D pixel-art, wall-less floating arena sisteminde arka plan/void derinliğini doğru kurmak istiyorum.
>
> 1. En doğru çözüm tek büyük background image mı, iki image'i birleştirip pano yapmak mı, yoksa repeating/tileable + parallax katman sistemi mi?
> 2. Hangi katmanlar repeating olmalı, hangi katmanlar wide strip olmalı, hangi katmanlar unique/special-room olmalı?
> 3. PixelLab boyut limitini düşünerek en mantıklı asset breakdown'ı (layer sayısı + her layer rolü + çözünürlük + opaque/transparent + tekrar/tekil)
> 4. Net ölçüler: Void base / Far ruins strip / Floating islands / Fog / Light beams + particles (her birine 512×512, 1024×576 vb. öner ve sebep)
> 5. Birden fazla görseli birleştirme — ne zaman/nasıl, neden rastgele pano kötü
> 6. Animasyon: hangi katman kayar / pulse / particle / statik
> 7. Unity sorting order tablo + parallax değerleri (örnek)
> 8. Combat / Boss / Treasure room varyasyonları (aynı sistem nasıl)
> 9. Production plan (ne önce, MVP vs nice-to-have)
> 10. PixelLab gen prompts (her layer için 1-2 cümle)

# Codex specific perspective (Antigravity'den fark)
- Bence sen MÜHENDİSLİK tarafı + Unity practical implementation derinleştir
- Antigravity büyük ihtimal industry örnekler verecek; sen Unity-spesifik shader / sorting / sprite pivot / draw call sayıları gibi pratik detaylara in
- Sprite Atlas, Texture Streaming, Compressed Texture, Sprite Pivot conventions
- Parallax math formulü (camera Δ ile layer Δ oranı)
- URP 2D Renderer'da parallax sprite'lar nasıl sort edilir (Sorting Group, Order in Layer, Z position)
- Performance budget: kaç sprite per room max, draw call hedef

# Çıktı format (same 7 bölüm)
1. Net karar
2. Katman tablosu
3. Asset size tablosu
4. Unity kurulum (sorting + parallax math)
5. Oda varyasyonları
6. Production checklist
7. PixelLab prompts

# Kısıtlar
- 800-1200 kelime
- Tüm 10 soruyu cevapla
- Net verdict, savun
- Final satır: `VERDICT: <single|panorama|parallax-modular>` + 1 cümle gerekçe
- Codex transcript otomatik CODEX_DONE'a yazılacak
