# Seviye-3 E2E PLAYTEST RAPORU — 2026-06-13

> **Görev:** Bu gecenin tüm fix'lerinin demo akışını uçtan uca BOZMADIĞINI kanıtla.
> **Yöntem:** UnityMCP `execute_code` ile Play Mode'da programatik state sürme + DirectorMode `*ForValidation` hook'ları + data-proof (screenshot yok — overlay UI screenshot'a çıkmaz).
> **Sahne:** `_Arena`. Konsol playtest BAŞINDA temizlendi. Kod DEĞİŞTİRİLMEDİ (saf doğrulama).
> **Play Mode:** Oturum sonunda isPlaying=False bırakıldı (timeScale=1).

## VERDİKT: 10/10 ADIM PASS — DEMO-HAZIR

Tüm playtest boyunca konsolda **0 Error** (başta ve sonda kontrol edildi).

---

## Adım adım kanıt

### 1. Boot — PASS
- Player tag'li `Player` spawn oldu, `CurrentHP=100` (>0).
- `PlayerController.enabled=True`, `PlayerAttack.enabled=True`.
- Konsol temiz (0 Error).
- **Not (kırık DEĞİL):** Boot anında `timeScale=0` (intro/class-select gate'i; ölüm değil — DeathActive=False). Combat adımları için Director Test state'e alınınca `timeScale=1` oldu. Demo akışını etkilemez.

### 2. Combat — PASS
- DirectorMode spawn API: `SelectFirstSpawnEnemyForValidation`=True → `SpawnSelectedEnemyAtForValidation`=True → count=1.
- Düşman: `FractureImp_Director`, `hpBefore=60`.
- Packetized LMB hasarı (base 20, physPower default ~110) → `finalDmg=22` → `hpAfter=38` (düşüyor).
- Telemetri `OnDamageApplied`: event count `0 → 1` (+1). 

### 3. Skill-kill juice (bu gecenin fix'i) — PASS
- `CombatEventBus.OnKill` sayacı bağlandı.
- Öldürücü vuruş → `IsDead=True`, kill sayacı `1` (PublishKill TAM 1 kez).
- Ölü hedefe 2. vuruş → dönüş `0`, kill sayacı **hâlâ 1** (2. publish YOK). Kill-juice fix doğru.

### 4. Stat tune (bu gecenin fix'i) — PASS
- `SetStatForValidation("physPower",50)` → base-20 LMB packetized hasar `10` (×0.5).
- `SetStatForValidation("physPower",250)` → `50` (×2.5). **Tam 5× ölçeklendi** (raw değil, packetized LMB yolu).
- `bypassStatScaling:true` skill paketi (Q/E/R/F): physPower=250'de bile base-20 → `20` (slider'a SAĞIR, beklenen).
- `debugGlobalDamageMult 1→3` → packetized LMB `50 → 150` (tam 3×). Evrensel kol packetized yolda görünür.

### 5. Stat preset (yeni) — PASS
- TANK: `maxHP=300`, `physPower=40`, `debugMult=0.5` (dmg~0.5). 
- GLASS: `maxHP=30`, `physPower=220`, `debugMult=4.5` (dmg~4.5).
- DEFAULT (VARSAYILAN): `maxHP=115`, `physPower=110`, `debugMult=1` (Warblade profil default'u — Quick Reset/Reset ile aynı kaynak, tutarlı).

### 6. Spawn cap — PASS
- Başlangıç director-spawn sayısı=4; +12 spawn istendi → yalnız 6'sı kabul (acceptedTrue=6) → toplam **tam 10**. `cap=10` uygulanıyor.

### 7. Ölüm → Quick Reset (bu gecenin fix'i) — PASS
- Oyuncu öldürüldü → `IsDeathActiveForDemo=True`, `timeScale=0`.
- `DemoQuickReset` → `CurrentHP=100/100` (full revive), `IsDead=False`, `DeathActive=False`.
- Director spawn'lar temizlendi (`spawnCount=0`), `timeScale=1`.
- `PlayerController.enabled=True` VE `PlayerAttack.enabled=True` (SetPlayerActive simetrisi geri yüklendi).

### 8. Director pause saldırı-kilidi (bu gecenin fix'i) — PASS
- Director (pause): `PlayerAttack.enabled=False`, `timeScale=0`.
- Test (kapat): `PlayerAttack.enabled=True`, `timeScale=1`.

### 9. Dual-class buton (yeni) — PASS
- Draft tetiklendi → Director overlay GİZLENDİ (`Canvas_DirectorOverlay` inactive), `ClassSelectionUI._canvas.so=190` topmost, canvas aktif, `timeScale=0`.
- Elementalist seçildi → `SecondaryClass=Elementalist`, skill controller `1 → 2`, `ManaSystem` eklendi, `timeScale=1`, `IsDualClassDraftAvailable=False` (buton gizlendi).
- **Ölüm-gate:** oyuncu ölüyken draft tetiklendi → seçim AÇILMADI (`_isOpen=False` kaldı — ölüm guard'ı reddetti). 
- **Not:** İlk trigger denemesinde CSU henüz manager'a abone olmamıştı (Update bir frame gerektiriyor, execute_code frame ilerletmez); abonelik kurulduktan sonra path birebir çalıştı. Canlı sunumda (frame'ler akarken) sorun değil.

### 10. TelemetryClock pause (bu gecenin fix'i) — PASS
- Director pause'da: `TelemetryClock` örnek1=415.979; ~15.5s gerçek zaman sonra (unscaledTime 415.98 → 431.49) örnek2=**415.979** (DONUK, ilerlemedi).
- Test'e dönünce clock baseline korundu (pause aralığı doğru düşüldü). DPS penceresi pause'da ilerlemiyor.

---

## DEMO ÖNCESİ KIRIK
**Yok.** Bu gecenin tüm fix'leri (kill-juice tek-publish, stat-tune packetized scaling, preset'ler, Quick Reset simetrisi, Director pause attack-lock, dual-class buton + ölüm-gate, TelemetryClock pause) data-proof'lu çalışıyor; demo akışını bozan regresyon saptanmadı. Konsol baştan sona 0 Error.

**Küçük hatırlatma (kırık değil, koreografi):** Boot'ta `timeScale=0` gate'i normal; combat'a geçmeden önce oyunun kendi akışı (class-select sonrası) veya Director Test state ts'i 1'e alır. Sunumda stat-tune'u **yalnız LMB ile** göster (Q/E/R/F bypass — adım 4'te kanıtlı).
