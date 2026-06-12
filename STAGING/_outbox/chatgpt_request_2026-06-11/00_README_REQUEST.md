# RIMA → ChatGPT — İSTEK PAKETİ (2026-06-11)

## Bu paket nedir
RIMA ekibinden ChatGPT'ye **görsel asset + diyagram + konsept** üretim isteği. `01_CONTEXT_CANON.md`'deki KANONA (kesin renk/lore) uy. `02_REQUESTS.md`'deki öncelikli listeyi üret.

## ChatGPT'nin rolü (net sınır)
- ✅ **ÜRET:** görsel asset (animated background, sprite frame, parallax katman), diyagram (sunum), konsept mockup, **bağımsız (self-contained) referans kod**.
- ❌ **ÜRETME:** RIMA-entegre gameplay kodu (ışık/dekorasyon/parallax wiring, IsoRoomBuilder/RoomLightingProfileSO entegrasyonu). Bunlar tam repo-context'i olan iç pipeline'da (Codex) yapılıyor. Senin gönderdiğin .cs'ler jenerik starter olarak kullanışlı ama entegrasyonu biz yapıyoruz.

## Çıktı formatı (ÖNEMLİ)
- Tek **ZIP** olarak dön.
- Asset'leri klasörlerle organize et (Background/FAR, Background/MID_frames, Diagrams/, Concepts/ vb.).
- Animasyonlar: **ayrı PNG frame'ler + sprite-strip**, şeffaf gereken yerde alpha-ON.
- Her ana klasöre kısa bir `MANIFEST.md` / README + önizleme (JPG/GIF) koy.
- Kanon-ihlali (renk/lore) gerekiyorsa AÇIKÇA işaretle, körlemesine uyma.
