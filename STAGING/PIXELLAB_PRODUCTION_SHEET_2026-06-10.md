# PIXELLAB ÜRETİM SAYFASI — 2026-06-10 (kullanıcı üretir, Claude indirir+bağlar)

> İş bölümü: SEN üret → "hazır" de → BEN indirip klip kontratına göre bağlarım (iskelet W2'de hazır).
> **VERIFY-FIRST:** Önce P1'i (Warblade RUN) üret → ben doğrulayayım → sonra seri devam.
> Limit: 1070 generation kaldı — aşağıdaki çekirdek liste ~40-60 gen, bol pay var.

## KLİP KONTRATI (bağlama bilgisi — senin işin değil ama adlandırma bununla eşleşir)
- State'ler: Idle (var) · **Run** · **Attack** (Warblade) / **Cast** (Elementalist)
- **Animator kontratı (W2'de keşfedildi): 4-DİYAGONAL state** (SE/NE/NW/SW; `Speed`+`DirX/DirY` parametreli AnyState geçişler). Klipler: `<class>_<state>_<SE|NE|NW|SW>.anim` — iskelet HAZIR, boş placeholder'lar bekliyor.
- SEN yine 8 yön üret (animate_character default'u) — bağlarken 4 diyagonali ben seçer/doldururum. 12 fps · Run loop AÇIK, Attack/Cast loop KAPALI

---

## P — OYUNCU ANİMASYONLARI (mevcut karakterler, yeniden üretme — sadece ANIMATE)

### P1 · Warblade RUN ⭐ İLK BU
- Karakter ID: `2656075d-d113-4f18-a6c1-94b5a6b8bf65`
- `animate_character` → **template**: `running-8-frames` · directions: HEPSİ (8) 
- Bozuksa fallback **v3**: action_description `"running fast"`, frame_count 8

### P2 · Elementalist RUN
- Karakter ID: `4c83c0be-e856-48f1-b8b5-9626e041a082`
- Aynı: template `running-8-frames` · 8 yön · fallback v3 `"running fast"`

### P3 · Warblade ATTACK
- ID: `2656075d-...` · **v3**: `"two-handed overhead greatsword swing, strong downward chop"` · frame_count 8 · 8 yön

### P4 · Elementalist CAST
- ID: `4c83c0be-...` · **template**: `fireball` · 8 yön
- Fallback v3: `"casting a spell, both hands thrust forward releasing energy"`, frame 8

---

## B — BOSS (PenitentSovereign — şu an placeholder, YENİ karakter üretilecek)

### B1 · Boss karakteri (create_character)
```
description: "towering corrupted knight-priest boss, cracked obsidian armor with
glowing cyan fracture lines, tattered ceremonial robes over heavy plate, broken
halo of floating stone shards above head, heavy imposing silhouette, dark fantasy"
n_directions: 8 · size: 160 · view: high top-down · proportions: heroic
```
- Palet notu: taş/arduvaz gövde + **cyan (#00FFCC) çatlak/mühür vurguları** (kanon: cyan=Rift enerjisi) + az sıcak turuncu YOK (boss yaşam değil yıkım).

### B2-B4 · Boss animasyonları (B1 onaylandıktan sonra, aynı ID'ye)
| # | State | Yöntem | Not |
|---|---|---|---|
| B2 | Walk/Advance | template `walking-8-frames` (ağır his için fallback v3 `"slow heavy menacing walk"`) | loop AÇIK |
| B3 | Attack | v3 `"massive overhead two-handed slam attack with both arms"` 8f | demo'da 1 atak animu yeter — telegraph'lar kod-VFX |
| B4 | Death | template `falling-back-death` | loop KAPALI |
- (ops) B5 Cast: v3 `"raising arms summoning dark energy"` — faz-2 büyüleri için, zaman kalırsa.

---

## M — MOB'LAR (OPSİYONEL — demo 3 animasyonlu mob'la zaten dönüyor; zaman kalırsa S43 roster)

> Mevcut animasyonlu: FractureImp ✓ HalfThrall ✓ Penitent ✓ (üçü de wave'de). Aşağısı çeşitlilik bonusu.

| # | Mob | create_character size (S43 LOCK) | Anim seti |
|---|---|---|---|
| M1 | Shard Walker | 112 | walk + attack (v3 `"lurching walk"` / `"crystal arm stab"`) |
| M2 | Void Thrall | 128 | walk + attack |
| M3 | Relic Caster | 80 | walk + cast |
- Stil: high top-down, 8-dir; palet = arduvaz/void-moru gövde + tek cyan vurgu. Her mob için önce karakter, onay, sonra anim.

---

## SIRA ÖZETİ
1. **P1 → doğrulama → P2-P4** (oyuncu hissi = en yüksek kazanç)
2. **B1 boss görseli → onay → B2-B4** (klimaks)
3. M'ler tamamen opsiyonel
Ben her "hazır" dediğinde: indir → Unity import (Point/PPU64) → klip doldur → Animator'da canlı doğrula.
