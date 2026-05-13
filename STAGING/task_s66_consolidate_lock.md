# Codex Konsolide Cleanup Paketi (S66 LOCK)

**Tarih:** 2026-05-13
**Tip:** MECHANICAL — judgment verilmiş, sadece yaz/düzelt/oluştur
**Effort:** medium (long task, mechanical)
**Çıktı:** CODEX_DONE.md'ye summary, ayrıca dosya değişiklikleri

**Kritik kural:** Tüm metinleri **olduğu gibi** yaz; ek yorum/karar ekleme. Orchestrator'ın judgment'ı zaten finalize. Sen sadece konsolide yazıcısın.

---

## 1) MASTER_KARAR_BELGESI.md Güncellemeleri

### 1a) Header tarih güncelle
`> Son güncelleme: 2026-04-30` → `> Son güncelleme: 2026-05-13`

### 1b) Karar #99 metin düzeltme (REVOKED 60° kalıntısı)
Karar #99 metninde geçen "~60 derece top-down" referansını **"~35° top-down (Hades reference, Karar #100)"** olarak değiştir.

### 1c) VFX renk tablosu fix
Summoner satırı: `#22FF88 nekro yeşil` → `dark indigo + cyan + violet (Karar #96)`

### 1d) Yeni Kararlar (#110-#114) — Kararlar Tablosu sonuna ekle

```markdown
| #110 | Combat FAZ 1.0 Mimari | AttackToken player-scope DISI (mob-only); Cancel window progress-based (BasicAttackProfile.cancelWindowFraction); MercifulDodge 0.18s grace flag | 2026-05-13 |
| #111 | Awakening + Trace Sistemi | Faz 1.0'da 4 base class için 60sn Awakening (ilk seçimde zorunlu, held-to-skip 0.5sn). Run içi Trace = rift crack overlay (NPC değil), post-combat/vista transition trigger, run başına max 1, 4-cap pool, pure narrative. Faz 2'de 6 class + 8 Trace + mekanik reward. | 2026-05-13 |
| #112 | Lore Glossary | Capital-S **Shard** = Fracturing reality fragment. lowercase **shard** = currency/material. **Trace** = run-içi cryptic identity sinyali. **Awakening** = class intro micro-segment. **Echoes** (currency) + **Echo Twin** (boss) korunur, "echo" kelimesi başka bağlamda kullanılmaz. | 2026-05-13 |
| #113 | Camera + Perspective Convergence | Karakter + tile + VFX tek konverjans ~35° high top-down (Hades reference). 45° tile REJECT. Geniş savaş alanı = Orthographic Size kalibrasyonu (combat framing profili 320x180 ref). PixelPerfectCamera: GridSnapping=UpscaleRenderTexture, FilterMode=Point, CropFrame platform-test. PPU=64 sabit. CameraFollow.combatOrthographicSize PixelPerfect ownership netleşmeli. Drop shadow = child SpriteRenderer opsiyonel polish (perspektif maskesi DEĞİL). | 2026-05-13 |
| #114 | 8 Direction Animation Locked | Tüm playable + T2 mob + boss animasyonları 8 yön (N/NE/E/SE/S/SW/W/NW). Üretim: 5 yön gen (S, SE, E, NE, N) + 3 mirror (W=E flip, SW=SE flip, NW=NE flip). Karar #53 + #88 REVOKED. VFX AngleMode (#102) 8 yön projection için Faz 1.0 sonu rekalibrasyon. PixelLab Custom Animation V3 pipeline değişmez, direction count artar. | 2026-05-13 |
```

### 1e) REVOKE notları (eski kararlara üst satır)
- Karar #53 metnine başına: `> **REVOKED 2026-05-13:** Karar #114 ile 8 yön LOCKED; bu karar pasifize.`
- Karar #88 metnine başına: `> **REVOKED 2026-05-13:** Karar #114 ile sayısal trigger atlatıldı, 8 yön doğrudan LOCKED.`

---

## 2) CURRENT_STATUS.md Güncelleme

Mevcut S66 bölümünün altına yeni "Recent LOCKED Kararlar" tablosuna #110-#114 ekle.

"Recent LOCKED Kararlar" tablosunda var olan #110 (Combat FAZ 1.0) **KORUNUYOR** — silme. Üstüne #111-#114 satırları ekle:

```markdown
| #111 | Awakening + Trace | Class intro shard + run-içi cryptic identity trace overlay, Faz 1.0 4-cap pool |
| #112 | Lore Glossary | Shard/Trace/Awakening/Echo terim disambiguation |
| #113 | Camera Convergence | ~35° tek konverjans + Orthographic Size kalibrasyonu, 45° tile REJECT |
| #114 | 8 Direction Animation | 8 yön LOCKED (5 gen + 3 mirror), Karar #53/#88 REVOKED |
```

