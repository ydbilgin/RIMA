# GÖREVLER — Aktif Geliştirme Takip Listesi

*Bu dosyayı her session başında oku, her session sonunda güncelle.*

---

## MEVCUT FAZ: FAZ 1 — Core Loop

### Yapılacaklar

#### Unity Kurulum
- [ ] Unity 6.3 LTS projesi aç — "2DRoguelite" adıyla
- [ ] URP 2D ayarla (New Project → 2D URP template)
- [ ] Input System paketini ekle
- [ ] Cinemachine paketini ekle
- [ ] Klasör yapısını kur: Assets/Scripts, Assets/Sprites, Assets/Animations, Assets/Prefabs

#### Art — FAZ 1 Minimum (Aseprite + PixelLab)
Rehber: `ART/ASEPRITE_PIXELLAB_REHBERI.md` → "FAZ 1 MİNİMUM LİSTESİ"

- [ ] warblade_front_idle.png
- [ ] warblade_front_walk.png
- [ ] warblade_front_attack1.png
- [ ] warblade_front_dash.png
- [ ] warblade_front_hit.png
- [ ] warblade_back_idle.png + walk + hit
- [ ] warblade_left_idle.png + walk + attack1 + hit
- [ ] grunt_front_idle.png + walk + attack + hit + death
- [ ] grunt_left_idle.png + walk
- [ ] tileset_dungeon_floor.png
- [ ] tileset_dungeon_wall_straight.png
- [ ] prop_torch_wall_anim.png
- [ ] prop_door_closed.png + open
- [ ] vfx_hit_sparks.png
- [ ] vfx_slash_white.png
- [ ] vfx_blood_splatter.png
- [ ] ui_hpbar_frame.png + fill
- [ ] ui_ragebar_frame.png + fill
- [ ] ui_skill_slot_empty.png
- [ ] icon_warblade_ground_stomp.png

#### Kod — FAZ 1
- [ ] PlayerController.cs (8 yönlü hareket + dash)
- [ ] CameraFollow.cs (Cinemachine + sınır)
- [ ] CombatSystem.cs (hitbox/hurtbox, hasar, knockback)
- [ ] EnemyAI.cs (basit: gör → yaklaş → vur)
- [ ] ClassResourceSystem.cs (her sınıfa özgü kaynak bar — Warblade için Rage 0-100)
- [ ] Warblade_SkillController.cs (Ground Stomp + Cleave — 2 skill yeterli FAZ 1 için)
- [ ] RoomManager.cs (wave spawn, oda tamamlama koşulu)

---

## TASARIM — Açık Görevler (Kod öncesi tamamlanacak)

- [x] **Skill pool revizyonu:** ✅ TAMAMLANDI (2026-03-28) — SINIF_SKILL_SISTEMI_FINAL.md yazıldı. 8→12 skill per class, tag sistemi, seçim mimarisi kesinleşti.
- [x] **GDD.md skill sistemi güncellemesi:** ✅ TAMAMLANDI (2026-03-28) — 12-skill, tag sistemi, Weighted Draft, Ranger Focus, Hexer faz sistemi, Brawler rename, Codex/Kayıt Defteri kararı eklendi
- [ ] **MASTER_SINIF_VE_CROSSCLASS.md güncellemesi:** 4 yeni skill per class eklendi — tablolar güncellenmeli
- [ ] **PASIF_VE_SKILL_SISTEMI.md güncellemesi:** Her 28 cross-class combo için unlock ekranında 3-5 seçenekten 3 seç sistemi → tüm 28 combo için 3-5 seçenek listesi yaz
- [ ] **SINIF_SKILL_HAVUZU.md isim güncellemesi:** "Berserker" → "Brawler" olarak güncelle (tüm dosyada)
- [ ] **GDD.md cross-class ultimate tablo:** 28 ultimate'in tümünün GDD'deki tabloda eksiksiz yazılı olduğunu doğrula

