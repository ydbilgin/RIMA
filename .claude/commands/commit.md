---
description: Uncommitted dosyaları mantıksal gruplara ayırıp commit et. /commit → preview, /commit --yes → direkt commit.
allowed-tools: Bash
---

Uncommitted değişiklikleri mantıksal gruplara ayır ve commit et.

**ARGUMENTS:** "$ARGUMENTS"
- Boş → grupları göster, commit ATMA (preview)
- `--yes` → tüm grupları direkt commit et
- `--yes code` / `--yes docs` / `--yes art` / `--yes staging` / `--yes config` → sadece o grubu commit et

## Adım 1 — Git status al

```bash
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" status --short 2>/dev/null
```

## Adım 2 — Dosyaları grupla

Her dosya ilk eşleşen gruba gider. `.meta` → parent asset'in grubu.

| Grup | Pattern eşleşmesi |
|------|-------------------|
| `code` | `Assets/Scripts/`, `Assets/Editor/`, `Assets/Tests/` (`.cs`, `.asmdef`, `.json` assembly) |
| `art` | `Assets/Art/`, `Assets/Prefabs/`, `Assets/Animations/`, `Assets/Tiles/`, `Assets/Scenes/`, `Assets/Screenshots/`, `Assets/Resources/` + bunların `.meta` |
| `docs` | `TASARIM/`, `MEMORY/`, `ARCHIVE/`, `CURRENT_STATUS.md`, `CLAUDE.md`, `RULES.md`, `AGENTS.md`, `CODEX_DISPATCH.md` |
| `staging` | `STAGING/` |
| `config` | `.claude/` (`.claude/nlm_sync_state.txt` HARİÇ — local state, commit etme), `*.lock`, proje root `.json/.yaml` |
| `other` | Geri kalan her şey |

`.claude/nlm_sync_state.txt` → hiçbir zaman commit etme.

## Adım 3 — Commit mesajı belirle

Her grup için değişikliklere bakarak uygun mesaj seç:
- `feat(scope):` — yeni dosya/özellik
- `fix(scope):` — düzeltme
- `chore(scope):` — config/tooling değişikliği
- `docs(scope):` — döküman güncelleme
- `art(scope):` — asset ekleme/güncelleme

scope örnekleri: `scripts`, `map`, `combat`, `ui`, `config`, `tiles`, `tasarim`, `memory`, `staging`

## Adım 4 — Preview veya Commit

**Preview (ARGUMENTS boş):** Her grup için dosya listesini ve önerilen commit mesajını göster. Commit ATMA.

**Commit (ARGUMENTS --yes ile başlıyorsa):**

Her grup için:
```bash
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" add -- <dosya1> <dosya2> ...
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" commit -m "$(cat <<'EOF'
type(scope): mesaj

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
EOF
)"
```

Silinen dosyalar için: `git -C "F:/Antigravity Projeler/2d roguelite/RIMA" rm -- <dosya>` önce çalıştır.

## Adım 5 — Doğrula

```bash
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" log --oneline -6
git -C "F:/Antigravity Projeler/2d roguelite/RIMA" status --short
```

`status --short` boşsa: "Tüm değişiklikler commit edildi." yaz.
