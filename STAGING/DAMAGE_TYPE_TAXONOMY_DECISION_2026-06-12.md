# DAMAGE TYPE TAKSONOMİSİ — KARAR (2026-06-12)

> Council: Gemini 3.1 Pro (deep arch) + Gemini 3.5 Flash (lean) + kod-feasibility (Opus, `DamageType.cs`/`DamageCalculator.cs` okundu). cx-yekta banlı, atlandı. Karar = Opus sentezi.
> Ham advisor çıktıları: `STAGING/_process/2026-06/_council_*_dmg-taxonomy.md`

## TL;DR
**İki eksen, ama sadece bir eksen matematiğe girer.**
- **Math ekseni — `DamageType { Physical, Ability, True }`** → KİLİT, değişmiyor. Formül burada. (NLM canon: Phys + AP)
- **Flavor ekseni — `ElementTag { None, Fire, Frost, Lightning, Void, Light, Poison }`** → YENİ. Formüle GİRMEZ. Sadece: (a) damage-number rengi, (b) status effect tetikleyici (burn/chill/shock/poison), (c) ileride per-element resist için hazır kanca.
- **Resist:** demo için sadece `armor` (Physical'ı azaltır) + `magicResist` (Ability'i azaltır); `True` ikisini de deler. **Per-element resist ERTELENDİ** (ElementTag kancası sayesinde sonradan eklemek kırılgan değil).

Bu, 3.1 Pro'nun "iki-eksen + data-driven" uzun-vade mimarisini, 3.5 Flash'ın "element formüle bulaşmasın, resist matrisini ertele" disiplinyle birleştirir. İkisi de memnun.

---

## 1. Damage Type listesi (KARAR)

### Math ekseni — `enum DamageType` (DEĞİŞMEZ)
| Tür | Rol | Formül |
|---|---|---|
| `Physical` | physPower ölçekli | `base × physPower/100`, `armor` azaltır |
| `Ability` | abilityPower ölçekli | `base × abilityPower/100`, `magicResist` azaltır |
| `True` | delici | ölçek 1.0, hiçbir savunma uygulanmaz |

### Flavor ekseni — `enum ElementTag` (YENİ, formüle girmez)
`None, Fire, Frost, Lightning, Void, Light, Poison`
- Çoğu yetenek `Ability + ElementTag`. Fiziksel-elemental de mümkün (`Physical + Fire` = alevli kılıç).
- **Poison eklendi** çünkü rogue/ranger zehir kimliği yaygın (3.1 Pro flag'i kabul).
- ElementTag = formül DIŞI. Yalnızca görsel + on-hit debuff + ileride resist.

## 2. Renkler (KARAR — 3.1 Pro erişilebilirlik düzeltmesi uygulandı)

| Anlam | Renk | Not |
|---|---|---|
| Physical | `#E89020` ember | §8.4 ile uyumlu |
| Ability / Magic | `#00FFCC` cyan | §8.4 KİLİT (değişmez) |
| True | `#F4F0E6` beyaz | delici |
| **Crit** | `#FFD24A` altın + BÜYÜK font + shake | §8.4 |
| Fire | `#FF6A1F` | |
| Frost | `#7FE0FF` | açık buz mavisi |
| **Lightning** | `#FFE600` (DÜZELTİLDİ) | ~~#FFD24A~~ Crit ile çakışıyordu → saf elektrik sarısı |
| Void/Shadow | `#7B3FA8` mor | |
| Light/Holy | `#FFF0B0` soluk altın | |
| Poison | `#7BC043` zehir yeşili | yeni |

**İki çatışma çözüldü:**
1. ~~Lightning `#FFD24A` = Crit `#FFD24A`~~ → Lightning artık `#FFE600`.
2. Ability cyan `#00FFCC` vs Frost `#7FE0FF` yakın → cyan KİLİT (§8.4) olduğu için Frost'u korudum; **ayrım renge DEĞİL şekle bırakılır** (aşağı).

**Erişilebilirlik kuralı (ZORUNLU):** Damage-number asla salt-renkle ayrışmaz. True = daha büyük punto; Crit = büyük + shake; element = küçük ikon/glyph prefix opsiyonu. Renk körü güvenliği şekil/boyut/animasyonla sağlanır.

**Ekranda renk ekonomisi:** Sayı normalde `DamageType` rengindedir; `ElementTag != None` ise element rengine geçer. Aynı anda az renk = net okuma (3.5 Flash).

## 3. Enum / kod genişletme (feasibility — en az refactor)

```csharp
// DEĞİŞMEZ
public enum DamageType { Physical, Ability, True }

// YENİ
public enum ElementTag { None, Fire, Frost, Lightning, Void, Light, Poison }
```
- **DamagePacket:** `ElementTag elementTag = None` alanı ekle → default sayesinde mevcut çağıranlar (SkillRuntime/PlayerAttack/mob attacks) KIRILMAZ.
- **DamageCalculator.Calculate:** statMultiplier switch AYNEN kalır. Sonuna savunma adımı ekle:
  - `Physical` → `dmg ×= (1 - armorReduction)`
  - `Ability` → `dmg ×= (1 - magicResistReduction)`
  - `True` → savunma atla.
  - ElementTag formüle GİRMEZ.
- **ClassStatRuntime:** `armor`, `magicResist` alanları (flat→% dönüşüm tek formül, ör. `r/(r+K)`). Per-element resist YOK.
- **Damage-number rengi:** statik `DamageColors` haritası (`ElementTag/DamageType → Color`), UI katmanında tüketilir. Math'e dokunmaz.
- **Status effect:** `ElementTag → on-hit debuff` ayrı sistem, şimdilik sadece kanca rezerve; gerçek burn/chill implementasyonu ERTELE.

## 4. Class kimlik eşlemesi (3.1 Pro, kabul)
| Class arketipi | DamageType | ElementTag |
|---|---|---|
| Warrior / Rogue (melee) | Physical | None (Rogue: Poison) |
| Mage (arcane) | Ability | None |
| **Elementalist** (switch) | Ability | Fire ↔ Frost ↔ Lightning (runtime) |
| Warlock / Necromancer | Ability | Void |
| Paladin / Templar / Cleric | Physical/Ability | Light |
| Ranger / Hunter | Physical | None / Poison |

Elementalist'in switch'i, ileride per-element resist gelince "doğru elementi seç" taktiksel derinliği yaratır — şimdilik sadece görsel+debuff farkı.

## 5. ERTELENENLER (bilinçli, demo kapsamı dışı)
- Per-element resist tablosu / `ElementalMatrixSO` (zayıflık-direnç matrisi) — ElementTag kancası hazır, gerçek matris SONRA.
- Element status-effect implementasyonu (burn DoT / freeze slow / shock chain / poison stack) — kanca var, mekanik sonra.
- 8-10+ element büyümesi: enum hardcoded kalır (switch perf), etkileşim matrisi data-driven (SO) olur — o gün geldiğinde.

## 6. Faz A'ya etki (DEMO_TOOLS plan)
A2 stat çekirdeğine eklenecek MINIMUM:
- `DamageType` (var) + `ElementTag` (yeni enum, 7 değer).
- DamagePacket'e `elementTag` alanı.
- DamageCalculator'a armor/magicResist azaltma adımı (True bypass).
- ClassStatRuntime'a `armor`+`magicResist`.
- `DamageColors` statik harita (UI tüketir).
- **GATE değişmez:** CombatContract `run_tests` yeşil. Per-element resist/status YOK → A kapsamı şişmez.
