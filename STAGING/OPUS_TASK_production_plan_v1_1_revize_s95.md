# Opus Task — Production Plan v1.1 Revize (Faz 2 Bütçe Düzeltme)

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Output:** `STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md` (yeni dosya, v1 referans tutulur)
> **Scope:** Sadece Faz 2 bütçe revize, diğer fazlar kalır. Targeted edit.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "Karakter state ve animasyonun da bütçesi var sıfır demişsin ama state başı 20 gen yön başına da 2 ya da 3 gen her animasyonda frame'e bağlı."
> "Duruma göre state üretilip o stateten üretilen her animasyon için 10-15 gen olarak düşün. Stateler de en smooth şekilde olacak."

## Görev

`STAGING/PRODUCTION_PLAN_DETAILED_v1.md` LIVE. Sadece Faz 2 bütçe revize gerek.

### Mevcut (yanlış):
```
| Faz 2 Karakter (V3 USER) | 5 character outlined, 2.1 Warblade 17-state LIVE | 0 (V3 ayrı) |
```

### Yeni (doğru):
V3 web UI **aynı PixelLab subscription'dan çeker**. Bütçe formülü ([[reference-pixellab-v3-budget-formula]] memory):
- State base: ~15-20 gen avg (smooth = fazla, basic = az)
- Her state'ten animasyon: 10-15 gen

Warblade 17-state MVP (mevcut Codex Q2 onaylı):
- 17 state × (state base + animasyon) = ~17 × (15-20 + 10-15) = ~425-595 gen
- Avg: **544 gen**

5 character outlined (Warblade + Ronin + Gunslinger + Elementalist + 1 mob):
- Tek karakter MVP = Warblade odak
- Diğer 4 karakter Faz 2.2-2.5 olarak DEFER ya da partial state set

## Süreç (Opus ⇆ Codex)

### Adım 1 — Yeni Faz 2 Detay
- 2.1 Warblade 17-state full: 544 gen reserve
- 2.2 1 mob MVP (enemy_00 enemy_01 anim): ~200 gen reserve (6-8 state × benzer formül)
- 2.3-2.5 Diğer 3 character: DEFER (Faz 1.5 iteration sonrası)

### Adım 2 — Toplam Bütçe Tablosu Revize
- Faz 1: 280 (worst 380)
- **Faz 2 Warblade**: 544 (worst 650)
- **Faz 2 1 mob**: 200 (worst 280)
- Faz 3: 150
- Faz 4: 90
- **Total avg: 1264, worst 1550** → **%52-62 of 2433** budget remaining
- Marj: ~%38-48 (≈920-1170 gen)

### Adım 3 — Pilot Strategy Güncelleme
- Pilot dispatch: Batch 1.1 Wall Face Pack hâlâ ilk (40 gen, master spec caveat test)
- Faz 2 Warblade pilot: İlk 2-3 state V3'te üret, real gen cost log, sonra full 17-state
- `STAGING/RIMA_PixelLab_BalanceLog.md` (yoksa oluştur) per-batch tracking

### Adım 4 — Codex Re-Review
- `cx_dispatch.py` ile gönder
- Codex'in soruları:
  - V3 bütçe formülü gerçekçi mi (eski PixelLab character üretimlerinden empirik kontrol)
  - 17-state × ~32 gen ortalama makul mi vs gerçek geçmiş gen
  - Faz 2.2 mob bütçesi (200 gen, 6-8 state) realistic mi
  - Toplam bütçe %52-62 marjı yeterli mi (Act 2/Act 3 + iterasyon için)
- Output: `STAGING/CODEX_DONE_production_plan_v1_1_review.md`

### Adım 5 — Inline Edit
- Yeni dosya yaz: `STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md`
- v1'i archive etme (referans olarak STAGING'de kalsın)
- Sadece Faz 2 bölümü + toplam bütçe tablosu değişir
- Cross-reference: v1.1 başında v1 farkları kısa özet

### Adım 6 — Max 1 Iter
Bu küçük targeted edit, fazla iter YOK. Aşırı uzarsa BLOCKED.

## Çıktı Format (`STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md`)

