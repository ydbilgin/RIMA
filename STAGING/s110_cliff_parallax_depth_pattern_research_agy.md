ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

LaurethStudio RIMA Room Painter (Phase A) için **cliff + parallax depth pattern** araştırması. User auto cliff'i bırakıp **manuel brush + Cliff/Parallax layer seçimli depth chooser**'a geçiyor.

## Bağlam

- RIMA aktif sahne: Hades Elysium estetiği, yüzen ada + cliff edge + cyan rune + parallax 7-layer KitC
- Mevcut Cliff sprite seti: KitB_Cliff 9 sprite (S/SE/SW dir + variants) 128×192 PPU 64
- Mevcut Parallax: ParallaxLayer.cs (UPM package), factor 0.03-0.14 exponential dağılım
- Engine: Unity 2022.3+, URP 2D Renderer

## Görev

Modern indie + AAA 2D oyunlarında "cliff" ve "parallax background" arasındaki depth tier'lerin nasıl yönetildiğini araştır. Özellikle **designer'ın manuel olarak cliff sprite'ını yakın (foreground gameplay) veya uzak (parallax bg) layer'a koyma kararı** ne pattern ile veriliyor?

### Araştır

1. **Hades (Supergiant)** — Elysium'da cliff edges var, hangileri gameplay layer hangisi BG?
2. **Children of Morta** — multi-layer cliff/island, parallax
3. **Death's Door** — top-down 3/4 cliff handling
4. **Hyper Light Drifter** — strict 4-layer parallax + cliff edge konumu
5. **Sea of Stars** — cliff edge + parallax mountain ridge — designer authoring tool
6. **Tunic** — 2D-pretending-3D cliff drop
7. **Eastward** — paralax mismatch + cliff edge story scenes

### Sorular

1. **Karar kuralı:** Bir cliff sprite ne zaman gameplay layer'da (player engelliyor) ne zaman parallax bg'de (sadece görsel) olur? "Walkable edge yakınında mı, uzakta mı" net kural?
2. **Depth tier sayısı:** Hangi oyun kaç tier kullanıyor? (5 layer? 10 layer? 3 tier?)
3. **Parallax factor by tier:** Lineer mi, exponential mi, log mı? Net oran (örn `[1.0, 0.7, 0.4, 0.2, 0.1, 0.05]`).
4. **Cliff drop face:** Sprite'ın drop face uzunluğu (kaç pixel yukarı-aşağı) gameplay vs BG'de değişiyor mu?
5. **Authoring UX:** Designer cliff'i layer A vs B'ye nasıl assign ediyor? (Dropdown? Drag-drop ayrı palette? Toggle?)

### Çıktı formatı

Markdown, max 1000 kelime.

#### Bölüm 1: 7 oyun matrisi (1-2 satır per oyun)
| Oyun | Cliff layer tier | Parallax tier | Depth chooser UX | LaurethStudio'ya çekilecek |

#### Bölüm 2: 3 PATTERN — depth tier organizasyon
Modern indie'lerin convergence ettiği 3 yaygın depth tier scheme. Her biri için: tier count, factor formula, cliff use case.

#### Bölüm 3: Designer authoring UX kararı (TOP 3)
- Pattern A: Sprite asset'e default layer assign + override option
- Pattern B: Tek brush, runtime depth slider
- Pattern C: Ayrı paletler (Cliff palette vs Parallax palette)

Her pattern için: Pros, Cons, Effort (S/M/L), LaurethStudio için verdict.

#### Bölüm 4: TOP 3 actionable insights
RIMA Room Painter'a doğrudan implement edilebilir 3 net karar.

### Yapma
- Speculasyon yapma — referans oyun + dokümante edilmiş kanıt iste
- RIMA-internal detaylara girme — studio-level pattern
- Implementation kod yazma
