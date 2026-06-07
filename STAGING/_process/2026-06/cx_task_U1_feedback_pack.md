ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
U1 — Kullanıcı geri bildirim paketi (2 parça): (A) çıkış portalları TEK TİP gövde, (B) Chamber dekompresyon (pillar/saçma asset temizliği + heykel/etiket çakışması + daha güzel kompozisyon).

# PARÇA A — Portal kemerleri tek gövde (kullanıcı: "kapılar farklı farklı olmasın, hepsine aynı olsun")
- Şu an `IsoRoomBuilder` tip başına FARKLI kemer gövdesi kullanıyor (combat=cyan, elite=yeşil-kahve, chest, boss — dün wire'landı). KULLANICI İSTEĞİ: TÜM portallar AYNI kemer gövdesi.
- Gövde seçimi: **Combat kemeri** (cyan/teal — en onaylı görsel). Frontal + açılı varyantları aynen (N/NW/NE kuralı değişmez).
- Tip ayrımı NASIL korunur: mevcut RÜN ikonları (zaten kapı üstünde tip rünü var) + İSTEĞE BAĞLI hafif glow/core tint (combat=cyan kalır, elite/chest/boss core tint'i — SpriteRenderer.color ile, yeni asset YOK). Rün/tint çocukta, flip kuralları aynen (NE flipX'te rün flip'lenmez).
- IsoRoomBuilder'daki tip-bazlı sprite alanları SİLİNMEZ (ileride geri dönülebilir) — sadece çözümleyici tek gövdeye düşer; _Arena ref'leri güncelle. Testlerdeki tip-bazlı assert'leri yeni davranışa uyarla.

# PARÇA B — Chamber dekompresyon (kullanıcı: "giriş ekranı daha büyük güzel olmalı, seçerken birbirine girmemesin; pillar güzel durmuyor kaldır, saçma assetleri temizle")
Dosya: `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (dün temizlik geçti — üstüne):
1. **Pillar/monolith ve gereksiz prop'ları KALDIR:** chamber'a spawn edilen dekor prop'larını bul (torch'lu pillar/monolith, yosunlu taş yığını vb.) — kullanıcı "saçma" dedi → chamber'da SADECE: zemin+cliff, 10 pedestal+heykel, kapı (ArchGate), dummy, oyuncu kalsın. Kaldırılanları raporla.
2. **Heykel/etiket çakışması:** son katalog karesinde (STAGING/screen_flow_2026-06-07/03_chamber_wide.png — BAK) sağ taraftaki heykeller ve unlock-condition metinleri üst üste biniyor. Fix: pedestal aralığını artır (hilal yarıçapı/spacing) VE/VEYA etiketleri sadece yaklaşınca göster (proximity-fade — [G] prompt'la aynı mantık); etiketlerin birbiriyle ve heykellerle çakışmadığını doğrula.
3. **Kompozisyon "daha büyük güzel":** kamera fit çarpanını heykeller OKUNUR olacak şekilde sıkılaştır (dünkü 8.44 ortho fazla genişti — hilal + kapı kadrajda AMA heykeller seçilebilir boyda; öneri: hilali ekranın ~%60'ını dolduracak fit). Serialized alanlar zaten var — default'ları ayarla.
4. Çakışma fix'i sonrası [G] bürünme menzillerinin hâlâ doğru çalıştığını play-probe ile doğrula.

# VERIFY
Play mode: chamber screenshot (`STAGING/_process/2026-06/u1_chamber_verify.png`) — pillar yok, çakışma yok, heykeller okunur. _Arena'da 2-3 çıkışlı oda kur → portal screenshot (`u1_portals_verify.png`) — tüm kemerler aynı gövde, tip rünleri görünür. Play'den çık, sahne kaydetme. İlgili testler (IsoRoomBuilderPortalTests + chamber testleri) yeşil. COMMIT YAPMA.

# Çıktı
CODEX_DONE'a: A+B işleri DONE/BLOCKED + kaldırılan chamber asset listesi + değişen dosya:satır + 2 verify screenshot + test sonuçları.
