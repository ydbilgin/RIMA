---
name: NotebookLM workflow
description: NotebookLM MCP kullanım stratejisi — Claude dosya okumak yerine notebook_query ile bağlam alır, token tasarrufu sağlar.
type: project
---

## Strateji

Claude, TASARIM/ ve MEMORY/ dosyalarını direkt okumak yerine NotebookLM'e `notebook_query` ile soru sorar.
Bu yaklaşım dosya okuma token maliyetini sıfırlar.

**Why:** TASARIM/ + MEMORY/ toplamı büyük. Her session'da re-read yerine NotebookLM indeksli bağlam daha ucuz.

**How to apply:** Bir TASARIM/ veya MEMORY/ dosyasının içeriğine ihtiyaç duyulduğunda önce `notebook_query` dene. Dosya yoksa / stale ise direkt Read.

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

## Güncelleme Stratejisi (Session Sonu)

```
git diff --name-only <last_sync> HEAD -- TASARIM/ MEMORY/
→ değişen dosyalar için: source_delete(eski) + source_add(yeni)
→ checkpoint: git tag nlm-sync-YYYYMMDD
```

Judgment gerekmez — pure mekanik, Claude direkt MCP üzerinden halleder.

## MCP Config

- Scope: local (project)
- Command: `uvx --from notebooklm-mcp-cli notebooklm-mcp`
- Auth: yasinderyabilgin@gmail.com (`nlm login` ile yapıldı)
- Düzeltilen hata: eski config `uvx notebooklm-mcp-cli` → yanlış executable, düzeltildi 2026-05-06
