# Codex Task: LaurethStudio Caterpillar — Derin Analiz

**Model:** gpt-5.5, effort=high
**Çıktı:** `F:\LaurethStudio\03_IDEAS\Caterpillar\CODEX_ANALYSIS.md`
**Süre tahmini:** 30-50 dk

---

## Bağlam

LaurethStudio (`F:\LaurethStudio\`) solo indie umbrella. RIMA Faz 1 prod aktif, CircuitBreaker pre-prod LOCK, 10 farklı pitch backlog'da. Yeni idea: **Caterpillar** — bir tırtılın gözünden mikrocosmos hayatta kalma → krizalit → kelebek metamorphosis oyunu.

**Kullanıcı talebi:** "Bir tırtılın hikayesi. Tırtıl hayatta kalırsa kelebeğe dönüşür. Doğadaki onunla alakalı olayları tırtıl gözünden oynayalım. Bir sürü şey ekleyebileceğimiz fikir."

Opus max brainstormu hazır: `F:\LaurethStudio\03_IDEAS\Caterpillar\BRAINSTORM_OPUS.md` — 5 pitch (Wingspan/Larva/Instar/Imago/Polychroma), mekanik havuz, STUDIO_KARAR uyum matrisi, risk tablosu var. **Senin görevin tekrarlamak değil, derinleştirmek + pazar/scope/risk gerçek dünya verisiyle test etmek.**

Studio bağlamı:
- `F:\LaurethStudio\STUDIO_GUIDE.md` — yapı + 8 STUDIO_KARAR
- `F:\LaurethStudio\00_RULES\STUDIO_CONSTITUTION.md` — high-level kurallar
- `F:\LaurethStudio\03_IDEAS\STUDIO_PITCH_BACKLOG.md` — 10 mevcut pitch (Slipway, Foldworld, Recurse, Tidemark vs.)
- `F:\LaurethStudio\03_IDEAS\oyun_fikirleri\` — 22 daha eski fikir + 8 kategori

RIMA bağlamı (transfer edilebilir pipeline):
- `F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\MAP_TILE_PIPELINE.md`
- `F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\ANIMATION_SPLIT_TECHNIQUE.md`
- `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Combat\CombatEventBus.cs` (VFX Router pattern)
- `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\room_designer_master_spec_v3.md`

---

## Görev — 7 Soru

### 1. Tür seçimi (KARAR_008 ihlal yok)

5 pitch'ten hangisi LaurethStudio'ya en uygun? STUDIO_KARAR_008 Solo Dev Single-Genre kuralına en az çakışan. Sırala 1-5, gerekçele.

**İstenen format:**
```
1. Pitch X — neden 1
2. Pitch Y — neden 2
...
5. Pitch Z — REJECT, neden
```

### 2. Pazar / Precedent analizi

Pitch sıralamandaki ilk 2 pitch için:
- Aynı tür+tema bileşkesinde 5-10 yakın oyun bul (Steam/itch.io)
- Steam etiket olarak: insect, caterpillar, butterfly, ecology, metamorphosis, microcosmos, biology, life-cycle
- Pazar boşluğu var mı? (mevcut Webbed, Cocoon, A Bug Story dışında insekt oyun yok denecek kadar az)
- Pricing benchmark ($10-15 indie cozy, $15-25 action-adventure, $20-30 narrative puzzle)
- En yakın "anchor reference" oyun seç (ör: Spiritfarer + Stardew + Tunic kombosu).

### 3. Scope tahmini (gerçekçi solo dev ay)

Birinci tercih pitch için:
- Asset count: tırtıl türü, kelebek türü, predator, plant, biome, weather var, UI screen
- Script count: gameplay system count
- AI pipeline yardımı (PixelLab/Codex/Gemini) ile **gerçekçi solo dev ay tahmini**
- 3-tier scope: **MVP (3 ay), Demo (6 ay), Full (12-18 ay)**

### 4. Risk tablosu (Opus brainstormu üstüne ekleme)

Opus brainstormundaki 7 risk'i eleştir + 3-5 yeni risk ekle:
- Marketing/pitch risk (insekt-themed pitch erişimi zor mu?)
- Steam algorithm risk (cozy genre kategorisi crowded)
- Localization risk (Türkçe→İngilizce çiçek/böcek terminoloji)
- Audio risk (insect ambient ses tasarımı pahalı)
- Platform risk (Switch tarzı cozy hedefi gerekli mi?)

### 5. RIMA pipeline transfer matrisi

RIMA Faz 1'den Caterpillar'a kolay transfer 3-5 sistem listele:
- Sistem adı / RIMA dosya path / Caterpillar'da kullanımı / efort (saat)
- Örnek: CombatEventBus → Caterpillar'da event-driven life-cycle state machine, ~4 saat refactor

### 6. AI pipeline prototip minimum

Pitch A "Wingspan" için 1-haftalık (40 saat) playable prototype:
- PixelLab: minimum asset count + prompt template count
- Codex: minimum script count + system list
- Gemini: minimum prompt pack / research output count
- Unity: scene count + prefab count + ScriptableObject count
- Çıktı: "Tek bir tırtıl, 1 yaprak, 1 kuş, 1 krizalit, 1 kelebek son" prototype minimum

### 7. Cut list — mutlaka olsun vs nice-to-have

Pitch A "Wingspan" için Opus'un 40+ mekanik havuzundan:
- **MUTLAKA OLMALI (MVP):** ~8-10 mekanik
- **NICE TO HAVE (Demo):** ~10-15 mekanik
- **CUT (Full game'e bile gerek yok):** ~10-15 mekanik

Her mekanik için 1-cümle neden.

---

## Format Kuralı

- **Türkçe yaz, teknik terimler İngilizce.**
- Madde işaretli, tablo bol, paragraf 3-4 cümle max.
- Steam linki/kanıtı ekleme — sen tahminci değilsin, pazar bilgisini değerlendir.
- Opus brainstormunda zaten olan şeyleri tekrarlama — derinleştir, eleştir, ekle.
- Codex eninde "Codex'in 1-cümle verdict'i" yaz: hangi pitch + neden.

---

## Output Path

`F:\LaurethStudio\03_IDEAS\Caterpillar\CODEX_ANALYSIS.md`

Yazdıktan sonra `CODEX_DONE_<profil>.md` güncelle, normal cx_dispatch protokolüne uy.

---

## Quick Reference (önemli)

- Opus brainstorm 9 bölüm yazdı, 5 pitch isimleri: **Wingspan / Larva / Instar / Imago / Polychroma**.
- STUDIO_KARAR_006 Form-changer signature → metamorphosis = literal form change.
- LaurethStudio'da Caterpillar şu an `03_IDEAS\Caterpillar\` altında, code-name "Caterpillar".
- Locked LaurethStudio Karar'ları: STUDIO_GUIDE.md'de listeli.
