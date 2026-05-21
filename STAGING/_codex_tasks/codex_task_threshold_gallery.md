# CODEX TASK — Threshold Concept Gallery + Reward/Map Mark Imagegen

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## Hedef

8 farklı threshold konseptinin **görsel render**'larını + her biri için **in-game mockup** üret. Ayrıca **Hades-style room clear reward & map mark** mekaniği için 3-4 görsel konsept render et.

User henüz konsept seçmedi — bu render gallery user'ın karar gate'i için referans olacak.

## ÖNEMLİ — Imagegen Route

Önceki `b4n5obvio` task'ında imagegen ÇALIŞMIŞTI (4 PNG output): Codex'in **built-in imagegen tool** kullan, shell OpenAI API DEĞİL. `bp27ve7c2` task'ında shell OpenAI route denenmişti, fail etmişti. Doğru yol: Codex agentic imagegen.

Source PNG'ler `C:\Users\ydbil\.codex-profiles\<profile>\generated_images\` altında oluşur, sonra `STAGING/concepts/threshold_gallery/` veya `STAGING/concepts/reward_mark/` altına kopyala + post-process (chroma key remove via `remove_chroma_key.py` veya benzeri).

## Bağlam — RIMA

- 35° izometrik ARPG, 2D pixel art (Hades + Diablo karışımı)
- Lore: "Echo Imprint Cascade" — die, room remembers, each death writes the arena
- Visual signature: cyan rift (floor cracks, wall accents, energy)
- Karakter: 64px chibi pixel art, dark armor, 8-dir movement
- Renderer: URP 2D, PPU=64, 128×128 standard wall/prop sprite
- Mevcut Codex output kritiği: stone arch + cyan portal = Hades clone, KULLANILMAYACAK

## GÖREV A — 8 Threshold Konsept Gallery

Her konsept için **2 render**:
1. **Showcase**: 256×256 veya 512×512, transparent bg, threshold "active" state, isolated
2. **In-game mockup**: 512×512 veya 768×512, scene context — chibi Warblade character (siyah zırh, sarı saç, ~64px), cyan rift floor tile zemin, boş cleared arena, character threshold'a YAKLAŞIYOR (yan/arkadan görünür), oda atmosferi (torch glow, debris, lit shadow). 35° iso projection.

### Konsept Listesi

#### Codex tarafı (önceki brainstorm'dan)

**C1 — Scar Compass Ring** (Codex final tavsiye, skor 19/20)
- Form: Yerde gömülü kırık pusula halkası, sadece çıkış yönünde 1/3 arc + cyan rift çatlaklar + döner "scar needle" ışık iğnesi
- Active state: çıkış arc parlıyor, needle döner
- In-game: Warblade temizlenmiş arenadan kapıya yaklaşıyor, compass ring yerde parıldıyor

**C2 — Echo Fault Loom** (Codex 2nd, skor 18/20)
- Form: Yatay tezgâh, 2 köşe taş anchor + 4-6 cyan memory thread + ortada yırtık. Locked = gevşek, Active = gerilmiş diamond mouth, Portal = woven cyan surface
- In-game: Warblade loom önünde, threadler gerilmiş cyan parlıyor

**C3 — Rift Ledger Slabs** (Codex 3rd, skor 18/20)
- Form: Floor'dan kalkmış shallow V-shape "ledger sayfaları" — carved tick mark + room symbol + cyan ink crack
- Active: 3 plaka kalkmış, cyan light cast
- In-game: Warblade plakalara yaklaşıyor, sayfa metaforu

**C4 — Mnemonic Rib Gate** (Codex 4th, skor 12/20)
- Form: Floor crack'lerinden çıkan kaburga parçaları, tepede birleşmeyen jagged silhouette
- Active: 3-4 rib kalkmış, cyan echo arcs uçları bağlıyor
- In-game: Warblade rib fan içinden bakıyor

#### Antigravity tarafı (paralel brainstorm'dan)

**A1 — Echo Anchor Monolith** (Antigravity, skor 92/100)
- Form: Yere bağlı değil — havada SÜZÜLEN obsidyen/granit parçalar dikey cyan enerji sütunu etrafında. Direction-invariant (her yönden aynı).
- Active: parçalar genişler, enerji sütunu parlar
- In-game: Warblade monolite yaklaşıyor, süzülen parçalar etrafında

**A2 — Imprint Scar / Floor Rift** (Antigravity FINAL TAVSİYE, skor 96/100)
- Form: Zemin karoları yırtılıyor, izometrik fay hattı, altında derin cyan void + yukarıya süzülen parçacıklar
- Active: karolar iki yana kaydı, bottomless cyan boşluk
- In-game: Warblade kenarda durmuş, rift'in içine bakıyor (fall-in moment)

**A3 — Resonance Mirror** (Antigravity 3rd, skor 88/100)
- Form: Havada asılı çerçevesiz oval sıvı cam ayna, billboard (kameraya bakar). Kilitliyken statik gürültü, active = berrak cyan yansıma
- Active: sıvı cam dalgalanır, içinde next room silüeti pikselli
- In-game: Warblade aynanın önünde, cam içinde next room glimpsing

