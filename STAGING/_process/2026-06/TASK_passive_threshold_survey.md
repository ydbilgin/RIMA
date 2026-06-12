ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ANALYSIS ONLY — KOD YOK, COMMIT YOK, TEST YOK. Sadece tarama + rapor.
Soru: RIMA'nın TÜM passive skill'leri arasında kaç tanesi "EŞİK → STAT MODIFIER (enter/exit histeresisli)" kalıbına TEMİZ uyuyor? Bu, `Passive_StatThreshold` generic component'inin yazılmaya değip değmeyeceğini belirleyecek (önceki adımda AOE/targeting konsolidasyonu test edildi → gerçek duplikasyon çıkmadı, bu yüzden generic'i körlemesine yazmadan önce ampirik kanıt istiyoruz).

## Kalıp tanımı (TEMİZ uyum = hepsi doğru olmalı)
1. Bir EŞİK izleniyor: HP ratio veya resource ratio bir değerin altına/üstüne geçiyor.
2. Eşik aşılınca bir STAT modifier UYGULANIYOR (örn rage.gainMultiplier += x, moveSpeed *= y).
3. Eşik geri aşılınca modifier GERİ ALINIYOR (enter/exit histeresis — sürekli ekleme yapmıyor).
Referans temiz örnek: `Assets/Scripts/Skills/Passives/WarbladePassives.cs:66` `Passive_WrathProtocol` (HP<%50 → rage gain +x, HP>%50 → geri al, `wasLowHP` flag'iyle).

## UYMAYAN örnekler (bunları NO say)
- Event-driven (on-kill heal, on-knockback rage) → eşik yok. Örn Passive_BloodDrinker, Passive_BerserkerResolve.
- Floor/clamp (her frame değeri sabitleyen) → modifier uygula/geri-al değil. Örn Passive_TemperedFury.
- Tek-seferlik kalıcı buff (eşiksiz, level-up'ta uygula, geri alma yok).
- Koşullu hasar/proc (stat modifier değil).

## İŞ
1. TÜM passive class'larını bul ve oku: `Assets/Scripts/Skills/Passives/**` + sınıf-spesifik dosyalarda olabilecek passive'ler (10 sınıf: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer). `PassiveBase`'den türeyen her şeyi tara. Grep: `: PassiveBase`.
2. Her passive için tablo satırı: **Passive adı | Sınıf | Dosya:satır | FIT (YES/NO) | NO ise neden (event/floor/oneshot/proc/diğer)**.
3. Özet sayım: kaç passive TEMİZ FIT, kaç NO, kaç belirsiz.
4. KARAR ÖNERİSİ: FIT sayısına göre `Passive_StatThreshold` generic'i yazmaya değer mi?
   - Pratik eşik: **≥3-4 farklı sınıfta temiz FIT varsa generic değer** (tek generic çok pasifi sadeleştirir).
   - **1-2 FIT ise generic OVERKILL** — WrathProtocol bespoke kalsın, generic yazma (bu durumda video'nun "her şeyi modülerleştirme" uyarısı geçerli).
5. Belirsizlik varsa BLOCKED yaz, tahmin etme.

## YAPMA
- Hiçbir kod yazma/değiştirme. Generic'i ÜRETME (bu task sadece karar verisi topluyor).
- Commit etme. Test çalıştırma.

## CODEX_DONE'a yaz
- Tablo (tüm passive'ler).
- Sayım (FIT / NO / belirsiz).
- Net karar önerisi (generic YAZ / YAZMA) + gerekçe.
