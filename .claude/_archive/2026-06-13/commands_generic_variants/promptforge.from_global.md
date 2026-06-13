---
description: Ham fikri Gemini 3.1 Pro Good Prompt formatına çevir (RIMA bağlamlı)
allowed-tools: Bash
---

## CONFIG (proje-başına düzenle)
- `NLM_REPO`: [Çalışılan proje kökü] (env `$env:NLM_REPO` ya da satır-içi değişken, default: git rev-parse ile tespit edilen kök veya RIMA dizini "F:/Antigravity Projeler/2d roguelite/RIMA")

Kullanıcının verdiği ham fikri Gemini 3.1 Pro'ya Good Prompt formatında işlet.

## Adımlar

1. Aşağıdaki komutu çalıştır (proje dizininden — GEMINI.md otomatik okunur):

```bash
REPO="${NLM_REPO:-$(git rev-parse --show-toplevel 2>/dev/null || echo 'F:/Antigravity Projeler/2d roguelite/RIMA')}"
gemini -p "$ARGUMENTS" -m gemini-3.1-pro-preview -o text 2>&1
```

2. Çıktıyı kullanıcıya sun.

3. Şu soruyu sor:
   > "Bu promptu **doğrudan uygulayayım mı** (hemen çalıştırırım), yoksa **kopyalamak** ister misin?"

## Hata Durumları

- **Rate limit / timeout:** "Gemini 3.1 Pro rate limit — Flash ile tekrar deneyeyim mi?" diye sor, onay gelince `-m gemini-flash-latest` ile çalıştır.
- **Boş çıktı:** Komutu bir kez tekrar dene.
- **Model bulunamadı:** `-m gemini-2.5-pro` ile fallback yap.

## Not

GEMINI.md proje kökünde (`<repo>/GEMINI.md`) mevcut. Gemini CLI her çalıştırmada bu dosyayı otomatik okur — Claude ayrıca bağlam göndermez, token harcanmaz.
