# CODEX TASK — Progression Plan v1 → v2 (NLM Canonical Reconcile)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS (auth refreshed):
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
```

---

## Hedef

Codex Progression Plan v1 üretti ama NLM data'sız çalıştı. NLM canonical (Karar #60/61/62/63 + Death Imprint) ile çelişen 3 nokta var. v2 = NLM canonical ile reconciled, locked plan.

## ZORUNLU OKUMA

1. `STAGING/PROGRESSION_PLAN_v1_CODEX.md` — Codex'in v1'i
2. `MEMORY/_temp_canonical_lock.md` (eğer varsa) — local canonical reference
3. **Aşağıdaki NLM canonical bloğu** (inline)

## NLM Canonical (Authoritative — v2 BUNA göre yazılacak)

### Karar #61 (LOCK 2026-05-09): Dungeon Architecture
- Hades-style discrete room flow + StS macro graph hybrid
- Açık dünya / Diablo REJECTED

### Karar #62 (LOCK 2026-05-09): Act 1 Map 15 Node
**Karar #29 (8-9 oda) OVERRIDE — stale referans drift.**

| Tip | Sayı | Threat budget / Notu |
|---|---|---|
| Entry | 1 | Tutorial, no threat |
| Combat | 6 | 8-12 budget |
| Elite | 2 | 14-18 budget, 1+ Elite mob |
| Rest | 2 | F1→F2 / F2→F3 transit |
| Shop | 1 | Safe NPC |
| Curse Gate | 1 (yan dal) | Risk/reward branch |
| Mystery | 1 (yan dal) | Event-based |
| Boss | 1 | Act finale |

13 main + 2 branch. Fixed topology, random content.

### Karar #63 (LOCK 2026-05-09): Map Fragment + Kırık Taş Tablet

**Fragment Drop Rules:**
- Combat: ✓ garanti
- Elite: ✓ garanti
- Mystery: ✓ olasılıklı (open node = chance)
- Rest/Shop/Curse Gate: ❌ YOK
- Boss gate = 8 fragment (6 Combat + 2 Elite zorunlu)
- Yan dal fragmenti bonus, kotaya girmez
- Pickup: G key MANDATORY, cyan glow + bobbing
- Reveal: 1 node %65 / 2 node %30 / 3 node %5
- Açık node bonus: +1 hop

**Map UI: "Kırık Taş Tablet"**
- Soyut grid + paslı altın çerçeve + cyan rift çatlakları
- TAB MapPanel (StS full screen)
- Sol-üst MiniMap (Hades 128×128)
- 4 Act görsel evrim: Act1 kale oymaları / Act2 damarlı et / Act3 yüzen parçalar / Act4 ayna

### Karar #60: Skill Draft (NO RUNE)

**Skill Draft:** Room clear → fragment pickup → 3-choice draft (new skill / tier upgrade / Echo Imprint)

Tiers: Common → Rare → Epic → Legendary. Max 3 upgrade slot/skill. Resource-free. REPLACE on full.

Echo Imprint: separate parallel (4/run max, 3 standard), covers Strike/Outlet/Surge Forms.

### Death Imprint (PROPOSAL, Karar #122 collision rename)
- Eski "Echo Imprint Cascade" → Death Imprint
- Top candidate, NOT locked, spec gate pending
- Records: encounterId + subRoomIndex + subRoomTag + mob comp + env context
- Cadence: per macro encounter

### Canonical Reward Catalog (LOCK)
- Map Fragment, Gold (5-75), Shards (Faz 2+, 1-5/enemy)
- 7 Components: Iron Shard / Void Fragment / Chain Links / Shadow Veil / Blood Gem / Rift Stone / Soul Ember
- 9 Combined Items (recipes)
- Relics (Boss + rare Event ONLY)
- Skill Draft 3-choice/room
- Meta-currency: **Shattered Echoes** (NOT "Echo Essence")

**YASAK:**
- ❌ Health Orb drop (HP from Elite %12 / Shop / Boss %50)
- ❌ Boss Key item (8-fragment rule replaces)
- ❌ Rune drop (no rune)
- ❌ "Echo Essence" name → use Shattered Echoes

## v1 Çelişkileri (v2'de Düzelt)

| v1 Diyor | NLM Diyor | v2 Action |
|---|---|---|
| Rune 3 slot MVP | NO RUNE SYSTEM | DELETE rune section, replace with Skill Draft canonical |
| Boss Key ayrı ekonomi | NO BOSS KEY ITEM | DELETE Boss Key, use 8-fragment rule |
| Echo Essence drop | Shattered Echoes meta-currency | Rename + reclassify drop vs meta |
| 9 room types | 8 room types (no corridor) | DELETE Corridor row |
| Skill Rune drop | NO SKILL RUNE | DELETE Skill Rune from reward gallery cross-ref |

## Görev

### Adım 1: v1 Oku + NLM Çelişkileri Bul
v1'i NLM canonical bloğuna karşı satır satır karşılaştır. Tüm çelişkileri listele.

### Adım 2: v2 Yaz (`STAGING/PROGRESSION_PLAN_v2_FINAL.md`)

v2 yapısı:
- **Header**: v2 yazıldı, NLM-canonical-reconciled, LOCK candidate
- **Section 1**: Mevcut görsel envanter (compact sheets) + her görselin canonical role'ü
- **Section 2**: 15-node Act 1 mapping (her node tipi için threshold visual + drop)
- **Section 3**: Map fragment progression (Karar #63 exact, reveal probability, pickup flow)
- **Section 4**: Reward catalog (sadece canonical 7 component + 9 combined + relics + Skill Draft)
- **Section 5**: Skill Draft Echo Imprint integration (Karar #60)
- **Section 6**: Death Imprint (proposal status, spec gate)
- **Section 7**: Compact sheet repurpose map (Sheet 3 reward gallery hangi visual hangi canonical reward'a remap, hangileri discard)
- **Section 8**: Production cost (gen budget, Codex implementation, shader work)
- **Section 9**: 5 next-step dispatch (orchestrator için)

### Adım 3: Sheet 3 Reward Visual Repurpose

Mevcut `STAGING/concepts/compact_sheets/03_reward_drops_gallery.png`'deki 8 visual nasıl repurpose edilecek? Detaylı table:

| Sheet 3 Visual | Canonical Reward Mapping | Action |
|---|---|---|
| Echo Orb | Shattered Echoes meta-currency | RENAME + reuse |
| Memory Shard | Component: Rift Stone (cyan crystal benziyor) | REASSIGN |
| Gold Pile | Gold (canonical) | KEEP |
| Skill Rune | (no rune system) | DISCARD |
| Health Orb | (no drop) | DISCARD |
| Map Fragment | Map Fragment (canonical) | KEEP |
| Curse Stone | Curse Gate burden/gift UI (NOT drop) | REPURPOSE → UI element |
| Boss Key | (no item) | DISCARD |

Eksik (gen lazım):
- 7 Components icon set (6 missing: Iron Shard / Void Fragment / Chain Links / Shadow Veil / Blood Gem / Soul Ember — Rift Stone Memory Shard'tan repurpose)
- 9 Combined Item icons
- Relic icon
- Skill Draft 3-choice UI screen

### Adım 4: CODEX_DONE_progression_plan_v2_nlm.md Yaz

- v2 dosya path
- Section-by-section değişiklik özeti
- Çözülen v1 çelişkileri sayısı
- Kalan açık soru var mı (varsa user/orchestrator)
- Next-step dispatch öneri (5 madde, dependency dahil)

## Kısıtlar

- NLM canonical hardcoded yukarıdaki bloktan al — RE-QUERY ZORUNLU değil, ama BLOCKED hatası yerine bu blok yeterli
- BLOCKED if: v1 okunamaz, write izni yok
- Kod yazma — sadece plan

## Stil

Terse, decision-first. "X canonical because Karar #N" — kararsızlık yok.

## Dispatch

```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_progression_plan_v2_nlm.md --effort high
```

Background. Notify on complete.
