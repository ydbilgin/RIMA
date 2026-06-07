ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Attunement Chamber (karakter seçim odası) görsel temizliği — KULLANICI ŞİKAYETİ (ekran görüntüsüyle): (1) kamera çok yakın, (2) pedestal heykellerinin birkaçı Warblade KOPYASI, (3) odada fare-mob + yeşil kutu var (olmamalı), (4) dummy ekstra asset (sınıf karakteri olmalı).

# Bağlam
- Chamber = ChamberSelectBootstrap (runtime kurulum; Chamber_CharSelect.asset RoomTemplateSO; 10 pedestal hilali; [G] bürünme; gerçek-combat dummy HP 100/regen). Unity AÇIK — verify için MCP kullan, PLAY MODE'a girip screenshot alabilirsin (çıkışta play'den çık).
- 10 sınıfın idle_south sprite'ları MEVCUT: `Assets/Art/Characters/<Class>/Rotations/` (CharSelect v2/v3 bunları kullandı) ve/veya `Assets/Resources/Characters/<Class>/`.
- Heykel görsel dili (eski karar): taş-gri/koyu silüet + cyan highlight ("donuk echo").

# İşler
1. **Heykel fallback teşhisi + fix:** ChamberSelectBootstrap'ta pedestal heykellerinin sprite seçimini bul. Neden birden çok Warblade görünüyor (sprite bulunamayan sınıflar Warblade'e mi düşüyor? path mi yanlış?) — kök nedeni raporla. FIX: HER pedestal KENDİ sınıfının idle_south sprite'ını kullanır (10 farklı silüet) + mevcut donuk/taş tint'i korunur. Sprite'ı gerçekten olmayan sınıf varsa Warblade kopyası koyMA — koyu jenerik silüet (mevcut siyah-silüet pattern'i) + raporla.
2. **Mob/yeşil-kutu temizliği:** Chamber'da fare/rat mob'u ve yeşil kutu (muhtemelen SeamCrawler eksik-sprite placeholder'ı veya BrokenStateVisual) NEREDEN spawn oluyor bul (dummy mu, EncounterController sızıntısı mı, prop mu?). Chamber'da DÜŞMAN SPAWN OLMAZ — kaldır (sadece chamber bağlamında; run odalarına dokunma).
3. **Dummy = sınıf karakteri:** Combat dummy'nin görselini fare/ekstra asset yerine MEVCUT bir sınıf karakteri sprite'ı yap (öneri: Brawler idle_south — "sparring partner" fantezisi; echo/yarı-saydam tint ile oyuncudan ayrışsın). HP/regen/hasar-alma davranışı AYNEN kalır. "DUMMY HP 100/100" yazısı kalabilir ama dummy'nin üstüne/yanına hizala (ekran köşesinde kayıyorsa).
4. **Kamera genişliği:** Chamber kamerası tüm hilali + kapıyı rahat çerçevelesin — oda bounds'una margin'li fit (serialized çarpan alanı ekle, default'u 10 pedestal+kapı görünür olacak şekilde ayarla; mevcut takip davranışı bozulmasın). Kullanıcının ekran görüntüsünde sadece 3-4 pedestal görünüyordu — hedef: tamamı + zemin hissi.
5. **Verify:** Play mode'da MainMenu→BAŞLA akışıyla chamber'a gir, screenshot al: `STAGING/_process/2026-06/chamber_cleanup_verify.png` — 10 FARKLI heykel + geniş kadraj + mob YOK + sınıf-karakteri dummy görünmeli. Play'den çık, sahne KAYDETME (bootstrap runtime kuruyor zaten).
6. Compile temiz + ilgili testler (chamber/bootstrap testi varsa) yeşil. COMMIT YAPMA.

# Çıktı
CODEX_DONE'a: kök neden raporları (Warblade kopyaları + mob kaynağı) + iş 1-6 DONE/BLOCKED + değişen dosya:satır + verify screenshot path'i.
