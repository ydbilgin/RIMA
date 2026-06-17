# BUILDER_HUD_DIRECTOR — DONE (2026-06-18)

## GÖREV A — HUDController.cs (sol-alt + modernize)
- Sabitler güncellendi: HpBarWidth 212→260, HpBarHeight 16→20, ResBarWidth 160→220, ResBarHeight 10→8. Yeni layout/renk const'lari eklendi (HudLeft=24, HpBarBottom=30, ResBarBottom=16, TextStackBottom=60; HpFillCrimson #C01020, ResFillCyan #10A0C0, BarTrackSlate #1B1F28 @0.8).
- `BuildHpBar`: track anchor/pivot bottom-left (0,0), pos (24,30), size (260,20); track rengi slate #1B1F28 a0.8; fill init crimson #C01020. HP label bottom-left, barin sag-ust kenarinda.
- `BuildResourceBar`: bottom-left (24,16), size (220,8); track slate; fill init cyan #10A0C0.
- `BuildEchoDisplay` + `BuildRoomNameLabel`: barlarin USTUNE tasindi (Echo 24,60 / RoomName 24,78), italik. Echo label a0.92→a0.70 italic.
- Fill mantigi (`hpFill.sizeDelta = HpBarWidth*pct`) BOZULMADI. Vignette/pulse/HitFlash, roomBanner, InteractionPrompt, ControlHint DOKUNULMADI.

## GÖREV B — DirectorMode.cs (skill scroll + kart stili)
- SCROLL FIX: `SkillCards` artik ScrollRect host → `Viewport`(Image a0 + RectMask2D) → `Content`(`classSkillCardsRoot`: GridLayout tek-kolon + ContentSizeFitter PreferredSize). vertical=true, horizontal=false, Clamped. Gravity Cleave dahil tum liste artik kaydirilabilir. RebuildSkillCards/AddSkillCard kart-ekleme yolu degismeden Content'e yaziyor.
- KART STILI: kart bg sabit #252A35 (DirectorRaised); secim cue = ince cyan Outline (effectDistance 2,-2; secili degilken alpha 0). RefreshSkillCards artik bg yerine outline alpha'sini degistiriyor → secili kart cyan outline.
- DirectorMode temel mimari/timeScale/serbest-kam/sekme mantigi DOKUNULMADI.

## RISKLI / ATLANAN
- BIND UX (Q/E/R/F + LMB/RMB gruplama "Bind to:" basligi altinda): ATLANDI, brief'in kacis yolu kullanildi. Sebep: (1) butonlar zaten fonksiyonel + derleniyor; (2) yeniden gruplama = Play-test olmadan riskli layout refactor; (3) "Bind to:" basligi yeni loc key gerektiriyordu → Loc.cs scope disi (brief sadece HUDController+DirectorMode listeliyor). Mevcut sabit-konum bind butonlari yerinde, fonksiyonel.
- HP/resource fill renk: build'de init crimson/cyan; OnHPChanged/OnResourceChanged value-driven threshold/class-tint'i RUNTIME'da uyguluyor (bu mantiga dokunulmadi, min-code). Sol-alt + boyut + track-slate + secim gorunumu spec'e uygun.
- HUDController'da `HudMargin` const artik referanssiz (zararsiz, derleme etkilenmiyor).

## CONSOLE
- refresh_unity(scripts, request) + read_console(Error) → **0 error**.
- validate_script (standard, her iki dosya) → errors:0. Uyarilar pre-existing (GameObject.Find/string-concat in Update) — degisikliklerle ilgisiz.
- Play moduna GIRILMEDI (orchestrator gorsel dogrulayacak).
