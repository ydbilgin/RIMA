# DECISION — OYNANABİLİR PLAN: Sıralı Roadmap (2026-06-05)

**Council:** ax-3.1-Pro (bağımlılık/kritik-yol) ‖ ax-3.5-Flash (lean/scope-cut) ‖ Opus-advisor (kod-audit,
`_council_opus_roadmap.md`) → Opus sentez. Girdiler: silah canon NLM sorgusu (`_nlm_weapons_anim.json`) +
`CODEANIM_DECISION_2026-06-05.md` + kullanıcı önceliği (silah → ele-oturtma → animasyon → diğer).

## 🔑 KOD GERÇEĞİ (Opus-advisor — planı değiştirdi)
1. **Silah sistemi TAM KODLU + Warblade UÇTAN UCA CANLI:** `OrientationSync.cs` (8-yön + prosedürel swing) ·
   `HandAnchorAttach.cs` (combo-step köprüsü + sort + swing-fade) · `WeaponDatabase.asset` (Warblade kayıtlı) ·
   `Warblade_Greatsword.prefab` + `Phase2_WeaponAttach_Test.unity`. "Var mı?" riski KAPANDI.
2. `eski_anchors` silinmesi BLOKLAMAZ (o Level-2 metadata; canlı sistem Level1Static `handOffsets[8]` SO array).
3. Swing canon güncellemesi: kod "gizle+VFX" değil **alpha-0.4 fade** yapıyor (HandAnchorAttach:35-37) — NLM
   "hide" reçetesi superseded.
4. ⚠️ ÇİFT Player prefab (`Assets/Prefabs/` + `Assets/Resources/Prefabs/`) — hangisi runtime-canlı Faz 0'da
   doğrulanacak; silah/tuning değişiklikleri ikisine de gitmeli (drift riski).

## SENTEZ KARARLARI
- **"Önce silah" tartışması ÇÖZÜLDÜ — paralel şeritlerle ikisi de önce:** otonom şerit B-11'e HEMEN başlar
  (Warblade zaten silahlı → döngü test edilebilir; Opus-adv: sistemler dik, silah beklemek döngüyü geciktirir);
  kullanıcı şeridi silah üretimine İSTEDİĞİ AN oturur (Flash: **Chamber dummy'si = silah test tezgâhı**,
  B-11 gerekmeden swing/feel testi).
- **Weapons-lite (Flash) + risk-kapsama (3.1+Opus-adv) birleşimi:** Demo = SADECE 4 açık sınıfın silahı.
  Warblade ✓ bitik → üretilecek 3: **Elementalist rün-diski** (ele takılmayan/yüzen — en zor) + **Ranger yayı**
  (asimetrik iki-el) + **Shadowblade ikiz hançer** (twin-mirror testi). Bu üçlü TÜM mimari risk sınıflarını
  kapsıyor; kalan 6 silah post-demo.
- **İkiz silahlar = TEK sprite + runtime flipX off-hand** (3/3 oybirliği; çift üretim = 2× maliyet + asimetri artefaktı).
- **Feel-tuning (hit-stop/knockback/knockdown değerleri) silahlar GÖRÜNÜRKEN yapılır** (3/3; "görünmez silahla
  tune etme" = rework). Knockdown LOGIC'i paralel yazılır, TUNING Faz 3'te.
- **Anchor-tuning editör kolaylığı pilot'tan önce şart** (Opus-adv: 8-yön offset'i Inspector'da elle = zaman-yiyici;
  mini scene-view gizmo/tool).
- Lean cut'lar (Flash): ESC codex + sol skill paneli + kalan 6 silah + tam RoomBank otomasyonu → post-demo.
  (B-12: demo için 3-5 odalık statik ilerleme yeter; 15-oda bank cilası sonra.)

## 📋 ROADMAP — 3 OTURUM (her biri oynanabilir-kontrolle biter)

### OTURUM A — OTONOM OYNANABİLİR İSKELET (kullanıcısız ilerler)
| İş | Boyut | Kim |
|---|---|---|
| 0.1 Çift Player prefab doğrula (hangisi canlı) + silah lane ön-kontrol | S | Sonnet/Flash |
| 0.2 Chamber bitir (in-flight) + QC | M | cx (sürüyor) |
| A.1 **B-11 combat lifecycle** (clear→slow-mo→reward→kapı→sonraki oda; 3-5 odalık statik zincir) | L | cx |
| A.2 **Knockdown paketi** (knockback birleştir + KnockdownDriver + 3 profil SO + Broken-tetik + get-up i-frame) | M | cx (B-11 sonrası) |
| A.3 Mob-ölüm squash-decal + spawn/dash-ghost | S | Flash |
| A.4 Anchor-tuning editör gizmo/tool | S | Flash/cx |
| ✅ KONTROL: Chamber→3 oda→Warblade ile dövüş→ölüm→"+n SHATTERED ECHO" tam döngü |

### OTURUM B — SİLAH ÜRETİMİ (KULLANICIYLA, PixelLab) ‖ paralel cx cila
| İş | Boyut | Kim |
|---|---|---|
| B.1 3 silah üret: Elementalist diski → Ranger yayı → Shadowblade hançer (S-XL + stil-transfer, Direction:None) | M | KULLANICI+Opus |
| B.2 Her silah sonrası anchor-fitting (A.4 tool ile; Chamber dummy'de anında swing testi) | M | KULLANICI+Sonnet |
| B.3 (paralel cx) GECE·3 Tier-1: draft-kart hover/TooltipSystem + echo-award play-verify | M | cx |
| ✅ KONTROL: 4 açık sınıfın hepsi silahlı; dummy'de 4 farklı silah hissi |

### OTURUM C — FEEL-TUNING + İÇERİK CİLASI
| İş | Boyut | Kim |
|---|---|---|
| C.1 Combat feel-tuning (hit-stop/knockback/knockdown SO değerleri — silahlar görünürken) | M | KULLANICI+Sonnet |
| C.2 Elementalist büyü-VFX Phase-1 (kod/particle) | M | cx |
| C.3 Modüler-props kalanı (küme-prop authoring + checker + auto-placer) + oda çeşitliliği | M | Sonnet/Flash |
| ✅ KONTROL: tam playtest — "oyun gibi hissediyor mu?" |

**Kritik yol:** Chamber → B-11 → (silahlar paralel) → feel-tuning → playtest.
**Post-demo park:** kalan 6 silah · ESC codex · sol panel · mob görselleri (PixelLab) · skill ikonları ·
boss anim seti (T3) · tam RoomBank otomasyonu.

## RİSKLER
1. **Juggle-lock** — get-up i-frame olmadan knockdown merge EDİLMEZ (hard gate).
2. **Çift prefab drift** — 0.1'de çözülmeden silah işine girilmez.
3. **Anchor-tuning zaman-yiyici** — A.4 tool'u B oturumundan önce hazır olmalı.
