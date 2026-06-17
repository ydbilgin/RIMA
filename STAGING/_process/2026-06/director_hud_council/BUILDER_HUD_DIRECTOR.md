# BUILDER-OPUS — #6 HUD sol-alt + #8 Director güzelleştir/scroll (2026-06-18)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS gerekmez (kod işi). cx KULLANMA. **Play moda GİRME** (orchestrator görsel doğrular). **git commit YAPMA. `git checkout/reset` YASAK.**
UNITY ERROR CHECK: iş bitince `mcp__UnityMCP__refresh_unity`(scripts,request) + `read_console`(Error) → kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR. 0 error şart.
Spec kaynağı: `STAGING/PLAYTEST_POLISH_DECISION_2026-06-17.md` + `STAGING/_process/2026-06/director_hud_council/AX_PRO_VERDICT.md`.

İki görev, ikisi de runtime-built UI (prefab yok, kod üretir). Cerrahi kal.

## GÖREV A — #6 HUD sol-alta + modernize (`Assets/Scripts/UI/HUDController.cs`)
Şu an HUD **top-left** (tüm Build* metotları anchor (0,1), pivot üst, `-HudMargin` ile aşağı). ax_pro spec'i (Hades/Diablo, sol-alt):
- Tüm bar grubu **bottom-left**: anchorMin/Max (0,0), pivot bottom-left.
- **HP bar** (üstte/büyük): anchoredPosition (24,30), sizeDelta (260,20), crimson `#C01020`. Track (arka) = slate `#1B1F28` alpha 0.8 (simsiyah DEĞİL).
- **Resource bar** (HP'nin hemen altı, ince): anchoredPosition (24,16), sizeDelta (220,8), cyan `#10A0C0`.
- **Echo + ODA metni**: barların ÜSTÜNE, ~(24,60)'tan başla, italik, alpha 0.7, minimal.
- Mevcut sabitler: `HpBarWidth=212 HpBarHeight=16 ResBarWidth=160 ResBarHeight=10 HudMargin=12`. Yeni boyutlara güncelle (HP 260×20, Res 220×8) ama fill mantığını (`hpFill.sizeDelta = HpBarWidth*pct`) BOZMA — width sabitini doğru kullan.
- Build metotları: `BuildHpBar`(595), `BuildResourceBar`(648), `BuildEchoDisplay`(686), `BuildRoomNameLabel`(742). Bunların anchor/pivot/pos'larını bottom-left'e çevir + stack yönünü yukarı yap. Düşük-HP vignette/pulse mantığına DOKUNMA.
- `roomBannerLabel` (center entry banner) ve `BuildInteractionPrompt` (bottom-center) YERİNDE KALSIN — sadece HP/Res/Echo/RoomName sol-alta.

## GÖREV B — #8 Director Mode scroll + güzelleştir (`Assets/Scripts/UI/DirectorMode.cs`)
ax_pro spec'i:
- **SCROLL FIX (öncelik):** Skill listesi kartlarının kökü bir `ScrollRect`'e bağlanmalı. Hiyerarşi: `SkillPanel → Viewport(Image + Mask/RectMask2D) → Content(VerticalLayoutGroup + ContentSizeFitter)`; ScrollRect.content = skill kartları kökü, vertical=true, horizontal=false. Şu an liste taşıyor → son skill "Gravity Cleave" kesik. Bunu çöz.
- **Kart stili:** kart bg zeminle aynı olmasın → `#252A35` (DirectorRaised); seçili kart cyan ince outline. Skill adı + açıklama okunur spacing/padding.
- **Bind UX:** alttaki kopuk Q/E/R/F ATA + LMB/RMB BASIC butonlarını, seçili skill detayının altına "Bind to:" başlığıyla derli toplu grupla (mümkünse). Riskliyse SADECE scroll + kart stilini yap, bind'i not düş.
- DirectorMode'un temel mimarisine / serbest-kam / timeScale / Spawn-Stats-Build-Map-Telemetry sekme mantığına DOKUNMA. Sadece skill paneli görseli + scroll.

## Çıktı
`STAGING/_process/2026-06/director_hud_council/BUILDER_HUD_DIRECTOR_DONE.md`'ye ≤25 satır: ne değişti (dosya+metot), console durumu (0 error?), riskli/atlanan kısım. Dönüşte ≤10 satır özet + bu yol.