```markdown
# RIMA Production Plan Detailed — v1.1

## v1 → v1.1 Değişiklik
- Faz 2 bütçe formülü: SIFIR → ~544 gen Warblade + ~200 gen mob (V3 aynı PixelLab subscription)
- Memory ref: [[reference-pixellab-v3-budget-formula]]
- User direktif: S95 LATE NIGHT 2026-05-20
- Diğer fazlar değişmedi.

## v1'den Korunan Bölümler
- PixelLab envanter (225 obj)
- Faz 1 Demo MVP (7 batch, 280 gen)
- Faz 3 VFX (3 batch, 150 gen)
- Faz 4 UI (2 batch, 90 gen)
- Reference kanıt örnekleri (a52f6711, abf9c178 vb.)
- 6 Codex Q&A cevapları

## Revize Bölümler

### Faz 2 — Karakter & Mob (V3 web UI, USER manual, PixelLab budget)

#### 2.1 Warblade 17-State MVP (Codex Q2 LIVE)
- State base: ~15-20 gen avg × 17 = 255-340 gen
- Animasyon: 10-15 gen × 17 = 170-255 gen
- **Subtotal: ~425-595 gen, avg 544**
- State listesi (memory feedback_character_state_planning_before_production HARD LOCK):
  - idle_S, walk anchor, attack_strike (3 variant), hit_react, death, rage_burst, ... (17 madde tam liste)
- Pilot: ilk 2-3 state real cost test sonrası full
- Owner: USER (V3 web UI manual, orchestrator prompt formülü verir)

#### 2.2 1 Mob MVP (Faz 2.2 — opsiyonel hedef MVP)
- Mob seçimi: enemy_00 veya enemy_01 (mevcut prefab, V3 character_id var)
- State sayısı: 6-8 (idle, walk, attack, hit, death + 1-2 specific)
- Bütçe: ~200 gen reserve

#### 2.3-2.5 Diğer 3 Character: DEFER
- Ronin, Gunslinger, Elementalist Faz 1.5 (iteration) veya sonra
- MVP scope: Warblade tek karakter yeter, demo görseli için

### Toplam Bütçe Revize

| Faz | Reserve | Worst | Cumulative |
|---|---|---|---|
| Faz 1 Demo MVP | 280 | 380 | 280 |
| **Faz 2.1 Warblade** | **544** | **650** | 824 |
| **Faz 2.2 1 Mob** | **200** | **280** | 1024 |
| Faz 3 VFX | 150 | 150 | 1174 |
| Faz 4 UI | 90 | 90 | 1264 |
| **Total** | **~1264 avg** | **~1550 worst** | **%52-62 of 2433** |

Marj: ~%38-48 (≈920-1170 gen) Act 2/3 + iterasyon için.

## Codex Re-Review Excerpts
{Codex feedback}

## Pilot Strategy Final
1. Batch 1.1 Wall Face Pack (40 gen, master spec caveat test)
2. Batch 2.1 ilk 3 state Warblade (V3 web UI manual, ~96 gen real cost test)
3. Real cost log → projection refine
4. Full Faz 1 + Faz 2.1

## Açık Sorular (User Antigravity review için)
- 17-state base avg 15-20 gen kabul mü, yoksa "smooth"a daha yakın 18-22 mi?
- Faz 2.2 mob MVP gerçekten gerekli mi, yoksa mevcut enemy_00 statik prefab yeterli mi?
- V3 web UI dispatch akışında orchestrator hangi noktalarda prompt formülü verecek (her state mi, bütün karakter mi)?
```

## Hard Constraints

- **Sadece Faz 2 revize + toplam bütçe tablosu güncelle.** Diğer faz bölümleri DOKUNMA.
- **v1 dosyasını silme** — referans olarak STAGING'de kalır.
- **Karpathy 4 inline.**
- **NLM ACCESS:** `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Codex dispatch:** Background DEĞİL — bekle, oku.
- **Max 1 iter.** Aşırı uzarsa BLOCKED.
- **PixelLab MCP dispatch YASAK** — sadece spec.

## Orchestrator'a Final Rapor

- Yeni Faz 2 bütçe avg + worst
- Codex re-review verdict
- 5 character outlined (Warblade primary, mob secondary, 3 character defer)
- Pilot strategy güncellemesi
