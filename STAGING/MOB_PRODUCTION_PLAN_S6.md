# RIMA — Mob Üretim Planı (S114 S6, 2026-05-29)

> **Yazar:** Opus (karar taslağı). **Durum:** cx (mevcut-asset ground-truth) + agy (üretim-verimlilik) review BEKLİYOR → sonra "beraber karar". **Kaynak:** NLM Q1 (boss 3-faz + roster) + memory (PixelLab KB, weapon/anim converged, demo-asset-locks) + Explore room-map.

---

## 1. Demo mob roster (LOCKED — Faz 1)
| Mob | Rol | HP | Mevcut durum (Explore) |
|---|---|---|---|
| **FractureImp** | temel melee imp | ~? | ✅ Tam: gerçek art + 8-dir + shard-scatter death |
| **ShardWalker** | skirmisher (hızlı) | 55 | 🟡 Graybox prefab + **8-dir idle/walk anim VAR**, attack/death YOK |
| **HollowHulk** | tank (yavaş, ağır) | 280 | 🟡 Graybox, **anim YOK** |
| **PenitentSovereign** | Act 1 BOSS, 3-faz | (canon 3-faz) | 🔴 Prefab var, **sprite YOK** — üretilecek |
| *(Corrupted Elite)* | varyant | +affix | ❌ W8: recolor+aura (yeni sprite YOK) |

Boss saldırıları (NLM canon): Chain Whip (6m çizgi) · Penitent Surge (4m radius itme) · Shackle Cast (8m slow). Faz geçişi %66/%33 HP, zincir kırılır → hız +%30 + Rift Tear hazard.

## 2. Üretim pipeline (LOCKED — PixelLab KB)
- **Boyut:** mob = karakter standardı (64px içerik / 120 canvas, **PPU 64**). Boss = **128-192px** (büyük silhouette).
- **Yön:** player 8-dir (5 bake S/SE/E/NE/N + 3 mirror flipX W/SW/NW). **Mob için KARAR GEREKLİ: 8-dir mi, 4-dir mi (S/E/N + W mirror)?** Mob'lar daha az incelenir → 4-dir maliyeti yarıya indirir.
- **Tool:** `create_character` (size≤128, directional + animation) — yaratıklar için. Ref-art önce `$imagegen` (Codex, gated DEĞİL) → PixelLab init-image. State-anchored: mid-walk state → `animate` (first_frame+enhance).
- **Anim state'leri (demo enemy):** idle + walk + attack + death. Hit-react = flash/shake (sprite gerekmez).
- **Gen maliyeti:** 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen **per dir**. 
- **🔒 GATED:** PixelLab MCP gen = kullanıcı onayı (HARD rule). $imagegen ref-art gated değil.

## 3. Per-mob üretim planı + gen maliyeti (8-dir / 4-dir)
| Mob | Gerekli | Gen tahmini (8-dir) | Gen (4-dir) |
|---|---|---|---|
| FractureImp | — (hazır) | 0 | 0 |
| ShardWalker | attack + death ekle (idle/walk var) | ~20 | ~10 |
| HollowHulk | tam set (idle/walk/attack/death) | ~35 | ~18 |
| PenitentSovereign | tam boss (idle/walk + 3 attack + intro/death, büyük) | ~50-70 | ~30-40 |
| Corrupted Elite | **0** (recolor + cyan aura shader + 1 tweak) | 0 | 0 |
| **TOPLAM** | | **~105-125 gen** | **~60-70 gen** |

## 4. Ucuz-çeşitlilik stratejisi (RIMA-özgün)
- **Corrupted Elite (W8):** mevcut mob → palette-swap (cyan-corruption) + Light2D aura + 1 saldırı tweak. Zero gen, anında çeşitlilik. (agy önerisi: "Corrupted" variant.)
- **HollowHulk demo opsiyonu:** graybox kalsın (loop çalışır) → demo sonrası gerçek sprite. Tank rolü silhouette-okunur olduğu sürece graybox kabul edilebilir.
- **Boss demo opsiyonu:** graybox boss ile loop'u tamamla (mekanik test) → gerçek sprite W10'da (gated, en pahalı iş).

