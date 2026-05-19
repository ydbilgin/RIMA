---
status: REFERENCE
faz: 2
tarih: 2026-04-30
ozet: "Polish backlog - Faz 2+ görevleri"
---
# RIMA — Makeup Backlog
*Karar #71 sonrasi eksiklikler. Ileride yapilacak game feel + atmosphere duzeltmeleri.*
*Son guncelleme: 2026-05-09 | Status: PLANNED*

## Bagiam
Silah hep elde karari (Karar #71) combat-readability + scope kazandirdi, ama Hub atmosferi
ve karakter "yaasamis" hissi icin asagidaki katmanlar gerekli. Hepsi opsiyonel --
v1 ship blocker DEGIL. Game polish phase'inde sirayla.

## Oncelikllendirilmis Eksikler

### 1. Hub Rest Pose [PRIORITY: HIGH]
**Sorun:** NPC'yle konusurken karakter savasa hazir pozda -> baris zamani hissi yok.
**Cozum:** Hub-only still pose (silah omuzda asili / bel'de). Tek frame, 4 yon.
**Maliyet:** ~3 gun, 4 yon x 10 sinif = 40 still pose
**Bagiimli:** TASARIM/HUB_DESIGN_v1.md (henuz yok -- lore audit onerisi)

### 2. HP Dusuk "Yorgun" Idle [PRIORITY: MEDIUM]
**Sorun:** HP=10 ile HP=100 idle pose'u ayni, drama eksik.
**Cozum:** HP < %25 -> karakter idle hafif egik siljet. Sadece idle override.
**Maliyet:** ~2 gun, 4 yon x 10 sinif = 40 ekstra idle frame
**Avantaj:** "Game Communication Requirement" -- durum sade dilli iletim

### 3. Boss Intro Freeze-Frame [PRIORITY: HIGH]
**Sorun:** Boss reveal cinematic anlam tasimimyor.
**Cozum:** Boss odasina girince 3-4 sn cinematic (kamera zoom + slowmo + boss silueti).
**Maliyet:** ~2 gun x 4 boss = 1 hafta. Mevcut animasyon yeniden kullanilir + 1 cinematic frame her boss.
**Bagiimli:** TASARIM/BOSS_DESIGN_FOUR_PHASES.md

### 4. Run Start Ritual [PRIORITY: MEDIUM]
**Sorun:** Hub'tan dungeon'a gecis siradan.
**Cozum:** Portal onunde 2 sn cinematic (cyan rift acilir, karakter girer).
**Maliyet:** ~3 gun, tek seferlik (4 yon gerekmez, kamera 1 yone sabit)
**Avantaj:** "Run basliyor" psikolojik commit

### 5. Death + Muhur Sequence [PRIORITY: HIGH]
**Sorun:** Olum sonrasi direkt Hub'a teleport mekanigi. Muhur lore'unu gorsel anlatamiiyor.
**Cozum:** Dusme -> cyan rift sizar -> ekran kararir -> Hub'da uyanir mini-cinematic.
**Maliyet:** ~3 gun, Muhur VFX + Hub uyanma sequence
**Bagiimli:** Hub design v1 + lore drip pipeline

### 6. Skill Draft Drama [PRIORITY: MEDIUM]
**Sorun:** Oda temizleyince draft direkt geliyor, agirlik hissi yok.
**Cozum:** Time.timeScale = 0.3 + karakter durur (mevcut idle) + 3 kart havada belirir.
**Maliyet:** ~2 gun, sadece UI + VFX work, animasyon yok.
**Avantaj:** Karar aninin weight'i

### 7. Echo Imprint Reveal [PRIORITY: LOW]
**Sorun:** Yeni pasif kazanma gorsel olarak fark edilmiyor.
**Cozum:** Karakter etrafinda 1 sn cyan rift dalgalanmasi, sinif rengi imzasi.
**Maliyet:** ~2 gun, sadece VFX, animasyon yok.

### 8. NPC Interaction Camera Zoom [PRIORITY: LOW]
**Sorun:** NPC dialog cinematic degil, donuk.
**Cozum:** NPC yaklasiminda kamera zoom + karakter rest pose (#1'in still'i).
**Maliyet:** Kamera kodu + #1'in still pose'u (Eksik #1 ile bundle)
**Bagiimli:** Eksik #1 (Hub Rest Pose)

## Toplam Scope (hepsi yapilirsa)
- Yaklasik 4-5 hafta sprint (sirasiyla)
- Yeni asset uretim: ~80 still + 20 cinematic frame
- VFX work: 6-8 gun
- Code: kamera + slowmo + sequence triggers (~5-7 gun)

## Sirali Plan (Onerim)
1. Once HIGH priority: #1 Hub rest pose + #3 Boss intro + #5 Death sequence (3 hafta)
2. Sonra MEDIUM: #2 yorgun idle + #4 run start + #6 skill draft drama (2 hafta)
3. En son LOW: #7 Echo reveal + #8 NPC zoom (1 hafta)

