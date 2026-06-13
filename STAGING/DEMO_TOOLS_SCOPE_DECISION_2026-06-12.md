# Demo Tool Kapsam Kararı — Sunum Demosu (2026-06-12)

**Bağlam:** Demo = hocaya CANLI SUNUM (Steam/oyuncu DEĞİL), capstone, not=SİSTEMLER üzerinden. Öğrenci iki şey gösterecek: (1) dual-class oynanabilirlik (10-dk slice), (2) ALTYAPI — Unity Editor'süz oyun-içi runtime tool'larla canlı stat/spawn/ayar = mühendislik derinliği kanıtı.
**Soru:** Tool işlevsel olarak nasıl olmalı, ne gerekli/yeterli, run anim yeterli mi, hangi tool'lar olmalı?

## Council (Gemini 3.1 Pro + Gemini 3.5 Flash + cx feasibility)

### Oybirliği
- **#1 altyapı flex = CANLI STAT TUNING** (+ class-switch + live spawn). Editörsüz runtime stat değişimi → event-driven / decoupled / data-driven mimari kanıtı. Öğrencinin sezgisi DOĞRU. Akademik "vay bu ciddi mühendislik" anı bu.
- **KES (gimmick + crash riski):** Manuel prop yerleştirme (rift crystal elle koyma), Build/Tile/Cliff, Map nav. Gerekçe: "Unity'nin editörü zaten var, runtime'da kötü UI ile reinvent" hissi → akademik değer ~0 + sunum bombası. Sekmeleri gizle/stub.
- **RUN anim ×2 = ŞART.** Idle-slide "buzda kayma" = "bitmemiş prototip" sinyali, akademik sunumda bile affedilmez. Zaman yoksa tek generic run paylaş ama kaydırma.
- **Telemetry/CSV** = bonus akademik güvenilirlik ("denge için runtime veri pipeline").

### Tool UX (3.1 Pro)
Sunucu-güdümlü (kör kullanıcı değil): BÜYÜK font, renk-kod (buff yeşil/nerf kırmızı), devasa slider/+- buton, **demo-koreografisi** (menüde gezinme yok, önceden belirli akış). "Bu tool hangi gerçek geliştirme problemini çözüyor?" → "tasarımcı editör açıp kapatmasın diye yazdım."

### Sunum-günü güvenliği (KİLİT)
- Serbest InputField YOK → **clamp'li slider** (backend clamp = robustness kanıtı)
- **Spawn cap ~10** (FPS koruması)
- **Scripted/prova akış** — dokunulacak stat'lar + spawn'lar önceden belirli, doğaçlama YOK

### Öncelik (3.1 Pro risk-first vs Flash perception-first → Opus uzlaştırdı)
Çelişki yok: kilitli `DEMO_FINAL_PLAN` zaten paralel lane. Dual-class = KOD kritik yolu (Lane A), run-anim = PixelLab async (Lane B), birbirini beklemez.
```
1. Dual-class oynanabilirlik   → kritik yol, çökmemeli
2. RUN anim ×2                 → en yüksek algı sıçraması (paralel)
3. Director: Stat + Spawn + Class-switch → altyapı şovu (sadece sunulacak kısmı cila)
4. KES: Build / Map / manuel prop
```