## 5. ★ KARAR NOKTALARI (beraber karar)
1. **Mob yön sayısı:** 8-dir (player kalitesi, ~105-125 gen) **vs** 4-dir (~60-70 gen, mob'lar için yeterli olabilir)? → Önerim: **mob 4-dir, boss 8-dir.**
2. **Demo art bar:** boss + HollowHulk **graybox ile demo-loop tamamla** (mekanik önce), gerçek sprite sonra → **vs** demo'dan önce tam sprite? → Önerim: **graybox loop önce, sprite W10 (gated).**
3. **Üretim sırası:** ShardWalker attack/death (en ucuz tamamlama) → Corrupted elite (zero-gen) → HollowHulk → Boss (en pahalı, gated). 
4. **Ref-first:** her mob için önce `$imagegen` ref-art (gated değil) → onayla → PixelLab init. Onaylar mısın bu akışı?
5. **Gen bütçesi:** demo mob seti 4-dir ~60-70 gen. Tier 2'de ~1208 gen kalan vardı → bütçe rahat. Onay?

---

## 6. agy REVIEW FOLDED (2026-05-29) — Opus benimsedi
- **Yön:** regular mob **4-dir** ✅ · **skirmisher (ShardWalker) 2-dir + flipX** (hızlı/slide) · **boss 8-dir** ✅.
- **Anim minimum:** sadece **Walk + Attack** elzem. **Hit-react sprite İPTAL** → white-flash + shake + hitstop (0-gen). **Death sprite İPTAL** → **shader dissolve + cyan particle scatter** (0-gen, Rift temasına da uyar).
- **Elite recolor reçetesi:** shader Red→Cyan(#00FFCC)+mor remap · scale ×1.2 · Light2D pulse aura · ParticleSystem rift külleri · 1 affix (speed ×1.2 / dmg ×1.5). 0-gen.
- **Boss:** demo = **2-faz** (canon 3 değil), faz geçişi yeni sprite değil **shader aura + hız çarpanı**. Graybox loop (Track 0) → tam sprite public demo'dan önce (Track 3/W10).
- **Gen bütçesi:** dissolve-death + 4-dir filtreleriyle ~60-70 → **~40-48 gen** (%30+ tasarruf). Tier 2 rezervi (~1208) rahat.

**Güncel toplam tahmin:** ~40-48 gen (4-dir + dissolve-death). cx ground-truth review (mevcut prefab/anim) hâlâ bekliyor → gelince final.

**Karar (Opus, beraber onaya):** mob 4-dir / skirmisher 2-dir / boss 8-dir · death=dissolve-shader · hit=flash+shake+hitstop · elite=recolor (0-gen) · boss 2-faz demo · graybox-loop-first. **Onaylıyor musun?**

## 7. Codex GROUND-TRUTH FOLDED — ⚠️ KRİTİK DÜZELTME
**TÜM demo mob'ları şu an runtime'da PLACEHOLDER-kare (FractureImp DAHİL).** Önceki "FractureImp DONE" YANLIŞTI.
- **FractureImp:** 8-dir clip+controller diskte VAR ama prefab'a **controller bağlı DEĞİL** (m_Controller fileID:0) + sprite atanmamış (PlaceholderSprite) + anim'ler **ARŞİV sprite'larına** referans (Assets/ dışı → kırık) + shardPrefab null.
- **ShardWalker:** aynı durum (controller bağsız, placeholder, anim'ler arşiv-sprite, attack/death yok).
- **HollowHulk:** sprite+anim tamamen yok.
- **PenitentSovereign:** sprite yok AMA **Health var + bossMaxHP 800 runtime → DemoComplete hook ÇALIŞIR** ✅.
- **EliteAffix:** tint(recolor) ÇALIŞIR; **aura/Light2D YOK**; Berserker/Vampiric affix base mob'a bağlı DEĞİL; **Shielded HP-scale BUG** (int div → x1).

**🟡 YENİ KARAR NOKTASI (decide together) — mob sprite'ları placeholder:**
- (A) **Arşiv-restore (0-gen):** `ARCHIVE/Sprites_Enemies_old/` sprite'larını Assets/'a taşı + controller'ları prefab'a bağla + sprite ata. Eski sanat kabul edilebilirse en ucuz.
- (B) **PixelLab regen** (gated, ~40-48 gen, agy-optimized).
- (C) graybox demo (wishlist için zayıf).
- **Opus önerisi:** ÖNCE (A) dene (FractureImp+ShardWalker restore+wire, 0-gen) → yetersizse (B). Boss = (B) regen (arşivde yok). Her mob için integration işi (controller-wire + sprite-assign + shardPrefab) gerekli — gen'den ayrı.
- **Yan fix'ler (W8):** EliteAffix Shielded int-div bug + aura/Light2D ekleme + Berserker/Vampiric wiring.
