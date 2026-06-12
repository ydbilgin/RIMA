# Claude’a Gönderilecek Prompt — RIMA Sandbox / Director Mode

Bu zip’te ChatGPT’nin RIMA Sandbox/Director Tool için paralel tasarım review’u, bitmiş HUD mockup görselleri ve uGUI hiyerarşi önerisi var.

Önce şu dosyaları oku:
1. `docs/A_design_review.md`
2. `docs/C_uGUI_hierarchy.md`
3. `docs/D_extra_ideas.md`
4. `mockups/01_director_spawn_mockup.png`
5. `mockups/02_director_stats_mockup.png`
6. `mockups/03_director_build_cliff_mockup.png`

Görev:
- Mevcut council tasarımını bu review ile karşılaştır.
- uGUI prefab yapısını Unity projesine uygulanabilir şekilde task list’e çevir.
- Önce implementation plan çıkar, direkt kod yazma.
- Özellikle şu riskleri doğrula:
  - `EncounterController.SpawnEnemy(id, pos)` public imzası var mı?
  - `PaintCell(cell,bool)` public yapılınca eski IMGUI overlay çakışır mı?
  - `CliffAutoPlacer.Regenerate()` scoped çalışıyor mu, yoksa tüm sahneyi eziyor mu?
  - Director kamera/tween/input tamamı `unscaledDeltaTime` kullanıyor mu?
  - Chrome sprite import ayarları Sliced/Point/No Compression olarak yapılmış mı?

Öncelik sırası:
1. Director/Test mode skeleton + free-cam + chrome UI shell
2. Spawn tab + placement ghost + delete/right click
3. Quick snapshot/reset
4. Stats tab + ClassStatRuntime sliders
5. Telemetry strip + DPS/TTK
6. Build tab tile/cliff/prop hook
7. Map tab node jump
8. Presentation mode polish

Mockup’lara birebir kopya gibi değil, uygulanabilir UI hedefi gibi bak. Görsel dil: RIMA chrome kit, void mor, ember accent, slate body, cyan sınırlı.
