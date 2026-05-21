# RIMA Production Plan Detailed — v1.1

> **Date:** 2026-05-20 S95 LATE NIGHT
> **Parent:** `STAGING/PRODUCTION_PLAN_DETAILED_v1.md` (LIVE, referans tutulur)
> **Scope:** Targeted edit — sadece Faz 2 bütçe formülü + Total Budget Table revize.
> **Status:** DRAFT — Codex re-review bekliyor

## v1 → v1.1 Changelog

| Bölüm | v1 (yanlış) | v1.1 (doğru) | Kaynak |
|---|---|---|---|
| Faz 2 bütçe | `0 (V3 ayrı)` | Avg 544 Warblade + 200 mob = **744** | User S95 LATE NIGHT direktif |
| Faz 2 ölçek | 17-state full direct | **17-state + Pilot B (3-state V3 cost log)** gate | Opus Decision 2 + 6 |
| Faz 2.3-2.5 | Outlined | **HARD DEFER** post-Faz 4 | Opus Decision 4 |
| Total bütçe | 520 reserve / 620 worst | **1264 avg / 1550 worst** (%52-64) | Opus Decision 5 |
| Pilot strategy | Single (Batch 1.1) | **2 sequential gate** (Pilot A wall + Pilot B Warblade) | Opus Decision 6 |
| Tracking | Yok | `STAGING/RIMA_PixelLab_BalanceLog.md` per-batch | Opus Decision 7 |

**v1'den korunan bölümler (referansla):**
- §1 PixelLab Envanter (225 obj + 17 char + 4 iso tile + 25 topdown + 55 tiles_pro)
- §2 Faz 1 Demo MVP (7 batch, 280 reserve / 380 worst)
- §4 Faz 3 VFX (3 batch, 150 reserve)
- §5 Faz 4 UI Polish (2 batch, 90 reserve)
- §6 Codex Q1-Q6 cevapları (inline integrated)
- Reference / Evidence kanıtları (a52f6711 archway, abf9c178 cracked wall, f88e821a sconce, 11127e69 hitspark, 60502d16 dust)

## V3 Bütçe Formülü (Memory LOCK)

`reference_pixellab_v3_budget_formula` LOCK:
- **State base sprite:** 15-20 gen avg (smooth=18-22, basic=12-15)
- **Per state animasyon:** 10-15 gen (frame count dependent)
- **Per state total:** avg 30, worst 35

**Önemli:** V3 web UI **aynı PixelLab subscription**'dan çeker. v1'deki "0 gen" iddiası YANLIŞ.

---

## §3 REVIZE — Faz 2 Karakter & Mob (V3 web UI, USER manual, PixelLab budget IÇİ)

**Memory HARD LOCK gate:** `feedback_character_state_planning_before_production` — state listesi üretim ÖNCESİ user onay.

### Faz 2.1 — Warblade 17-State MVP (Codex Q2 LIVE + Opus Pilot B Gate)

