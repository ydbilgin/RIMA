---
status: LOCKED
faz: 1
tarih: 2026-05-13
ozet: "Kararlar #81+83 açık vista kompozisyon ve guardrail"
---
# Open World / Vista Integration — Draft Decision (Opus)
**Tarih:** 2026-05-13 (S60)
**Status:** DRAFT for Codex review
**Trigger:** User 2026-05-13 — "maplerin kapali alan yerine daha acik alanlar olmasini istiyorum dunyalarin birlesimini acik dunyada yariklarla anlatabilir miyiz"

---

## Konu

Mevcut LOCKED mimari (Karar #61): Hades-style discrete room flow + StS macro graph hibrit. Open world REJECTED (sebep: 55-70dk run + draft pacing + backtracking allow).

Karar #62: Act 1 = 15 node (Entry/Combat/Elite/Rest/Shop/Curse/Mystery/Boss) lineer + 2 dal.

Karar #63: Map Fragment + Kirik Tas Tablet (StS2-style reveal).

Lore (NLM): "The Fracturing" = dunyalar arasi koparma operasyonu. Rift March = dis guclerin gercekligumize yuruyusu. RIMA = Shattered Keep'i koruyan muhur sistemi. Mob/boss = "muhur muhafizlari veya icerden sizan rift anomalileri".

**User sorusu:** Acik alanlar + dunyalarin birlesimi yariklarla anlatabilir miyiz?

---

## Options Analyzed

### Option A — Open Vista Within Closed Rooms (DOOR-PASS, NO OPEN WORLD)

Her oda discrete kalir (Karar #61 KEEP), ama icinde **3-katman parallax**:
1. **Sky/Vista layer (background, parallax-slow):** distant rift activity, ufuk silueti, baska dunyanin parcalari (per Floor tema)
2. **World-bleed layer (mid, parallax-medium):** rift cracks in walls/floor/sky reveal "other world bleeding through" — farkli palette/biome'dan parcalar
3. **Gameplay layer (foreground, parallax=0):** mevcut 32x32 tile + sprite

Boss arena = MASSIVE rift opening backdrop (huge visible rift showing actual world-merging).
Per Floor sky theme:
- F1 (Shattered Keep): ruined golden kingdom horizon, distant tower silhouettes
- F2 (Guard->Ossuary): corrupted forest canopy bleeding in, twisted root silhouettes
- F3 (Ritual->Containment): void sky reveals stars/rifts, "other side" visible

**Sonuc:** Player FEELS open world, plays Hades-style. Lore = world-merging SHOWN, not just told.

### Option B — Open Vista + Optional Rift Side-Pockets

Option A + per run **2-3 "rift side-pocket" mini-zones** sebze edilmis (Mystery/Curse Gate odalarda):
- Bir rift'e gir → kisa "open vista" pocket world (orneğin 20x40 open courtyard, sky open, distant world visible)
- Limited (max 2-3 per run), themed as "different world fragments" (F1 kingdom ruin, F2 corrupted grove, F3 void temple)
- Side rewards: lore fragment + cosmetic + rare upgrade (1 sec choice)
- Exit door = back to dungeon

**Sonuc:** Open world feel TASTE (mini pocket worlds) + macro structure korunur. ~3-5 dakika ek run time.

### Option C — Reject (Status Quo)

Sadece parallax decoration ekle, structural degisme yok. Visual update only.

---

## Recommendation

**Option B — Open Vista + Rift Side-Pockets**

Sebep:
1. Karar #61 (Hades-style) IHLAL ETMEZ — discrete room flow + macro graph KEEP.
2. Karar #62 (15 node Act 1) IHLAL ETMEZ — pocket'lar Mystery (6b) ve Curse Gate (9b) icinde, mevcut topolojiye gomulu.
3. Karar #63 (Map Fragment) IHLAL ETMEZ — pocket entry = mevcut Mystery/Curse Gate icinden.
4. Lore tam destek — "dunyalarin birlesimi yariklarla" gercekten gozlu gosterilir, sadece dialogda anlatilmaz.
5. Karar #77 (Vivid Vulnerability) ile uyumlu — kucuk chibi karakter vast hostile vista karsisinda.
6. Production cost: 2-3 yeni "pocket vista" template (Faz 2-3 scope) + parallax kit per floor (Faz 1 polish scope).
7. Player attractor: "Bir sonraki run'da rift portal'a girersem ne goreceğim?" — replay value +.

Risk:
- Visual inconsistency riski (open vista vs closed dungeon): per-floor palette/lighting LOCK ile cozulur.
- Production scope artisi: yapilabilir, post-pilot (16 sprite batch sonrasi sira).

---

## Yeni LOCKED Karar Onerisi

### Karar #81 — Open Vista Composition + Rift Side-Pockets
> Her oda 3-katman parallax: (1) sky/vista (per-floor tema), (2) world-bleed (rift overlay), (3) gameplay 32x32 tile. Karar #61 Hades-style room flow KEEP. Mystery (6b) ve Curse Gate (9b) odalarinin %50'sinde "rift side-pocket" entry: 20x40 open vista pocket world (2-3 per run max). Lore: world-merging visual demonstration. Production: parallax kit per floor (Faz 1 polish) + 3 pocket template (Faz 2-3).

---

## Riskler ve Mitigasyon

| Risk | Mitigasyon |
|------|------------|
| Visual inconsistency (open vs closed) | Per-floor palette+lighting LOCK ile uyum |
| Production overload (pocket templates) | Faz 2-3 scope, v1 sadece 1 pocket per floor pilot |
| Pacing kayma (3-5dk ekstra/run) | Pocket = 1 secim + 1 reward, hizli giris-cikis |
| Karar #61 indirect ihlal | Discrete room flow + door = pocket'a giris, open world DEGIL (sinirli pocket) |
| Karar #75 (Map Tools yasak) etki | YOK — pocket = elle authored template, Map Workshop kullanmaz |

---

## Sonraki Adimlar (Codex review sonrasi)

1. Karar #81 LOCKED olunca MASTER_KARAR_BELGESI'ne ekle.
2. TASARIM/PARALLAX_VISTA_KIT.md (yeni dosya, per-floor sky/vista spec).
3. TASARIM/RIFT_POCKET_TEMPLATES.md (yeni dosya, 3 pocket spec).
4. PixelLab brief: per-floor sky background (Faz 1 sonrasi).
5. dungeon_act1_map.md UPDATE — Mystery+Curse Gate'da pocket portal seed olasiligi.

