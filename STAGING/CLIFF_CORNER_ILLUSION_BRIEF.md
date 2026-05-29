ACTIVE RULES: (1) think before answering (2) concrete/min-fluff (3) scope: corner-naturalness illusion only (4) flag uncertain.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
RESPOND INLINE. Do NOT edit code/scene — inline ideas only.

# Amaç
RIMA yüzen-ada arena, 3/4 top-down (~70-80°), Isometric diamond tilemap, PPU 64, abyss nebula backdrop. Adanın KÖŞELERİ keskin (diamond uçları). Floor geometrisini yuvarlamayı denedik AMA tile-grid olduğu için sonuç BASAMAKLI pahla — pürüzsüz doğal kavis değil. Kullanıcı (keskin sanat gözü): "geometriyi değiştirmeden, **VİZÜEL İLLÜZYONLA** doğal köşe sağlanabilir mi?" diye soruyor.

# SORU (concrete, illüzyon-yaklaşımı)
Tile geometrisini DEĞİŞTİRMEDEN, adanın sivri köşelerini DOĞAL/yumuşak göstermenin en iyi vizüel yolu ne? Değerlendir + sırala (kalite/efor):
1. **Köşe sis/mist overlay** (backdrop fog köşeye taşar, sivri uç sise karışır)
2. **Köşe vignette/karartma** (radyal dark gradyan, uç abyss'e fade)
3. **Köşe overlay sprite** (sivri noktaya kavisli kaya "cap" decal, basamağı maskele) — boyut? nasıl hizalanır?
4. **Backdrop bleed / additive cyan glow** köşede
5. **Lighting** (köşeyi karanlıkta bırak, ışık merkeze)
- Endüstride (Hades/CoM) yüzen-ada köşeleri böyle illüzyonla mı yumuşatılıyor, yoksa hep gerçek-art-köşe mi?
- Geometri-round + illüzyon KOMBO mu, yoksa SAF illüzyon mu daha temiz?
- Her birinin Unity-implementasyonu (sprite/shader/Light2D/particle) + PPU-64 pixel-art uyumu (sub-pixel titreme riski)?

# İSTENEN: madde-madde, RIMA bu hafta uygulanabilir, en yüksek kalite/efor + en yüksek-impact ilk adım. Mevcut asset'lerle (yeni art minimal) ne kadarı yapılabilir?
