ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Kullanıcı chamber'ı "hala çok kötü" buluyor + 3 net istek. Ana dosya `Assets/Scripts/UI/ChamberSelectBootstrap.cs` (+ currency için EchoWallet/cüzdan sistemi + HUDController + dummy HP bar). Legacy'ye dokunma. Karar: Hades cilası = SADECE sınıf seçimi (kullanıcı onayladı; ayrı silah sistemi YOK).

# 1) HADES-TARZI DİZİLİM (chamber "hala kötü" + "karakterlerin yerini değiştir")
Şu anki dağınık/grid yerleşim kötü. DELİBERE, SİMETRİK, OKUNUR bir Hades "seçim odası" formasyonu kur:
- Merkez bir yürüyüş ekseni: player spawn ÖN-ORTA → portal ARKA-ORTA (rework-4'te portal (14,18)). Spawn'ı bu eksenin önüne al.
- 10 sınıf figürü merkez ekseninin İKİ YANINA simetrik: sol 5 + sağ 5, eşit aralıklı iki kolon/yay (Hades silah-standı dizilimi gibi). Hepsi walkable, çakışma yok.
- HER figürün altına küçük bir PEDESTAL/PLATFORM görseli + yumuşak ışık/glow (Hades stand hissi). Mevcut bir uygun sprite kullan (rift/pedestal/decal); yoksa basit prosedürel disk/halka kabul.
- Figürler player ölçeğinde + gerçek idle poz (rework-3 korunur), kilitli = okunur gri (rework-3 korunur).
- Yaklaşılan/seçili figürün pedestal'ı VURGULANSIN (glow artışı veya renk).
- G+onay sınıf-geçiş akışı (rework-3) + dummy decouple (rework-3/4) KORUNUR.
- Koordinatları chamber bounds + spawn + portal'a göre SEN hesapla (kör sabit kullanma); simetri ve eşit-aralık şart.

# 2) DEMO BAŞLANGIÇ CURRENCY + HUD
- Demo run'ı belli bir Shattered Echo ile BAŞLASIN (öneri ~200 — kullanıcı kilitli sınıf açabilsin; chamber unlock maliyeti örn. Ronin 150). Sabit/anlaşılır bir başlangıç değeri ver.
- Currency HUD'DA GÖRÜNSÜN (chamber'da ve run sırasında). Mevcut cüzdan sistemini bul (EchoWallet / Shattered Echo) + HUDController'a Echo göstergesi bağla (ikon + sayı). Zaten varsa görünür yap + başlangıç değerini set et.
- Kilitli karakter açma akışı çalışmaya devam etsin (mevcut). (Demo 2-sınıf kilidi AYRI karar — burada uygulama; sadece currency+unlock çalışsın.)

# 3) DUMMY HP BAR
- Dummy üstündeki "KUKLA CANI 100000/100000" metni yerine (veya yanına) gerçek bir HP BAR koy: hasar alınca AZALAN dolu-bar.
- Dummy ölümsüz kalsın (mevcut) ama bar hasarı GÖRSEL yansıtsın (vurunca düşsün; isteğe bağlı yavaş geri dolsun veya düşük kalsın — basit tut). World-space bar, dummy ile birlikte hareket etsin.

# KORU / KISIT
- Walkable enforcement + room-completion + chamber testleri yeşil.
- Cerrahi: ChamberSelectBootstrap.cs + HUDController + cüzdan + dummy HP bar bileşeni. PlayerAnimator/combat'a dokunma.
- Compile temiz olsun.

# ÇIKTI (CODEX_DONE.md)
- Değişen dosyalar + file:line + her 3 maddenin nasıl çözüldüğü.
- Dizilim koordinat mantığı (spawn/portal/figür/dummy nerede), pedestal/ışık yaklaşımı.
- Başlangıç Echo değeri + HUD bağlama yeri.
- Dummy HP bar bileşeni.
- Compile durumu. (Unity açık → test/screenshot bloke olabilir, not düş; kullanıcı playtest edecek.)
- BLOCKED yaz belirsizse. Commit ETME. Untracked dosyalara dokunma.
