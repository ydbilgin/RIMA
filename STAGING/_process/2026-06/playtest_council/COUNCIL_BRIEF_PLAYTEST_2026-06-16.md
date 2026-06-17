# COUNCIL — RIMA Playtest Bug'ları + Endüstri Araştırması + Gameplay Tasarımı + Playtest Planı (2026-06-16)

ACTIVE RULES: (1) think before answering (2) somut, kaynaklı öneri — spekülasyon değil (3) demo-scope farkında (19 Haz, solo dev) (4) belirsizse "BELİRSİZ" yaz.
NLM ACCESS gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"` (auth expired olabilir → kod/brief temelli devam et).

> **Bağlam:** RIMA = solo-dev 2D top-down ARPG roguelite (Unity URP 2D, high top-down 3/4, pixel-art). **Demo 19 Haziran (3 gün).** Sahne: dev-direct `_Arena` (RoomRunDirector + EncounterController + DungeonGraph branching map). Akış: oda→combat(wave)→clear→slow-mo→reward spawn→G ile topla→draft→skill grant→kapı aç→sonraki oda. Run-map = M (KOŞU YOLU branching DAG). Sınıf: Warblade (LMB melee + Q/E/R/F).
> **Bu council = kullanıcının açık talebi:** (A) bulunan bug'ları + **ENDÜSTRİ bunları nasıl çözüyor** (Hades / Dead Cells / Risk of Rain / Enter the Gungeon / Binding of Isaac mimari & pattern), (B) gameplay tasarımı (Hades-style wave / boss-mob / mob boyutu), (C) **kapsamlı playtest planı** (ne test edilmeli, iyi-oynanış kriteri). Her advisor 3 ekseni de KENDİ merceğinden cevaplasın.

---

## BULUNAN BUG'LAR / SORUNLAR (canlı playtest + screenshot incelemesi, hepsi data-proof)

