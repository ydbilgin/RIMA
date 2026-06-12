> **⚠️ v2 — COUNCIL SENTEZLENDİ (2026-06-12). En alttaki "COUNCIL SYNTHESIS" bölümü §2/§3/§8'i GEÇERSİZ KILAR (lean'e çekildi). Aşağıdaki taslak bağlam için duruyor; bağlayıcı olan v2 sentezdir. Sıra: ChatGPT review → Opus reconcile → ÜRETİM.**

# SKILL VFX PRODUCTION SPEC — 2026-06-12 (DRAFT, onay bekliyor)

**Amaç:** Demo'da Warblade + Elementalist skill'lerinin "mantıklı, güzel görünen" VFX'e kavuşması. Şu an çoğu skill **prosedürel placeholder** (beyaz daire/çatlak, `ElementalistRuntimeVisuals.cs` — dosyada bile "TODO: replace" yazıyor).
**Onay zinciri:** bu DRAFT → ChatGPT repo-review → council (cx + Gemini 3.1 Pro + 3.5 Flash) → Opus sentez → ÜRETİM.
**Kapsam (kullanıcı kararı):** 6 opening-kit skill + her iki sınıfın basic attack'ı. Draft havuzundaki kalan ~26 skill = sonraki dalga.

---

## 1. Kapsam — hangi VFX'ler üretilecek
Opening kit (garanti ekranda, `DraftManager.ClassKits`):
- **Warblade:** Iron Charge · Gravity Cleave · Earthsplitter
- **Elementalist:** Fireball · Glacial Spike · Chain Lightning
- **Basic attack ×2:** Warblade melee swing · Elementalist cast bolt

