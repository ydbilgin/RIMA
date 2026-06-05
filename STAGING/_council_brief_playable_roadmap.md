# COUNCIL BRIEF — OYNANABİLİR PLAN: Sıralı Roadmap (2026-06-05)

## Amaç
Kullanıcı: "Artık oynanabilir planımızı yapalım — önce silahlar, silahları karakterlerin eline oturtma,
animasyonlar, ve diğer şeyler. Sıralamayı council'le detaylı düşünün. NLM'den faydalanın."
Karar: "OYNANABİLİR DEMO"ya giden SIRALI yol haritası (bağımlılıklar + paralellik + kullanıcı-gated işler).

## "Oynanabilir" tanımı (hedef döngü)
MainMenu → Attunement Chamber (yürüyerek sınıf seç; cx ŞU AN inşa ediyor) → run: oda→combat→clear→reward→
kapı→sonraki oda→…→ölüm/zafer → "+n SHATTERED ECHO" → Chamber'da unlock → tekrar. His: silahlı karakter,
juice'lu combat (knockback/knockdown), çeşitli odalar.

## ENVANTER — durum + kalan işler (NLM-sync'li; ek sorgu serbest)
### A. SİLAHLAR (kullanıcının 1 numarası)
- Canon tablo HAZIR (NLM `_nlm_weapons_anim.json`): 10 sınıf silahı (Warblade=greatsword, Elementalist=YÜZEN
  RÜN DİSKİ asa-yasak, Ranger=yay, Shadowblade=ikiz hançer, Ronin=katana+kın, Ravager=ikiz balta,
  Gunslinger=ikiz tabanca, Brawler=eldiven, Summoner=ruh feneri, Hexer=grimoire).
- Kilitli mimari: gövde silahsız ✓ (üretildi) · silah = TEK doğu-yönlü statik sprite → `HandAnchor` child →
  8 yön KODLA (OrientationSync+WeaponSorter) → hızlı vuruşta sprite gizle→slash-arc VFX.
- ÜRETİM: PixelLab Create Image S-XL + karakter-anchor init-image stil transferi, **KULLANICIYLA (GATED)** —
  10 sprite (+ikiz silahlar çift mi tek sprite mi? advisor düşünsün).
- ⚠️ KOD DURUMU BELİRSİZ: HandAnchor/OrientationSync/WeaponSorter kodda VAR MI yoksa sadece tasarım kilidi mi —
  Opus-advisor DOĞRULASIN (`Assets/Scripts/` grep). `Characters/eski_anchors/*` metadata silinmiş (git) —
  el-anchor verisi yeniden mi çıkarılacak?
### B. SİLAH-ELE-OTURTMA (kullanıcının 2 numarası)
Per-class per-yön HandAnchor offset tuning (10 sınıf × 8 yön; mirror'la 5). Veri-bazlı olmalı (kullanıcı ayarlayabilsin).
### C. ANİMASYONLAR (kullanıcının 3 numarası) — KARAR BUGÜN VERİLDİ
`STAGING/CODEANIM_DECISION_2026-06-05.md`: DEMO'DA ÜRETİM SIFIR; kod-only T1 işleri: [M] knockback-birleştirme+
KnockdownDriver+3 profil SO+Broken-tetik+i-frame · [S] mob-ölüm squash-decal · [S] spawn/dash-ghost.
### D. DİĞER KALAN İŞLER
- **B-11 combat lifecycle** (clear→slow-mo→reward→kapı yak→walk-into-door→sonraki oda) — DÖNGÜNÜN KALBİ, eksik.
- **Chamber** (cx in-flight: P0-P4) + sonrası feel-test.
- **B-12 production RoomBank** (15 oda hazır; bank+pacing+depthCount 10-15) + modüler-props kalanı
  ([S]küme-prop authoring ✅audit'li · [M]checker+auto-placer-koşturma).
- Echo award play-verify (kod hazır) · Run-içi GECE·3 backlog (draft-kart hover/TooltipSystem ✓kısmen,
  sol skill paneli, ESC codex) · Skill ikonları (PixelLab GATED) · Mob görselleri (PixelLab GATED) ·
  Elementalist büyü-VFX Phase-1 (kod/particle).

## SORULAR
1. **SIRALAMA:** Kullanıcı önceliği (silah→oturtma→anim→diğer) ile teknik bağımlılıkları birleştir: tek
   sıralı plan (madde madde, her maddeye S/M/L + kim: cx/Flash/Sonnet/KULLANICI-gated). Kritik yol nedir?
   Neler PARALEL koşar (örn. kullanıcı PixelLab'da silah üretirken cx B-11 yazar)?
2. **Silah üretim oturumu tasarımı:** 10 silahı tek oturumda mı, 2'li pilot (Warblade+Ronin) + kalan 8 mi?
   İkiz silahlar (hançer/balta/tabanca) tek sprite mi çift mi? Anchor-fitting işini üretimle nasıl zincirlersin?
3. **Sıralamada risk:** Silahlar combat-feel'i değiştirir — B-11'den ÖNCE mi sonra mı daha az rework?
   Knockdown ne zaman (B-11 ile birlikte mi)?
4. Plan 2-3 "oturum"a bölünsün (kullanıcılı oturumlar vs otonom bloklar) — net teslimatlı.

## Çıktı formatı
Sıralı roadmap tablosu (faz → işler → boyut → kim → bağımlılık) + kritik yol + paralellik şeması + 2-3 risk.
