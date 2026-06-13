# Inversion Mekaniği — RIMA + laurethstudio Değerlendirme Kararı (2026-06-12)

**Kaynak:** Inversion Day (X: @InvDayGame/2064920558454874131 · Steam app 4436240). Beat Saber yapımcıları, 2026 indie.
**Mekanik (öz):** Oyuncu parlayan blok koyarak uzayın **KATI↔BOŞ ikili durumunu tersine çevirir** (boş→yürünebilir platform, duvar→geçilebilir boşluk). 2D side-scroll puzzle-platformer, combat YOK. Viral olan: dinamik neon piksel-ışıklandırma. Akraba: VVVVVV, Antichamber, Patrick's Parabox, Baba Is You.
**Soru:** Mekaniğin özü (sadece platformer/puzzle olarak değil) RIMA (top-down ARPG roguelite) + laurethstudio için kullanılabilir mi?

## Council (Gemini 3.1 Pro + Gemini 3.5 Flash + cx feasibility)

### Oybirliği
- **RIMA demo'suna SOKMA.** Shiny-object/scope-creep tuzağı; combat odağını dağıtır. Demo çekirdek combat loop'u kanıtlamalı.
- **Tür çatışması:** top-down hızlı ARPG *flow* (kas hafızası) vs deliberate uzay-toggle puzzle → harita okunurluğu/flow kırılır (Flash: "Frankenstein").
- **Evrensel sistem (tüm odalar invertible) = KESİN HAYIR:** level design + pathfinding/navmesh + roguelite denge kırılır.

### Mekaniğin özü transplant edilebilir mi? (3.1 Pro — gerçek ölçüm)
Evet, combat'a çevrilebilir: anlık cover yaratma · düşman altında zemini uçuruma çevirme · flank için duvar açma · horde izolasyonu. Fikir güçlü AMA RIMA'nın akışını bozma riski yüksek.

### Doğru ölçek (3.1 Pro)
- 🥇 **Boss/Echo bespoke** = en doğru. "Rift-Architect" boss; arena ona özel; faz geçişinde arenayı tersler; oyuncu adapte olur, mekaniği kullanmaz. Sistemleri bozmaz, spectacle verir.
- Signature skill ("Riftshaper" AoE inversion field) = mümkün ama AI maliyeti yüksek/riskli.
- Tematik uyum **mükemmel:** Shattered Keep / void / rift → "gerçekliği katlamak" lore'a oturuyor.

### laurethstudio açısı
Mekaniğin tam potansiyeli (tüm-seviye inversion + meta-puzzle) = **kendi başına OYUN pillar'ı** (Baba Is You / Antichamber soyu), RIMA feature'ı DEĞİL. RIMA'ya core sistem yedirmek mekaniği harcar + RIMA'yı sulandırır.

## KARAR (Opus sentez)
1. **RIMA demo: YOK.** Park et.
2. **RIMA post-demo:** SADECE bespoke Act-boss / Void-Echo gösterisi olarak (3.1 Pro opsiyon c). Asla core sistem / asla evrensel invertible.
3. **laurethstudio:** Bu **ayrı bir aksiyon-puzzle IP'sinin tohumu**. Şimdi mühendislik değil — istenirse 1 sayfalık konsept park (RIMA scope'una sokmadan).
4. **Fırsat maliyeti (Flash):** aynı efor → combat cilası + game-feel + asset pipeline + stabil build. Demo'yu ayakta tutan bu.

## cx feasibility (teknik) — GELDİ, kararı doğruluyor

**Greenfield DEĞİL — güçlü reuse temeli var:** `RoomTemplateSO.walkableGrid` + `WalkabilityMap` + `IsoRoomBuilder` boundary collision + validator pattern'leri (`BridsonPoissonAutoPlacer`, `PropFootprintValidator`, `CompositionRoleMapGenerator`, `RoomTemplateValidator` flood-fill).

**Eksik parça = runtime terrain mutation authority.** Şu an hiçbir API runtime'da `walkableGrid`'i mutate edip dirty-cell rebuild yapmıyor. Tek başına tilemap değişimi → walkability ile desync; tek başına walkability → görsel/collision ile desync. Gerekli: `RuntimeTerrainState`/`InversionTerrainController` (mutable per-room kopya + dirty rebuild + actor eject + per-cast flood-fill).

**Enemy AI pathfinding DEĞİL** (`BaseMobBehavior`: doğrudan chase + `WalkabilityMap.ClampVelocityToWalkable`). Yeni duvar → düşman etrafından dolaşmaz, dayanır/kayar. Zemin altından kaybolursa düşме/ölüm handling YOK (eklenmeli). Door sistemi authored north-exit varsayıyor; runtime keyfi duvar açımına uygun değil.

**Risk kanıtı:** `RoomRunDirector.cs:322-327` Combat/Elite'te template prop'larını zaten DEVRE DIŞI bırakıyor çünkü blocking prop cluster'ları spawn/cliff yanında soft-lock yapıyordu → runtime topology değişimi tightly-guarded olmadan yüksek risk (canlı kanıt).

**cx maliyet ayrımı:**
- Constrained SKILL (2-3 tile radius, geçici, validator-gated, unsafe cast reddi, oda temizlenince restore) = **orta maliyet**, feasible.
- Whole-room invertible = **yüksek maliyet** — room-system feature, generation+combat+AI+encounter+validation hepsine dokunur. **Yapma.**

**cx + 3.1 Pro sentez incelik:** En ucuz VE en güvenli = **boss'un terrain'i SCRIPT'le flip ettiği** bespoke arena (oyuncu free-form cast etmez). Böylece per-cast flood-fill/actor-eject edge-case'leri ortadan kalkar — flip'ler authored/deterministik. Yani 3.1 Pro'nun "Boss/Echo" önerisi hem tasarım-en-iyi hem feasibility-en-ucuz, ÇÜNKÜ oyuncu-skill'i değil scripted-boss-flip.
