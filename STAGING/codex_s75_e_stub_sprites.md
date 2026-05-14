# S75-E — Stub Placeholder Sprites

**Effort:** small
**Prereq:** S75-D merged

---

## GOAL

PixelLab gen olmadan oyun mekanik test edilebilsin. Renkli placeholder 64×64 PNG'ler oluştur, CharacterClassDefinition + MobDefinition SO'larına assign et.

User PixelLab Create Image Pro batch'ini bitirdiğinde manuel replace edebilir.

---

## STUB SPRITE GENERATOR

**Yeni dosya:** `Assets/Editor/StubSpriteGenerator.cs`

Menu: `RIMA > Tools > Generate Placeholder Sprites for Classes + Mobs`

Davranış:
1. `Assets/Art/Placeholders/Classes/` ve `Assets/Art/Placeholders/Mobs/F1/` klasörleri oluştur
2. Her CharacterClassDefinition asset için:
   - 64×64 PNG: solid background color (deterministic from class name hash) + 2-letter initials center-aligned (örn. "WB" Warblade, "SB" Shadowblade)
   - Save as `Assets/Art/Placeholders/Classes/{className}_placeholder.png`
   - Import as Sprite, PixelsPerUnit=64, FilterMode=Point
   - Assign to CharacterClassDefinition.idleSprite
3. Her MobDefinition asset için:
   - canvasSize PNG (64x64 / 80x80 / 96x96): role-based color (Swarm=green, Elite=red, Caster=purple, etc.) + 2-letter initials (örn. "SC" Seam Crawler)
   - Save as `Assets/Art/Placeholders/Mobs/F1/{mobName}_placeholder.png`
   - Import as Sprite
   - Assign to MobDefinition.idleSprite
4. Weapon placeholders (CharacterClassDefinition.weaponSprite):
   - Use canvasSize from class def (56x20, 48x56, etc.)
   - Solid color (tan #B0935D for sword, dark blue #2A3A5A for bow, etc.) + small "W" letter
   - Save as `Assets/Art/Placeholders/Weapons/{class}_weapon_placeholder.png`
   - Assign to weaponSprite if weaponDecoupled
5. AssetDatabase.SaveAssets + Refresh
6. Debug.Log özet

---

## IMPLEMENTATION HINT

Texture2D create + SetPixels color block + initials via font rasterization (basic 8x8 pixel font OR draw rectangles for letters):

```csharp
private static Texture2D CreatePlaceholder(int w, int h, Color bg, string initials)
{
    var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
    var pixels = new Color[w * h];
    for (int i = 0; i < pixels.Length; i++) pixels[i] = bg;
    tex.SetPixels(pixels);
    // Draw initials with a simple 5x7 pixel font (hardcoded letter bitmaps)
    DrawText(tex, initials, w / 2 - initials.Length * 3, h / 2 - 4, Color.white);
    tex.Apply();
    return tex;
}

private static void DrawText(Texture2D tex, string text, int x, int y, Color color) {
    // Use a hardcoded 5x7 pixel font bitmap for A-Z. Each letter is byte[7] with 5-bit rows.
    // Iterate text characters and stamp into texture.
}
```

Font bitmap source: built-in basic 5×7 font (e.g., Hexagon or any simple bitmap font). Define inline as static readonly dictionary.

---

## VALIDATION

1. `dotnet build` PASS
2. Run "Generate Placeholder Sprites" menu → 16 character + 6 mob + 8 weapon placeholders created (CUT grimoire and bare fists don't get weapon placeholders)
3. CharacterClass Warblade.idleSprite ≠ null, weaponSprite ≠ null
4. Mob SeamCrawler.idleSprite ≠ null
5. Inspector preview shows colored square with letters
6. Console error 0

---

## COMMIT MESAJI

```
[S75-E] Stub placeholder sprites for classes + mobs

- StubSpriteGenerator editor menu: Generate Placeholder Sprites
- 64x64 character placeholders (solid color + initials)
- 64/80/96 mob placeholders (role-colored + initials)
- Weapon placeholders per class canvas size (56x20, 48x56, etc)
- Auto-assigned to CharacterClassDefinition + MobDefinition asset idleSprite/weaponSprite
- Allows gameplay test without real PixelLab gen (user replaces later)
```