Active Priorities bölümünü güncelle:
```
P0: F1 Shattered Keep tileset pilot (PixelLab Maps > Tileset Pro, 35° high top-down, prompt STAGING/'de hazır)
P1: PixelPerfectCamera ayar test (GridSnapping, CropFrame, Orthographic Size kalibrasyonu - Codex Unity)
P2: 8 yön animation rework — 4 char × 5 anim × 5 gen + 3 mirror (sequence: tile PASS sonrası)
P3: Awakening + Trace asset üretim (4 char Faz 1.0, intro shard reuse vista backdrop)
P4: Combat FAZ 1.0 playtest (InputBuffer+AttackToken+MercifulDodge Unity'de test)
P5: PIXELLAB_MASTER_REHBER S60 override revize sonrası referans kullanım
```

---

## 3) FAZ_MASTER.md Güncelleme

### 3a) Senkron tablosu — #72-#114 sync notu
"SYNC GUNCELLENDI 2026-05-09 S46" satırını şuna güncelle:
```
SYNC GUNCELLENDI 2026-05-13 S66 (#72-#114 eklendi, MASTER_KARAR_BELGESI canonical)
```

### 3b) Curse Gate düzelt
Oda Tipi Dağılımı tablosunda Curse Gate satırı: "Faz 4" → "Faz 2 (Karar #62)"

### 3c) Mob boyutu etiketi
Set A mobları "128px (S43)" → "128px (PPU=64 standardı, Karar #74)"

---

## 4) GDD.md Düzeltme

### 4a) Oda sayısı
"Act 1 başlar (8-9 oda)" → "Act 1 başlar (15 node, Karar #62)"

### 4b) Belge başlığına uyarı koruma — değişiklik yok (zaten var)

---

## 5) TASARIM/ANIMATION_REDESIGN.md — ARCHIVED işaretle

Dosya en üstüne (frontmatter altına, ilk satır olarak):
```
> **ARCHIVED 2026-05-13:** Bu dosya tarihsel kayıt amaçlı korunmaktadır. Aktif animasyon pipeline'ı için: TASARIM/PIXELLAB_MASTER_REHBER.md (Karar #114 8 yön LOCK ile birlikte) + MEMORY/INDEX.md referansları kullanılmalıdır.
```

---

## 6) Orphan Dosya Oluştur

### 6a) MEMORY/project_64px_armed_character_locked.md
İçerik:
```markdown
# 64x64 Armed Character Sprite — LOCKED (Karar #73)

**Tarih:** 2026-05-12
**Status:** LOCKED
**Owner:** rima-asset, rima-design

## Mimari Karar
Karakter sprite formatı: **64x64 chibi pixel art**, silah karakterle aynı sprite içinde (1-piece, body+weapon separate DEĞİL).

## Gerekçe
- 32x32 chibi'de ~11px kafa = yüz detayı yok, animasyon pipeline yetersiz (S62 mature pivot REVOKED, Karar #100)
- Body+weapon ayrı sprite anchor sistemi (eski S57-S58) revoked — Karar #72 ile 2.5D pipeline tamamen kaldırıldı
- 64x64 chibi: yüz okunaklı, animasyon detayı yeterli, PPU=64 ile pixel-perfect uyum

## Uygulama Kuralları
- Tüm playable class (10), T2 mob (6+), boss (4): 64x64 native generation
- Silah karakterin elinde (sol el konvansiyonu, Karar #99 weapon-in-hand)
- 8 yön animasyon (Karar #114): 5 gen + 3 mirror
- View angle: ~35° high top-down (Karar #113)
- PPU=64, Point filter, no compression, no mipmap (S60 LOCKED pipeline)

## Referanslar
- Karar #100 (chibi 64x64 RESTORE)
- Karar #104 (10/10 anchor PASS)
- Karar #105 (Create Character: view=low top-down, n_directions=8, proportions=chibi)
- Karar #113 (Camera Convergence ~35°)
- Karar #114 (8 Direction Animation)
```

---

## 7) PIXELLAB_MASTER_REHBER.md Revize

