# KIRO — Sprite Organize Görevi

**DURUM:** Hazır. `organize_sprites.py` çalıştır.

---

## Ne yapıyor?

1. `STAGING/` klasörünün tamamını `_BACKUP/{timestamp}/` altına yedekler
2. `sprites.zip` dosyalarını açar, yön PNG'lerini doğru isimlendirmeyle Unity'e kopyalar:
   - `STAGING/Characters/Players/Warblade/sprites.zip` → `RIMA/Assets/Sprites/Characters/Warblade/Warblade_S.png` (S/SE/E/NE/N/NW/W/SW)
   - `STAGING/Enemies/Act1/ShardWalker/sprites.zip` → `RIMA/Assets/Sprites/Enemies/ShardWalker/ShardWalker_S.png`
   - TwiceBorn gibi alt klasörlüler → `TwiceBorn_Primary_S.png`
3. Skill ikonlarını kopyalar: `STAGING/Icons/Skills/Warblade/DeathBlow.png` → `RIMA/Assets/Sprites/UI/Icons/Icon_Warblade_DeathBlow.png`
4. Tile'ları kopyalar: `STAGING/Tiles/Act1/floor_stone.png` → `RIMA/Assets/Sprites/Tiles/Act1_floor_stone.png`
5. Animasyon zip'lerini çıkarır
6. Tüm zip dosyalarını siler (gerek yok artık)

---

## Çalıştır

```bash
cd "F:\Antigravity Projeler\2d roguelite"
python organize_sprites.py
```

---

## Sonrası

- Unity Editor'de **Ctrl+R** ile Asset Database'i yenile
- Görev tamamlandığında bu dosyaya `[DONE]` ekle
