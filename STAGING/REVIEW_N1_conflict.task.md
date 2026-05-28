ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

RESPOND INLINE — do NOT write to any file. Return your review as your reply text.

# Amaç
RIMA tasarım çelişki + saçmalık çözümünü (Opus ön-kararları) review et. Bu bir KARAR review'ı, kod değil.

# Oku
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\NLM_CONFLICT_RESOLUTION_S114.md
(Gerekirse: CURRENT_STATUS.md "Çözülen çelişkiler" tablosu + Assets/Scripts/Combat/OrientationSync.cs procedural swing.)

# Bağlam
RIMA = wall-less floating-island Hades-style 2D ARPG. Kamera High Top-Down 3/4 ~70-80°, PixelPerfect 640×360 @ PPU64. Silah pipeline = weaponless body + HandAnchor child SR + OrientationSync (8-yön kod: rotation+flipY+sort + procedural swing). Silah üretimi şu an PAUSED.

# Sorular (kısa, net, gerekçeli yanıtla — her birine AGREE/DISAGREE + 1-2 cümle)
1. **SAÇMA-1 (Mixel boss):** Opus kararı = boss büyük tuval (128-192px) AMA PPU 64 sabit (eski 252px@PPU32 superseded). Doğru mu? Mixel'i çözer mi? Kaçırılan?
2. **SAÇMA-2 (silah swing vs gizle):** Opus = procedural swing KORU (ağır/yavaş saldırıda silah görünür+arc okunur = değerli), hızlı flurry'de VFX baskın + küçük arc + hitstop maskeleme. NLM "swing ölü efor, gizle+VFX" diyor. Demo combat-feel için hangisi doğru? Mount kodunun swing'i silinmeli mi korunmalı mı?
3. **Cliff oto-placer:** NLM "2-stage hibrit (oto kenar + manuel dokunuş) AKTİF" diyor; bazı S110 memory "auto cliff DEPRECATED, full manuel" diyor. Demo oynanabilir loop için cliff yerleşimi oto mu manuel mi olmalı? (Demo'da kullanıcı 5 oda elle mi boyamalı?)
4. **Gözden kaçan:** başka tasarım çelişkisi/saçmalığı/uygulanamaz karar var mı? (özellikle demo oynanabilirliğini bloklayan)

# Çıktı formatı
Her soruya 1 paragraf. Sonda "EN KRİTİK 1 BULGU" satırı.
