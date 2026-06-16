# B1 — Reward kart tooltip dikey-şerit fix (U1)

ACTIVE RULES: (1) think before coding (2) min code, surgical (3) sadece tooltip/SkillOfferUI dosyaları (4) BLOCKED if unclear.
UNITY ERROR CHECK: read_console (Error+Warning) = 0; kendi hatanı çöz, ilgisiz/önceden-var bildir; raporla.
PERSISTENCE: edit sonrası `git status --short` + `git diff --stat` çalıştır, çıktıyı done dosyasına yapıştır (diske yazıldı kanıtı).
E1: done dosyasına yaz, dönüş ≤10 satır.

## Belirti (kullanıcı canlı playtest + screenshot)
Reward draft kartına mouse ile gelince metin **tek karakter/satır DİKEY şerit** halinde çıkıyor (screenshot: orta kart "OPPORTUNISTIC STRIKE / CC altındaki hedefe" dikey). = hover-tooltip genişliği 0'a çöküp dikey sarıyor.

## KARAR (kullanıcı onayladı)
1. **Reward draft kartlarında hover-tooltip'i KAPAT.** Kart zaten tam açıklamayı (başlık + body) gösteriyor → tooltip gereksiz + buggy. SkillOfferUI'da kartlara tooltip kaydı/tetiği varsa kaldır/devre dışı bırak.
2. **TooltipSystem'i genel olarak onar** ki başka yerlerde (skill bar ikonları gibi kompakt yerler) dikey-sarmasın: TMP `enableWordWrapping=true` + sabit/preferredWidth (ya da ContentSizeFitter horizontal preferred + max width). Genişlik 0'a düşmesin.

## Yer (incele)
- `Assets/Scripts/UI/TooltipSystem.cs` (tooltip render — width/wrapping).
- `Assets/Scripts/UI/SkillOfferUI.cs` (reward kart hover → tooltip tetiği; burada kartlarda tooltip'i kapat).

## Doğrulama (GATE)
- Compile + read_console = 0 error.
- Mümkünse: reward kart hover yolunda tooltip tetiği artık YOK (kod-kanıt) + TooltipSystem genişlik > 0 (dikey-sarma imkansız).
- Görsel teyit (kart hover'da dikey-şerit YOK) = orchestrator play-verify'da alacak.

## Kısıt
8-yön canon LOCKED (dokunma). RewardPickup/DraftManager/DungeonGraph'a DOKUNMA. Surgical, no refactor.

## Rapor (done, ≤10 satır)
VERDICT + root-cause + değişen dosyalar + uygulanan (kartlarda kapat + TooltipSystem wrapping) + console 0-error + git status çıktısı.
