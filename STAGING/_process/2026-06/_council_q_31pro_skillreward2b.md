# Council — ax Gemini 3.1 Pro (High) — DEEP / architecture lens

ANALYSIS ONLY. Read-only. Hiçbir dosyayı değiştirme, Unity'yi açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff'i al: `git diff Assets/Scripts/Skills/DraftManager.cs Assets/Scripts/Core/ChestBehavior.cs`
Sonra DraftManager.cs ve ChestBehavior.cs'de ilgili metodları + çağıran/state-yöneten yolları READ et.

## Fix'ler
- FIX1 = `DraftManager.AssignActive`: skillType==null ise erken return + warning (eskiden dead entry ekliyordu).
- FIX3 = `DraftManager.ShowDraftWithSkill`: IsDraftActive/IsDraftPending guard + EnsureDependencies() + IsDraftActive=true.
- FIX2 = `ChestBehavior.BuildChestOffers`: GetAll() yerine isImplemented-filtreli liste.

## Derin sorular (state-machine / mimari lens)
1. **FIX3 draft state-machine bütünlüğü:** ShowDraftWithSkill artık IsDraftActive=true set ediyor ama normal ShowDraft yolu bunu nasıl yönetiyor (set + reset)? İki yol arasında state lifecycle TUTARLI mı? Skill seçilince VE skip/iptal edilince IsDraftActive false'a dönüyor mu? Dönmüyorsa kalıcı kilit riski — kanıtla (dosya:satır).
2. **FIX1 kontrat:** AssignActive'in "her pick'i tüketir" varsayımı var mı? Erken return draft akışının invariant'larını (slot sayacı, currentActiveSkills tutarlılığı, UI kapanışı) bozuyor mu?
3. **FIX2 doğruluk:** chest pool için "tüm isImplemented" doğru tasarım mı, yoksa class/act/rarity gibi başka kısıt da gerekir mi (RIMA draft tasarımına göre — gerekirse NLM sorgula)?
4. Daha temiz/sağlam bir alternatif tasarım var mı (over-engineering'e kaçmadan)?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (severity · dosya:satır · 1-cümle + öneri). #1 için net evet/hayır.