Dosyanın en üstüne (frontmatter varsa altına, yoksa H1'in altına) şu bölümü ekle:

```markdown
## ⚠️ RIMA-S60+S66 OVERRIDE (Bu bölüm canonical, video notları alt referans)

Bu rehber genel PixelLab eğitimini kapsar. **RIMA proje kararları aşağıdaki override'lara tabidir** — çelişki durumunda Master Karar Belgesi canonical:

### Pipeline (S60 LOCKED + S66 Update)
- **Karakter üretim:** Create Image Pixen NEW (S-XL) → reference image → Create Character → 8 yön (Karar #114)
- **Animasyon:** Custom Animation V3 (Karar #108): 4-16 frame, 3 gen/dir; Create State 20-40 gen
- **Tile üretim:** Maps > Tileset Pro (Gemini-backed, S66 onaylı `create_topdown_tileset` Wang autotile). 16 tile + transition. Eski Maps Tab/Inpaint Pro tileset workflow Karar #75 REVISION ile kısmen onaylı — sadece izole varyant için `create_tiles_pro`.
- **MCP:** Üretim için YASAK (Karar #106). Web UI kullan.

### Görsel Standartlar
- **Karakter:** 64x64 chibi (Karar #100 + #73)
- **Tile:** 32x32 (Karar #100)
- **View angle:** ~35° high top-down (Karar #113, Hades reference)
- **Yön sayısı:** 8 yön — 5 gen + 3 mirror (Karar #114, eski 4 yön kuralları #53 + #88 REVOKED)
- **PPU:** 64 sabit
- **Filter:** Point, no compression, no mipmap

### Tema (Karar #77 Vivid Vulnerability)
- Salt and Sanctuary chibi-but-serious + Hades theatrical mythic
- Cyan/violet rift accent, blood/horror YASAK
- F1 palette: #2C2A2A (floor), #4A3F3F (wall), #7BA7BC (cold blue), torch #C4682A
- Ritual Catastrophe framing (void cracks, broken sigils), grimdark DEĞİL

### Naming (Karar #112 Glossary)
- **Shard** (Capital-S) = Fracturing reality fragment
- **shard** (lowercase) = currency/material
- **Trace** = run-içi cryptic identity sinyali
- **Awakening** = class intro micro-segment
- **Echoes** = currency (Karar #27)
- **Echo Twin** = Act 2 boss

### REVOKED Pipeline Sections (Bu rehberin alt bölümlerinde olabilir, görmezden gel)
- 2.5D / KayKit / 3D pre-render (Karar #72 REVOKED)
- Mature ARPG 60° tile (Karar #100 REVOKED)
- 4 yön rotation kuralı (Karar #114 REVOKED, şimdi 8 yön)
- Karakter+çevre hibrit perspektif (Karar #113 REJECTED, ~35° tek konverjans)
- Map Workshop multi-tile connected room output (Karar #75 LOCKED, S66 revision: `create_topdown_tileset` Wang Pro mode istisna)
```

---

## 8) CODEX_DONE.md Summary

Tüm bu değişiklikleri tek bir özet bölümü olarak CODEX_DONE.md'ye append et:

```markdown
# S66 Konsolide Cleanup Paketi — TAMAMLANDI

**Tarih:** 2026-05-13
**Lock'lanan Kararlar:** #110, #111, #112, #113, #114
**REVOKE'lar:** #53, #88
**Lint fix'leri:** Karar #99 metin, Summoner VFX renk, MASTER header tarih, FAZ_MASTER sync, GDD oda sayısı, Curse Gate Faz, ANIMATION_REDESIGN ARCHIVED, orphan Karar #73 dosyası oluşturuldu
**PIXELLAB_MASTER_REHBER:** RIMA-S60+S66 override bölümü eklendi (top), eski sectionlar alt referans
**Etkilenen dosyalar:**
- TASARIM/MASTER_KARAR_BELGESI.md (header + #99 + VFX + #110-#114 + REVOKE notları)
- CURRENT_STATUS.md (#111-#114 ekle, priorities güncelle)
- TASARIM/FAZLAR/FAZ_MASTER.md (sync, Curse Gate, mob boyut etiketi)
- TASARIM/GDD.md (15 node fix)
- TASARIM/ANIMATION_REDESIGN.md (ARCHIVED header)
- TASARIM/PIXELLAB_MASTER_REHBER.md (RIMA override top section)
- MEMORY/project_64px_armed_character_locked.md (yeni oluştur)
```

---

## Kısıtlar

- Tüm yazımı **tam olduğu gibi** yap. Orchestrator'ın judgment'larını sorgulama, sadece yaz.
- Eğer bir dosya bulunamazsa belirt ama panik yapma — orchestrator çözer.
- Edit gerektiren noktalar için **Edit** tool kullan (Read sonrası); yeni dosya için **Write**.
- Git commit YOK — orchestrator review sonrası kendisi commit eder.
- Bittikten sonra CODEX_DONE.md'ye yukarıdaki özet bölümü append.

Bu mekanik bir paket — yaratıcı judgment beklenmiyor.
