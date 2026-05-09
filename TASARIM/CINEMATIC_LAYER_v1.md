# RIMA — Cinematic Layer v1
*Storytelling + animasyon zenginligi -- RIMA'nin 2D pixel art + roguelite + solo dev kisitlari icinde*
*Son guncelleme: 2026-05-09 | Status: SPEC v1*

## Felsefe
Alabaster Dawn'da "oyun ici storytelling zenginligi" 3 katmandan geliyor:
1. Cinematic gecisler (cutscene)
2. Environment storytelling (yasiyan dunya)
3. Karakter reaction (NPC tepkileri)

RIMA bunu **scope-dostu** sekilde 4 katmanda yapacak:

## Katman A: Camera-Driven Moments (animasyon degil, kod)
**Ne:** Kamera zoom + slowmo + Time.timeScale ile dramatize.
**Nerede:**
- Boss reveal: zoom + slowmo
- Skill draft: hafif slowmo + zoom out
- Death moment: zoom-in + slowmo + fade
- Run start: portal yaklasim + zoom transitions
- Final hit on boss: 0.2x slowmo + freeze frame

**Implementation:** `Assets/Scripts/CameraDirector.cs` (yeni). Sequence trigger'lar (event-driven).
**Maliyet:** ~5-7 gun kod, animasyon 0.

## Katman B: Environmental Storytelling (statik sprite + animation)
**Ne:** Dunya yasiyor -- sinifa-agnostik environment animation.
**Nerede:**
- Hub: NPC mini-loop'lari (Ferryman geminin basinda, Cartographer harita ustunde)
- Act 2: Zemin shader nabiz atisi (organik et hissi)
- Act 3: Yuzen platform sallanma
- Boss arenasi: ozel ambient (cyan rift dalgalanmasi, isik titresimi)

**Implementation:** Tile/prop AnimatedTile, shader graph (URP 2D).
**Maliyet:** ~1-2 hafta (Act-by-Act dagitilir).

## Katman C: Diegetic UI Moments (UI + lore)
**Ne:** UI'a lore enjekte et.
**Nerede:**
- Death recap "MUHUR" ekrani + 1 satir mini lore reveal (run-by-run progression)
- 9-run progression (her run sonrasi NPC'den 1 cumle daha)
- Echo Imprint kazanilinca "Bu pasif sana ne hatirlatiiyor?" 1 satir
- Skill draft kart'larin altinda 1 cumle flavor text
- Hub portal ustunde degisen rune metni

**Implementation:** UI work + LocalizedString tablo (Karar #51 LOCKED).
**Maliyet:** Yazili icerik production + UI ~1 hafta.

## Katman D: Manuel Cinematic Frames (ozel anlar)
**Ne:** Hand-painted cinematic still'ler veya kisa frame setleri.
**Nerede:**
- Boss intro: 1 manuel cinematic frame + camera move (4 boss = 4 frame)
- Architect 4-faz: her faz girisinde 1 cinematic frame (4 frame)
- 3 ending (KAL/KIR/TASI): her biri 3-5 cinematic frame (9-15 frame total)
- Run 1 intro: 5-7 frame (oyuncu Hub'a ilk kez girisde)
- Final reveal: 5-7 frame

**Implementation:** Aseprite/PixelLab Edit Image Pro, single-frame paintings.
**Maliyet:** ~30 manuel cinematic frame total. ~3-4 hafta.

## Toplam Scope (Cinematic Layer v1 hedefi)
- Kod: ~2 hafta (camera director + sequence system)
- Asset: ~30 cinematic frame + environment animation
- Yazili icerik: ~50-100 lore satiri (death recap + dialog drip)
- Toplam: ~2-3 ay sprint (paralel asset uretimi ile)

## Sirali Plan (Faz Master ile uyum)
- **Faz 2 sirasinda:** Katman A (camera director) + B (Hub environment) -- temel
- **Faz 3 sirasinda:** Katman C (lore drip + UI) + D (boss intro frames)
- **Faz 4-5:** Architect 4-faz cinematics + 3 ending frames

## Bagimliliklar
- TASARIM/HUB_DESIGN_v1.md (henuz yok -- lore audit kritik)
- TASARIM/STORY_RUN_PROGRESSION.md (henuz yok -- lore audit kritik)
- LocalizedString sistemi (Karar #51)
- CameraDirector kod
