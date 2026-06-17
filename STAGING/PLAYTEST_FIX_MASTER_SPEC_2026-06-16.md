# RIMA Playtest Fix Master Spec (council 3/3 sentezi, 2026-06-16)
> Kaynak: `STAGING/_process/2026-06/playtest_council/RESP_{cx,axpro,axflash}.md` (cx+ax Pro+ax Flash oybirliği). Demo 19 Haz, solo dev. **Scope-kilit: mevcut sistemleri ŞİŞİR, sıfırdan yazma.**

## ÖNCELİK SIRASI
**P0 — DEMO-KRİTİK (soft-lock / oynanamaz):**
### FIX-1 🔴 Reward-flow stall (B1) — `RoomRunDirector.cs`
Kök-neden (cx): clear coroutine'i exception yiyor (null/destroyed) → `finally` yok → `timeScale` 0.3'te asılı + reward-spawn/door-open tetiklenmiyor (State=Cleared ama activeReward=null).
- **(a)** Clear→slowmo akışını `try/catch/finally` ile sar; `finally { Time.timeScale = 1f; }` HER ZAMAN çalışsın.
- **(b) DECOUPLE (en kritik, 3/3 vurguladı):** Kapı-açma'yı reward-spawn'dan AYIR. Düşman ölünce/State=Cleared olunca kapılar **sarsılmaz bir bool ile bağımsız** açılsın — reward spawn çökse bile kapı çalışsın (Binding of Isaac pattern: door-open = logical+instant, animasyon beklemez).
- **(c) Timeout safety-net:** State=Cleared sonrası 3sn içinde `activeReward==null` ise → `SpawnFallbackReward()` ya da direkt `OpenExitDoors()` zorla.
- **(d) Fallback "joker" (ax Pro):** Reward spawn fail ederse oyuncuya otomatik düşük-değer ödül (HP/echo) ver; ASLA boş geçme.
- Verify: execute_code ile clear→Cleared→reward/door geçişi + timeScale 3sn sonra ==1 assert.

### FIX-2 🔴 Overlay scrim / modal-stack (B3) — `UIManager.cs` + yeni `UI_Scrim` prefab
3/3: Canvas z-order karışık. **Tek paylaşılan `UI_Scrim_Dimmer`** (siyah ~%75 transparan, raycast-block) her modal (Draft/Pause/RunMap/Director/Codex) açılınca ALTINA otomatik eklensin + modal `Canvas.sortingOrder` en üste. LIFO modal-stack. → M draft-üstüne basınca bleed biter.

**P0.5 — Combat hissi (demo-feel):**
### FIX-3 🟠 Hades-style wave (B2) — `EncounterWaveSO` + `EncounterController.cs`
Sıfırdan değil: `EncounterWaveSO.Budget` x3 + yeni `NextWaveTriggerRatio` (önceki wave %~75-80 ölünce yeni wave). Demo hedef: **oda başına 3 wave, wave başına 4-5 mob**, toplam ~20-30sn combat. + **spawn telegraph** (1-2sn zemin/portal işareti spawn'dan önce — "unfair" hissini önler, Hades pattern).

**P1 — DEMO-WOW:**
### FIX-4 🟢 Boss mob — `Boss_FractureImp` (FractureImp duplicate)
Scale **x2.5**, HP/posture **x10**, **telegraph mekaniği** (saldırı öncesi 1sn windup → altında kırmızı circle-decal fill scale → dolunca hitbox aktif). Boss-skill'leri ŞİMDİLİK sadece VFX placeholder (gerçek mekanik post-demo). Üstte **BossHealthBar** (zaten var: `UI/BossHealthBar.cs`) + boss adı.

### FIX-5 🟢 Background ambiyans (B4)
Siyah void YASAK → atmosferik renk (koyu mor `#1a0f2e` / koyu mavi-yeşil). "Out-of-bounds/bug" hissini kaldır. (Cliff-ada arkası dolsun.)

### FIX-6 🟢 Mob/HUD okunabilirlik (B5)
- Düşman siyah-blob → **outline/rim-light** (beyaz/kırmızı kontrast) + **focus-point pixel** (göz/kırmızı) + size **x1.5**.
- HUD kırmızı tint → alpha **max %30**, sadece ekran KENARLARI (full-screen değil).

**SUPPORT:**
### FIX-7 DebugLogCanvas (B6) — playtest log paneli
Sol-alt dev-only on-screen log (skill alındı / wave geldi / reward spawn / HATA). Lunar Console tarzı basit UI Text. Test'i + kullanıcının "log penceresi" isteğini karşılar.

## PLAYTEST CHECKLIST (ax Flash, demo golden-path — PASS kriteri)
- [ ] MainMenu backdrop doğru + kırmızı tint sıfır
- [ ] Oda combat: 3 wave spawn-işaretiyle geldi + düşman siluet okunur
- [ ] Clear: 0.3 slow-mo tetik + SONRA normale döndü (timeScale=1)
- [ ] Ödül belirdi + G çalışıyor + **skill grant oldu** (owned listesi büyüdü)
- [ ] Draft: arka scrim karardı · M basınca map arkayı kapadı (bleed yok)
- [ ] Kapı: yeşil + collider açık + sonraki odaya geçiş
- [ ] **Flow: 5 oda KESİNTİSİZ döngü** (1 takılma = FAIL); boss-fight telegraph okunur
> İyi-oynanış (ax Pro): flow-state 5x kesintisiz · juice (hit-stop 0.05 + flash + particle) · escalation (oda 2 belirgin zorlaşır). Combat-feel/okunabilirlik = MANUEL OBS (kullanıcı); NullRef/state/timeScale = otomasyon (orchestrator execute_code).
