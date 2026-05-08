---
name: ChatGPT Canvas Tile Size Fix
description: ChatGPT canvas eşit grid bölme talimatıyla doğru tile boyutu üretebilir; pipeline fallback mevcut
type: project
---

ChatGPT, tile sheet üretirken yanlış boyut veriyor (örn. 1254×1254 yerine 1024×1024, 1774×887 yerine 1024×512).

**Fix:** ChatGPT'de canvas açıkken şu şekilde iste: "Bu canvas'ı tam olarak eşit [4]×[4] grid olarak böl, her hücre [256]×[256] px olsun" → doğru piksel boyutlarını üretme ihtimali artar.

**Why:** ChatGPT canvas tool'u freeform render ediyor; explicit grid constraint verince boyut tutarlılığı artıyor.

**Fallback:** `process_tiles.py` `W // cols, H // rows` ile arbitrary input boyutunu otomatik handle eder → NEAREST resize ile 64×64 çıkış. Non-standard input = hafif kalite kaybı (1-2px edge clipping) ama kabul edilebilir piksel sanat için.

**How to apply:** Her yeni ChatGPT tile sheet üretiminde önce canvas bölme talimatı dene. Yine de yanlış gelirse process_tiles.py ile geç.
