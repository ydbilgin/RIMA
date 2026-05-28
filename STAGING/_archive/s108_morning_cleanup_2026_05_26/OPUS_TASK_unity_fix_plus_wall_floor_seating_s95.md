# Opus Task — Unity Console Fix + Duvar Zemine Oturma (S95)

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Mode:** İki faz — A: kod fix (uygulanır, geri alınabilir), B: tasarım spec (uygulanmaz)
> **Constraint:** Aşırı uzarsa BLOCKED yaz, dur.

## İki Faz

### Faz A — Unity Console Hata Fix (önce bu)

**Adım 1:** Codex'e UnityMCP üzerinden Console oku task'i ver:
```bash
python '/f/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py' \
  --task-file STAGING/CODEX_TASK_unity_console_diagnose_s95.md --effort high
```

Codex task'i içeriği:
- UnityMCP `read_console` (veya muadili) ile **error + warning** liste
- Her error için: mesaj, dosya:satır, stack trace ilk 3 satır
- Output: `STAGING/CODEX_DONE_unity_console_diagnose_s95.md`
- Sınıflandırma:
  - **Compile error** (script regression) → fix dispatch
  - **Runtime null/missing** (scene reference) → fix dispatch
  - **Warning** (deprecation, leak) → düşük öncelik, listele

**Adım 2:** Console temizse → "no errors, skipping Faz A" yaz, Faz B'ye geç.

**Adım 3:** Console'da error varsa → Opus diagnose et:
- Her error için root cause + fix önerisi (1-3 satır)
- Codex'e fix task'i: `STAGING/CODEX_TASK_unity_fix_s95.md`
- Codex direkt uygulasın (kod değişikliği yapsın), `dotnet build` ve `EditMode test` sonuçlarını rapor etsin
- Output: `STAGING/CODEX_DONE_unity_fix_s95.md`
- **Geri alınabilir:** Codex git diff temiz olsun, her hatayı ayrı dosyada minimal değişiklikle çöz, atomic commit DEĞİL (user manual commit eder)

**Adım 4:** Faz A tatmin olunca (build 0 error, test pass) → Faz B'ye geç.

---

### Faz B — Duvar Zemine Oturma Mimari Kararı

**Bağlam:**
- Aktif sahne: `Assets/Scenes/Demo/PathC_BaseTest.unity`
- Grid: `cellLayout=Isometric`, `cellSize (1, 0.5, 1)`, `scale (1, 0.5, 1)` (transform squash y=0.819 değil, direkt 0.5)
- Floor: 16 isometric tile (PixelLab `b340684f-552b-49e6-a281-ab360d376564`), 16×10 grid
- Walls: henüz üretilmedi, mevcut wall asset'ler (`Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/` × 5) fake-iso uyumluluğu test edilmedi
- Production plan: `STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md` (L2a base tile + L2b tall sprite hibrit)
- Eski karar: `[[project_tile_angle_verdict_branch_d_e_lock.md]]` (Branch D+E, transform squash)
- Mevcut bug (RimaUnifiedPainter): "Auto-connect walls Walls subgrubunu bypass ediyor"

**Soru:**
Duvar sprite'ı (L2b tall sprite, 64×128) isometric floor tile'ın (64×64 diamond) **alt kenarına nasıl tam oturur**?

Spesifik alt-sorular:
1. **Pivot noktası nerede olmalı?** (sprite alt-orta? alt-iki-köşe arası?)
2. **Y-offset nasıl hesaplanır?** Hücre alt kenarı = cell.y - 0.25 (cellSize 0.5/2)? Yoksa transform squash sonrası farklı?
3. **L2a base tile (thickness 0.15) ile L2b tall sprite üst üste mi, yan yana mı?**
   - Üst üste: L2a footprint + L2b yüzey overlay (Hades pattern)
   - Yan yana: L2a base, L2b ayrı cell footprint?
4. **Sorting layer/Order in Layer:** Karakter Y-sort ile uyumlu mu?
   - Wall sortingOrder = round(world.y * 100)?
   - Veya `IsometricZAsY (4)` cell layout?