**State listesi (v1'den korundu, 17-state MVP):**

| Kategori | State | Frame önerisi |
|---|---|---|
| Movement | idle_S, idle_SE, idle_E, idle_NE, idle_N (5 produce + 3 mirror) | 4-6 |
| Movement | walk_S, walk_SE, walk_E, walk_NE, walk_N (5+3 mirror) | 6-8 |
| Combat | attack_strike_S, attack_strike_E | 8-10 |
| Combat | attack_heavy_S, attack_heavy_E | 10-12 |
| Combat | hit_react | 3-4 |
| Combat | death | 6-10 |
| Signature | rage_burst | 10-15 |

**Total: 17 state**

**Cross-class 6 slot:** DEFER post code-side wiring proven (memory Shadowblade ComboPointSystem gap)

**Anchor character_id:** `2656075d-d113-4f18-a6c1-94b5a6b8bf65` (canonical roster v2)

#### Pilot B Gate — 3-State Real-Cost Log (Opus Decision 2 + 6)

**Sebep:** V3 budget formula **modelled, not measured**. 17 mevcut character full smooth-state pipeline ile üretilmemiş. **Pilot real-cost log zorunlu.**

**Pilot B kapsam:**
- `idle_S` (movement core)
- `walk_S` (movement core)
- `attack_strike_S` (combat core)
- **Tahmini cost:** 3 × 32 avg = **96 gen, worst 105**

**Gate verdict:**
| Real avg cost/state | Aksiyon |
|---|---|
| ≤ 35 | Fire full 14 remaining state |
| 35-45 | Re-plan: 14-state cut (defer attack_heavy_E + 2x mirror cleanup) |
| > 45 | BLOCKED, user'a escalate |

**Pilot B paralel** Faz 1'e (farklı worker — USER V3 manual vs orchestrator MCP).

#### Bütçe Reserve

- **Pilot B (3 state):** ~96 gen avg, 105 worst
- **Full 17-state (eğer pilot pass):** **544 gen avg, 650 worst**
- **Subtotal Warblade:** 544 avg / 650 worst

#### USER Action (V3 manual workflow)

1. Anchor `2656075d` V3 web UI'da load
2. **Pilot B önce:** 3 state üret (idle_S + walk_S + attack_strike_S), real gen cost log
3. **Pilot B PASS gate sonrası:** kalan 14 state batch dispatch V3'te manuel
4. Output: 17 sprite × 5 direction (8-dir mirror sonrası) = 85 sprite
5. Unity import: `Assets/Art/Characters/Warblade/V3_*` klasörü
6. **Her batch sonrası `STAGING/RIMA_PixelLab_BalanceLog.md` update**

### Faz 2.2 — 1 Mob MVP (Opus Decision 3)

**Önkoşul:** Pilot B PASS (V3 cost model validated).

**Mob seçimi:** `enemy_00` veya `enemy_01` (mevcut prefab, V3 character_id var)

**State sayısı:** 6-8 minimum demo viable

| Kategori | State | Frame |
|---|---|---|
| Movement | idle, walk | 4-6, 6-8 |
| Combat | attack | 8-10 |
| Combat | hit, death | 3-4, 6-10 |
| Mob-specific (varsa) | 1-2 (örn telegraph, summon) | 4-8 |

**Bütçe Reserve:** **200 gen avg, 280 worst** (6-8 state × 30 avg)

**Faz 2.2 Explicit Gate (Codex Q3 LIVE):**

| Pilot B avg gen/state | Faz 2.2 Aksiyon |
|---|---|
| **< 30** | Full 6-8 state mob OK (200 gen) |
| **30-35** | Full 6-8 state mob, ama bütçe disiplini sıkı |
| **≥ 35** | Cut 4-state minimum (idle/walk/attack/die ≈ 120 gen) |
| **≥ 45** | DEFER mob (statik enemy_00 prefab + Faz 3 VFX yeter) — Act 2 bütçe disiplini |

**Demo MVP yeterli mi?** Tek mob = thin combat ama vertical slice için yeter. Pilot B sonuçlarına bağlı 200/120/0 senaryosu.

### Faz 2.3-2.5 — DEFER (Opus Decision 4)

**HARD DEFER:** Ronin, Gunslinger, Elementalist post-Faz-4 + Pilot B real-cost validation.

**Sebep:**
- Demo MVP için 1 playable class yeter (Warblade primary)
- 4 char × 544 avg = 2176 gen — neredeyse tüm kalan bütçe
- Ronin code LIVE (memory `project_ronin_live_s93_night`), sprites bekleyebilir
- Canonical roster "4-char playtest focus" — Warblade/Elementalist/Ranger/Shadowblade; Gunslinger zaten focus'ta değil

**Trigger gate (yeniden açma için):**
- Pilot B real cost < 30 avg gen/state → 1 ek char Faz 2.3 promote edilebilir (Elementalist tercih)
- Pilot B real cost ≥ 35 → defer hard

---

## §6 REVIZE — Toplam Bütçe Tablosu

**Bütçe baseline:**
- Plan baseline (yazıldığı anda): 2,433 / 5,000 gen
- **Live check (Codex re-review):** 2,567 / 5,000 gen (refresh sonrası, +134 gen)
- Hesaplar conservative baseline 2,433 üzerinden — refresh bonus drift redo için ek marj

| Faz | Reserve | Worst | Cumulative Avg | Cumulative Worst |
|---|---|---|---|---|
| Faz 1 Demo MVP | 280 | 380 | 280 | 380 |
| **Faz 2.1 Warblade (pilot B + full)** | **544** | **650** | **824** | **1030** |
| **Faz 2.2 1 Mob** | **200** | **280** | **1024** | **1310** |
| Faz 3 VFX | 150 | 150 | 1174 | 1460 |
| Faz 4 UI Polish | 90 | 90 | 1264 | 1550 |
| **Total** | **~1264** | **~1550** | — | — |

**Bütçe kullanım:**
- **Avg:** 1264 / 2433 = **52% kullanım, 48% marj (1169 gen reserve)**
- **Worst:** 1550 / 2433 = **64% kullanım, 36% marj (883 gen reserve)**

**Margin verdict (Codex Q4 LIVE):**
- ⚠️ **TIGHT for Act 2/3** — 1264/2433 = 1169 gen kalan, modeled Act 2+3 ihtiyaç ~1400 → **~230 gen SHORT**
- Live balance (2567): 1303 kalan, halen ~100 short
- ✗ NOT SUFFICIENT for Faz 2.3-2.5 (4 char × 544 = 2176 gen) → defer call locked
- ✓ SUFFICIENT for Act 1 + Warblade scope only
- **Discipline gate:** Faz 2.2 mob conditional (aşağıdaki Pilot B-ye-bağlı gate)

---

## §7 REVIZE — Pilot Strategy (Opus Decision 6)

**2 sequential gate + 1 parallel (USER worker):**

### Pilot A — Batch 1.1 Wall Face Pack (MCP)
- **Tool:** `mcp__pixellab__create_object` size=128 view=side directions=1 n_frames=4 + 4 item_descriptions
- **Cost:** 25-40 gen (1.6% bütçe)
- **Test:**
  1. `view="side"` parameter forward
  2. `item_descriptions` field MCP wrapper forward (master spec Iter 2 caveat)
  3. n_frames=4 style coherence
  4. Granite + cyan HEX palette
- **GATE:** PASS → fire Batch 1.2-1.6. FAIL → Plan B (REST API) or Plan C (4× n_frames=1).

### Pilot B — Warblade 3-State V3 (USER manual)
- **Owner:** USER (V3 web UI manual)
- **Scope:** idle_S + walk_S + attack_strike_S
- **Cost log:** real gen per state recorded in `RIMA_PixelLab_BalanceLog.md`
- **GATE:**
  - ≤ 35 avg → fire full 14 remaining state
  - 35-45 → re-plan (cut to 14-state set)
  - \> 45 → BLOCKED, user escalate

**Pilot B paralel Faz 1'e** (different worker streams).

---

## §8 NEW — RIMA PixelLab Balance Log Tracker (Opus Decision 7)

**Dosya:** `STAGING/RIMA_PixelLab_BalanceLog.md` (orchestrator create, per-batch append)

**Schema:**
```
| Batch | Date | Tool | Reserve | Real | Δ | Notes |
|---|---|---|---|---|---|---|
| Pilot A 1.1 Wall Face | YYYY-MM-DD | create_object 128 n=4 | 40 | XX | ±X | view=side validated |
| Pilot B 2.1 Warblade idle_S | ... | V3 manual | ~32 | XX | ±X | smooth frames 5 |
```

**Owner:** Orchestrator writes after each batch (Codex/USER dispatch).

**Reads:**
- `mcp__pixellab__get_balance` before/after for empirical check
- Retroactive Faz 2 formula validation

---

## Açık Sorular (User Antigravity Review İçin)

1. **State base avg gen 15-20 (modeled):** Geçmiş V3 character üretiminden empirik veri var mı? Memory'de kayıt yok, list_characters'tan görüldü kadarıyla full smooth-state pipeline ile üretilmiş char yok.
2. **Smooth-state frame breakdown:** walk genelde kaç frame? Attack kaç frame? Formula refinement için kritik. Üst tablo öneri (4-15 range), Antigravity Gemini bunu refine edebilir.
3. **Faz 2.2 mob MVP zorunlu mu:** Demo için enemy_00 statik prefab + Faz 3 hitspark VFX (`11127e69`) yeterli "alive" görüntü verir mi? Yoksa animated mob zorunlu mu?
4. **Pilot B fail aksiyon:** Avg 40+ gen/state çıkarsa cut nereden — state sayısı (17→14) mi, smoothness (avg 32→25) mi, Faz 2.2 mob defer mi?
5. **V3 dispatch akışı:** Orchestrator hangi noktada prompt formülü pasını verecek — her state ayrı mı (17 ping), tek batch prompt set (1 ping), 3 chunk (movement / combat / signature)?

---

## Codex Re-Review — PASS_WITH_REVISIONS ✓

**Done:** `STAGING/CODEX_DONE_production_plan_v1_1_review.md` (5 Q verdict)

| Q | Verdict | Notes |
|---|---|---|
| Q1 Formula empirical | INCONCLUSIVE | 17 mevcut char animations=none → smooth-state cost validate edilemez. Pilot B mandatory. |
| Q2 Warblade 544 avg | PASS_WITH_NOTES | Frame counts realistic, ama empirik kanıt yok → 650 worst cap aktif tut |
| Q3 Mob 200 | Accept 200 | 4-state minimum (120 gen) budget-pressure option |
| Q4 Budget margin | **TIGHT** | 1169 kalan vs ~1400 Act 2/3 ihtiyaç = 230 SHORT. Discipline kritik |
| Q5 Pilot B gate | PASS | 3-state scope doğru, threshold realistic |

**4 revize uygulandı (inline edit):**
1. Balance line: 2,433 plan baseline + 2,567 live check
2. Margin verdict: SUFFICIENT → TIGHT
3. Faz 2.2 explicit gate (Pilot B-ye bağlı 200/120/0 senaryo)
4. Q1 empirik unavailable note (tracker file authoritative)

**Codex açık soruları (user'a yöneltildi):**
- Balance discrepancy (2433 vs 2567 live) beklenen mi? → Refresh bonus olarak işlendi
- Faz 2.2 mob demo MVP gerekli mi yoksa statik enemy + VFX yeter mi? → Açık sorular listesinde

## Status

**LIVE v1.1** (Codex PASS_WITH_REVISIONS uygulandı) → Antigravity (Gemini 3.5 Flash) review için hazır → pilot dispatch onay.
