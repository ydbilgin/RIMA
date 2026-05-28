# Room Visual Target — Research Brief (Next Session)

**User reference image:** Saved upload (Hades/Diablo-style dungeon room, cyan rift cracks, mini-map top-left = matches Kırık Taş Tablet, HP/currency HUD, 4 enemy + central character with visible weapon, atmospheric torch+fire bowl lighting).

**Status:** Bekleyen — skill sheet v6 + Antigravity gate review tamamlandıktan SONRA başlanacak.

---

## User Soruları (sırayla)

### Soru 1 — Reference Aesthetics Bizim Açıyla Uygulanabilir mi?

Reference image özellikleri:
- Top-down isometric ~30-35° (RIMA canonical ile aynı)
- Granite floor + heavy cyan rift cracks
- Atmospheric lighting (torch flicker + central fire bowl + cyan rim light)
- Mini-map top-left rounded frame (Kırık Taş Tablet pattern ile uyumlu)
- HUD: HP bar + currency icon sol-üst
- Character central + 4 enemy arranged radially
- Stone walls + decorative debris

**Cevap aranan:** Bu kaliteyi RIMA canonical pipeline'da (PixelLab character + Codex environment + Unity composition) nasıl üretiriz? Reference image painterly illustration mu, in-game render mı?

### Soru 2 — Silah South Frame'de Doğru Duruyor mu?

Canonical karakter sprite'ları 8-yön rotation'lı (south, south-east, east, north-east, north, north-west, west, south-west).

**Problem:** Skill animasyonu south frame'de iken karakter karşıdan bakıyor → silah önde mi arkada mı? Anatomi olarak doğru pose'da mı?

Reference image'deki karakter: south-ish facing, double-handed weapon held diagonal across body. Bu pose'un bizim Warblade greatsword'la uyumu var mı?

**Hesap gerekli:**
- 8 yön için silah-el bağlantısı
- South frame'de silah önde mi (zoom near) yoksa arkada mı (depth correct)
- Weapon-as-child-SR mantığı (canonical: silahsız body + WeaponSR child SR — memory `project_weaponless_animation_v1`) ile composition

### Soru 3 — Little Master Pattern (Steam Video Reference)

User notu: Little Master Steam sayfasındaki video'da karakter ve silahı **ayrı çizdiriyor** gibi.

**Research:** Little Master video'sunu izle (Steam sayfası gameplay clip):
- Karakter sprite stand-alone mı (silahsız body)?
- Silah ayrı child sprite olarak mı render ediliyor?
- 8 yön için silah konumlandırma nasıl yapılmış?
- Animation cycle'da silah hangi frame'de el-pozisyonunu takip ediyor?

Bu zaten bizim canonical pattern (memory `project_weaponless_animation_v1` Karar #144). Ama Little Master'ın **görsel sonucu** karşılaştırılmalı — bizim pattern'imizle aynı output kalitesi üretir mi?

### Soru 4 — Industry Solutions: 8-Direction Weapon-Body Composition

**Research areas:**
- Hades: Zagreus + weapon overlay nasıl çalışıyor? Skin/weapon system?
- Diablo II: 8-dir char + equipment overlay (DCC format)
- Dead Cells: Karakter + weapon ayrı animation set
- Path of Exile: 8-dir karakter + weapon attachment
- Hyper Light Drifter: tek-yön karakter + weapon child
- Tunic: 4-dir + sword overlay
- Stardew Valley: tool overlay system

**Patterns to extract:**
- Anchor point per direction (hand position 8 frame için)
- Weapon Z-ordering (önde/arkada switch logic)
- Weapon animation independence (swing cycle weapon-only)
- Mirror flip optimization (5 unique dir + 3 mirror — canonical bizde de var, memory `feedback_8dir_mirror_production_strategy`)

### Soru 5 — Smooth Visual Achievable?

Reference image'in painterly quality. RIMA chibi pixel art sprite + PixelLab + Codex pipeline ile bu smooth quality üretilebilir mi?

**Cevap aranan:**
- PixelLab anim frame interpolation smoothness limit
- Codex illustration vs game asset rendering gap
- HD post-processing (URP 2D bloom, glow, screen-space ambient) ile reference quality'ye ne kadar yaklaşırız?
- Reference image **aspirational concept** mi yoksa **achievable target** mı?

---

## Codex Review Task (bu task hazırlanırken)

Bu research'ü tamamladıktan sonra Codex'e review prompt yaz:
- 5 soru için canonical pipeline cevabı
- Industry pattern catalog
- "Achievable smooth quality" verdict
- Action items: hangi tool gerekli, hangi gen, hangi Unity-side script

---

## Bağlı Memory + TASARIM Dosyaları

- `memory/project_weaponless_animation_v1.md` — silahsız body + WeaponSR child SR Karar #144
- `memory/feedback_8dir_mirror_production_strategy.md` — 5 sprite üret + 3 mirror
- `memory/project_camera_angle_revisit_s43.md` — 30-35° + 8-dir LOCK Karar #114
- `memory/project_canonical_character_roster_v2.md` — 10 class PixelLab ID
- `memory/project_rima_visual_vision_reference_s95.md` — Hades+Diablo iso ARPG target
- `memory/project_shadow_standard.md` — Runtime shadows
- `memory/project_perspective_templates.md` — itch.io template rules

---

## When to Start

✋ NOT NOW. Önce şunlar bitsin:
- ⏳ Codex v6 skill sheet (10 class × 115 skill) — dispatched
- ⏳ ChatGPT v6 skill sheet (paralel, user-side)
- ⏳ Antigravity gate review (user paste edecek)
- ⏳ v2.3 progression LOCK (gate kararları sonrası)

Sonra:
- ✅ Bu research'ü başlat (orchestrator + rima-research + Codex review zinciri)
- ✅ Output: weapon-body composition spec + smooth visual target verdict + Unity implementation roadmap
