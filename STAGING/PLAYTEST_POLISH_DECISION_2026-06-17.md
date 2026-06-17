# PLAYTEST POLISH DECISION — 9-konu batch (2026-06-17)

Kaynak: kullanıcı canlı playtest (görsel 01-06) + Opus kod-okuma kanıtı + ax_pro council vision (`director_hud_council/AX_PRO_VERDICT.md`). Demo ~19 Haziran. **Demo sınıfları: Warblade + Elementalist (kullanıcı kararı).**

## Locked triyaj tablosu (Opus kanıtı ax_pro'yu nerede override etti açıkça)

| # | Sorun | Kök neden (kanıt) | Fix kaldıracı | Tür | Öncelik |
|---|---|---|---|---|---|
| **7** | Boss barı düşmüyor | `BossHealthBar.cs` hpFill = Filled-Image ama **sprite=null** → fillAmount çalışmaz, hep dolu. phaseText doğru (metin). KESİN. | hpFill'e 1x1 white Sprite ata (runtime, dependency-free) | KOD-S | **P0 ship** |
| **1** | Prop Y-sıralama | **KANIT (TagManager):** sorting layer sırası `…Entities(6)…Props(11)`. Props katmanı Entities'in ÜSTÜNDE → proplar her zaman karakter üstünde. ⚠️ ax_pro "Default'a eşitle" dedi = YANLIŞ (Default en altta, prop zeminin altına gömülür). | Dik prop → **Entities** layer + YPosition; yer-decal → **Decals**(4) layer + walkable. `PropDefinitionSO.sortingLayerOverride` doğru ID. | DATA+KOD-M | **P0 ship** |
| **3/5a** | Silah sağ ele oturmuyor | `HandAnchorAttach` ön/arka flip MANTIĞI VAR; pozisyon yanlış = `WeaponDatabaseSO.anchorOffset` + handAnchor transform. | WeaponDatabaseSO anchorOffset/gripOffset (Warblade) data tuning | DATA-S | **P0 ship** |
| **2** | Sandık/fıçı collider | `PropColliderAutoBuilder` offset `size*0.5` → tabandan kayık; ayrıca Phase B-2 alanları (colliderShape/ratio) IGNORE ediliyor (iki sistem, biri ölü). | Offset'i taban-merkeze düzelt + per-prop `blocksWalkable`+`footprintSize`. ⚠️ B-2 refactor = RİSK, demo'da YAPMA (cerrahi DATA) | KOD-S+DATA | **P0 ship** |
| **6** | HUD sol-alta + akıllı | Sol-üstte segmentli bar; istenen sol-alt modern. | `HUDController.cs` anchor/pos/renk redesign (spec ↓) | KOD-M | P1 |
| **8** | Director güzelleştir + scroll | Skill listesi ScrollRect yok → Gravity Cleave kesik; UX zayıf. | `DirectorMode.cs` ScrollRect + kart stil + bind UX (spec ↓) | KOD-L | P1 |
| **5b** | Item drop hitbox saçma | ax_pro: yerdeki = RewardPickup elması, trigger collider çok büyük (grid-kare). | `RewardPickup` prefab trigger radius daralt | DATA-S | P1 |
| **4** | Yarık yürünür+orta-blok | #1+#2 özel hali. | `blocksWalkable=false` + prefab'a küçük merkez collider; decal layer | DATA-S | P1 |
| **9a** | Elementalist VFX (Fireball dışı + basic) yok | Sadece Fireball SkillVfx bağlı; diğerleri wiring eksik. | `Skills/Elementalist/*` + `SkillVfx`/`SkillRuntime` VFX-wiring | KOD-M | P1 |
| **9b** | Elementalist 8-yön yok | Sprite yok → **PixelLab balance=0**. ax_pro "CUT" dedi ama kullanıcı iki-sınıf istedi → CUT DEĞİL, **BLOCKED**. | Yeni sprite gen = PixelLab top-up GEREK. O zamana kadar mevcut yön + flipX reuse. | DATA-L | **BLOCKED (top-up)** |

## HUD redesign spec (ax_pro, sol-alt — Hades/Diablo)
- Anchor/pivot (0,0). HP bar üstte/kalın: pos (24,30), size (260,20), crimson `#C01020`. Resource altta/ince: pos (24,16), size (220,8), cyan `#10A0C0`. Oda/Echo metni (24,60), italik, alpha 0.7. Track = slate `#1B1F28` alpha 0.8 (simsiyah değil). Düşük HP'de titreme kalır.

