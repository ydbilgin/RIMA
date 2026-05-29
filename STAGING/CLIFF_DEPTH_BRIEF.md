ACTIVE RULES: (1) think before answering (2) concrete, min-fluff (3) scope: 2 questions below only (4) flag uncertain claims.
NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.
RESPOND INLINE in transcript, NOT to a file.

# Amaç
RIMA yüzen-ada arena. İki somut problem için endüstri-standart + uygulanabilir çözüm istiyorum. Setup: Unity URP 2D, **yüksek top-down 3/4 (~70-80°, Hades/Children of Morta)**, Isometric diamond tilemap, PPU 64, cellSize (1,0.61,1). Shadowcaster2D YASAK. 2D pixel art.

# Mevcut cliff sistemi (bağlam)
- Cliff = floor kenar hücresine konan dikey kaya sprite (cliff_S 128x192, top-center pivot, aşağı sarkar).
- Cliff sorting floor'un ALTINDA (Ground < Floor) → floor cliff üstünü örter, sadece void'e sarkan kısım görünür.
- Yerleşim: floor cell'in S/SE/SW komşusu boşsa o cell'e cliff. Yükseklik per-cell rastgele varyasyon var.
- Floor ada şekli ORGANİK/DÜZENSİZ — iç çentikler (notch), arka yarımadalar (peninsula), concave köşeler var.

# SORU A — Düzensiz kenarda hangi cliff'ler "taşıyor", nasıl kesilir?
Problem: concave çentik / arka yarımada kenarlarındaki cliff'ler, GÜNEYLERİNDE birkaç hücre sonra YİNE floor olduğu için, o alçak floor'un önüne/üstüne sarkıp "ortada dikilen kule" gibi duruyor (kullanıcı bunları reddetti). Ölçüm: 90 cliff'in 35'inin güneyinde 5 hücre içinde floor var.
- Endüstride 3/4 iso yüzen-ada kenarında HANGİ kenarlara cliff yüzü konur, hangilerine konmaz? (sadece kameraya bakan dış alt-çevre mi? concave/iç kenarlar atlanır mı?)
- "Güneyinde N hücre içinde floor varsa cliff KOYMA" kuralı doğru mu? N kaç olmalı? Daha iyi bir kural var mı (ör. exterior-drop bağlantısı kontrolü)?
- Kesmek (cell atlama) mı, sprite'ı floor'a göre clip/mask etmek mi daha doğru?

# SORU B — "Layer 3" derinlik backdrop: endüstride nasıl?
Zemin=L1, cliff=L2 düşünürsek, ada ALTINA/ARKASINA (z/sorting daha geride) **animasyonlu derinlik görseli** koyacaktık ki abyss/derinlik hissi olsun. Elimizde KAPALI parallax katmanları var: void / nebula / ruins / floating-island / fog.
- Endüstride yüzen-ada arenanın ALTINDAKİ derinlik nasıl kurulur? Kaç parallax katman, ne içerik, parallax factor aralığı?
- ANİMASYON tekniği: yavaş-kayan sis, partikül, drifting bulut — URP 2D'de pixel-art uyumlu nasıl? (sprite sheet anim / shader scroll / particle / transform drift?)
- Ada KENARI ile abyss nasıl birleştirilir ki cliff'in altı boşlukta asılı kalmasın — fog/gradient/vignette? cyan falloff?
- Sorting: bu katmanlar cliff'in (Ground) altında nasıl sıralanır, parallax factor'lar ne olur?
- Hades / Children of Morta / Diablo abyss referansı.

# İSTENEN: her soru için kısa, madde-madde, RIMA'da bu hafta uygulanabilir somut adımlar + en yüksek-impact ilk adım.