## 2. Yaklaşım — ZATEN KİLİTLİ (DEMO_FINAL_PLAN Lane C2)
- Impact/skill VFX = **Unity ParticleSystem (Shuriken)** + kod/prefab. **PixelLab DEĞİL** ("particle = kendi animasyonlu; PixelLab patlama üretilmez").
- **AÇIK TASARIM SORUSU (council/ChatGPT'ye):** PPU-64 pixel-art bir oyunda default yuvarlak-yumuşak particle'lar NON-pixel görünüp sanat yönüyle çakışır. Çözüm önerisi (onaya): **pixel-snapped chunky particle** — küçük kare particle texture'ları, Point filter, düşük sayı (8-20), `Pixel Perfect`/snap, additive blend glow için. Patlama/şatter gibi "an" momentleri için seçici **flipbook sprite-sheet** (4-6 frame) opsiyonu. Saf default-particle'dan KAÇIN.

## 3. Reusable çekirdek — `SkillVfx` sistemi (modülerliğin HAK ETTİĞİ yer)
> Not: Skill *mantığı* modülerliği hak etmedi (bkz. `MODULAR_ABILITY_DECISION_2026-06-12.md` — 3 hedef ampirik çöktü). Ama VFX hak ediyor: her skill aynı 3-parçalı yapıyı tekrarlıyor → tek reusable sistem gerçek kazanç, tutarlı görsel dil sağlar.

3 arketip (her skill bunların alt-kümesini kullanır):
1. **CAST** — caster'da kısa flash/charge (el parlaması, renk-kodlu). Mevcut `HandGlowVFX.prefab` temel.
2. **TRAVEL** — projectile trail (TrailRenderer + emitter) VEYA melee swing arc (yay sprite/trail).
3. **IMPACT** — hedefte burst (particle patlama + kısa flash). Mevcut `HitSpark.prefab` temel; `PlayerProjectile` impact hook'u VAR.

Önerilen API:
```csharp
public enum VfxElement { Physical, Fire, Frost, Lightning, Void, Arcane }
public enum VfxArchetype { Cast, ImpactBurst, GroundCrack, MeleeArc, ChainBolt, FrostShatter }

public static class SkillVfx {
    // Havuzlanmış (pool) particle spawn; renk taksonomi enum'undan gelir.
    public static void Play(VfxArchetype archetype, VfxElement element, Vector3 pos, Vector2 dir = default);
}
```
- **Renk taksonomisi (KİLİT — DAMAGE_TYPE_TAXONOMY_DECISION):** Physical=ember `#E89020` · Fire `#FF6A1F` · Frost `#7FE0FF` · Lightning `#FFE600` (DÜZELTİLDİ — eski `#FFD24A` Crit ile çakışıyordu) · Void `#7B3FA8` · Arcane/magic cyan `#00FFCC`. Crit `#FFD24A` ayrıdır, VFX'te element rengi olarak KULLANMA.
- Pooling: GC spike yok (demo'da skill spam). Mevcut prefab'lar (HitSpark/DeathBurst/HandGlowVFX/RiftGlowVFX) yeniden kullanılır, sıfırdan değil.
- Sorting: `sortingLayerName="VFX"`, order 20+ (Fireball'daki konvansiyonla tutarlı).

## 4. Per-skill VFX tasarımı

| Skill | Element | CAST | TRAVEL | IMPACT | Notlar |
|---|---|---|---|---|---|
| **Fireball** | Fire | el alev parıltısı | mevcut 8-dir sprite + ateş trail + kıvılcım emitter | patlama burst + yanan ember + kısa flash | 8-dir sprite VAR (`Resources/VFX/Fireball/`); trail+impact eklenir |
| **Glacial Spike** | Frost | el buz şimmer | buz mızrak sprite/şekil + frost trail | şatter (kırılan buz parçaları flipbook) + chill sis | frost mavi, keskin/köşeli okuma |
| **Chain Lightning** | Lightning | el statik kıvılcım | **LineRenderer zikzak** segmentleri (hedefler arası zıplama) | her hedefte spark burst + arc flash | zincir mantığı kodda var; görsel=segment bolt |
| **Iron Charge** | Physical | gövde momentum parıltısı | dash trail (hız çizgileri/motion) | çarpma burst + toz | ileri atılım; trail kritik |
| **Gravity Cleave** | Physical/Void | kılıç ön-parıltı | geniş swing arc (yay trail) | yer çatlağı + içe-çekiş vortex (pull) | pull force VAR; vortex onu görselleştirir |
| **Earthsplitter** | Physical | yer-altı titreşim ön-tell | (instant/line) | yer çatlağı çizgisi + yukarı toz/moloz fışkırma | hat boyunca crack |
| **Warblade basic** | Physical | — | melee swing arc trail (kısa, hızlı) | küçük hit spark | en sık tetiklenen → temiz olmalı |
| **Elementalist basic** | Arcane | minik el flash | küçük magic bolt projectile + ufak trail | mini impact spark | düşük maliyet bolt |

## 5. Kalite çıtası + doğrulama
- **Telegraph okunabilirliği:** düşman skill'leriyle karışmamalı (player VFX = parlak/doygun, taksonomi rengi net).
- **Performans:** pooled, frame başına particle cap, 60fps korunur.
- **Pixel tutarlılığı:** Point filter, pixel-snap; default soft-glob particle yasak.
- **DOĞRULAMA = Play-mode görsel (Unity AÇIK):** VFX world-space → Unity MCP screenshot YAKALAR (ScreenSpaceOverlay UI'nin aksine). Her skill tetiklenip gözle bakılır. **Kör commit YOK** — "visual unverified" notu + sabah/canlı onay.
- Combat doğruluğuna dokunulmaz: VFX additive katman, `DamagePacket`/hasar/hitbox DEĞİŞMEZ.

## 6. Üretim sırası (faz)
1. `SkillVfx` çekirdeği + `VfxElement`/`VfxArchetype` + renk mapping + pooling (mevcut prefab reuse).
2. 6 kit skill wiring (Fireball→Chain Lightning→Glacial Spike, sonra Warblade 3'lü).
3. Basic attack ×2.
4. Play-mode görsel tune (renk/boyut/lifetime) — iteratif, gözle.
5. cx compile gate; commit "visual unverified"; sonra canlı onay.

## 7. Reuse vs yeni (israf önleme)
- **REUSE:** HitSpark, DeathBurst, HandGlowVFX, RiftGlowVFX, DamageZone prefab'ları; PlayerProjectile impact hook; Fireball 8-dir sprite; VFX sorting layer.
- **YENİ:** SkillVfx çekirdeği + pooling; LineRenderer chain-bolt; melee swing arc trail; ground-crack decal; frost-shatter flipbook (gerekirse).

## 8. Açık sorular (ChatGPT + council yanıtlasın)
1. Pixel-snapped chunky particle mi, flipbook sprite-sheet mi, hibrit mi — PPU-64 pixel-art'a en iyi oturan + en hızlı üretilen hangisi?
2. SkillVfx tek static API mı, yoksa SO-tabanlı VfxProfile mi (skill başına asset)? (Modüler ders: SO sadece gerçekten varyasyon varsa.)
3. Chain Lightning LineRenderer vs particle-trail — pixel-art'ta hangisi daha temiz?
4. Melee swing arc: prosedürel mesh/trail mi, yoksa elle çizilmiş arc sprite mi (kullanıcı çizer)?
5. Demo süresi içinde 8 VFX gerçekçi mi, yoksa öncelik sırası (Fireball + 2 Warblade önce) mi?

---

# 🔒 COUNCIL SYNTHESIS — v2 (BAĞLAYICI — yukarıdaki §2/§3/§8'i geçersiz kılar)

**Danışmanlar:** cx (feasibility, kod-temelli) · Gemini 3.1 Pro (deep arch) · Gemini 3.5 Flash (lean). Sentez: Opus. Ham çıktılar: `_process/2026-06/_council_*_vfx-spec.md` + `CODEX_DONE_yekta.md`.

## Anlaşmazlık + Opus kararı
**Mimari (3'lü ayrım):** 3.1 Pro → SO-VfxProfile (data-driven, en yapısal) · cx → static SkillVfx (orta) · Flash → sistemsiz, per-skill SerializeField (en yalın).
**KARAR (cx+Flash ağırlıklı, modüler ders taze):** SO-VfxProfile YOK (overkill — gerçek authored varyant yokken erken soyutlama). Pooling YOK (Flash: 100-200 spawn/demo GC-spike yapmaz; cx: prefab'larda `stopAction:2` pooling'i mayınlı yapıyor). Flash'ın "hiç sistem yok"u da fazla gevşek → **tek tekrar eden şey element→renk eşlemesi + sorting-fix**, onu ~20 satırlık minik static helper'a koy.

## Mimari (LOCK)
```csharp
public enum VfxElement { Physical, Fire, Frost, Lightning, Void, Arcane }
public static class SkillVfx {
    static readonly Dictionary<VfxElement,Color> Palette = { Physical:#E89020, Fire:#FF6A1F,
        Frost:#7FE0FF, Lightning:#FFE600, Void:#7B3FA8, Arcane:#00FFCC };
    // prefab'ı instantiate eder, element rengine tint'ler, sorting'i "VFX"/order≥20'ye ZORLAR
    // (cx red-flag: prefab'lar sorting layer=0 geliyor), stopAction=Destroy ile kendini siler.
    public static void SpawnTinted(GameObject prefab, Vector3 pos, VfxElement element, Vector2 dir = default);
}
```
- Per-skill: `[SerializeField] GameObject castPrefab/travelPrefab/impactPrefab`. Skill kendi Execute() hook'unda `SkillVfx.SpawnTinted(...)` çağırır.
- **Pooling YOK** (Instantiate/Destroy). **SO YOK.** Post-demo profiler spike gösterirse pooling eklenir.

## §8 açık sorular — ÇÖZÜLDÜ
1. **Particle yöntemi:** HİBRİT, chunky particle'a yatık (cx+Flash). Flipbook sadece authored silüet gerektirenlere: frost shatter, ground crack, melee arc. Default soft particle YASAK → texture 4-16px, **Point filter**, max 8-20 particle, alpha-blend ana + additive sadece çekirdek (3.1 Pro whiteout uyarısı). 2D Light flash sadece impact anı 0.1-0.2s.
2. **API:** static `SkillVfx` (cx+Flash). SO-VfxProfile post-demo.
3. **Chain Lightning:** LineRenderer (3 advisor hemfikir; `SpawnArcVisual` ZATEN var). **FIX:** `ChainLightning.cs:79` her arc'ta `new Material` → tek paylaşılan cached Material (cx perf red-flag).
4. **Melee arc:** elle-çizilmiş smear arc sprite (3.1 Pro+cx) = **kullanıcı çizer** ([[feedback_user_draws_weapons_claude_mounts]] mantığı). Gelene kadar geçici prosedürel LineRenderer arc.
5. **Kapsam:** 8 VFX cila riski (cx+Flash). Hedef 8 (6 kit + 2 basic) AMA **öncelik sırasıyla**, demo-zamanı bitince kalan placeholder kalır.

## Üretim öncelik sırası (cx+Flash birleşik)
1. **Fireball** (8-dir sprite hazır → trail + impact patlaması)
2. **Warblade basic** (en sık tetiklenen → slash arc + HitSpark)
3. **Iron Charge** (dash trail + toz + impact, hafif shake)
4. **Glacial Spike** (shatter + chill tint — combo okunaklılığı için şart)
5. **Chain Lightning** (mevcut LineRenderer cleanup + material-cache fix)
6. **Gravity Cleave** (pull vortex — fizik anına bağlı)
7. **Earthsplitter** (ground crack flipbook decal, Floor layer, particle DEĞİL)
8. **Elementalist basic** (mini magic bolt)

## cx red-flag'leri — ÜRETİMDE ZORUNLU FIX
- Prefab sorting layer `0` → `SkillVfx.SpawnTinted` "VFX"/order≥20'ye zorlasın.
- `ChainLightning.cs:79` material-per-arc → cached shared material.
- HandGlowVFX/RiftGlowVFX looping/controller prefab → one-shot pool/spawn yoluna SOKMA; cast-glow için ayrı attach.
- Impact bazları = HitSpark/DeathBurst (gerçek one-shot, order 30). Tint'le.

## Hook noktaları (cx kod-temelli, additive-safe — DamagePacket/hitbox DOKUNULMAZ)
Fireball `Fireball.cs:42-49/64-68`+SetOnHit · Glacial Spike `GlacialSpike.cs:35-40/43-65` · Chain Lightning `ChainLightning.cs:29-39/63-82` · Iron Charge `IronCharge.cs:36-54/61-75` · Gravity Cleave `GravityCleave.cs:30-52` · Earthsplitter `Earthsplitter.cs:26-46`.

## Basic attack BLOCKED → çözüm
cx basic'leri göremedi (dosya verilmemişti). Üretim kapsamına EKLE: `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs · MeleeChainBehavior.cs · CastRhythmBehavior.cs · BasicAttackProfile.cs`.

## Doğrulama (değişmez)
Play-mode görsel (Unity AÇIK, world-space → screenshot yakalar), kör commit YOK, combat doğruluğu (DamagePacket/hitbox) additive katmanla DEĞİŞMEZ. Telegraph ayrımı: player VFX = dışa/burst/parlak/order≥20; enemy = içe/geometrik/fill/order 1-5 (3.1 Pro şekil-dili).

**Durum:** Council ONAY (lean revizyonla). Bekleyen: ChatGPT review → reconcile → ÜRETİM dispatch.

---

# 🔒 v3 — ChatGPT RECONCILE (BAĞLAYICI — üretim öncesi son hali)

ChatGPT review (`STAGING/_inbox`/yüklenen paket): **KOŞULLU ONAY**, 3 şart. Reconcile + Opus kararı:

## Şart 1 — Lightning rengi → `#FFE600`
✅ ZATEN uygulandı (v2, §3 + Palette). Crit `#FFD24A` element rengi olarak kullanılmaz. KAPANDI.

## Şart 2 — "static façade + VfxProfile registry" (TEK gerçek çatışma)
**Skor:** SO/profile isteyen = ChatGPT + 3.1 Pro (2). İstemeyen = cx + Flash + v2-Opus (3).
**Opus kararı: SO-registry HAYIR (demo), ama ChatGPT'nin asıl derdini v2 ZATEN karşılıyor — açık yazıyorum:**
- ChatGPT'nin korkusu = "config skill script'lerine hardcode magic-number olarak dağılır". v2'de **dağılmıyor**: görsel tunable'lar (lifetime/scale/particle-count/sorting) **PREFAB üzerinde** yaşıyor (inspector'dan ayarlanır, designer-friendly) — kodda değil. Renk = **kilitli taksonomi Palette** (runtime değişmez, kodda olması DOĞRU).
- 3.1 Pro'nun "Frost köşeli, Fire yuvarlak (şekil farkı)" noktası = particle-tint ile çözülmez. **Ama v3 bunu zaten ayırıyor:** şekil-kritik efektler (frost shatter, ground crack, melee arc) = **authored flipbook/sprite** (tint değil). Particle burst'ler = runtime-tint (ucuz, sadece renk). Yani şekil farkı authored asset'te, renk farkı Palette'te → registry'siz çözülür.
- **Sonuç:** v2'nin per-skill `[SerializeField] prefab` + static `SkillVfx.SpawnTinted` helper'ı KALIR. VfxProfile SO-registry = **post-demo upgrade path** (skill sayısı 8→30+ olunca veya per-archetype default config paylaşımı gerçekten gerekince). Modüler ders: soyutlamayı ölçek hak edince ekle.

## Şart 3 — Sert pixel-art kuralları (ChatGPT checklist ADOPTE)
Üretimde ZORUNLU (spec'e eklendi):
- Particle texture = chunky kare/elmas shard, **default soft circle YASAK**.
- Import: **Point filter, compression yok, mipmap yok** (pixel texture).
- Particle count: burst 8-20, major cast/impact max 24. Lifetime: burst/spark 0.08-0.35s.
- **`new Material(...)` cast/hit loop içinde YASAK** (cx red-flag: ChainLightning.cs:79 → cached shared material).
- Tüm VFX `VFX` sorting layer + explicit order≥20 (helper zorlar; prefab'lar 0 geliyor).
- **PPU 64** konvansiyonu; `ElementalistRuntimeVisuals` PPU-32 placeholder'ları SADECE geçici.
- Additive sadece çekirdek particle; ana alpha-blend (whiteout önlemi).

## ChatGPT eksik-listesi (ADOPTE — spec'e eklendi)
- **Ses-sync hook:** her VFX çağrısına opsiyonel `sfxId` kancası (audio asset placeholder olsa bile yapı hazır). Üretimde stub.
- **Screen-shake / hit-stop koordinasyonu:** VFX bunları TETİKLEMEZ, mevcut sistem varsa onunla senkron çağrılır (Iron Charge/Earthsplitter impact'inde hafif shake — sistem varsa, onaylıysa). Combat timing DEĞİŞMEZ.
- **Material reuse kuralı:** archetype başına tek paylaşılan material; per-cast allocation yok.
- **8-yön tutarlılığı:** projectile + melee arc 8-yön sprite/flip mantığıyla (Fireball konvansiyonu) hizalı.
- **Telegraph ayrımı:** (v2'de var) player = dışa/burst/parlak/order≥20; enemy = içe/geometrik/fill/order1-5.
- **VFX QA recipe:** boş oda + combat oda; her skill; basic ×20 spam (GC/material spike testi); demo zoom; karanlık zemin; yakında enemy VFX (telegraph ayrımı kontrol). Play-mode görsel capture, kör commit YOK.

## Üretim öncelik (council + ChatGPT MERGE)
**Tier 1 (önce, sınıf-hissini tanımlar):** Fireball · Warblade basic · Gravity Cleave.
**Tier 2:** Chain Lightning · Iron Charge · Earthsplitter.
**Tier 3:** Glacial Spike (combo okunaklılığı notu) · Elementalist basic bolt.
Kural: 8 bespoke mini-sinema DEĞİL → 5 reusable archetype (CastFlash/ImpactBurst/ProjectileTrail/MeleeArc/GroundCrack+ChainBolt) kur, skill başına tune.

## İKİ KAPI DA GEÇTİ
Council = ONAY (lean). ChatGPT = KOŞULLU ONAY → 3 şart reconcile edildi (1 zaten yapıldı, 2 v2 zaten karşılıyor + post-demo path, 3 adopte). **Spec ÜRETİME HAZIR.** Sonraki adım: Faz 1 (SkillVfx çekirdeği + 5 archetype) → Tier 1 wiring → Play-mode QA. Melee arc sprite'ı = kullanıcı çizer (gelene kadar geçici prosedürel).
