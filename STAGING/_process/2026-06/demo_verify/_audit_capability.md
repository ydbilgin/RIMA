# CAPABILITY AUDIT — golden-path live-test "duvar"lari bagimsiz denetim (2026-06-16)

Denetci: MCP-yetenek + test-plani auditor. Kaynak: tool-def + ManageScene.cs + ScreenshotUtility.cs (MCP 9.7.3).

---

## VERDICT OZET
- **W2 (SCREENSHOT) = YANLIS.** Orchestrator'in "overlay UI screenshot'a cikmaz" iddiasi, kullanilan
  cagri sekli icin GECERSIZ. Kosul saglandiginda overlay DAHIL yakalar. Bu nedenle bircok beat data-proof'a
  ek olarak GORSEL teyit de edilebilirdi; orchestrator gorseli gereksiz yere kullaniciya atti.
- **W1 (INPUT) = BUYUK OLASILIKLA GECERLI ama KANIT EKSIK.** `wasPressedThisFrame=False` gozlemi
  gercek (one-frame edge'i editor-pump'la basmak zor); AMA orchestrator kendi yaziyor: cift-play-session +
  EnsureOwns key-registry kirliligi ELENMEDI. Temiz-sahne + tek-session retry yapilmadan "kesin imkansiz"
  demek erken pes. (Mandatim W2; W1 = ikincil not.)

---

## W2 KANITI (kod, tartismasiz)
- `ManageScene.cs:594-628`: camera YOK + `include_image:true` + `Application.isPlaying` ise
  `ScreenshotUtility.CaptureComposited(...)` cagrilir (camera-render YOLU DEGIL).
- `ScreenshotUtility.cs:199-201` docstring: `ScreenCapture.CaptureScreenshotAsTexture` = "final composited
  frame **including UI overlays**". `manage_camera` tool-def de ayni: "no camera -> ScreenCapture API
  captures ALL layers including Screen Space-Overlay UI".
- **9.7.3 'wait for end-of-frame' fix GERCEKTEN VAR ve aktif:** `ScreenshotUtility.cs:218-223` play-mode'da
  `CaptureCompositedAfterFrame` -> `ScreenshotCapturer` MB `yield return new WaitForEndOfFrame()` (sat:763-766)
  SONRA capture. Tam olarak "backbuffer'i UITK composite olmadan okuma" bug'ini cozer. Eski "play'de ScreenCapture
  PATLAR/kirmizi hata" memory'si bu fix ile SUPERSEDED.
- KOSUL (uymazsan eski davranis): camera parametresi VERME + `include_image:true` + Play-mode'da ol. Camera
  verirsen camera-render -> overlay HARIC (tool-def dogru). recipe'lerdeki "scene_view kullan, game-view patlar"
  notu artik konservatif/bayat.

## (b) GORSEL DOGRULANABILIR ama kullaniciya ATILAN beat'ler
- **B0 panels estetik** (Pause/Settings/Codex/HUD "guzel mi", tasma, font): overlay-composited SS ile
  ORCHESTRATOR teyit edebilirdi; "overlay SS'a cikmaz" gerekcesiyle tumuyle kullaniciya atilmasi YANLIS.
- **B3 UI-01 footer cokmesi** (Desc dikey serite cokuyor mu): GORSEL imza; composited SS net gosterir.
  Sadece rect/lineCount proxy'sine indirgemek gereksiz korluk.
- **B7 director overlay-bleed** (Test'e gecince overlay kayboldu mu): `Canvas_DirectorOverlay` ScreenSpaceOverlay
  -> composited SS A1(gorunur)/A2(yok) FARKINI dogrudan gosterir; data-proof'a gorsel eklenebilirdi.
- B6 BuildPaletteCanvas / reward-spawn dunya gorseli de composited SS ile gorulebilir (input ayri mesele).
- Not: SS GORSELI ile INPUT'u karistirma — SS, "panel acildiginda nasil gorunur"u kanitlar; tetigi (F2/G/ESC)
  basmaz. Yani input duvari dogru olsa bile, state'i execute_code ile kurup composited-SS estetik teyidi
  YAPILABILIRDI; bu firsat kacirildi.

## Plan/sonucta diger hatali "yapilamaz" / zayif noktalar
- Plan B0 sat:51/69 + tum recipe'ler "overlay SS'a CIKMAZ" diyor -> YANLIS on-kabul; W2 ile cusur (composited yolu var).
- Plan tutarli "scene_view + data-proof" varsayimi: composited game-view ile artik GEREKSIZ; plan bunu hep
  fallback sayiyor, halbuki 9.7.3'te ana yol olabilir.
- DOGRU "yapilamaz"lar (bunlar gecerli, pes degil): execute_code overlay-UI'yi render-target sorunundan
  cizdiremez kabulu zaten composited-SS ile asiliyor; tek-Unity-ajan serisi dogru.

## DUZELTME (aksiyon)
1. Plan + 7 recipe'deki "overlay SS'a CIKMAZ" ifadesini guncelle: "camera-render'da cikmaz; **camerasiz +
   include_image:true + Play = composited ScreenCapture overlay DAHIL yakalar (9.7.3 end-of-frame fix)**".
2. B0-estetik / B3-UI01 / B7-bleed icin: state'i execute_code ile kur -> camerasiz `manage_camera screenshot
   include_image:true` cek -> gorseli rapora ekle (kullaniciya tum-yuk ATMA, sadece nihai juri-provasi kalsin).
3. W1: "imkansiz" demeden once TEK temiz Play-session + key-registry temiz + InputSystem.QueueStateEvent
   ardindan InputSystem.Update() (Update icinde edge yakalansin) ile en az 1 retry; sonucu LIFE/duvar olarak isaretle.
