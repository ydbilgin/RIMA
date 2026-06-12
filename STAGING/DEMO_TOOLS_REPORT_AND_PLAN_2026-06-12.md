# RİMA — Demo Araçları: Rapor + İmplementasyon Planı (2026-06-12)

> Bu rapor: araçların NE olduğu, NEREDE çalıştığı (oyun-içi mi Unity'de mi), NASIL kullanılacağı + dispatch-hazır faz planı. Yeni session'da cx ile parça parça yaptırılacak.

---

## BÖLÜM 1 — Araçlar NEREDE çalışacak? (oyun-içi vs Unity)

İki katman var, karıştırma:

### A) Unity Editör'de (tek seferlik kurulum — cx yapar)
Bunlar projede **bir kez** oluşturulur, build'e gömülür:
- `ClassStatProfile` asset'leri (10 class stat verisi) — ScriptableObject
- TMP Font Asset (Jersey 10, Türkçe SDF)
- Chrome + sembol import (✅ zaten yapıldı, atlas'lı)
- Stat sistemi kodu (DamagePacket/DamageCalculator) + Director Mode prefab/script

### B) Oyun-içinde, runtime (asıl kullandığın yer — SUNUMDA)
**Director Mode = oyun-içi overlay**, Unity editör penceresi DEĞİL. Build'de çalışır (`#if DEMO_BUILD || DEVELOPMENT_BUILD`). Oyunu oynarken `` ` `` tuşuyla açarsın, içinde spawn/stat/tile/test yaparsın.

**Neden oyun-içi:** Sunumu çalışan oyunda canlı yapıyorsun; Unity editör tool'u build'de yok. Bu yüzden her kontrol runtime overlay'de.

**Özet:** Kurulum = Unity (cx). Kullanım = oyun-içi (sen, sunumda).

---

## BÖLÜM 2 — Director Mode araçları (ne / nasıl)

`` ` `` (backtick) → **Director** (oyun durur, kamera yukarı serbest-cam) ↔ **Test** (oyna). "Başlat" butonu geçiş. Sol dikey rail, 6 sekme:

| Sekme | Ne yapar | Nasıl | Hook |
|---|---|---|---|
| **Spawn** | Mob/boss SINIRSIZ koy | palette seç → zemine tıkla (ghost preview), sağ-tık sil | `EncounterController.SpawnEnemy` |
| **Class & Skill** | Karakter + skill değiştir | 10 class butonu, skill slot ata | `PlayerClassManager.SetPrimaryClass` |
| **Stats** | Canlı stat slider | HP/phys/AP/atkSpd/move/dmg kaydır → anında etki | `ClassStatRuntime` |
| **Build/Tile** | Zemin tile boya | palette + brush, tıkla-boya | `InPlayMapPaintOverlay.PaintCell` |
| **Build/Cliff** | Uçurum üret | Regenerate/Undo/manual override | `CliffAutoPlacer.Regenerate` |
| **Build/Prop** | Dekor üret | density slider + Generate | `BridsonPoissonAutoPlacer` |
| **Map** | Haritayı gör/seç | node grafiği, tıkla→atla, reroll seed | `DungeonGraph`/`JumpToNode` |
| **Telemetry** | DPS/TTK/CSV | canlı sayaç + export | hasar event |

**Akış (sunum):** `` ` `` → spawn 8 mob + boss, arena seç → class Elementalist → stat abilityPower kaydır → **Başlat** → stress test, DPS izle → `` ` `` → Warblade'e geç, tekrar test → CSV export.

**Hotkey'ler:** `` ` `` panel · Enter Başlat · F8 debug HUD · F9 presenter mode · F7 slow-mo · F12 screenshot · H hitbox overlay · F5/F9 snapshot al/dön.

**ChatGPT ek UX (kabul):** ghost preview zorunlu, sağ-tık tutarlı sil, selection inspector (tooltip_box), dummy AI mode (Passive/Attack/Move/Cast/BossLoop), snapshot stack, encounter recipe export JSON.

---

## BÖLÜM 3 — Stat sistemi (temel)

Director'ın Stats sekmesinin ÖNKOŞULU + dengeleme demosunun çekirdeği:
`ClassStatProfile (SO) → ClassStatRuntime (kopya) → DamagePacket → DamageCalculator → Health`
**Phys/AP ayrı stat** (canon). Formül: `base × (phys|AP)/100 × cap(identityBuild,3) × cap(situational,2) × debugMult`. 10 class değeri kilitli (Warblade 115HP/110phys ... bkz `SANDBOX_DIRECTOR_DECISION_2026-06-12.md`).
**Kritik:** HP → `PlayerStats.maxHP`'ye yaz (Health.cs değil). attackSpeedMult sadece cooldown'a (commitment'a değil). `DealDamage(int)` SİLME, içten nötr packet'e çevir (60 çağrı kırılmaz).

---

## BÖLÜM 4 — Dil: EN + TR (ayrı ayarlar) — ZATEN VAR ✅

`Assets/Scripts/Core/Loc.cs` çift dilli localization **mevcut ve çalışıyor**:
- `Loc.T("key")` / `Loc.SetLanguage("tr"/"en")` + PlayerPrefs (`rima.lang`) + `OnLanguageChanged`
- TR (varsayılan) + EN tam string tabloları (menu/settings/combat/draft/death/victory/codex)
- **Settings menüsünde dil toggle'ı var** (TÜRKÇE/ENGLISH)

**Yeni iş (küçük):** Director Mode + Stats tool string'leri de `Loc.T(...)` kullanacak → `_tr` ve `_en` tablolarına `director.*` / `stats.*` key'leri eklenir. Yeni altyapı YOK, mevcut sistem genişletilir. Jersey 10 fontu hem TR hem EN destekler.

---

## BÖLÜM 5 — Font: Jersey 10

Tam Türkçe (ğşıİöçü ✓), OFL/ücretsiz, temiz dark-fantasy pixel-game hissi. cx `.ttf`'i `Assets/Fonts/`'a koyup TMP Font Asset (SDF, Türkçe glyph) üretir. Tüm UI + tool buna geçer. (Swap kolay — beğenmezsen Handjet/PixelifySans alternatif.)

---

## BÖLÜM 6 — İMPLEMENTASYON PLANI (dispatch-hazır, yeni session)

Her faz = ayrı cx görev dosyası. Test-gate'li.

### FAZ A — Temel (Director'dan bağımsız, hemen)
- **A1 Font:** Jersey 10 `.ttf` import + TMP Font Asset (Türkçe SDF) + tema referansı
- **A2 Stat çekirdeği:** DamageType/DamagePacket/DamageCalculator + ClassStatProfile/ClassStatRuntime + 10 asset (kilitli değerler) + `DealDamage(packet)` overload (int nötr-map)
- **A3 Wiring:** PlayerClassManager → PlayerStats.maxHP + moveSpeed + atkSpeed; 2 basic-attack damage girişi
- **GATE:** `run_tests` (CombatContract) yeşil → A bitti

### FAZ B — Director iskelet (en yüksek "güzel" getirisi)
- **B1:** DirectorMode controller — `` ` `` toggle, DIRECTOR/TEST mod, kamera lerp (unscaledDeltaTime), timeScale
- **B2:** uGUI Canvas + chrome skin (minimap_frame Window + sol rail + ribbon Başlat) — ChatGPT hiyerarşi ağacı referans
- **B3:** Sekme sistemi (CanvasGroup toggle), boş 6 panel + Jersey 10 font

### FAZ C — Sekmeler (demo sırası)
- **C1 Spawn** (SpawnEnemy hook doğrula → tıkla-koy + ghost + sağ-tık sil + dummy AI)
- **C2 Class&Skill** (10 swap + skill ata)
- **C3 Stats** (slider'lar → ClassStatRuntime canlı + Reset/Save/Export)
- **C4 Build** (Tile: F2 absorbe/PaintCell public · Cliff: Regenerate · Prop: Generate)
- **C5 Map** (node grafiği + jump + reroll)
- **C6 Telemetry** (DPS/TTK + CSV export)

### FAZ B/C arası — HUD LAYOUT (EDIT MODE) — YENİ (2026-06-12 karar)
- **HUD1:** `HudLayoutManager` — 24×14 grid + occupancy map (bloklama engeli), Director C1 ghost+collision REUSE
- **HUD2:** Tüm widget'lar (skill bar/minimap/HP-XP barları) taşınabilir + scale; anchor+hücre saklama
- **HUD3:** PlayerPrefs persist (JSON layout) + "Sıfırla" + 2-3 preset; `Loc.T()` key'leri
- **Kapsam:** GERÇEK oyuncu-facing özellik (Director değil). Görsel doğrulama → sabah toplu kontrol.
- Detay → `STAGING/HUD_LAYOUT_DECISION_2026-06-12.md`

### FAZ D — Cila + dil
- **D1:** snapshot stack + Quick Reset, hitbox overlay, selection inspector, encounter recipe export
- **D2:** Localization — `director.*`/`stats.*` key'leri `Loc.cs` TR+EN tablolarına; tüm tool metni `Loc.T()`
- **D3:** presenter mode, slow-mo, screenshot hotkey'leri

**Sıra:** A → B → C → (HUD Layout) → D. A her halükarda gerekli (dengeleme çekirdeği). B+C "güzel tool". D cila.

### Damage taksonomisi (2026-06-12 council kararı)
- FAZ A2 stat çekirdeğine dahil: `ElementTag` enum + DamagePacket alanı + armor/magicResist adımı + renk haritası.
- Per-element resist + status-effect ERTELENDİ. Detay → `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md`

---

## BÖLÜM 7 — Hazır vs Gerekli

| Hazır ✅ | Gerekli ⏳ |
|---|---|
| Chrome kit (atlas, 9-slice) | Jersey 10 TMP asset (A1) |
| 8 node sembol | Stat sistemi kodu (A2-A3) |
| Localization (Loc.cs EN+TR) | Director Mode (B-C) |
| Runtime sistemler (tile/cliff/prop/spawn) | Tool dil key'leri (D2) |
| Palette içerik (mob/class/skill ikonları) | 3 takılı sembol (rest/unknown/player) |
| Kilitli kararlar (stat + sandbox + font) | — |
| ChatGPT uGUI hiyerarşi + mockup | — |

**Sonuç:** Tüm tasarım/karar/asset hazır. Sıradaki = saf implementasyon, yeni session'da cx dispatch (Faz A'dan başla).

---

## BÖLÜM 8 — Mob AI + Combat Tuning + Renk taksonomisi (2026-06-12 ek)

### 8.1 Mob skill — ZATEN VAR
Mob'lar attack-component ile skill atıyor: `MobAttack_Melee/Throw/ChainPull/Barrier/PenitentCombo/Summon` (`Assets/Scripts/Enemies/Attacks/`). Per-attack param inspector'da (damage/cooldown/range/telegraph). Telegraph sistemi (`EnemyTelegraph.cs`: circle/line/cone + wind-up) var. Affix (Blazing/Glacial/Void/Fractured) + EnemyTier çarpan + AttackToken (eşzamanlılık) var. Boss `PenitentSovereign` 3 faz, 8 saldırı, sequential rotation.

### 8.2 Mob AI — demo kararı (over-engineer ETME)
Mevcut = FSM (Idle/Chase/Attack/Dead, mesafe+cooldown). Demo için:
- **Küçük mob:** FSM + durumsal kural — mesafe<attackRange & CD→melee · mesafe>castRange & LoS→ranged · HP<%20→flee. Utility AI/BT GEREKSİZ.
- **Boss:** sequential yerine **Weighted Random Bag** (çek-oyna, aynı 3× üst üste yok — Hades/RoR). Faz geçişi makro-FSM (HP eşiği) korunur.
- Telegraph zaten doğru (wind-up'ta ses + zemin decal).

### 8.3 Combat Tuning sekmesi (Director, Stats genişlemesi)
Player + mob skill değerlerini CANLI düzenle: damage / cooldown / HP / regen / range / telegraph. Veri: `MobDefinition` SO (şu an runtime'da UYGULANMIYOR → aktive et) + per-attack param + player ClassStatRuntime/skill. Spawn'lanan mob'u seç (selection inspector) → slider → anında uygula. Preset save/load.

### 8.4 Stat renk taksonomisi (KİLİT — editör + tooltip + damage number)
| Stat | Renk | Damage number | Renk |
|---|---|---|---|
| HP | `#C82026` | Fiziksel | `#F4F0E6` beyaz |
| HP Regen | `#2DBE6C` | Ability/magic | `#00FFCC` cyan |
| Damage | `#E89020` ember | Crit | `#FFD24A` altın + BÜYÜK |
| Speed | `#00FFCC` cyan | Heal | `#2DBE6C` (yukarı float) |
| Crit | `#FFD24A` altın | Burn | `#E89020` ember |
| Armor | `#8A9098` slate | Poison | `#7B3FA8` mor |
| Cooldown | `#A65BFF` mor | | |

**Upgrade diff:** artış yeşil ▲ / azalış kırmızı ▼ (`mevcut → yeni`). **Rarity:** common gri / rare cyan / epic mor / legendary altın (mevcut ribbon'larla uyumlu). Crit renk+boyut → renk körü erişilebilirlik.

### 8.6 Damage type taksonomisi — YENİ SESSION'DA COUNCIL KARARI
**Karar verilecek:** RIMA'da hangi damage type'lar olacak? Renkler **global konvansiyon** (LoL/ARPG) takip etmeli — uydurma değil.

**Global konvansiyon (sabit referans):**
- **True** = beyaz `#F4F0E6` (resist deler) · **Physical** = ember/turuncu-kırmızı `#E89020` · **Magic/Ability** = cyan/mavi `#00FFCC`
- Elemental alt-türler: Fire `#FF6A1F` · Frost `#7FE0FF` · Lightning `#FFD24A` · Void/Shadow `#7B3FA8` · Light/Holy `#FFF0B0`

**Canon başlangıcı (NLM):** 2 ana stat = Physical Damage + Ability Power (AP). Açık soru: AP elemental alt-türlere bölünür mü (Elementalist element-switch → muhtemel evet) yoksa tek mü? Light/True damage var mı?

**Council brief (yeni session):** (1) RIMA damage type listesi kesinleştir (physical/magic/elemental-subtypes/true/light) — class kimlikleriyle tutarlı · (2) her tür → global-konvansiyon renk · (3) DamageType enum'u buna göre genişlet (ChatGPT'nin Physical/Ability/True'su başlangıç) · (4) damage-number + resist sistemi. Kod zemini + NLM canon + Gemini global-konvansiyon araştırmasıyla.

### 8.5 Plan eklemeleri
- **FAZ C3'e ek:** Stats sekmesi = Combat Tuning (player + mob skill değerleri, color-coded).
- **FAZ A'ya ek:** stat renk taksonomisi sabitleri (`RimaUITheme`'e renk tablosu) + damage-number renk sistemi.
- **FAZ C1'e ek:** mob durumsal AI kuralı (mesafe/HP) + boss Weighted Random Bag.
- **FAZ A:** `MobDefinition` SO'yu runtime'da uygula (şu an kullanılmıyor) → mob stat tek kaynaktan.
