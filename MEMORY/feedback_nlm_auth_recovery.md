---
name: NLM Auth Recovery
description: NLM CLI sorgusu "Authentication expired" döndürürse, bash `nlm login` çalıştır → otomatik Chrome auth → komutu tekrar dene
type: feedback
---

# NLM Auth Recovery

## Problem
NLM sorgusunda hata:
```
Error: Query failed: Authentication expired. Run 'nlm login' in your terminal to re-authenticate.
```

## Çözüm
```bash
nlm login
```

- Otomatik olarak Chrome açılır (CDP)
- Kullanıcı login eder
- Cookie + CSRF token kaydedilir
- Komut tekrar denenebilir

## Notes
- Profil: `default` (yasinderyabilgin@gmail.com)
- Credentials: `C:\Users\ydbil\.notebooklm-mcp-cli\profiles\default`
- ~30 cookie alınır
- Genelde 7-14 gün geçerli
