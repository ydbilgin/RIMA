# Sang Hendrix Realtime Parallax Map Builder — Inspector/UX deep dive

## Mission
Sang Hendrix'in itch.io sayfasını ve oradaki tüm videoları ayrıntılı analiz et. RIMA Unity ARPG projemizdeki "Room Painter" EditorWindow'unu daha işlevsel ve güzel hale getirmek için **somut, kopyalanabilir** UX/inspector tasarım kararları üret.

## Source
- Ana URL: https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
- Sayfadaki tüm gömülü videoları (Vimeo/YouTube/MP4) frame-by-frame izle. Eğer link verilmişse her birini ayrı ayrı aç.
- Yan ürün/teaser tweet'leri (@sanghendrix0904) da kontrol et — UI/inspector ekran görüntüleri içerebilir.

## RIMA mevcut durum (kıyaslama için bilmen gerekenler)
**Konum:** `Assets/Editor/RoomPainter/` — Unity Editor 2022.3 IMGUI EditorWindow.

**Layout:** 3-pane (palette | live preview | inspector). Default genişlikler: 270 / 580 / 320. Min window 1180x700.

**Palette panel:**
- Asset grid (64x64 thumbnail butonlar)
- Dinamik kolon sayısı (genişliğe göre)
- 2 sekmeli: Gameplay Cliffs / Parallax BG Cliffs
- Parallax sekmesi seçiliyken bir "Tier" dropdown var (FG/Playable/Near/Mid/Far/Skyline/Horizon)
- Kategori chip filtre satırı: All / Floor / Cliff / Props / Parallax / All others

**Live Preview panel:**
- Dark checker background
- Sprite preview (Fit / 1x / 2x ... / 8x zoom, scroll wheel zoom, MMB pan, R rotate)
- 3D mock overlay (shadow, cliff ramp, parallax tint, pivot crosshair, y-sort line, dashed bounds)
- Footer toolbar: zoom toggle row + Show 3D mock + hint + status

**Inspector panel (yeni revize):**
- Üstte mode banner (Editing SO / Editing instance)
- Hero card: 88x88 thumbnail + asset adı + layer renk rozeti + sprite px boyutu + dosya adı
- 6 renkli section bant: Identity (cyan) / Placement (yeşil) / Physics (turuncu) / Parallax (mor) / Visual (pembe) / Metadata (gri)
- Her bant tıklanır → genişler / kapanır
- **Identity:** displayName, path (readonly), GUID (readonly), Ping/Reveal butonları
- **Placement:** defaultLayer enum, sortingLayer popup, order int, ySort toggle, ySortAxis enum, pivotAnchor Vector2, scale Vector2, visualOffset Vector2
- **Physics:** isBlock/isTrigger/respectPrefabColliders toggle, bodyType enum, colliderShape enum, colliderSize Vector2, physicsLayer popup, **mini hitbox preview canvas (sprite outline + colored collider rect)**, quick action buttons (Fit/Tight/Half/Square), Apply To Scene Instance button
- **Parallax:** Tier dropdown (FG/Playable/Near/Mid/Far/Skyline/Horizon/Custom) + Custom override slider (0.01-1.50), cameraRelative enum, pixelSnap enum
- **Visual:** tint color, material override, castShadow, receiveLight, Apply Visual button
- **Metadata:** tags list (string + remove button per tag, Add Tag button), notes textfield

**Scene placement:**
- SceneView'da snap-to-iso-cell ile asset koyma, R-rotate, ghost preview (cyan transparent)

**RIMA spesifik kısıtlar:**
- 2.5D top-down iso ARPG (dimetric, ~26.57° pitch)
- Pixel art, PPU 32-64 karışık
- Cliff/Parallax katmanları ana özellik (Hades benzeri görsel derinlik)
- Y-sort + sorting layer + parallax factor + camera-relative + pixel snap tüm gereken eksenler

## Bulmamı istediklerim

### Bölüm A — Sang Hendrix plugin'i ne yapıyor?
1. Plugin'in 3-5 cümlelik öz tanımı
2. Tüm öne çıkan özellikler madde madde (her özelliği gördüğün screenshot/video saniye damgasıyla destekle)
3. Layer/asset/inspector hiyerarşisi nasıl? Tek pencere mi, dock mu, modal mı?
4. Real-time preview nasıl çalışıyor? Editor → game view senkronu var mı?
5. Inspector/property editor field'ları neler? Field-by-field listele (örn parallax speed X/Y slider, opacity, scroll lock, fade-on-player, etc.)
6. Eğer videoda görülüyorsa: kullanıcı bir parallax katmanı eklerken kaç tık yapıyor? Workflow loop'u nasıl?

### Bölüm B — UX/UI patterns
Sang Hendrix'in başardığı ama bizim Room Painter'da eksik olan UX kalıplarını listele. Her biri için:
- **Adı** (örn "Per-layer live opacity slider", "Drag to reorder layer stack")
- **Ekran görüntüsü/video referansı** (varsa saniye)
- **Neden işlevsel?** (kullanıcı problemine değdiği yer)
- **RIMA'ya entegre edersek hangi panel'e gider?** (palette / preview / inspector / yeni panel)
- **Tahmini effort:** S/M/L (Small <2sa, Medium ~yarım gün, Large 1+ gün)
- **Önem skoru:** 1-5 (5 = killer feature)

### Bölüm C — Rekabet taraması
- Sang Hendrix'e benzer 3-5 başka tool/plugin (Tiled Map Editor, LDtk, Aseprite Tilemap mode, RPG Maker MZ default editor, Unity 2D Tilemap Brush, GameMaker Room Editor, Godot TileMap)
- Her birinin inspector/property editor pattern'i 1-2 cümle özet
- Hangi tool'un inspector tasarımı RIMA için en iyi referans? Neden?

### Bölüm D — Verdict
Şu üç soruya net cevap ver:
1. **Mevcut RIMA Room Painter inspector'ı şu hâliyle yeterli mi?** (Yeterli / Eksik / Yanlış yönde — tek kelime + 2-3 cümle gerekçe)
2. **Inspector'a daha çok yatırım yapmalı mıyız, yoksa gameplay'e dönmeli miyiz?** (Yatırım / Pivot — gerekçeyle)
3. **Eğer yatırım yapacaksak, ilk 3 adım nedir?** (priority order, her biri için 1 cümle açıklama + S/M/L effort)

## Çıktı formatı
Markdown. Saniye damgası içeren cümleler için `[2:14]` notasyonu kullan. Dosyaya doğrudan yaz:
- Output path: `C:/tmp/sanghendrix_inspector_research_agy.md`

Görmediğin bir şey hakkında uydurma — "Plugin sayfasında görünmüyor" diye yaz. Speküle ettiğin yerleri **[SPECULATION]** etiketle.
