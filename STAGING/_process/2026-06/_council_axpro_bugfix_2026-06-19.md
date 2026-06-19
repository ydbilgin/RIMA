# Bug-Fix Council: 2026-06-19 Triyaj Raporu (READ-ONLY)

## 1. T9 Fix Yaklaşımı
* **Öneri:** (A) AttackTokenManager aynalanmalı. `OnDestroy` içindeki `_shuttingDown = true;` satırı kaldırılmalı (sadece `instance = null;` kalmalı). 
* **Gerekçe:** Kanonik ve güvenli yol budur. `ResetStatics` (SubsystemRegistration) scene reload'da çalışmaz, sadece play-mode girişinde çalışır. `_shuttingDown` uygulamanın tamamen kapandığını ifade eder, bu yüzden sadece `OnApplicationQuit` içinde set edilmelidir.
* **Risk:** Sıfıra yakın (AttackTokenManager'da kanıtlanmış).

## 2. Kapsam (Kardeş Singletonlar)
* **Öneri:** `RunStats`, `CheckpointManager` ve `BuildTileBrushController` ŞİMDİ düzeltilmelidir.
* **Gerekçe:** Yapılan statik analizde (grep) aynı `_shuttingDown=true` hatasının bu dosyaların `OnDestroy` metodlarında olduğu doğrulandı. Restart sonrası statlar veya checkpoint erişilemez olursa demo doğrudan kırılır (demo-killer risk).
* **Risk:** Yok, tutarlılık sağlar.

## 3. T7 (TimeScale Race)
* **Öneri:** Ucuz savunma kalkanı demo sigortası olarak EKLENMELİDİR.
* **Gerekçe:** `RoomRunDirector.cs:1940` içindeki `RestoreGameplayTimeScale` doğrudan timescale'i 1 yapıyor. Run clear `finally` bloğu ile Draft UI pause'u çakışırsa panel açıkken oyun akmaya başlar. `if (DraftManager.Instance?.IsDraftActive == true) return;` kontrolü eklenmesi çok ucuz ve garantili bir sigortadır.
* **Risk:** Normal akışa zarar vermez, defansif açıdan risksiz.

## Özet
* **Fix Önceliği:** DraftManager (T9) > Kardeş Singletonlar > RoomRunDirector (T7).
* **Demo Minimum Şartı:** T9 ve kardeş singletonlar kesinlikle fixlenmeden demo derlenmemelidir.