## KARAR (Opus)
1. **Director tool kapsamı = Stats (canlı tune) + Spawn (cap'li) + Class-switch.** Sunumun yıldızı canlı stat tuning. Telemetry/CSV varsa bonus.
2. **KES:** Build/Tile/Cliff/Prop manuel palette + Map nav. Stub bırak veya UI'dan gizle. → Rift crystal MANUEL yerleştirme demo'dan çıktı.
3. **Rift crystal lighting** (Light2D) yine yapılır ama **editörde sahne dressing olarak** (bir kez kur), in-game manuel-yerleştirme tool'u DEĞİL.
4. **RUN anim ×2 şart** — idle fallback son çare, ama hedef gerçek run.
5. **Sunum güvenliği:** clamp slider + spawn cap + scripted prova zorunlu.

## cx feasibility — GELDİ (kararı doğruladı + somut recipe/mayın ekledi)

**Sekme durumu (kod-doğrulanmış):**
| Sekme | Durum |
|---|---|
| **Stats** | ✅ ÇALIŞIYOR — en güçlü kanıt. Slider: maxHP/physPower/abilityPower/attackSpeedMult/moveSpeed/debugGlobalDamageMult. Pipeline `ClassStatProfile→ClassStatRuntime→DamagePacket→DamageCalculator→Health` canlı. 10 class profili `Resources/Balance/Classes`'ta. |
| **Telemetry** | ✅ **ÇALIŞIYOR** — 5sn DPS + TTK + source-type breakdown + **CSV export**. 3.1 Pro'nun "akademisyen veriye bayılır" dediği şey HAZIR. |
| **Spawn** | ✅ ÇALIŞIYOR — palette→ghost→tıkla→sil (direct prefab sandbox). |
| **Class&Skill** | ⚠️ kısmi — sadece implement edilmiş sınıflar (2-5, 10 değil). |
| **Build / Map** | ❌ stub ("yakında"). |

**🎬 cx canlı sunum recipe'si (KİLİT):**
1. `` ` `` Director aç → 2. 3-5 düşman spawn → 3. Warblade/Elementalist seç → 4. physPower/abilityPower/debugGlobalDamageMult kaydır → 5. Başlat/Test → 6. **Vur, Telemetry'de DPS/TTK canlı değişsin** → 7. CSV export.

**⚠️ Sunum mayınları (prova'da kaçın):**
- **Damage slider SADECE packetized saldırıda kanıtlar.** Legacy `SkillRuntime.DealDamage(int)` bypassStatScaling → bazı eski skill'ler phys/AP değişimini GÖSTERMEZ. → **Temel saldırı veya bilinen packetized skill kullan.**
- **timeScale=0 duraklatır** → stat/telemetry etkisini Test mode'a dönünce göster.
- **Director raycast riski:** overlay CanvasGroup blocksRaycasts + `IsPointerOverDirectorUi` EventSystem.current'a bağlı; EventSystem yoksa UI'ye tıklama dünyaya da spawn eder. → **A4 Play-mode doğrula** (CURRENT_STATUS'taki açık madde).
- **5 class buton kullan, 10 değil** — desteklenmeyen sınıf controller'sız bırakır.
- Telemetry sadece SkillRuntime damage'ı görür; off-path damage görünmez.
- Stats Save PlayerPrefs yazıyor ama load UI yok.

**Kalan iş (cx tahmini):** 0.5 gün PlayMode smoke + kilitli sunum recipe'si · 0.5 gün raycast/overlay fix (tekrarlanırsa) · Build/Map GEREKMİYOR.

**Sonuç:** Minimal ama derin tool seti = **Spawn + Stats + Telemetry + Warblade/Elementalist switch.** Asıl flex = runtime stat-tuning pipeline'ı, Spawn hedef kurar, Telemetry sayısal etkiyi kanıtlar, CSV export kapatır.

---

## 🔄 GÜNCELLEME 2026-06-13 — Kullanıcı kararı + Faz 1 smoke

**KULLANICI KARARI (council'ı override etti):** Oyun-içi **prop/ışık yerleştirme demo'ya GİRİYOR.** Sunum anlatısı iki ayaklı: "editörsüz dengeliyorum" (stat tuning) + "editörsüz içerik yerleştiriyorum" (prop/light placement). Yani council'ın "Build/Prop KES" maddesi **PROP için geri alındı** (Tile/Cliff/Map hâlâ KES).

**Genişleyen tool kapsamı:** Stats + Spawn + Telemetry + **Prop/Light placement** (+ Class-switch).
- "Asset + ışık" tek prefab: rift_crystal = SpriteRenderer + Light2D(cyan) + LightFlicker → koyunca ışığı da gelir.
- Placement UX = Spawn sekmesi pattern'i (palette→ghost→tıkla→sil) aynalanır.

**Faz 1 smoke sonuçları (play-mode data-proof):**
- ✅ DirectorMode + `*ForValidation` API erişilebilir; Spawn 3 düşman → count=3.
- ✅ EventSystem MEVCUT → A4 raycast mayını muhtemelen YOK.
- ✅ Telemetry hook'ları var: `TelemetryDpsForValidation`, `TelemetrySourceDamageForValidation`, `ExportTelemetryCsvForValidation`. Class: `SelectClassForValidation`.
- ⚠️ **Stat-tune için validation hook YOK** → cx `SetStatForValidation(string,float)` eklemeli (otomatik stat data-proof için).
- ❌ **MCP screenshot ScreenSpaceOverlay UI'yi (Director+HUD) yakalayamıyor** (Main Camera path). Tool UI görseli sadece kullanıcının ekranından/OS-capture ile. Council otomasyonu = **data-proof** (sayısal) + gameplay screenshot.

**Otonom plan (6 faz, task'lara işlendi):** Faz1 smoke (Opus,✓) → Faz2 cx implement → Faz3 council review (3.1Pro+Opus4.6) → Faz4 Opus re-test → Faz5 sıralı model playtest (data-proof+gameplay SS+notes) → Faz6 kullanıcı testi.
