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
- **Renk taksonomisi (KİLİT — DAMAGE_TYPE_TAXONOMY_DECISION):** Physical=ember `#E89020` · Fire `#FF6A1F` · Frost `#7FE0FF` · Lightning `#FFD24A` · Void `#7B3FA8` · Arcane/magic cyan `#00FFCC`.
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
