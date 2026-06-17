# DEMO MANUEL TEST CHECKLIST — senin son testin (2026-06-17)

> Nasıl: Full-flow için Play = **MainMenu**'den başlat (playModeStartScene=MainMenu). Her madde: **YAP → BEKLENEN → 🚩kırmızı bayrak**. Bir madde kırmızıysa not düş, devam et. Sıra demo akışını izliyor.

## A. Giriş / Boot
- [ ] **Play (MainMenu)** → menü açılıyor, yazılar net (Jersey10/SDF), 🚩 garbled yazı / boş ekran / console error.
- [ ] **Character Select** → sınıf kartları görünür, seç → oyuna giriyor. 🚩 çift menü / takılma.

## B. ⚔️ COMBAT (en kritik — bu oturumda fix edildi)
- [ ] **İlk odaya gir** → düşmanlar **sana doğru geliyor** (idle durmuyor). 🚩 düşman uzakta donuk duruyor (engagement bug geri geldi).
- [ ] **LMB ile vur** → düşman hasar alıyor + ölüyor, **damage number/şok** görünüyor (CombatJuice eklendiyse). 🚩 vuruş hissi yok / düşman ölmüyor.
- [ ] **Q/E/R/F skill** → çalışıyor, VFX çiziliyor. 🚩 skill tepkisiz.
- [ ] **Wave'i temizle** → ölmeden bitirebiliyorsun, ~20-40s, ölen düşman sayılıyor (kills). 🚩 **3-4sn'de ölüyorsun** (lethality bug) / **hiç kill olmuyor**.
- [ ] **Düşman görünürlüğü** → koyu mob'lar zeminde seçilebiliyor (SeloutOutline eklendiyse). 🚩 mob zeminde kayboluyor.
- [ ] **HP düşünce** → bar azalıyor, düşük-HP'de vignette. 🚩 HP göstergesi yanlış.
- [ ] **Öl → death ekranı** → "TEKRAR DENE / ANA MENÜ" çalışıyor. 🚩 soft-lock / takılma.

## C. 🎁 Skill Draft (ödül)
- [ ] **Oda sonu / ödül** → 3 kart açılıyor (IRON CHARGE vb.), oku-net. Bir kart **SEÇ** → etki uygulanıyor. 🚩 kart bozuk / seçim çalışmıyor / **ödül-monolog combat'a sızıyor**.

## D. 🗺️ Run Map
- [ ] **M tuşu** → harita açılıyor, **dallanma + 8 node tipi** (Combat/Elite/Event/Chest/Forge/Boss/Curse/Current) görünür, sonraki oda seçilebiliyor. 🚩 harita boş / node'lar görünmüyor.

## E. 🛒 Merchant + Elite
- [ ] **Merchant oda** → shop standları, ürün al/çık. 🚩 stand bozuk.
- [ ] **Merchant'tan sonraki odaya geçiş** → **sarı-kare placeholder kart SIZMIYOR** (özellikle boss odasına). 🚩 boss odasında shop placard'ı görünüyor.
- [ ] **Elite oda** → elite düşman daha güçlü/farklı. 🚩 normal mob'dan ayırt edilemiyor.

## F. 👑 Boss (Penitent Sovereign)
- [ ] **Boss odası** → boss spawn, **crimson health-bar** + subtitle. 🚩 bar yok / boss görünmez.
- [ ] **Boss fazları** (P0→P1→P2/P3) → faz geçişi oluyor. 🚩 tek fazda takılı.
- [ ] **Boss saldırı belirtileri (telegraph)** → daire/çizgi yer-uyarısı çiziliyor mu (telegraph eklendiyse, özellikle P2/P3 + ChainExplosion). 🚩 saldırı habersiz geliyor / belirti süresi gerçek vuruşla tutmuyor ("yalan telegraph").
- [ ] **Boss'u yen** → ölüm + residue + sonraki akış. 🚩 ölmüyor / akış kırılıyor.

## G. 🏗️ Build Mode (F2) — CENTERPIECE
- [ ] **F2** → editor açılıyor (NOT `"`). 🚩 açılmıyor.
- [ ] **Prop paleti** → **19 prop** görünür (yeni 10 RIMA prop dahil: barrel/crate/chest/brazier/pillar...). 🚩 prop ikonları boş/eksik.
- [ ] **Prop yerleştir** → tıkla, prop oturuyor, snap çalışıyor, phantom yok. 🚩 yanlış yere / hayalet prop.
- [ ] **Tile tool** → FLOOR/WALKABLE/OVERLAY paint + erase. 🚩 boya çalışmıyor.
- [ ] **F2 kapat → oyna** → yerleştirdiğin prop **kalıyor** + oyun devam ediyor. 🚩 prop kayboluyor / oyun donuyor.

## H. 🎬 Director Mode
- [ ] **Director aç** → IDE-dock layout (app bar + sol rail + library + inspector + status bar), viewport geniş (≥%55), font net. 🚩 debug-overlay gibi dağınık.
- [ ] **Spawn tab** → düşman seç + spawn → sahnede beliriyor. 🚩 spawn olmuyor.
- [ ] **Stats tab** → slider ile stat değiştir (örn. PHYS) → oyuncuya yansıyor. 🚩 slider etkisiz.
- [ ] **Telemetry / Prop / Map / Free-cam** tab'ları → açılıyor. 🚩 boş/hata.
- [ ] **Director kapat → oyna** → oyun normal devam. 🚩 leak / donma.

## I. 🔗 SEAM'LER (en sinsi hatalar — "green-assert ≠ çalışıyor")
- [ ] **Combat sırasında F2 aç/kapa → tekrar combat** → juice/combat hâlâ çalışıyor, donma yok, leftover scrim yok. 🚩 >1s freeze / yarı-saydam karartı kalıyor.
- [ ] **Combat sırasında Director aç/kapa → tekrar combat** → timeScale normal, hit-stop overlay'e sızmıyor. 🚩 oyun yavaş kalıyor / donuyor.
- [ ] **Oda portal geçişleri** → temiz, bir önceki odadan obje sızmıyor. 🚩 hayalet obje / yanlış oda.

## J. 🔊 Ses
- [ ] **SFX duyuluyor mu**: vuruş, düşman ölümü, boss intro, oda-temizleme, draft-seç. 🚩 sessiz.
- [ ] **Müzik bed** (eklendiyse) → arka plan loop'u var, SFX'i bastırmıyor. 🚩 tamamen sessiz demo.

---
## 📌 NOTLAR
- 🚩 **En kritik 3 madde** (bunlar kırmızıysa demo riski yüksek): **B (combat oynanabilir mi)** · **G/H (F2+Director = tez centerpiece)** · **I (seam'ler)**.
- Combat fix bu oturumda yapıldı + scripted doğrulanıyor; sen **B bölümünü özellikle dikkatli oyna** (düşman geliyor mu + ölmeden wave temizleniyor mu).
- Bulduğun her 🚩'yi bana söyle → düzeltiriz. Bu liste = council kilitli-sıra "step-1 full-flow dry-run gate"in senin elinle versiyonu.
