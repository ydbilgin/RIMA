ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Run-sonu Shattered Echo kazanımı (meta-currency earn akışı HİÇ yok — council kararı `STAGING/CHARSELECT_V3_DECISION_2026-06-05.md` §Ekonomi). Oyuncu run bitirince (ölüm VEYA demo-complete) Echo kazanır, kalıcı bakiyeye eklenir, ekranda "+n ◈" görünür.

# Bağlam (kendi raporundan — CODEX_DONE_yasinderyabilgin.md)
- Bakiye ŞU AN sadece `CharacterSelectScreen` içinde: `EchoBalancePrefsKey` + demo-200 init (`Assets/Scripts/UI/CharacterSelectScreen.cs:37-40`), read/write `:951-960`, spend `:973-980`. PlayerPrefs tabanlı.
- `RunStats` kills/rooms/time tutuyor, currency YOK (`Assets/Scripts/Core/RunStats.cs:13-21`).
- Death ekranı oda/kill/süre gösteriyor (`Assets/Scripts/Core/DeathScreenManager.cs:160-163`); `DemoCompleteOverlay.cs:104-107` aynı.

# İş (cerrahi, minimum)
1. **`EchoWallet` static helper (YENİ, küçük):** `Assets/Scripts/Systems/EchoWallet.cs` — `Balance` (get), `Add(int)`, `TrySpend(int)`, aynı PlayerPrefs key + demo-200 init mantığı CharacterSelectScreen'den TAŞINIR (davranış birebir korunur). `CharacterSelectScreen` bu helper'ı kullanacak şekilde güncellenir (UI/görsel koduna DOKUNMA — sadece bakiye okuma/yazma çağrıları).
2. **Award formülü (tunable):** `EchoWallet.ComputeRunAward(RunStats)` — demo'da Act yok → oda-bazlı proxy: `roomsCleared * 3 + kills / 5`, clamp [5, 60]. Sabitler const/tunable alan olarak. Hedef his: ortalama run ~20-40 ◈ (decision doc).
3. **Award tetikleme:** ölümde (DeathScreenManager akışında uygun tek nokta) + demo-complete'te (DemoCompleteOverlay). ÇİFT-AWARD GUARD şart (aynı run iki kez ödeme yapmasın — RunStats'a awarded flag veya benzeri minimal çözüm).
4. **UI satırı:** Death ekranı + DemoCompleteOverlay istatistik satırlarına "+{n} ◈" eklenir (mevcut satır stilini aynen taklit et; yeni panel/kutu YOK). Glif "◈" literal string.
5. **EditMode test (1 dosya, 2-3 test):** ComputeRunAward formül sınırları (clamp alt/üst + örnek orta değer). Mevcut test klasör konvansiyonu: `Assets/Tests/EditMode/`.

# Doğrulama
- `dotnet build` ile compile-verify (önceki sessionlarda RIMA.Runtime.csproj ile yaptın). Unity açıksa UnityMCP `read_console` ile CS hatası kontrolü; açık değilse compile-verify yeterli, play-verify PENDING raporla.

# YASAK
- CharacterSelect UI/layout/label değişikliği (o ayrı task — mockup onayı bekliyor).
- Yeni ekonomi manager/singleton sınıf hiyerarşisi (static helper yeter).
- PlayerEconomy (run-içi Gold) dosyasına dokunmak.

# Çıktı
CODEX_DONE.md: değişen dosyalar + satır aralıkları, formül, guard mekanizması, build sonucu, play-verify durumu.
