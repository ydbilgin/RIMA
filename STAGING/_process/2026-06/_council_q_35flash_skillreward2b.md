# Council — ax Gemini 3.5 Flash (High) — LEAN / ship-fast / over-engineering critique lens

ANALYSIS ONLY. Read-only. Hiçbir dosyayı değiştirme, Unity'yi açma.

Repo: F:\Antigravity Projeler\2d roguelite\RIMA
Diff: `git diff Assets/Scripts/Skills/DraftManager.cs Assets/Scripts/Core/ChestBehavior.cs`
Gerekirse DraftManager.cs / ChestBehavior.cs ilgili metodları READ et.

## Fix'ler
- FIX1 = AssignActive: skillType==null → erken return + warning.
- FIX3 = ShowDraftWithSkill: IsDraftActive/IsDraftPending guard + EnsureDependencies() + IsDraftActive=true.
- FIX2 = ChestBehavior: GetAll() yerine isImplemented-filtreli inline liste.

## Lean sorular (demo ~20 Haz; en az kod, en hızlı doğru)
1. **En kritik canlı risk:** Bu 3 fix içinde demo'yu BOZABİLECEK tek bir şey var mı? (özellikle: FIX3 IsDraftActive false'a dönmüyorsa draft kilitlenir — diff+kod ile hızlı doğrula, evet/hayır.)
2. **Over-engineering / gereksiz kod:** Fix'ler gereğinden fazla mı? Daha yalın bir yol var mıydı (örn. FIX2 tek satır LINQ, FIX1 mevcut iç guard yeterli miydi)?
3. **Eksik basit kontrol:** Ucuza eklenebilecek (1-2 satır) ama atlanmış bir guard/log var mı?
4. Commit'e GİDER mi, yoksa önce düzeltilmesi şart bir şey mi var?

## Çıktı
NET PASS/FAIL + en fazla 3 yüksek-güven bulgu (1-cümle her biri). Kısa tut.
