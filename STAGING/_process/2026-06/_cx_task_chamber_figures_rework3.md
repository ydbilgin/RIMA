ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Attunement Chamber'daki sınıf figürlerini ve seçim mekaniğini kullanıcı playtest geri bildirimine göre düzelt. Ana dosya: `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (chamber'ı runtime kuruyor). Sahne = chamber (_IsoGame/_Arena chamber bootstrap akışı). Legacy'ye dokunma.

# MEVCUT DURUM (playtest screenshot)
Sınıf figürleri: player'a göre KÜÇÜK · rastgele/dağınık konumlanmış · bazıları siyah blob/silüet · seçim dummy üzerinden ("KUKLA CANI 100000/100000") yapılıyor ve dummy seçimi PLAYER'ı da değiştiriyor.

# İSTENEN 5 DÜZELTME (kullanıcı, kesin)
1. **BOYUT:** Sınıf figürleri PLAYER karakteriyle AYNI ölçekte olmalı (şu an çok küçük). Player'ın sprite ölçeğini/PPU'sunu referans al; figür transform.localScale'ini player'a eşitle.
2. **GERÇEK DURUŞ:** Her figür o sınıfın GERÇEK idle sprite'ını göstersin (ön-yüz idle_south veya sınıfın authored idle pozu) — siyah blob/silüet DEĞİL. Sprite bulunamıyorsa anlamlı fallback + log; ama hedef gerçek poz.
3. **DÜZGÜN SIRALAMA:** Figürler temiz, eşit-aralıklı, OKUNUR bir düzende dizilsin (tek sıra yatay hat, veya 2 düzenli sıra) — rastgele saçılma YOK. Player'ın spawn'ı ve kapı (portal) ile çakışmasın, hepsi walkable üzerinde.
4. **DUMMY AYRIŞTIR:** Dummy ("Kukla") sınıf değiştirme SADECE dummy'nin görselini değiştirsin. PLAYER'a / PlayerClassManager'a DOKUNMASIN. Yani dummy artık player-sınıf seçici DEĞİL (o iş figürlere taşındı). Dummy bir antrenman kuklası olarak kalsın (ölümsüz, mevcut davranış korunur).
5. **YENİ SEÇİM MEKANİĞİ (figür = seçici):** Oyuncu bir sınıf figürünün yakınına gelince dünya-içi "[G] <SINIF>'a geç" prompt'u çıksın. G'ye basınca ONAY sorusu: "Bu karaktere geçmek ister misin?" (Evet/Hayır — basit dünya-içi veya küçük panel; Evet=tekrar G veya Enter, Hayır=Esc). Evet → `PlayerClassManager.SelectedClass` o sınıfa set + player görseli o sınıfa güncellensin. Hayır → kapan, değişiklik yok.

# KORU / KISIT
- Mevcut kilit/cost mantığı VARSA koru (kilitli sınıf → onay yerine "kilitli / X ECHO" gösterilebilir); YOKSA ekleme (over-engineering yapma). Demo 2-sınıf kilidi AYRI karar — burada uygulama.
- Dummy ölümsüzlüğü/trigger-only davranışı bozulmasın.
- Walkable enforcement + room-completion testleri yeşil kalsın.
- Cerrahi: sadece ChamberSelectBootstrap.cs (+ gerekiyorsa küçük yardımcı). PlayerAnimator/combat'a dokunma.

# ÇIKTI (CODEX_DONE.md)
- Değişen dosyalar + file:line + her 5 maddenin nasıl karşılandığı.
- Figür ölçeği nasıl player'a eşitlendi, sıralama düzeni (koordinat mantığı), G-confirm akışı.
- Compile temiz mi + ilgili testler. (Unity açıksa test runner/screenshot bloke olabilir — not düş, sorun değil; kullanıcı playtest edecek.)
- BLOCKED yaz belirsizse. Commit ETME. Untracked dosyalara dokunma.