5. **Diamond facet alignment:** Wall sprite alt kenarı diamond'un üst kenarına denk gelmeli mi (tile'ın "north edge")?
6. **Multi-cell walls:** Uzun duvar 3 cell uzunluğunda → 3 ayrı L2b sprite mi, tek 3w×1h sprite mi?

**Süreç (Opus ⇆ Codex):**

1. **Iter 1 — Opus taslak:** Yukarıdaki 6 soruya cevap + ASCII diagram (top-down + perspective view) + pivot/anchor değerleri (Unity Vector2 0-1 range).
2. **Iter 1 — Codex review:** `cx_dispatch.py` ile gönder:
   - Codex'in task'i: UnityMCP ile mevcut wall asset'lerden 1 tanesini PathC_BaseTest sahnesine 5 farklı pivot/Y-offset kombinasyonunda place et, screenshot al, hangisi diamond tile alt kenarına tam oturuyor rapor et.
   - Output: `STAGING/CODEX_DONE_wall_seating_test_v1.md`
3. **Iter 2 — Opus revize:** Codex test sonuçlarına göre pivot/offset finalize et.
4. **Iter 2 — Codex re-validate:** Final spec'le tekrar place et, görsel doğrula.
5. **Iter 3 (opsiyonel) — STOP** Codex onay verdiğinde.
6. **Aşırı uzarsa (>3 iter veya >90 dk):** BLOCKED yaz, kalan açık sorular enumerate, durdur.

**Çıktı format — `STAGING/WALL_FLOOR_SEATING_SPEC_v1.md`:**

```markdown
# Duvar Zemine Oturma — DRAFT v{N}

## Verdict
{LIVE / NEEDS_USER_INPUT / BLOCKED}

## Iter Log
- v1: Opus taslak → Codex visual test (5 pivot variant)
- v2: Opus revize → Codex re-test (PASS)

## Final Spec
### Pivot & Anchor
- L2b wall sprite pivot: Vector2(0.5, 0.0625) — alt-orta-altı
- Sprite import: pixels per unit 64, pivot custom
- World Y-offset: cell.y - 0.25 (cellSize.y/2)

### L2a vs L2b Konumlama
- L2a base tile: isometric diamond, thickness 0.15, sortingOrder = -100
- L2b tall sprite: L2a'nın "north edge" üzerinde, sortingOrder dinamik (Y-sort)

### Y-Sort Kuralı
- Karakter & L2b wall: sortingOrder = round(-transform.position.y * 100)
- Floor tile (Grid child): IsometricZAsY (4)

### Multi-Cell Walls
- Her cell ayrı L2b instance (RuleTile-style, auto-connect)
- Adjacency: PaintWallWithConnections, sub-grup parent: Walls (bug B2'den sonra)

### Ekran Görüntüsü Referansı
{Codex screenshot path}

## Codex Review Excerpts
{Quote key feedback}

## Açık Sorular (user'a)
- {Soru 1: ...}
```

**Geri dönülebilir:** Sahnede test yaparken Codex değişikliklerini commit etme. `git stash` veya scene save'i bırak ki user iptal edebilsin. Sadece spec dosyası `STAGING/`'a kalıcı yazılır.

---

## Hard Constraints (her iki faz)

- **Karpathy 4 inline.**
- **NLM ACCESS:** Tasarım context'i için NLM:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Codex dispatch:** Her cx_dispatch.py çağrısı **background DEĞİL** — subagent burada bekleyebilir, Codex sonucu gelince devam et.
- **Aşırı uzama:** Faz A toplam >45 dk veya 3+ failed fix iter → BLOCKED. Faz B toplam >90 dk veya >3 iter → BLOCKED.
- **Geri dönülebilir:** Faz A'da kod değişikliği var ama auto-commit YOK. Faz B'de hiç kod değişikliği YOK.

## Orchestrator'a Final Rapor

- Faz A: kaç error vardı, kaçı fix'lendi, kalanlar (varsa)
- Faz B: kaç iter, verdict, pivot/anchor değeri, açık sorular
- Toplam süre
