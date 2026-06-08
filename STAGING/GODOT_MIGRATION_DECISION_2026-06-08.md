# Godot Geçişi + 3 Video Dersleri — COUNCIL KARARI (2026-06-08)

**Soru:** RIMA ve GELECEK oyunlar için Godot'a geçmek mantıklı mı? (agent/MCP olgunluğu, 2D pixel-art workflow, öğrenme eğrisi, "ben kolay müdahale ederim") + 3 izlenen YouTube videosundan uygulanabilir dersler.
**Yöntem:** Council — cx (feasibility/reuse, file:line kanıtlı) + ax-3.1-Pro (derin/web) + ax-3.5-Flash (lean) → Opus sentez. Veri orchestrator tarafından web + yt-dlp transcript ile toplandı.

---

## TL;DR (karar)
1. **RIMA = Unity'de KAL ve BİTİR.** Oybirliği. Demo ortasında taşınabilirlik için refactor = bitirme riski.
2. **GELECEK oyunlar için Godot = MEŞRU ve İYİ seçim** — özellikle 2D pixel-art + senin "kendim kolay müdahale ederim" tercihinle. Dil = **GDScript** (C# port etme).
3. **"Godot MCP kötü" İDDİAM YANLIŞTI** — düzeltildi: olgun (godot-ai ~39 tool/503★/v2.6.1, Coding-Solo 4.1k★). Tek gerçek zayıflık: GDScript dinamik-tipleme **ajan** verimini düşürür (insan için değil — senin için sorun değil).
4. **RIMA'dan "reusable sistem KÜTÜPHANESİ" çıkarma — ŞİMDİ DEĞİL.** Bunun yerine ucuz olanı al: sistemlerin **tasarım/spec/test-senaryolarını** not et (kod değil). cx adayları aşağıda.

---

## 3 advisor ne dedi

| Lens | Özet |
|---|---|
| **cx** (reuse/feasibility) | RIMA'nın çoğu Unity'ye kilitli (MonoBehaviour/SO/Tilemap/URP). Taşınabilir küçük "rules/data core" VAR: RewardOffer DTO, draft offer selector, RoomRunLifecycle state machine, room slot/walkability helper, Loc, event payload DTO. Minimal core 1-2 hafta; full engine-agnostic 6-10+ hafta (yapma). Gelecek=Godot+GDScript, kodu değil **tasarım+test-spec+DTO şemasını** reuse et. |
| **ax-3.1-Pro** (derin/web) | C# ekosistemi AJANLAR için üstün (strict-typing + compile-time hata yakalama → az halüsinasyon; GDScript dinamik-tip → runtime-crash). "Godot daha az karışık" = **insan için doğru, ajan için ters.** Öneri: **Pure C# POCO** mimarisi (mantığı MonoBehaviour'dan ayır) = motor-agnostik sigorta + %100 unit-test. ⚠️ Verimde hata yakaladı: StraySpark muhtemelen **Unreal** MCP'si, Godot değil (doğrulanacak). |
| **ax-3.5-Flash** (lean) | Motor araştırmasının kendisi demo-erteleme (yak-shaving). **RIMA'yı bitir/yayınla.** Godot MCP solo-dev için fazlasıyla yeterli. GDScript daha az kafa-ağrısı; C# port etmek = "over-engineering felaketi". "Make Systems Not Games" demoya UYGULAMA (spagetti de olsa bitir). |

## Çelişki + benim kararım
- **3.1-Pro "şimdi POCO refactor yap" ↔ Flash "hiçbir şey çıkarma, bitir".** 
- **Kararım = cx'in orta yolu:** Büyük POCO/library refactor'u ŞİMDİ YAPMA (Flash haklı: demo riski). Ama 3.1-Pro'nun hijyen noktası geçerli → **bir dahaki o dosyaya dokunduğunda** ilgili kuralı/şemayı ayrı bir spec notuna geçir. Kütüphane inşa etme; **spec hasat et.** Bu, motor kararını sigortalayan ucuz yol (kod-port değil, tasarım-transferi her motora bedava gider).
- **GDScript vs C#:** Üçü de C#-port'u düşük-değerli buluyor. Gelecek Godot oyununda **GDScript** kullan. 3.1-Pro'nun ajan-endişesi gerçek ama **GDScript'in opsiyonel static-typing'i** (type hint'ler) farkı daraltır — ajanlara "typed GDScript yaz" dedirt.

---

## "Agent-verimi vs ben-kontrol" ekseni (senin asıl kararın)
| Önceliğin | Kazanan |
|---|---|
| Codex/Claude editörü tam güçte sürsün (en az ajan-hatası) | **Unity + C#** |
| SEN elinle kolay müdahale + hafif + sade + açılış-anında | **Godot + GDScript** |

Sen "kendim kolay müdahale ederim" diyorsun → bu **insan-kontrol** eksenini öne koyuyor; orada Godot kazanır. Agent-pipeline'ı bir miktar zayıflar ama yok olmaz (Godot MCP olgun). **Not:** Godot SADECE 2D değil — tam 2D+3D motoru (2D'de en güçlü, 3D Godot 4'le gerçek/gelişiyor).

---

## 3 VİDEODAN SOMUT DERSLER

### 1. "Make Systems Not Games" (en değerli)
- **Gelecek oyunlar:** Dream-game'i direkt yapma; ayrı projelerde bağımsız/test-edilebilir SİSTEMLER yap, gereksinime göre tasarla, sonra birleştir. Refactor-bırakma = bir numaralı bitirme katili.
- **RIMA'ya ŞİMDİ:** Demoyu BİTİR (sistemleştirme erteleme olur). Tek ucuz aksiyon = cx'in listelediği rules/data core adaylarını dokundukça **spec olarak not al** (kod kütüphanesi DEĞİL).

### 2. "Yūgen Terrain Toolkit" (Godot 3D-pixel-art terrain)
- **Ders:** Gelecek oyunlarda ajanlara level "dizdirme", **algoritma yazdır** (Marching Squares, cellular automata). Prosedürel = motor-agnostik + ajan-dostu (saf mantık).
- Hazır plugin varsa (TileMapDual, Yūgen) kullan; kendin yazma. 3D-pixel-art = gelecek-oyun stil opsiyonu.

### 3. "You Don't Need to Be an Artist" (her iki motor için geçerli)
- **Pixel ayarları:** filter=Nearest · iç çözünürlük 640×360 (RIMA ZATEN bunu kullanıyor ✅) veya 320×180 · integer scaling (yarım-sayı yok = bulanıklık yok). Godot: stretch=canvas_items/keep/integer. Unity karşılığı: Pixel Perfect Camera (RIMA'da var).
- **Sanat felsefesi:** basit/tanınır formlarla başla, düşük detay, referans kullan; Krita (ücretsiz) veya Aseprite ($20); karakter-dışı asset'leri pixel-pixel değil fırçayla boya; **görsel yükü shader/particle/screen-shake'e kaydır** ("pop" çizmeden gelir).

---

## NET TAVSİYE
1. **Şimdi:** RIMA'yı Unity'de bitir. Godot araştırmasını burada kapat (daha fazlası erteleme).
2. **RIMA biterken (ucuz):** cx'in rules/data core adaylarına dokundukça spec/test-senaryosu notu al — gelecek motora bedava sigorta.
3. **Bitirme demosu teslim olunca:** Godot 4 + GDScript ile KÜÇÜK bağımsız bir sistem prototipi yap (örn. envanter veya bir combat-loop). Bir Godot MCP server (godot-ai, AssetLib 1-tık) kur, agent-pipeline'ı CANLI test et. Bu hafta sonu deneyi sana iki şeyi ölçtürür: öğrenme eğrisi + pipeline'ın Godot'ta gerçekten ne kadar çalıştığı.
4. **Karar kriteri:** O prototipte agent-pipeline yeterince çalışıyorsa → gelecek oyunlar Godot/GDScript. Çalışmıyor/sinir bozucuysa → Unity/C# kal. Körlemesine geçme; prototiple kanıtla.

## Caveat'lar
- StraySpark = Godot mu Unreal mı, doğrulanacak (ax-3.1-Pro Unreal dedi; benim ilk verim Godot demişti). Sonucu değiştirmez (godot-ai+Coding-Solo zaten yeterli kanıt).
- Tüm MCP/sürüm bilgisi 2026-06-08 web-anlık; üretimden önce VERIFY-LIVE.
- Council ham çıktıları: `STAGING/_process/2026-06/_council_*godot-migration.md` + `CODEX_DONE_yekta.md`.