### Skill Sistemi Kararları — Karar Günlüğü (2026-03-28)
- Pool 8→12 aktif skill per class genişletildi (C(12,4)=495 kombinasyon vs 70)
- Tag sistemi: 8 fonksiyonel etiket (⚓▶⚡↑↓💥⬡✦), skill başına max 2 tag
- CONTROL ve AMPLIFIER tag'ları başlangıçta gizli, keşif mekaniği
- Seçim mimarisi: "Signature + Weighted Draft" — oda başına 3 teklif, ağırlıklı
- Slot 5-6 sadece FLUX/Elite/Boss Soul ile açılır (oda ödülünden değil)
- Ranger resource değişikliği: CD-only → Focus (0-100, mesafe bazlı)
- Hexer phase sistemi: 0-3 / 4-6 / 7-9 / 10 eşikleri, her geçişte feedback
- Summoner felsefesi: feda döngüsü (kalıcı ordu değil, güçlü tek-kullanımlık)
- Referans: TASARIM/SINIF_SKILL_SISTEMI_FINAL.md

---

## FAZ 2 — Sistemler (Ay 2) — HENÜZ BAŞLAMADI

- [ ] SkillAcquisition sistemi (oda sonrası 3 kart sun)
- [ ] DualClass sistemi (Fusion modu) + primary resource seçim UI
- [ ] CrossClassUltimate sistemi (charge tracking, [U] tuşu)
- [ ] RiftShard sistemi ([G] consumable, max 3/run)
- [ ] UI: Skill slot'ları, kart seçim ekranı
- [ ] Tüm Warblade skill ikonları (7 ikon)
- [ ] Sınıf seçim ekranı (run başı)

---

## FAZ 3 — Düşman + Boss — HENÜZ BAŞLAMADI

- [ ] Grudge sistemi kodu
- [ ] Elite düşman (PlaguenKnight)
- [ ] Grudge badge ikonları
- [ ] Boss 01 art + AI
- [ ] Oda akışı (hub → dungeon → boss)

---

## FAZ 4 — Tüm 8 Sınıf — HENÜZ BAŞLAMADI

- [ ] Elementalist art + skills
- [ ] Rogue art + skills
- [ ] Ranger art + skills
- [ ] Brawler art + skills
- [ ] Paladin art + skills
- [ ] Summoner art + skills
- [ ] Hexer art + skills
- [ ] 49 skill ikonu

---

## FAZ 5 — Polishing — HENÜZ BAŞLAMADI

- [ ] Meta progression sistemi
- [ ] URP 2D Lighting setup
- [ ] Normal map'ler (Laigter workflow)
- [ ] VFX skill efektleri (11 efekt)
- [ ] Hub sahnesi art
- [ ] Ses efektleri + müzik

---

## FAZ 6 — Steam — HENÜZ BAŞLAMADI

- [ ] Denge testleri
- [ ] Steam store page
- [ ] Capsule görseller
- [ ] Trailer
- [ ] Demo build

---

## KARAR GÜNLÜĞÜ

| Tarih | Karar | Neden |
|-------|-------|-------|
| 2026-03-25 | Top-down 2D (izometrik iptal) | PixelLab skeleton top-down 2D için optimize, izometrik z-sorting çok zor |
| 2026-03-25 | Sınıf adı Warrior → Warblade | Daha özgün isim, stance swap mekaniğine uygun |
| 2026-03-25 | Skill acquisition: Slay the Spire modeli | Oda sonrası 3 kart, max 6 slot — test edilmiş, işe yarıyor |
| 2026-03-25 | Ultimate: Identity Transform | 10s boyunca tamamen farklı 4 skill seti — Lost Ark Identity ilhamı |
| 2026-03-26 | Proje klasörü: F:\Antigravity Projeler\2d roguelite | N8NSearch'ten bağımsız yeni klasör |
| 2026-03-27 | Evrensel Rage barı kaldırıldı | Her sınıf kendi kaynağını kullanıyor, [V] burst kaynak tabanlı. Dual-class'ta primary seç |
| 2026-03-27 | Sınıf adı Berserker → Brawler | GDD + MASTER'da Brawler. SINIF_SKILL_HAVUZU.md'de güncellenmeli |
| 2026-03-27 | Cross-class ultimate: 28 combo'ya özgü, [U] tuşu, sadece dual-class'ta | Her combo farklı charge koşulu ve efekt. Tüm 28'i MASTER'da tamamlandı |
| 2026-03-27 | Rift Shard: run içi reset consumable (max 3/run) | [G] ile primary bar doldur VEYA 1 ult charge ver |
| 2026-03-27 | Skill pool: sayı artırma değil, mevcut 8'i revize et | Dual-class 16 havuzdan 4 seçim yapıyor, çeşitlilik yeterli |
