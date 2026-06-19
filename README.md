# RIMA

RIMA, bitirme projesi olarak geliştirilen Unity 2D roguelite aksiyon-RPG'sidir. Oyuncu bir sınıf seçer (yakın dövüş için **Warblade** veya büyücü için **Elementalist**), prosedürel olarak birleştirilen odalardan ilerler, düşmanlarla ve bir act boss'uyla savaşır, odalar arasında yetenek draft eder. Oyunun kendisinin ötesinde, RIMA'nın asıl merkezini özel seviye-tasarım araç zinciri oluşturur: birleşik bir **Map Designer** editör penceresi ve oyun-içi bir **Build Mode (F2)** — ikisi de tek bir paylaşılan çekirdek üzerinden aynı oda-şablonu verisini düzenler. Bu sayede RIMA, oynanabilir bir dikey-dilim (vertical slice) olduğu kadar yeniden kullanılabilir bir seviye-tasarım ortamıdır.

---

## 🚀 Nasıl Çalıştırılır

1. Projeyi **Unity** ile açın (depo kökü doğrudan bir Unity projesidir: `Assets/` + `Packages/` + `ProjectSettings/`).
2. Demo/savaş sahnesi: **`Assets/Scenes/_Arena.unity`**.
3. Çalıştırma akışı:
   - Tam akış her zaman ilk build sahnesinden başlar: **MainMenu → CharacterSelect → Chamber → _Arena**. Bunu sağlayan editör kancası `PlayFromStartScene` (Play modunda `playModeStartScene`'i ayarlar).
   - Hızlı oda iterasyonu için, **`RIMA/Play From Main Menu`** menü öğesini kapatın — böylece açık olan sahneden doğrudan Play'e basabilirsiniz.

### Oyun-İçi Kısayollar

| Kısayol | İşlev |
|---|---|
| **WASD** | Hareket |
| **Sol Tık (LMB)** | Temel saldırı |
| **Q / E / R / F** | 4 yetenek slotu |
| **F2** (alias: `"` ) | Oyun-içi **Build Mode**'u aç/kapat (kamera geri çekilir, oynanış duraklar) |
| **` (backquote)** | **Director Mode** (ham stat/spawn/telemetry overlay'i) |

---

## 🗺️ Özellik → Dosya Haritası

Bu bölüm her özelliği onu sağlayan kaynak dosyaya eşler; yola tıklayarak ilgili kodu bulabilirsiniz.

### Editör Araçları

| Özellik | Açıklama | Anahtar Dosya(lar) |
|---|---|---|
| **Map Designer** (editör penceresi) | `RIMA/Map Designer` editör penceresi: oda/seviye tasarımı için Library, Floor, Cliff, Object, Portal, Light, Layers sekmeleri. Yüzeyden-bağımsız çekirdeğin ince bir GÖRÜNÜM katmanıdır; F2 overlay'i ile aynı veri yolunu paylaşır, asla sapamaz. | `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`<br>`Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs`<br>`Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` |
| **Room Painter / Room Designer** (fırça + şablon) | Oda-şablonu tasarım alt sistemi: katmanlı fırça/stroke pipeline'ı (tile, stamp, scatter, decal executor'ları) + oda-şablonu verisi, kaydet/yükle ve dekorasyon geçişleri. Şablonlar, hem editörün hem runtime'ın tükettiği `RoomTemplateSO` asset'leridir. | `Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs`<br>`Assets/Scripts/MapDesigner/Brush/Executors/Editor/BrushExecutorRouter.cs`<br>`Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`<br>`Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs`<br>`Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateLoader.cs` |
| **Build Mode (F2)** — yaşam döngüsü/aç-kapat | Oyun-içi seviye editörünün giriş noktası: aç/kapat kamerayı geri çeker, DirectorMode durumu üzerinden oynanışı duraklatır ve Build sekmesini zorlar. F2 (ve `"`) toggle'ının TEK sahibidir; yeni Input System ile (`Key.F2`) `InPlayToolKeyRegistry.RegisterExclusive` üzerinden talep edilir. | `Assets/Scripts/UI/BuildModeController.cs`<br>`Assets/Scripts/UI/BuildMode/InPlayToolKeyRegistry.cs` |
| **Build Mode** — tile/walkability fırçası | Build Mode Faz 3: hücre-otoriter bir tile / walkability / overlay fırçası; oynanış sırasında gerçek zamanlı olarak ayrık `Grid SetTile` + `RoomTemplateSO` dizileri yazar (kasıtlı olarak organik/shader terrain değil). Eşlik eden `BuildPlacementController` oyun-içi prop yerleştirmeyi yönetir. | `Assets/Scripts/UI/BuildMode/BuildTileBrushController.cs`<br>`Assets/Scripts/UI/BuildMode/BuildPlacementController.cs`<br>`Assets/Scripts/UI/BuildMode/BuildCommandStack.cs` |
| **Director Mode** (runtime stat/spawn/telemetry aracı) | Oynanışı duraklatan ve canlı demo için stat ayarı, düşman spawn'ı ve telemetry açan oyun-içi debug/demo overlay'i. Ham backquote toggle'ının sahibidir ve duraklatılmış "Director state"'ini Build Mode ile paylaşır (Build Mode, üzerine giydirilen cilalı alias'tır). | `Assets/Scripts/UI/DirectorMode.cs` |

### Oda Sistemi

| Özellik | Açıklama | Anahtar Dosya(lar) |
|---|---|---|
| **Oda runtime / run akışı** | Run-başına oda yaşam döngüsünü sürer: `RoomRunDirector` oda seçimi/ilerlemesini orkestre eder; `IsoRoomBuilder` seçilen bir `RoomTemplateSO`'yu runtime'da oynanabilir bir cliff-tile odaya örnekler. `RoomTemplateValidator`, şablonlar kullanılmadan önce geçit-kontrolü yapar. | `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`<br>`Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`<br>`Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs` |

### Combat & Skills

| Özellik | Açıklama | Anahtar Dosya(lar) | Tuş |
|---|---|---|---|
| **Elementalist LMB fireball** — mermi + VFX | Elementalist temel-saldırı ritmi: LMB bir Rift Bolt / fireball mermisi atar, güçlendirilmiş 3. atışla; cast-flash, mermi izi ve çarpma patlaması VFX'i ile süslenir. Fireball yeteneği kendisi bir Burning DoT ve 8-yön sprite seçimi ekler; `SkillVfx.ProjectileBlaze` motor-katmanı iz/çarpma juice'unu sağlar. | `Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs`<br>`Assets/Scripts/Skills/Elementalist/Fireball.cs`<br>`Assets/Scripts/VFX/SkillVfx.cs` | LMB (temel); Fireball = Q/E/R/F slotu |
| **Status-effect sistemi + düşman tint** (chill/burn) | `StatusEffectSystem` istiflenebilir Chill/Burning efektleri uygular (buz-kırılma ve yanma zincirleri, DoT, yavaşlatma). `StatusEffectTint` otomatik bağlanır ve Dead-Cells tarzı geri-bildirim için düşman sprite'larını boyar: Chill=mavi, Burning=kırmızı (titreşimli), Frozen=camgöbeği-mavi. | `Assets/Scripts/Systems/StatusEffects/StatusEffectSystem.cs`<br>`Assets/Scripts/VFX/StatusEffectTint.cs` | — |
| **Yetenek sistemi + draft** | `SkillBase`, her aktif yeteneğin türediği soyut taban sınıftır (cooldown, cast, harcama kuralları); sınıf controller'ları 4 yetenek slotunu eşler; `DraftManager` oyuncunun yeni yetenek seçtiği odalar-arası ödül/draft akışını yürütür. | `Assets/Scripts/Skills/Base/SkillBase.cs`<br>`Assets/Scripts/Skills/Base/Warblade_SkillController.cs`<br>`Assets/Scripts/Skills/Elementalist/Elementalist_SkillController.cs`<br>`Assets/Scripts/Skills/DraftManager.cs` | Q / E / R / F |
| **Temel saldırı hitstop / combat juice** | `HitStop`, vuruşta zamanı kısaca donduran singleton'dır (normal/heavy/kill için yoğunluk-bazlı süreler), `PlayerAttack`'tan çağrılır. `HitPauseDriver` hit-pause hissini koordine eder; `ScreenShakeDriver` savaş ağırlığı için sarsıntı ekler. | `Assets/Scripts/Core/HitStop.cs`<br>`Assets/Scripts/Combat/Juice/HitPauseDriver.cs`<br>`Assets/Scripts/Player/PlayerAttack.cs` | LMB |
| **Act 1 Boss — Penitent Sovereign** | İki fazlı Act 1 boss'u: Faz 1 (%100–50) ağır, tahmin edilebilir saldırılar; Faz 2 (%50–0) zincirleri kırılınca +%40 hız ve yeni saldırılar. Senaryolu bir intro ve zincir mermisiyle desteklenir. | `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`<br>`Assets/Scripts/Enemies/Boss/BossIntroController.cs`<br>`Assets/Scripts/Enemies/Boss/BossChainProjectile.cs` | — |

### Karakterler

| Özellik | Açıklama | Anahtar Dosya(lar) | Tuş |
|---|---|---|---|
| **Player controller / karakterler** (Warblade, Elementalist) | `PlayerController` + `PlayerMovementController` oyuncu hareketini ve girdisini yönetir; `PlayerAttack` temel saldırıyı sürer. İki demo sınıfı: **Warblade** (yakın dövüş, greatsword; controller `Skills/Base`'de) ve **Elementalist** (büyücü; controller `Skills/Elementalist`'te). Warblade karakteri bir prefab'tır. | `Assets/Scripts/Player/PlayerController.cs`<br>`Assets/Scripts/Player/PlayerMovementController.cs`<br>`Assets/Scripts/Player/PlayerAttack.cs`<br>`Assets/Prefabs/Characters/Warblade.prefab` | WASD, LMB |

### Demo Sahnesi

| Özellik | Açıklama | Anahtar Dosya(lar) |
|---|---|---|
| **Demo sahnesi + başlatma akışı** | `_Arena.unity` savaş/demo arenasıdır. Tam akış her zaman ilk build sahnesinden başlar (MainMenu → CharacterSelect → Chamber → _Arena) çünkü `PlayFromStartScene` editör kancası `playModeStartScene`'i ayarlar; `RIMA/Play From Main Menu` menü öğesini kapatmak, hızlı oda iterasyonu için açık olan sahneden doğrudan Play'e basmanızı sağlar. | `Assets/Scenes/_Arena.unity`<br>`Assets/Scripts/Editor/PlayFromStartScene.cs` |

---

## 📁 Proje Yapısı

- **Oyun kodu** `Assets/Scripts/...` altında yaşar (Editör araçları `Assets/Scripts/Editor/...` ve `Assets/Scripts/MapDesigner/...`; combat, skills, player, enemies kendi alt klasörlerinde).
- **Unity projesi** standart üç klasörden oluşur: `Assets/` (içerik + kod), `Packages/` (paket bağımlılıkları) ve `ProjectSettings/` (proje ayarları).
- Prefab'lar `Assets/Prefabs/...`, sahneler `Assets/Scenes/...`, oda/veri asset'leri `Assets/Data/...` altındadır.

---

## 📄 Rapor

Projenin akademik bitirme raporu (Senior Design Report) depo içinde şu yolda bulunur:

**`Rapor/RIMA_Senior_Design_Report.pdf`**

Rapor; sistem mimarisini, seviye-tasarım araç zincirini, prosedürel oda üretimini ve combat tasarımını detaylı olarak açıklar.
