# Fable Playtest Notları — 2026-06-13 (gece)

**Model:** Fable 5 (orchestrator). **Yöntem:** Unity MCP play-mode, gerçek oyun akışı (API-validation DEĞİL: sınıf seç → skill ata → cast → AI → prop → ölüm akışı). Screenshot'lar bu klasörde.

## Senaryo sonuçları

| # | Senaryo | Sonuç | Kanıt |
|---|---|---|---|
| 1 | Elementalist'e canlı geçiş + Fireball slot-0 atama | ✅ | `AssignSkill=True`, oyuncu sprite'ı değişti (final_scene) |
| 2 | Fireball GERÇEK cast (`SkillBase.TryActivate` — mana/cooldown kapılı) | ✅ | `TryActivate=True`, 1 canlı `PlayerProjectile` spawn |
| 3 | Mermi uçuşu | 🟡 ıskaladı | aim input'a bağlı (idle facing yönüne gitti); gerçek demoda mouse-aim var. Vuruş yolu ayrıca packetized DealDamage ile kanıtlı (HP 60→0, telemetry +2, DPS 600) |
| 4 | Düşman AI (Director-spawn) | ✅ **kovalıyor** | FractureImp 8.00→6.74→melee menzili; AI=`BaseMobBehavior`+`MobAttack_Melee` |
| 5 | Ölüm akışı | ✅ var | `DeathScreenCanvas_Auto` + `PlayerController` disable (input kilidi) |
| 6 | Prop placement play-mode (1.8× rift_crystal) | ✅ | Resources.Load palette yolu (cx fix'i play-mode'da da doğrular), cyan glow belirgin (final_scene) |
| 7 | Console hijyeni | ✅ | tüm oturum 0 error 0 warning |

## 🚩 Bulgular (demo provası için)

1. **Ölüm GERİ DÖNÜŞSÜZ:** `Heal()` ölü oyuncuda no-op (IsDead guard). Respawn yok → **sunumcu ölürse tek çare scene/run restart. Provada ezberlenecek.** Ayrıca death screen ScreenSpaceOverlay → MCP screenshot'ta görünmez (oyuncu dakikalarca ölü kaldı, otomasyon fark etmedi — canlı demoda insan gözü görür, sorun değil).
2. **Ölüyken Director class-switch çalışıyor:** `Elementalist_SkillController` ölü oyuncuda enable oldu (IsDead check yok). Input kilidi duruyor ama state kirli. Minor — istenirse cx'e tek satır guard.
3. **Düşman HP bar'ı full canda KIRMIZI** (`TierHPBar/Fill` 0.85,0.15,0.15): screenshot'larda "kırmızı kare" gibi okunuyor. Kozmetik; demo screenshot'larında kafa karıştırabilir.
4. **VFX tint'ler AYRIŞIYOR** (flag'in korkusu doğrulanmadı): Fire=kırmızı-turuncu, Lightning=sarı, Frost=cyan net; **Void koyu zeminde çok zayıf** → palette'te Void'i lighten (minor cx). Kanıt: `vfx_hitspark_simulated.png`. Not: HitSpark=ParticleSystem → otomasyon yakalaması için `ps.Simulate()` tekniği gerekti.
5. **🔴 VFX build-safety:** `HitSpark`/`DeathBurst` prefab'ları `Assets/Prefabs/VFX/`'te, Resources'ta DEĞİL → standalone build'de ImpactBurst/death VFX görünmez (rift_crystal'la aynı sınıf bug, editor fallback maskeliyor). Task #16, cx'e gidecek. Demo editor-canlı → yarın blocker değil.

## Önceki doğrulamalarla birleşik tablo
- Calc pipeline: 3 tanık (Opus orchestrator + ax Gemini + ax Opus 4.6) birebir → rima-qc PASS
- Gerçek vuruş: packetized hit, physPower 50→250 ⇒ HP düşüşü 10→50 (5×), telemetry +2/DPS 600, düşman öldü
- Bu playtest: oyun akışı + AI + ölüm + prop + VFX görsel
