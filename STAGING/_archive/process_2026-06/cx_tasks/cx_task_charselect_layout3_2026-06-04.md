ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v2 layout PASS 3 (kullanıcı feedback): (1) paneller karakteri KAPATMASIN (no-occlusion), (2) boyutlar mantıklı/endüstri-standardı, (3) seçili karakter altındaki seçim göstergesi = Unity-driven VFX (canvas'ta çalışan animasyonlu glow). KOD-ONLY, sadece CharacterSelectScreen.cs. Sahne .unity'ye dokunma.

# Dosya
Assets/Scripts/UI/CharacterSelectScreen.cs

# Mevcut durum
Layout pass 2 DONE: tight hit rects (%45×%70), 2-row chars (front y0.34 / back y0.62, x0.12-0.795), sol identity popup, sağ dikey skills, alt SEÇ/GERİ, room_bg backdrop, pedestal seçim halkası.

# Yapılacaklar

## 1. NO-OCCLUSION — paneller karakteri örtmesin
Sorun: sol identity popup karakterlerin sol kısmını örtüyor. Çözüm: panelleri DARALT + karakterleri merkeze topla (kenarları panellere değmesin):
- **Sol identity popup:** x 0.012-0.175 (dar ~16%), y 0.34-0.84.
- **Sağ skills panel:** x 0.86-0.988 (dar ~13%), y 0.14-0.96.
- **Alt strip (SEÇ/GERİ):** y 0.0-0.11.
- **Karakterler (merkez bandı, kenar-payı dahil panellere değmez):**
  - Front (4 unlocked, scale ~0.92, size ~250x350): Warblade 0.34 · Elementalist 0.47 · Ranger 0.61 · Shadowblade 0.73 (hepsi y0.34)
  - Back (6 locked, scale ~0.74, size ~200x285 = uzakta/küçük, depth): Ronin 0.27 · Ravager 0.375 · Gunslinger 0.48 · Brawler 0.585 · Summoner 0.69 · Hexer 0.79 (hepsi y0.62)
  - Doğrula: her char'ın sprite KENARI sol-popup (≤0.175) ve sağ-skill (≥0.86) zone'una girmesin (size'ı gerekirse küçült). 10'u TAM görünür, ön/arka gap 0.28.

## 2. SENSIBLE SIZES
Panel/font/char boyutları abartısız, endüstri-standardı. Identity popup = kompakt kart (portre + name + tag + motto + resource sığsın, taşma yok). Skills satırları okunur ama dev değil.

## 3. SELECTION VFX (canvas-compatible — KRİTİK)
⚠️ Gerçek Unity ParticleSystem Screen-Space-Overlay Canvas'ta RENDER OLMAZ. O yüzden seçili karakter ayağının altındaki göstergeyi **canvas'ta çalışan animasyonlu UI-VFX** yap:
- Mevcut `pedestal_seal` halkasını taban olarak tut + ÜstÜNE **kod-animasyonlu cyan (#00FFCC) additive glow**: yavaş DÖNEN bir halka (Image, rotation lerp) + PULSE eden (scale/alpha sin) glow disk. İstersen 2-3 küçük "mote" Image yukarı süzülüp fade (basit Update/coroutine tween). Hepsi UI Image, additive/translucent.
- Bu VFX seçili char sprite'ının ARKASINDA/ALTINDA sıralanmalı (char'ı KAPATMASIN). Sadece seçili char'da aktif; seçim değişince taşınır.
- Eğer Canvas Screen-Space-Camera ise gerçek ParticleSystem + 2D light denenebilir; DEĞİLSE (overlay) UI-Image animasyon yolu. Hangisi olduğunu kontrol et, çalışan yolu seç. ParticleSystem koyup render olmazsa BAŞARISIZ sayılır.
- Mevcut AnimateRoomSelection/glow/dim mantığını genişlet (yeni sistem değil).

## 4. KORU
SelectClass data/portre, SkillDatabase query (dikey), IsUnlocked/locked-CTA, RuntimeRoot+authored-disable, backdrop resilient-load, tight-hit, scene-load. Procedural. Yeni layout-engine YOK.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode probe: identity popup anchorMax.x≤0.18; skills anchorMin.x≥0.86; her char sprite edge'i 0.175-0.86 bandında (panel örtmüyor); 10/10 char; front y0.34 back y0.62; seçili char altında animasyonlu glow VFX objesi var + char sprite'ın arkasında (sortOrder/sibling); Canvas renderMode raporla; SelectClass(Warblade) identity dolu; SelectClass(Ronin) KİLİDİ AÇ disabled; backdrop=room_bg.
- Değişen metot/satır + compile + probe (+ Canvas renderMode + VFX yaklaşımı) profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
- BELİRSİZLİK → BLOCKED.