### B1 🔴 Reward-flow stall (KULLANICININ YAŞADIĞI ANA BUG)
- **Kullanıcı raporu:** "Ödül aldım, kapılar açıldı, ama hiçbir şey vermedi" (reward toplandı, skill grant olmadı).
- **Benim canlı bulgum (force-kill ile, CAVEAT: doğal combat'ı bypass etmiş olabilir):** Tüm düşman ölünce → `lifecycle.State=Cleared` AMA `activeReward=null` (ödül HİÇ spawn olmadı) + `Time.timeScale=0.3'te STUCK` (slow-mo geri dönmedi) + kapılar `isOpen=False`. Akış Cleared'da takıldı.
- İki farklı failure-mode (kullanıcı: grant yok / ben: spawn yok) → **reward-flow genel kırılganlığı.** RoomRunDirector clear→slowmo→spawn→collect→draft→grant→door zinciri çok adımlı ve kırılgan.
- Skill grant mekanizması ÇALIŞIYOR (opening-kit `ForcePickFirstOpeningKitSkill` → owned=[Iron Charge] doğrulandı) → sorun reward SPAWN + collect + slow-mo return adımlarında.

### B2 🟠 Wave boyutu çok küçük (DESIGN — kullanıcı şikayeti)
- Oda başına ~2 wave, her wave ~1-2 mob (FractureImp). Kullanıcı: "2 mob kesiyorum bölüm geçiyorum, **Hades'te böyle değil, wave wave geliyor**." İstenen: yükselen, dolu wave-after-wave combat.

### B3 🟠 M-overlay bleed (kullanıcı "karışıklık")
- M (run-map) draft AÇIKKEN basılınca draft kartları run-map'in ARKASINDAN sızıyor → node'lar + kart yazıları üst üste. Run-map dimmer opak değil / draft farklı canvas'ta üstte. (Director overlay'de de aynı bleed sınıfı.)
- Branching mantığı SAĞLAM (DAG doğru, sol-kapı→choice0→node1-sol / sağ-kapı→choice1→node2-sağ tutarlı). Sorun = overlay z-order/scrim.

### B4 🟠 Background void (kullanıcı şikayeti)
- Arena cliff-ada SİYAH boşlukta yüzüyor; kullanıcı "siyah ekran görünüyor, olmamalı, **gerçek bir ambiyans gibi olmalı**." Ayrıca kamera mesafesi sorgulanıyor (büyümeli/yaklaşmalı mı).

### B5 🟡 UI/görsel (screenshot incelemesi)
- HUD'a **agresif kırmızı tint** (düşük-can vignette çok güçlü, ekranı kaplıyor). · Düşmanlar **siyah siluet blob** (sprite okunmuyor). · MainMenu backdrop **tutarsız** (bazen painted-keep, bazen düz koyu). · Settings/Pause/Director overlay'lerde **dim/scrim yok** → arka sızıntı. · Skill-hover **tooltip yerleşimi** belirsiz (dikey-şerit geçmişi).

### B6 🟡 Instrumentation gap
- `read_console` play-mode'da çoğu zaman 0 log döndü → kullanıcının istediği "oyun-içi olay log penceresi" (şu skill alındı / wave geldi / reward spawn / HATA) mevcut değil/erişilemiyor. Debug HUD log paneli gerekebilir.

---

## SORULAR (her advisor 3 ekseni de cevaplasın)

### EKSEN A — Bug fix + ENDÜSTRİ ARAŞTIRMASI (kullanıcı özellikle istedi)
Her bug (özellikle B1 reward-flow, B2 wave, B3 overlay-scrim) için:
1. **Kök-neden hipotezi** + RIMA'da nasıl cerrahi düzeltilir (demo-scope, 3 gün).
2. **Endüstri nasıl çözüyor:** Hades / Dead Cells / Risk of Rain 2 / Enter the Gungeon / Binding of Isaac — room-clear & reward spawn'ı nasıl ROBUST yapıyorlar (event-driven mi, reconcile/timeout safety-net mi, state-machine mi)? Wave/encounter mimarisi nasıl (spawn-director, budget, escalation)? Overlay/modal yönetimi (tek modal-stack + scrim) nasıl? Somut pattern adı + neden çalışıyor.

### EKSEN B — Gameplay tasarımı (kullanıcı istekleri)
1. **Hades-style wave sistemi:** oda-tipine göre kaç wave, wave başına kaç mob, escalation eğrisi, spawn cadence (önceki wave %X ölünce yeni wave / timer / arena-kapı). RIMA'nın mevcut EncounterController + EncounterWaveSO + budget sistemine nasıl oturur (sıfırdan değil).
2. **Boss-mob (demo):** 1 mob'u boss yap — scale faktörü (okunur + tehditkâr), boss-skill'leri ŞİMDİLİK sadece VFX placeholder (telegraph + VFX, gerçek mekanik sonra), HP/posture. "Gerçek boss-fight hissi" için minimum ne lazım (telegraph + faz + arena)?
3. **Mob boyutları:** okunabilirlik + oyuncu-mob ölçek oranı (top-down ARPG'de ideal). Mevcut siyah-blob sorunuyla birlikte.

### EKSEN C — KAPSAMLI PLAYTEST PLANI (kullanıcı: "ne olması/olabileceğini detaylı düşünsün")
1. Demo golden-path için **yapılandırılmış playtest checklist'i** — her sistem (combat feel, wave pacing, reward flow, run-map nav, kapı yön, UI/overlay, boss, ölüm, ekonomi) için **ne test edilmeli + PASS kriteri**.
2. **"İyi oynanış" kriterleri** — combat-feel (juice, hit-stop, wave ritmi), okunabilirlik, flow. Hangi metrikler/gözlemler "demo-ready" sayılır.
3. Otomatik/manuel test bölünmesi: neyi MCP execute_code ile data-proof test edebilirim (ben orchestrator), neyi OBS/elle kullanıcı görmeli.

---
## ÇIKTI
Yanıtını `STAGING/_process/2026-06/playtest_council/RESP_<advisor>.md` (cx/axpro/axflash) yaz. A/B/C başlıklı. Endüstri iddialarını mümkünse OYUN ADIYLA örnekle. Demo-scope (3 gün, solo) gerçekçi öncelik sırası ver: "DEMO-KRİTİK FIX" vs "POST-DEMO". Türkçe açıklama. Spekülasyon=BELİRSİZ.
