# 06 — UI Flow and Playtest Bugs

Bu dosya, önceki screenshot/playtest hatalarını demo akışına bağlar.

## Bug 1 — Play basınca Skill Codex açılıyor

### Gözlem
Play/New Run sonrası CharacterSelect/Chamber gelmesi gerekirken Yetenek Kodeksi açılıyor.

### Repo kontrolü
UIManager içinde ESC doğrudan SkillCodex toggle ediyor. Bu demo için yanlış davranış.

### Demo kararı
ESC:
```text
Gameplay → PauseMenu
PauseMenu açık → Resume
Codex açık → Codex kapat
```

Codex:
- Pause menüden açılabilir.
- Ayrı debug tuşu olabilir.
- Oyun başlangıcında asla otomatik açılmaz.

## Bug 2 — Codex kapatınca kilitlenme

Muhtemel:
- Time.timeScale 0 kalıyor.
- Overlay state resetlenmiyor.
- Cursor/custom cursor çakışıyor.
- Input action map yanlış açık/kapalı.

Demo fix:
- Scene load sırasında UIManager.ResetForSceneLoad() doğrula.
- CharacterSelect → _Arena geçişinde ResumeGame() çağır.
- Codex Close() sonrası TooltipSystem.Hide() çağrısı var, ama overlay state ve timeScale de doğrulanmalı.

## Bug 3 — İki mouse cursor

Muhtemel:
- System cursor + custom cursor aynı anda aktif.

Demo fix:
- Tek CursorManager.
- Mode:
  - UI mode: system cursor visible.
  - Gameplay custom cursor varsa system cursor hidden.
  - Şimdilik en basiti: custom cursor kapalı, sadece system cursor.

## Bug 4 — Hover’da mavi tooltip/text bozulması

Muhtemel:
- Tooltip canvas yanlış canvas’a parent oluyor.
- Tooltip panel oyun sahnesi/overlay kapanınca orphan kalıyor.
- TMP overflow ve size hesapları bozuk.
- Skill icon/frame eksik olunca layout çöküyor.

Demo fix:
- Tooltip yalnızca aktif UI canvas altında yaşamalı.
- Panel kapanınca force hide.
- Tooltip max width/height sabit.
- Tooltip mavi debug text gibi değil, küçük koyu panel olarak görünmeli.
- Demo için tooltipleri kapatmak bile kabul edilebilir, yeter ki oyun kilitlenmesin.

## Bug 5 — Skill iconları bazen yüklenmiyor

Muhtemel:
- SkillData.icon null.
- fallback icon yok.
- Resource path/case mismatch.
- UI refresh icon load’dan önce çalışıyor.

Demo fix:
- Warblade ve Elementalist skill iconları için manuel Sprite reference.
- Missing icon log:
```csharp
Debug.LogWarning($"Missing icon for skill {skill.skillName}");
```
- Fallback icon zorunlu.
- Demo’da sadece aktif skilllerin iconları şart.

## Bug 6 — Warblade sword cliff altından görünüyor

Muhtemel:
- Weapon player SortingGroup dışında.
- Wrong sorting layer/order.
- Weapon child değil.
- Pivot/offset yanlış.

Demo fix:
- Weapon PlayerRoot child.
- SpriteRenderer Characters/Entities layer.
- SortingGroup altında.
- Ground/Cliff altında render olmaz.
- Offset profile class+direction bazlı.

## Bug 7 — Sağ tarafa yürünemiyor / görünmez duvar

Muhtemel:
- Collision tilemap floor’dan farklı.
- Room bounds clamp küçük.
- CompositeCollider2D içeri taşıyor.
- Walkability map eski/yanlış data kullanıyor.

Demo fix:
- Walkable debug overlay.
- Player blocked collider log.
- Visual floor = physical floor invariant testi.
- Combat odaları büyütülmeli.

## Bug 8 — Oda küçük

Demo kararı:
- Combat 24×18 altına düşme.
- Boss 32×24 altına düşme.
- Merkez combat alanı geniş.
- Cliff dekor, gameplay’i boğmamalı.

## Priority

```text
P0:
- Play flow
- ESC/Pause
- TimeScale/input lock
- Walkable/collider
- Weapon sorting

P1:
- Skill icons
- Tooltip
- Shop UI
- Boss telegraph

P2:
- Visual polish
- Better sprite pass
- More props
```
