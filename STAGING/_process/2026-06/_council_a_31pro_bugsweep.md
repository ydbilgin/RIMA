Bu dosyalar arası mimari ve state-machine derin analizinde, özellikle sistemlerin birbirini nasıl ezebileceğine (TimeScale override, lifecycle leak, kamera desync) odaklandım. Sistemik riskler ve demo-blocking bulgular aşağıdadır:

### SİSTEMİK RİSK & BUG RAPORU

1. `[DEMO-BLOCKING] DraftManager.cs:114` — Secondary class seçim event'ine (OnSecondaryClassSelected) cleanup yapılmayan anonim lambda ile abone olunuyor — 2. Run'da (tekrar oynanışta) yok edilmiş eski instance hata fırlatır ve dual-class draft'ı açılmaz.
2. `[DEMO-BLOCKING] DirectorMode.cs:658` — Free-camera hedefini tutan `hasCameraTarget` flag'i oda değişimlerinde veya çıkışlarda hiçbir zaman `false` yapılmıyor — İkinci odada Build Mode'a (veya Director'a) geçildiğinde kamera mevcut odayı bırakıp birinci odanın koordinatlarına (void'e) geri fırlar.
3. `[DEMO-BLOCKING] DirectorMode.cs:338` — `ResolveTimeScaleForState`, TimeScale'i 1'e ayarlarken `DraftManager.IsDraftActive` durumunu hesaba katmıyor — Draft UI açıkken Director veya Build modundan çıkılırsa oyun unpause olur ve oyuncu draft ekranında kilitliyken hasar alır.
4. `[DEMO-BLOCKING] RoomRunDirector.cs:1754` — `RestoreGameplayTimeScale()` fonksiyonu `Time.timeScale` değerini koşulsuz olarak 1f'e zorluyor — Draft ekranı (örn. chest'ten) açıkken slow-mo veya clear effect biterse oyun time scale'i sıfırlanır ve draft pause'u bozularak oyunu akıtır.
5. `[DEMO-BLOCKING] DraftManager.cs:159` — `HandleRoomCleared` metodu 2 saniyelik `ShowDraftDelayed` coroutine'ini başlatırken `IsDraftPending` flag'ini `true` yapmıyor — Bu 2 saniyelik korumasız aralıkta farklı bir draft trigger'lanırsa (örn. kit açılışı) state yarışa girer ve oyun iki Draft UI'ını üst üste açar.
6. `[DEMO-BLOCKING] DraftManager.cs:195` — `ShowDraft()` gövdesinin en başında aktif draft'ı engelleyen `if (IsDraftActive) return;` koruması bulunmuyor — Portal bypass ve gecikmeli çağrılar (ShowDraftDelayed) aynı anda tetiklenirse UI çoklu defa yüklenip oyunu kilitler.
7. `[DEMO-BLOCKING] DraftManager.cs:179` — `TriggerDraftFromFragment`, draft çağrısı yaparken yalnızca `IsDraftActive`'i kontrol edip `IsDraftPending`'i atlıyor — Mevcutta 2 saniye gecikmeli bir draft beklemesi varken porta girilirse pending draft kesintiye uğrar ve üst üste binme yaratır.
8. `[POLISH] DamageCalculator.cs:50` — (SUSPECTED) `cappedIdentityBuild` ve `debugMult` stat çarpanları doğrudan ham hasarla çarpılıyor (`1 + multiplier` formülü yerine) — Eğer nötr `ClassStatProfile` default değerleri 0 dönüyorsa, oyuncunun base hasarı tamamen sıfırlanıp min threshold olan 1'e yuvarlanır.

