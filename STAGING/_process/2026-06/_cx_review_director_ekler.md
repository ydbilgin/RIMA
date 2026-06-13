ACTIVE RULES: (1) think before judging (2) evidence-based — diff'i kendin oku (3) surgical — SADECE git diff/dosya oku + en fazla BİR EditMode koşusu, başka HİÇBİR ŞEY değiştirme (4) emin değilsen UNCERTAIN.
NLM ACCESS: Gerekmez.

# Amaç — DIRECTOR EKLERİ REVIEW (writer=Claude Opus sub-agent; sen bağımsız reviewer'sın)
Council-onaylı 2 özellik: DUAL-CLASS DRAFT butonu + stat preset butonları. Tek dosya diff: `Assets/Scripts/UI/DirectorMode.cs` (uncommitted). Writer raporu: `STAGING/_process/2026-06/_opus_director_ekler_2026-06-13.md` + karar: `STAGING/DEMO_DIRECTOR_EKLER_DECISION_2026-06-13.md` (ikisini önce oku).

## Doğrula:
1. **DUAL-CLASS butonu:**
   (a) Görünürlük: yalnız secondary class YOKKEN; seçim sonrası gizleniyor mu (her frame mi event mi — maliyet makul mü)?
   (b) `TriggerClassSelection()` çağrısı güvenli mi: null-guard (PlayerClassManager.Instance yokken), çift-tık koruması (UI açıkken ikinci basış)?
   (c) **🔑 ANA EDGE-CASE — timeScale sahipliği:** Buton Director pause'da (timeScale=0) basılıyor; seçim UI'ı da timeScale=0 kullanıyor. Seçim TAMAMLANINCA timeScale'i kim, hangi değere geri veriyor? Director hâlâ açıksa 0 kalmalı (Director sahibi), Director kapatılırsa 1 olmalı. Bugün eklenen `ResolveTimeScaleForState`/death-guard mantığıyla çakışma var mı? Kod yolunu satır satır izle — Play Mode koşamazsın, statik analiz yeter ama net ol.
   (d) Ölüm ekranı açıkken butona basılırsa ne olur (death-state + selection çakışması)?
2. **Stat preset'leri:** Mevcut slider yolundan mı geçiyor (yeni stat uygulama yolu YOK şartı — karar dosyası)? Preset değerleri slider clamp aralığında mı? VARSAYILAN gerçekten profil default kaynağını mı kullanıyor (Quick Reset ile tutarlı)?
3. **Scope:** Diff SADECE DirectorMode.cs mi? Başka dosya sızmış mı?
4. **EditMode flake iddiası:** Writer 12 fail gördü (baseline 11), "timing/DDOL flake, ilgisiz" diyor. EditMode'u BİR KEZ koş (assembly RIMA.Tests.EditMode): fail sayısı ve fail İSİMLERİ baseline'la uyumlu mu? DirectorMode-ilişkili fail var mı? (Flake iddiasını doğrula veya çürüt.)

## RAPOR → `CODEX_DONE_<kendi profilin>.md`
Madde başına PASS/FAIL/UNCERTAIN + kanıt (satır no). Sonda: GENEL VERDİKT + commit'e uygun mu.
