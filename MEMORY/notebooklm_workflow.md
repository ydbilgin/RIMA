---
name: NotebookLM workflow
description: NotebookLM MCP kullanım stratejisi — Claude dosya okumak yerine notebook_query ile bağlam alır, token tasarrufu sağlar.
type: project
---

## HARD RULE (2026-05-07)

**Claude hiçbir dosyayı direkt Read ile açamaz — tek istisna: CURRENT_STATUS.md (session start).**

Bütün dosya okuma işlemleri NotebookLM MCP üzerinden yapılır:
1. `notebook_query` ile sor
2. Dosya notebook'ta yoksa veya stale ise → önce `source_add` ile push et, sonra query
3. Auth expire olduysa → `PowerShell: nlm login` çalıştır (Chrome otomatik açılıp kapanır, ~30s). Başarısız olursa `! nlm login` öner.

**Why:** TASARIM/ + MEMORY/ toplamı büyük, her session re-read token maliyetini patlatır. NotebookLM bağlam daha ucuz ve indexed.

**How to apply:** Her dosya ihtiyacında önce NotebookLM. "Yoksa direkt Read" artık geçerli değil — önce push, sonra query.

## MCP Tool'ları (notebooklm MCP — jacob-bd/notebooklm-mcp-cli)

- `notebook_query` — notebook içeriğini sorgula (ana kullanım)
- `cross_notebook_query` — birden fazla notebook'u sorgula
- `source_add` — URL, text, local file, Google Drive ekle
- `source_delete` — kaynağı sil
- `batch(action="add_source")` — toplu kaynak ekleme (bootstrap)
- `notebook_create` / `notebook_list` — notebook yönetimi

## Notebook

- **ID:** `ed3c8952-417c-4988-84a7-425d25ba3b08`
- **Başlık:** RIMA Game Design Knowledge Base
- **Bootstrap:** 2026-05-06 — 80 kaynak (79 dosya + INDEX.md), sıfır hata
- **Sync tag:** `nlm-sync-20260506`
- **İçerik:** TASARIM/*.md (42 dosya) + MEMORY/*.md (37 dosya)

## Sorgu

```bash
uvx --from notebooklm-mcp-cli nlm notebook query ed3c8952-417c-4988-84a7-425d25ba3b08 "soru"
```

## Bootstrap Tamamlandı ✓

## Session Başı Auth Kontrol Sırası (ZORUNLU)

1. `mcp__notebooklm__refresh_auth` — disk cache'den token yükle
2. Küçük bir test: herhangi bir `source_add` veya `source_delete` dene
3. Başarısız → `PowerShell: nlm login` (timeout 120s, Chrome açar/kapatır)
4. Tekrar `refresh_auth` → devam

**NOT:** `notebook_query` auth expire olsa bile çalışabilir (read token ayrı). Source işlemleri write token gerektirir — her zaman session başında kontrol et.

## Güncelleme Stratejisi (Session Sonu)

**DOĞRU DÖNGÜ — sadece add yapma, duplicate birikir:**
```
1. nlm source list <notebook_id>           → eski source ID'yi title ile bul
2. source_delete(source_id=..., confirm=True) → eski kaynağı sil
3. source_add(source_type="text", title="CURRENT_STATUS.md — nlm-sync-YYYYMMDD", ...)
```

Source title formatı: `"<dosya_adı> — nlm-sync-YYYYMMDD"`
Sync tag CURRENT_STATUS.md'ye de yazılır.

Judgment gerekmez — pure mekanik, Claude direkt MCP üzerinden halleder.

## MCP Config

- Scope: local (project)
- Command: `uvx --from notebooklm-mcp-cli notebooklm-mcp`
- Auth: yasinderyabilgin@gmail.com (`nlm login` ile yapıldı)
- Düzeltilen hata: eski config `uvx notebooklm-mcp-cli` → yanlış executable, düzeltildi 2026-05-06