## Director redesign spec (ax_pro)
- ScrollRect: `SkillPanel → Viewport(Mask+Image) → Content(VerticalLayoutGroup)`, content = skill kartları kökü.
- Kart bg zeminle aynı olmasın: `DirectorRaised #252A35`, seçili = cyan ince outline. Hasar/CD = ikon+badge (Hades boon stili), düz metin yığını değil.
- Q/E/R/F atama: alttan kopuk butonlar → Inspector panelinin EN ALTINA "Bind to:" başlığıyla yuvarlak tuşlar.

## Execution sırası (LOCKED — cx blocked → builder-opus serial, TEK Unity-ajan)
1. **SHIP bloğu (P0):** #7 boss bar → #1 layering → #3/5a silah → #2 collider.
2. **P1:** #6 HUD → #8 Director → #5b drop + #4 yarık → #9a Elementalist VFX-wiring.
3. **BLOCKED:** #9b 8-yön (PixelLab top-up bekler).

## ⛔ DOKUNMA / risk notları
- Collider B-2 refactor YAPMA (cerrahi data). DOKUNMA listesi: GATE/Boss-akış/reward-bleed/Build-çekirdek/weaponless-anim/branching-seed.
- Her Unity-süren adım sonrası read_console (0 error). Kullanıcı Unity'de aktif → recompile onu kesebilir, haber ver.

## 📌 RAPOR FİGÜR FOLLOW-UP (kullanıcı notu 2026-06-18)
- **Raporun karakter görseli (Şekil 1 / silahlı Warblade) GÜNCELLENECEK.** İki seçenek: (a) silah TÜM yönlerde elde doğru oturunca yeniden çek (şu an sadece SE doğrulandı), YA DA (b) **Elementalist ile çek** (silahsız mage → silah-mount sorunu hiç olmaz, daha temiz figür).
- Etkilenen dosyalar: `STAGING/report/figures_2026-06-18/fig_weapon_mount.png` (+ rapora gömülü hali). Görsel değişince akademik DOCX'i `make_akademik_docx.py` ile yeniden üret.

## 📌 YENİ SESSION'DA YAPILACAK (kullanıcı notu 2026-06-18)
- **#9a Elementalist skill VFX:** Fireball'un VFX'i güzel (arkasından iz/trail çıkıyor) ama DİĞER skiller bu VFX'i göstermiyor. Diğer Elementalist skillerine de aynı VFX-katmanını bağla. `Skills/Elementalist/*` + `VFX/SkillVfx.cs` + `SkillRuntime`.
- **#9b Elementalist 8-yön = BLOCKED DEĞİL (DÜZELTME):** Yeni gen GEREKMİYOR → PixelLab **karakter ID'sinden mevcut 8 yön SEÇİLEBİLİR/indirilir**. (Önceki "PixelLab balance=0 blocked" değerlendirmesi YANLIŞTI.) Yeni session: char ID'den 8 yönü çek → import → 8-dir kur. Rapordaki "8-yön BLOCKED" ifadesi de güncellenmeli.

## 📌 RAPOR REVİZYONU — YENİ SESSION + /council feedback (kullanıcı notu 2026-06-18)
Akademik rapor (`RIMA_Senior_Design_Report.docx/.md`) yeni session'da **/council'e sorulup feedback'le** elden geçirilecek. Kullanıcının somut maddeleri:
1. **Tüm figürler gözden geçirilecek.** Belirli: **Şekil 6 Unity resmi DEĞİL** (yanlış/yersiz). Her figürün gerçekten RIMA/Unity'den + yerinde olduğunu doğrula; ChatGPT-vari/gereksiz görselleri çıkar.
2. **AI-odağını AZALT.** Şu an fazla "full AI" → AI'dan BAHSEDİLSİN ama dengeli (proje AI-süreç raporu gibi durmasın; oyun+mimari öne çıksın).
3. **"Ne ne işe yarıyor" + dosya/klasör yapıları EKLE.** Sistem/sınıf sorumlulukları (hangi script ne yapar) + Assets/Scripts klasör yapısı açıklamaları (referans BarberApp'teki "Proje Yapısı" + "katmanlı kod analizi" gibi).
4. **ChatGPT'den alınmış gibi duran gereksiz pasajları çıkar** (kullanıcı böyle bir şey gördü).
5. Süreç: yeni session → /council (rapor review) → feedback maddeleri → `make_akademik_docx.py` ile yeniden üret.
