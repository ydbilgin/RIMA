# RIMA — Execution Workflows + Map/Gate Design (S114 S6, 2026-05-29)

> **Karar:** Opus (kullanıcı "tamam kararı verebilirsin" yetkisi). **Liste canon:** `STRATEGIC_SYNTHESIS_S6.md §8`.
> **Routing (her workflow'a gömülü):** Writer = **Opus** (mekanik bulk = Sonnet) · Reviewer + fikir = **Codex (cx_dispatch yekta)** + **agy (agy_detached.ps1 wrapper, flash-free)** · canon = **NLM** (30ddffa5) · yapmadan-önce-kararlaştır · agy hem sentez hem ham veri.

---

## 1. ★ HARİTA PARÇASI / MAP / GATE — RIMA'ya özgün tasarım

**Canon (NLM, Karar #63):** Oda temizlenince zemine **Map Fragment** düşer (Hades Ambrosia gibi — almak ZORUNLU). Toplayınca 1s ritüel + **mürekkep reveal** → önündeki 1-2 odanın TİPİ görünür (içeriği değil). Kısmi görünür harita (StS-vari plan). Parça alınmadan geçilemez. Lore: harita = "parçalanmış dünyanın görsel anlatısı"; toparlamak = hatırlamak.

### Demo akışı (tek canonical sıra — çelişkiyi bitirir)
```
Oda temizlendi (mob=0)
   → SKILL DRAFT (3 kart, GÜÇ ödülü)  ── oyuncu seçer
   → MAP FRAGMENT zemine materyalize olur (gate önü)
   → oyuncu üstüne gider → 1s RİTÜEL pickup
   → MAP PANEL'e parça oturur + cyan mürekkep reveal (SONRAKİ oda tipi)
   → GATE (rift-seam) açılır → sonraki oda
```
Draft = güç ödülü, Fragment = ilerleme/harita ödülü. İkisi ardışık, tek sıra. (Eski "combat odası direkt unlock" + "reward odası fragment" ÇELİŞKİSİ kapanır — HER oda bu akıştan geçer; boss odası hariç.)

### 3 RIMA-özgün dokunuş
1. **Gate = mühürlü rift, jenerik kapı DEĞİL.** Kilitli: çatlak taş üstünde sönük cyan damar. Fragment yerleşince "mühür deseni tamamlanır" → cyan rift yarığı açılır + pulse. Lore bağı: kapılar = mühürlenmiş rift'ler, fragment = anahtar-şart. (The Fracturing temasıyla doğrudan.)
2. **Map Panel = "Ashen Glyph" parşömen** (köşe, context-sensitive). Toplanan her parça = koyu çatlak-taş üstüne cyan mürekkep çizilen bir oda-düğümü. Mevcut oda ışıklı, sonraki 1-2 tip ikon+renk (combat=gri / elite=kırmızı-rift / reward=altın / boss=büyük kırmızı), ötesi karanlık/çatlak. Demo lineer olsa bile "sıradaki Elite/Boss" reveal'ı StS-gerilimi yaratır.
3. **Reveal MEKANİĞE bağlı (sadece dekor değil):** sıradaki Elite/Boss açılınca hasar-tipi ipucu belirir ("Sovereign — Rift damage ahead") → oyuncu draft/altar'ı ona göre seçer. Canon'daki "build planlaması yapabilirsin" gerçekleşir → harita ANLAMLI olur.

### Kod durumu (Explore+Codex) → yapılacak
- VAR: `RoomLoader` runtime Gate (prosedürel placeholder sprite), `MapFragmentSpawner` (sadece reward odası), fragment pickup→gate unlock (map-preview UI YOK, draft reward-path'te YOK).
- YAP: (a) akışı her odaya birle (clear→draft→fragment→reveal→gate), (b) **Map Panel UI** (yeni, Ashen Glyph), (c) gate'i rift-seam görseline bağla, (d) reveal'a hasar-tipi ipucu. → W1'in parçası.

---

## 2. ★ WORKFLOW SIRASI (sequenced, detaylı promptlar)

**Mekanizma seçimi:** Tek-feature işler = **lineer pipeline** (Opus yazar → cx+agy review → Opus verify). Gerçek fan-out (çok-dosya tarama, tasarım-opsiyon paneli) = **Workflow TOOL** (Claude writer agent'ları paralel). Her Wn altında işaretli.

### Sıra: W1 → (W2 ∥ W3) → W4🔒 → W5 → W6 → W7 → W8 → W9 → W10🔒 → W11(araya)

---
### W1 — Çekirdek loop dikey dilim (T0.0+T0.1 + map/fragment/gate)  [lineer + Unity]
**Hedef:** clear→draft→fragment→map-reveal→gate, 5 oda E2E oynanır, gerçek ikonlu draft.
**Prompt (Opus writer'a / kendime):**
> Writer=Opus, Unity MCP. Önce cx'in UNCOMMITTED T0.1 kodunu compile-verify et (`read_console` 0-error): SkillIconRegistry, SkillDatabase.Add icon, RoomLoader.UnlockGateAfterDraft, DraftDrivenByRoomLoader. Sonra: (1) **Bootstrap dep pass** — `SkillDatabase` playable scene'de YOK (Codex blocker), `DraftManager.EnsureDependencies` onu auto-create ETMİYOR → SkillDatabase'i deterministik auto-create et (ya da scene'e ekle). (2) **Tek draft-driver** — RoomLoader/RuntimeRoomManager/RewardPickup/MapFragmentBridge/HandleRoomCleared rakip çağrılarını TEK canonical driver'a indir (RoomLoader.UnlockGateAfterDraft), diğerlerini guard/disable. (3) **Map/fragment/gate akışı** §1: her oda clear→draft→fragment→reveal→gate. (4) **Map Panel UI** (yeni, Ashen Glyph, köşe, parça-reveal next-room-type+danger-hint). (5) **Gate=rift-seam** görsel. AssignActive için skill component'leri player'da garanti et. Min code, surgical. Belirsizse BLOCKED.
> **Review:** cx (kod: driver tekliği, scene-dep, compile, regression) + agy (his: akış oyunu yavaşlatıyor mu, reveal anlamlı mı). **Verify:** Unity play, Room1→5, screenshot.

### W2 — Audio skeleton (T0.2)  [lineer, Sonnet writer]
> Writer=Sonnet. `AudioManager` (singleton, SFX bus + music bus, volume). Combat hook'ları: hit (thwack), zırhlı-hit (shatter/clink), dash, skill-cast, draft-card-select, gate-open, death. Placeholder SFX (royalty-free veya basit prosedürel). Mixer + global volume. Min code. **Review:** cx (event-hook doğru yerlerde mi, leak) + agy (his katmanı yeterli mi). Verify: play, sesler tetikleniyor.

### W3 — Death/Victory + Wishlist CTA (T0.3, PROMOTED)  [lineer + frontend/imagegen]
> Writer=Opus. Death+Victory ekranı: run-stat (süre/kill/aktif build adı "Rift Cleaver Warblade") + **Wishlist CTA** (`steam://openurl/...` overlay, +%30-40 dönüşüm) + **Share-Build** seed clipboard (`RIMA-WB-SWIFT-3x`) + sonraki-sınıf teaser silüeti. **ÖNCE frontend mockup:** `impeccable` ile layout/hiyerarşi taslağı (HTML→UI Toolkit USS). **Görsel:** `$imagegen` (Codex skill, OpenAI key DEĞİL) ile death-bg + teaser silüet uygun boyutta (bg 640×360, silüet 256px). Unity'de kur. **Review:** cx + agy (CTA dönüşüm açısından).

### W4 — İlk-60sn juice + A5 playtest (T0.4)  [lineer + 🔒gate]
> Writer=Opus. Açılışta direkt combat (3-4 FractureImp ortasında, tutorial yok). Hit-stop 0.06s + cyan slash-arc + shake ilk vuruşta. Sonra **A5 = KULLANICI combat-feel playtest (F5)** — gated. Review: agy (feel).

### W5 — 2 silah/stance (T1.1, retention)  [lineer]
> Writer=Opus. Run başı 2 seçim: *Sovereign's Will* (sweep, cyan) / *Rift Carver* (thrust+ileri-atılma, mor). Aynı sprite, farklı attack-code + slash VFX renk + başlangıç pasifi. Zero yeni sprite. Review: cx (kod-mimari) + agy (varyasyon yeterli mi).

### W6 — Tag display + combo pop-up (T1.2)  [lineer — sadece DISPLAY]
> Writer=Opus. Heavy/Rift/Bleed/Swift tag'lerini draft kartında göster + tetiklenince ekran combo pop-up ("Rift Burst x5!"). **DİKKAT (Codex): gerçek multiplicative synergy hidden-expensive (enum yok, effect-hook yok) → DEFER.** Sadece görsel/screenshot value. Review: cx (enum ekleme min mi) + agy.

### W7 — Rift Altar (T1.3, ekonomi yerine)  [lineer]
> Writer=Opus. Reward odası: tek UI Altar. Seçenek A "%30 HP feda → skill reroll", B "en güçlü skill feda → full heal". Currency YOK. Reroll state SkillOfferUI'da (Codex: yeni UI action gerek). Review: cx + agy (risk-reward his).

### W8 — Corrupted elite (T1.4)  [lineer]
> Writer=Opus. Mevcut mob recolor + cyan aura + 1 saldırı tweak. **Codex caveat: EliteAffix speed/Shielded multiplier'ları runtime'da OKUNMUYOR + Shielded integer-bug → 1 affix'i DOĞRU wire et.** Review: cx (affix wiring) + agy.

### W9 — Narrative ucuz (T2.1)  [lineer + imagegen ops]
> Writer=Opus. Run-içi diyalog (cyan kutu + daktilo; boss odası + 3. odada Sovereign fısıltısı "Zincirlerim adımlarınla gevşiyor..."), boss intro (yarıktan yükseliş + gotik font "The Rift Keeper"), 3 etkileşimli rune-lore (The Fracturing/corruption). Review: agy (ton: Vivid Vulnerability) + cx.

### W10 — Görsel/PixelLab (T3)  [🔒gated — kullanıcı]
> cliff/depth+backdrop · boss sprite (PenitentSovereign). PixelLab MCP gen kullanıcı onayı gerektirir. $imagegen ile ref-art üretilebilir (gated değil) → PixelLab init.

### W11 — Temizlik (T4)  [Workflow TOOL — fan-out]
> Çok-dosya: SkillDraftSystem stub sil (ActiveSkillData→SkillSlot bağı, Codex) · _IsoGame test triage (ELEVATED) · CameraFollow seçim-debt · /lint + MEMORY.md slim + CURRENT_STATUS sadeleştir. Workflow tool ile paralel-dosya tarama → cx+agy review.

---

## 3. BAŞKA NELER YAPILABİLİR (ek fikirler — Opus + agy)
- **Seed sharing** (streamer funnel: wishlist'in %70'i streamer): run seed göster/paylaş, "aynı run" tekrar.
- **Combo GIF/screenshot capture** (Balatro screenshot value) — combo anında otomatik yakalama.
- **Controller desteği** (action oyunu; Input System ile ucuz) — playtest erişimi artar.
- **Onboarding tutorial-less** (combat içine düş, oynayarak öğren — agy).
- **Boss telegraph polish** (Sovereign 3 saldırı net tell — canon'da var, görselleştir).
- **Erişilebilirlik:** shake toggle, cyan/kırmızı renk-körü ayrımı kontrolü.
- **Steam sayfa hazırlığı** (capsule art $imagegen, wishlist setup) — demo'nun amacı.
- **Demo .exe build target + Steam upload** (oyun-dışı ama demo'yu canlıya alır).

## 4. FRONTEND DESIGN + $imagegen KULLANIMI
- **`impeccable`** (frontend skill, web/HTML): layout/hiyerarşi mockup → Unity UI Toolkit (USS≈CSS). Hedefler: Map Panel, Death/Victory, Rift Altar, draft kartı. SADECE tasarım dili, final art değil.
- **`$imagegen`** (Codex skill, OpenAI key DEĞİL): uygun boyutta üret → Unity import. Hedefler: fractured-map parşömen frame (9-slice), death-bg (640×360), next-class teaser silüet (256px), map oda-tipi ikonları (combat/elite/reward/boss 48-64px), Rift Altar art, cyan rift-seam gate sprite. PPU tipe göre.
- **Akış:** design/mockup (impeccable) → asset (\$imagegen / PixelLab) → Unity import → wire. "Bunlar olmadan önce design'la üret, oradan al."

---
**İLK HAMLE:** W1 — cx'in uncommitted T0.1 kodunu compile-verify + bootstrap-dep + tek-driver + map/fragment/gate + Map Panel. Opus yazar, cx+agy review, Unity verify.
