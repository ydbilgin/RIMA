# Review — T4 hover tooltip commit (69ebdd2a) — READ-ONLY

Unity proje kökü: "F:/Antigravity Projeler/2d roguelite/RIMA". Kod DEĞİŞTİRME, commit YAPMA.

`git show 69ebdd2a` diff'ini incele (SkillOfferUI.cs / SkillBarUI.cs / TooltipSystem.cs):
1. Hover → TooltipSystem.Show/Hide bağlantısında leak/çakışma var mı (kart destroy edilirken tooltip açık kalır mı; timeScale=0'da tooltip delay çalışır mı — unscaled time kullanılmış mı)?
2. Synergy pulse: SkillBarUI'a eklenen pulse metodu yan etkisiz mi (mevcut ready-flash/cooldown görselleriyle çakışma)?
3. Stil değişikliği TooltipSystem'de başka caller'ı bozar mı (CharacterSelectScreen :1613-1628 de kullanıyor)?

Çıktı: `STAGING/_review_T4_hover_flash.md` — verdict PASS/PASS-WITH-NOTES/FAIL + file:line bulgular.