**A4 — Chrono-Crypta Wall Seam** (Antigravity 4th, skor 80/100)
- Form: Duvar bloklarının havada süzülmüş çöküntüsü — temporal patlama, frozen explosion. Cyan light blokların arasından
- Active: blokları daha geniş açılır, yırtık genişler
- In-game: Warblade duvardaki süzülen bloklar arasından geçiyor

### Output Structure

```
STAGING/concepts/threshold_gallery/
  C1_scar_compass_ring/
    showcase.png
    ingame.png
  C2_echo_fault_loom/
    showcase.png
    ingame.png
  C3_rift_ledger_slabs/
    showcase.png
    ingame.png
  C4_mnemonic_rib_gate/
    showcase.png
    ingame.png
  A1_echo_anchor_monolith/
    showcase.png
    ingame.png
  A2_imprint_scar_floor_rift/
    showcase.png
    ingame.png
  A3_resonance_mirror/
    showcase.png
    ingame.png
  A4_chrono_crypta_wall_seam/
    showcase.png
    ingame.png
```

Toplam: **16 PNG** (8 konsept × 2 render).

## GÖREV B — Reward + Map Mark (Hades-Style)

Hades'te oda clear olunca:
- Player ortada dönen ödül itemi alır (orb, coin, key)
- Map progress mark görünür (next room icon revealed)

RIMA için Echo Imprint Cascade temalı versiyon:
- **Reward drop**: zemine "echo essence", "memory shard", "rift gem" gibi RIMA-thematic drop
- **Map mark**: oyuncunun haritada ilerlediğini gösteren marker (fragments arranging, threads weaving, scar healing)

### 3-4 Konsept Render Et

**R1 — Echo Essence Orb (floor drop)**
- Hades coin-like ödül, ama RIMA cyan rift essence olarak. Floor'da dönen küre, cyan particle, slight glow
- 4 state: idle pulse / hover-pickup / collected burst / faded
- Output: 4-frame contact sheet veya 4 ayrı PNG

**R2 — Memory Shard Pickup**
- Kırık kristal şekli, cyan içi, dönen pickup. Lore: "the room's memory shed into a usable shard"
- Showcase + in-game (Warblade shard'a doğru yürüyor, halo)

**R3 — Map Mark — Echo Thread Weave**
- Hades pop-up "next room revealed" UI yerine, RIMA'da map sahne içinde dokuma örneği — cyan thread'ler boşluktan beliriyor, next room siluetini örüyor
- 2-3 frame mid-animation

**R4 — Map Mark — Scar Path Brand**
- Alternatif: zemine cyan scar yanıyor, path forward işaretliyor. Map değil immediate floor mark
- Showcase + in-game

### Output Structure

```
STAGING/concepts/reward_mark/
  R1_echo_essence_orb/
    idle.png
    pickup.png
    collected.png
  R2_memory_shard/
    showcase.png
    ingame.png
  R3_map_mark_thread_weave/
    frame_1.png
    frame_2.png
    frame_3.png
  R4_scar_path_brand/
    showcase.png
    ingame.png
```

Toplam: **~12 PNG**.

## Stil Tutarlılığı (Hepsi İçin)

- 35° iso pixel art, painterly
- Renk paleti: dark granite gray + cyan rift accent + occasional orange torch glow
- Transparent corners (chroma key + remove)
- Size 256×256 or 512×512 showcase, 768×512 mockup
- Karakter: Warblade chibi 64px referansı (`Assets/Art/Characters/Warblade/Rotations/warblade_south.png` veya benzeri açıdan)
- Floor reference: `STAGING/concepts/` mevcut cyan rift floor look
- Wall reference: stone block + cyan crack (gerekirse `STAGING/concepts/rift_threshold_active_act1.png` neutral stil olarak)

## Acceptance Criteria

- ✅ 16 threshold gallery PNG + 12 reward/mark PNG = ~28 PNG
- ✅ Tümünün transparent bg veya temiz background
- ✅ Showcase = isolated subject, ingame = scene context
- ✅ Warblade karakter recognizable (chibi pixel art style)
- ✅ Her konsept lore'a sadık (Echo Imprint Cascade)
- ✅ Hiçbir konsept "Hades stone arch + cyan portal" formülüne benzemez

## BLOCKED if

- imagegen tool erişimi yok (use built-in tool, NOT shell API)
- Output write izni yok
- NLM auth expired (offline mode'da çalış, RIMA bağlamı bu task'ın içinde verildi zaten)

## Final Report

`STAGING/CODEX_DONE_threshold_gallery.md`:
- 28 PNG listesi + paths
- Her konsept için 1-2 cümle visual verdict (clean / muddy / readable / drift)
- Hangi konsept en sterk render olduysa öne çıkar
- Reward/mark için en güçlü mekaniği öner
- Karşılaştırma matrisi: Showcase quality, In-game readability, Lore fit, Production-ready ölçeklik

## Dispatch

Background, effort=high:
```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_threshold_gallery.md --effort high
```

Notify when complete.
