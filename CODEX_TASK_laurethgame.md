ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.

# Codex Task: BanditKnightG Stealth Indie — Vizyon Analizi (RIMA'ya Aktarım)

**Sorumlu:** Codex (gpt-5.5, high effort)
**Çıktı:** `STAGING/banditknightg_vision_analysis_codex.md`
**Süre tahmini:** 25-40 dk

## Bağlam

Kullanıcı şu twitter postunu inceledi: https://x.com/BanditKnightG/status/2055256885637374172
Tweet: "THIS IS WHAT AN INDIE GAME LOOKS LIKE" — yüksek polish 2.5D pixel/HD hybrid stealth thief action RPG.

Önceki Codex twitter research (`STAGING/twitter_research/2055256885637374172/notes.md`) **yüzeysel** — RIMA katki paragrafı 3 cümle, vizyon perspektifi yok. Bu görev mevcut notu **derinleştirip RIMA'ya somut karar adayları çıkarır**.

## Görsel materyal

- `STAGING/twitter_research/2055256885637374172/contact_sheet.jpg` — 3-frame özet
- `STAGING/twitter_research/2055256885637374172/frames/frame_001.png` — iç mekân taverna/saloon, "119 SEVER" damage, yellow keyword, white/blue sparkle VFX, gold counter 77,666g, HP/MP bars top-left, skill hotbar bottom-left
- `STAGING/twitter_research/2055256885637374172/frames/frame_002.png` — dış mekân orman/glade, merkez hero prop (büyük ağaç) + mid props (kayalar/çiçek), 2 karakter (player + ally/mob)
- `STAGING/twitter_research/2055256885637374172/frames/frame_003.png` — iç mekân kale/noble house, kırmızı perde + mumlar + mor lavabo, damage "106" gold 76,701g, mob sağda
- `STAGING/twitter_research/2055256885637374172/twitter/BanditKnightG/2055256885637374172_1.mp4` — orijinal video

## Analiz isterleri (per frame + bütün)

Her frame için:
1. **Kompozisyon kuralı** — hero prop yerleşimi, mid prop dağılımı, walkable path/clutter zone ayrımı, edge-biased density tespit
2. **Palet ve atmosfer** — accent renkler, mood light source sayısı/yeri, karakter vs background saturation kontrastı
3. **UI yerleşimi** — HP/MP, gold/loot, skill hotbar, damage feedback konumu ve oran
4. **Damage number stili** — sarı bold + "SEVER" keyword, font size hiyerarşisi, crit vs trash differentiation
5. **VFX overlay** — sparkle/parlama tipi (spark / star / radial burst), opacity, duration, "crit emphasis" timing
6. **Karakter silüet** — rim light/outline, saturation, environment'tan ayrışma teknigi
7. **Prop density** — "hero + N mid + M accent" formülü çıkar
8. **Animation read** — videodan görülen attack timing, telegraph, hit pause feedback

Bütün için:
- **Stilin tek-cümle özeti** ("ne yapıyor, neden okunabilir")
- **RIMA'ya transferable 5-8 somut kural** (her biri: kural / nereye / bedel)
- **RIMA'ya REJECT 3-5 unsur** (neden uymaz)

## RIMA mevcut karar haritası (bağlam)

- **Karar #80 Silhouette Bible** — 10 class siluet kanonik
- **Karar #100 Chibi 64x64 + ~35° camera** — Hades-match top-down
- **Karar #100b Wide Arena FOV (ÖNERİ)** — 640x360 ref res, 35° korunur
- **Karar #118 Hybrid Tile Composition 4-layer** — NLM canon tile pipeline
- **Karar #135 Procedural+Paint Hybrid** — Codex generator + Map Designer paint + 5 organic layer
- **Karar #137 VFX Router** — CombatEventBus + tag-based prefab routing + ProcLimiter
- **Karar #138 2-layer draw order** — body+head birleşik, weapon ayrı, north weapon -1
- **Karar #143 (ADAY)** — 6-Layer Map Architecture (L1 floor + L2 var + L3 wall overlay + L4 transition + L5 detail + L6 accent)
- **Karar #75 (REVISION ADAYI)** — create_map_object kullanım kuralı

Her karar adayında "şu mevcut karara bağlanıyor / şunu genişletiyor / yeni karar # gerekir mi" diye işaret koy.

## Çıktı şablonu

```markdown
# BanditKnightG Vizyon Analizi — RIMA Transfer

## Tek-cümle stil özeti

[X yapıyor, çünkü Y]

## Frame 1 Analiz
**Kompozisyon:** ...
**Palet:** ...
**UI:** ...
**Damage:** ...
**VFX:** ...
**Silüet:** ...
**Prop density:** ...
**Anim read:** ...

## Frame 2 Analiz
[aynı format]

## Frame 3 Analiz
[aynı format]

## RIMA Transfer Kuralları (BORROW)

### TR-1: [kural başlığı]
- **Ne:** ...
- **Nereye uygular:** Karar #X'e bağlanır / yeni karar adayı
- **Bedel:** ... (kod / asset / tasarım eforu)
- **Risk:** ...

### TR-2 ... TR-8

## REJECT (RIMA için uymayan)

### RJ-1: [unsur]
- **Neden uymaz:** ...
- **Yerine:** ... (RIMA'nın mevcut çözümü)

## Karar Adayı Önerileri (önemli sıra ile)

1. **Karar #X-aday — [başlık]** — TR-N referansı, Faz 1 / 1.5 / 2 öneri, kullanıcı LOCK için 1-paragraph rationale
2. ...

## Codex'in QC önerisi

Bu vizyon kurallarından hangileri **3 günlük production'da (deadline 2026-05-18)** prototip yapılabilir, hangileri **Faz 1.5'a defer** edilmeli? Karar # ile listele.
```

## Kısıtlar

- Bütün frame'leri Read tool ile aç, görsel kanıt al
- Mevcut `notes.md`'yi tekrarlamak yerine **derinleştir** (Codex önceki Sonnet notunu bilir)
- Türkçe yaz, teknik terimler İngilizce kalsın
- Code yazma, sadece analiz + karar adayı önerisi
- contact_sheet.jpg + 3 frame.png + video MP4 erişilebilir; MP4 timeline incelemesi opsiyonel (frame'lerden çıkarsam yeter)
- **Önceki notes.md'yi de cite et** ama tekrarlama


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.