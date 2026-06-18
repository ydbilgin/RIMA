# Council next priority - Bugra - 2026-06-18

## Karar

Demo arifesinde hedef, "canli sunumda bozuk gorunur" risklerini kapatmak ve core combat davranisina minimum dokunmaktir. Siralama: once seyircinin hemen gorecegi sessiz/yanlis feedback ve Director Mode kiriklari, sonra dusuk riskli UI okunurlugu, sonra mevcut polish'i tek paket halinde dondurme. Derin combo correctness ve perf ertelenmeli.

## Yarina kadar aksiyon plani

1. Failed-cast feedback ekle.
   Demo degeri: cok yuksek. Oyuncu skill basip hicbir sey gormediginde oyun bozuk sanilir. Kaynak yetersiz, menzil disi, hedef yok gibi veto durumlari icin kisa SFX, caster flash veya toast yeterli.
   Risk: dusuk-orta. Hasar ve skill execution path'ine degil, CanExecute/veto sonucunun UI/audio feedback ucuna dokunulmali.

2. Director dup-slot engelini kapat.
   Demo degeri: cok yuksek. Director Mode demo-centerpiece oldugu icin ayni skill'in ikinci slota alinmasi ve shared cooldown davranisi sunumda direkt gorunur.
   Risk: dusuk. Loadout/slot validation seviyesinde duplicate reject veya swap kuralidir; combat numerics'e dokunmadan cozulmeli.

3. Merchant PERSISTENT Echo drain'i kapat.
   Demo degeri: yuksek. Shop'un meta-currency harcamasi run-vs-meta sinirini guvensiz gosterir ve demo sonrasi progress kaybi hissi yaratir.
   Risk: dusuk-orta. Merchant spend kaynagi run-local currency'ye sabitlenmeli veya persistent spend yolu demo icin hard-block edilmelidir.

4. Polish'i commit'lemeden once exposure'i tek ayar olarak degerlendir: postExposure 0.35 -> 0.6.
   Demo degeri: yuksek. Editor canli sunumunda karanlik sahne, yapilan combat/polish isini saklar. Moody atmosfer korunacaksa bile oynanabilirlik icin 0.6 tercih edilir.
   Risk: dusuk. Rendering ayari, core gameplay riski yok. Tek sahne goruntu kontrolunden sonra mevcut polish ile ayni commit'e alinmali.

5. HUD HP-bar lerp + toast ease uygula.
   Demo degeri: orta-yuksek. Canli izleyici hasar ve bildirim akisini daha rahat okur.
   Risk: dusuk. UI katmani, core combat sonucu degismez.

6. Low-HP/Rage red-screen de-stack yap.
   Demo degeri: orta. Overlay glitch, boss/low-health anlarinda gorunurse amator durur.
   Risk: dusuk-orta. Sadece overlay arbitration/alpha clamp seviyesinde kalmali.

7. Dead-but-acting penceresini kapat, yalnizca en dar guard ile.
   Demo degeri: orta. Oyuncu oluyken kapi/odul alirsa sunumda bariz bozulur; ancak 2.3s pencere rastlantiya bagli.
   Risk: orta. Interaction gate'e `isDead/isDying` guard eklemek yeterli; reward/door state machine'e genis refactor yapilmasin.

8. Hit-flash beyaz icin sadece mevcut hasar feedback ucunda lokal degisiklik yapilabiliyorsa al; degilse ertele.
   Demo degeri: orta. Combat okunurlugunu artirir.
   Risk: orta. Damage path'ine genis dokunma gerektirirse demo arifesinde degmez.

## Ertele

- healMultiplier kalici bozulma: Post-demo. AntiHealAura x Crippling Blow race ciddi ama concurrency/save-restore davranisi core stat sistemine dokunur; demo olasiligi dusukse riskli.
- Glacial+Burn detonate, Ice-Shatter dead code, Severance 1-Scar: Post-demo. Combo correctness onemli ama subtle; demo akisini direkt kurtarmaz.
- 9 Find-in-hot-path perf isi: Post-demo. CameraFollow, BaseMobBehavior, PlaytestRoomClearedHelper en kotu adaylar olsa da demo-blocker degil. Guarded cache/refactor demo sonrasi yapilmali.

## Commit karari

Mevcut demo-polish commit'lenmeli, ama tek sartla: exposure karari ayni paket icinde netlestirilsin. Benim kararim 0.6 postExposure; canli editor sunumu icin "gorunur ve okunur" atmosferden daha yuksek oncelik. Commit, gameplay fixlerinden ayri tutulmali: once polish commit, sonra demo-relevant bugfix commitleri.
