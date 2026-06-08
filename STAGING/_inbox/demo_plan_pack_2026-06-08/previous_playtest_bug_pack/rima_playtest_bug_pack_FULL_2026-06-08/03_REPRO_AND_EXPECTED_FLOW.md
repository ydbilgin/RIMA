# Repro Steps ve Beklenen Akış

## Repro 1 — Start / Character Select bypass

1. Unity'de oyunu Play mode'da başlat.
2. Main Menu'de Start/Play butonuna bas.
3. Beklenen: CharacterSelect veya Attunement Chamber açılır.
4. Gerçek: Kullanıcı görsellerinde doğrudan arena/gameplay benzeri sahne ve/veya Codex/UI paneli görünüyor.

Beklenen flow:

```txt
MainMenu
→ CharacterSelect / ChamberSelectBootstrap
→ Class seçimi / Attunement
→ _Arena
→ RoomRunDirector.BeginRun
→ IsoRoomBuilder.Build(template)
```

## Repro 2 — ESC yanlış panel açıyor

1. Gameplay veya arena sahnesinde ESC bas.
2. Beklenen: Pause Menu açılır.
3. Gerçek: “Yetenek Kodeksi” full-screen paneli açılıyor.

Beklenen ESC davranışı:

```txt
Gameplay + ESC = PauseMenu aç
PauseMenu + ESC = Resume / Close
SkillCodex açıkken ESC = SkillCodex kapat, önceki state'e dön
```

Pause Menu minimum seçenekleri:

```txt
Resume
New Run
Settings
Skill Codex
Exit to Main Menu
Exit Game
```

## Repro 3 — Görsel floor var ama player yürüyemiyor

1. Arena içinde sağ taraftaki floor çıkıntısına yürü.
2. Mor aim/debug line'ın sağ tarafına geçmeyi dene.
3. Beklenen: Floor görünen tüm alan walkable olmalı.
4. Gerçek: Sağ taraf invisible wall / clamp ile bloklanıyor.

## Repro 4 — Weapon/sword cliff altında görünüyor

1. Player'ı cliff kenarına ve köşelere yürüt.
2. Weapon sprite'ın nerede göründüğüne bak.
3. Beklenen: Weapon karakterle birlikte floor üstünde ve Entities sorting layer'da görünmeli.
4. Gerçek: Weapon cliff arasından/altından görünüyor, açı ve silüet yanlış.

## Repro 5 — Codex hover / icon loading

1. ESC veya mevcut yolla Skill Codex panelini aç.
2. Skill satırlarının üstüne mouse ile gel.
3. Beklenen: Temiz tooltip veya hiç tooltip yok.
4. Gerçek: Mavi bozuk text/katman çıkıyor.
5. Bazı skill iconları boş/fallback görünüyor.
