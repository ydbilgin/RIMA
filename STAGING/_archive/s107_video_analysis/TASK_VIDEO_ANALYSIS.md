# TASK: YouTube Video Analizi — RIMA + LaurethStudio Çıkarımları

ACTIVE RULES: (1) think before reviewing (2) min response, no speculation (3) cite specific moments/timestamps from the video (4) BLOCKED if video inaccessible.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Verilen YouTube videosunu izle, ne anlattığını özetle, RIMA (2D top-down roguelite ARPG) ve LaurethStudio (master game plan) için somut çıkarımlar üret. Çıktı inline — dosya YAZMA (kullanıcı kendi taşıyacak LaurethStudio'ya).

## Video
**URL:** https://youtu.be/7vhCpACf1Hg

## Görev

### Bölüm 1 — Video Özeti (~200 kelime)
- Video ne hakkında? Konu, tez, ana iddia
- Kim anlatıyor (oyun geliştirici / oyun tasarımcısı / kanal)
- Hangi oyunlar/örnekler referans alınıyor
- Anahtar momentleri timestamp ile not düş (örn: "12:34 — X kavramı tanıtılıyor")
- 3-5 ana başlık

### Bölüm 2 — RIMA için Çıkarımlar (somut, eyleme dönük)
**RIMA bağlamı:**
- 2D top-down 3/4 roguelite ARPG (Hades / Children of Morta / Diablo III referans)
- V1 wall-less Hades Elysium görsel dili LOCKED (floating arena + cliff edges + cyan rune + warm brazier)
- 3-Kit BG mimarisi: A=floor / B=cliff face / C=parallax bg
- 8 yön karakter sprite, 64 PPU, 10-12 fps animasyon
- Mevcut sınıflar: Warblade, Elementalist + roster v2
- Yarık (rift) 3-scale visual language, oda türüne göre density
- Karpathy 4 design principles (think first, min code, surgical, goal-driven)

**Çıkarım formatı:** Her madde için
- "Videodaki X kavramı/teknik" → "RIMA'da nasıl uygulanır" (somut sistem/asset/UX karar önerisi)
- Şu an LIVE olan kararlarla CELISIYOR MU? — açıkça flag et
- Effort tahmini (S/M/L)

5-10 madde hedef.

### Bölüm 3 — LaurethStudio için Çıkarımlar (cross-cutting)
**LaurethStudio bağlamı:**
- User'ın master oyun planı — RIMA dahil 13 3D + 6 2D oyun
- Procgen stack: Poisson+Dual Grid MASTER, WFC AVOID default
- Shared lib hedefi: LaurethProc
- Studio-wide visual identity arayışı

**Çıkarım formatı:**
- Videodaki hangi prensip/teknik birden fazla studio oyununa uygulanabilir?
- Studio-wide pipeline/tool önerileri
- Hangi oyun türü için en güçlü ders var

3-5 madde hedef.

### Bölüm 4 — Karşı görüşler / Riskler
- Videodaki tavsiyelerin sınırları neler
- RIMA'nın mevcut LIVE kararları (V1 wall-less, top-down 3/4) bu videodaki yaklaşımla çelişir mi
- Hangi tavsiyeler RIMA'nın scope'una büyük gelir

## Kısıtlar
- **Inline response only** — STAGING'e veya başka yere DOSYA YAZMA, scratch dahil. User kendi LaurethStudio klasörüne kaydedecek.
- **Specific timestamps zorunlu** — "videoda diyor ki" değil, "07:23'te X argümanı" gibi
- **Speculative chaining yasak** — "şöyle de olabilir, böyle de olabilir" şeklinde belirsiz öneriler verme; net karar veya net BLOCKED
- **Video erişilemezse** BLOCKED yaz, neden olduğunu (region lock, age restriction, deleted) söyle

## Beklenen toplam uzunluk
600-1000 kelime arası. Karpathy 4 — minimum useful content, max signal.
