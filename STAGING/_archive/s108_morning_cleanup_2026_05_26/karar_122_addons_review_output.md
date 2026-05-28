# T2/T3/T4 Production Review — rima-sonnet
## Date: 2026-05-14 (S70 gece, orchestrator kaydetti)

---

## 1. Cost Validation

**T2 ~30h — REVIZE: 18-24h daha gerçekci**
- 10 Altar pasifi SO veri katmanı: ~2h
- Chance roll + ICD timer (T1 ICD manager extends): ~4-6h
- T1/T2 çakışma priority logic + edge case test: ~4h
- Family tag apply (T4 stub gerekirse): +6h
- UI grey-out: ~3h
- **Gerçekci toplam:** T4 önce varsa ~18h, T4 stub gerekirse ~24h

**T3 ~80h — REVIZE: 75-90h kod (weapon swap hariç), gen cost ayrıca**
- 80 evrim SO veri girişi (10 class × 4 slot × 2 path, ~20dk/SO): ~27h
- Algorithmic auto-bond (edge case + tie breaking): ~12-15h
- Hades-style dialog UI (modal + 3 kart + animasyon + persist): ~20-28h
- T3 trigger/VFX (cyan trail + CD -10% passive): ~8h
- Test + integration: ~10-15h
- **Alt bound 75-80h makul, üst 90h risk var**

**T4 ~40h — REVIZE: 44-50h gerçekci**
- 4 Family Tag class + apply/refresh/clear: ~6h
- Per-mob tag dictionary + event-driven subscribe: ~10-14h (**en hafife alınan nokta**)
- 3-tag detection + immunity window + reset: ~6h
- UI HUD (4 mini icon + fade + screen pulse): ~10-12h
- Rift Proc burst (0.3s freeze + simultaneous echo + armor pen): ~10h
- **Toplam: ~44-50h. 40h underestimate.**

---

## 2. Balance

**Algorithmic family-match: UYGULANABILIR, risk orta**
- Monoton düşüş riski: echo havuzu tag çeşitliliği yetersizse her zaman aynı 2-3 echo seçilir
- **Mitigation:** T3 alpha'da Warblade + Elementalist ile tag dağılımı validate et — monoton düşüş → 3-tag system

**T1→T4 burst gap: MANTIKLI**
- T1 deterministik baseline (Beat 3 %35) — solo bile yeterli
- T2/T3 arasındaki geçiş (proc frequency vs. proc weight) VFX + 0.3s slow-motion ayrımıyla hissettiriliyor — OK
- **Risk:** T4'ün 3-tag şartı bazı class kombinasyonlarında doğal oluşmayabilir (Fracture tag için Warblade/Ravager/Brawler LMB gerekli)

---

## 3. Family Tag Implementation

**Event-driven: ÖNERILEN**
- State machine per-enemy (50+ mob → 200+ state instance): GC baskısı riski
- Event-driven `OnDamageDealt → TagManager.Apply(enemy, TagType, 2f)`: merkezi Dictionary<EnemyId, TagSet>
- Despawn'da explicit cleanup zorunlu

**Refresh logic: DÜŞÜK karmaşıklık**
- `tagSet[type].expiry = Time.time + 2f` — single line reset
- T2 ICD apply vs T4 detection arasında race condition yok (farklı frame'ler)

**⚠️ KRİTİK Implementation note (Codex dispatch öncesi ekle):**
> Per-enemy tag cleanup: `OnEnemyDeath` event'inde `TagManager.Remove(enemy)` explicit çağrı ZORUNLU. Atlanırsa memory leak + stale reference. TagManager singleton, Dictionary<EnemyId, TagSet>.

---

## 4. T3 UI

**Boon dialog: FLOOR BAŞI — her cast'te değil**
- Floor load kapandıktan sonra ilk frame'de dialog aç
- Run içinde bind sabit, floor geçişinde değişebilir
- Hades boon'undan daha tutarlı (taktiksel floor-level karar)

**3-seçenek UI mock-up:**
```
[Echo Bond Selection]
Floor 3 — Warblade Q Slot (Iron Combo Slam — Path A)

[ Fireball Burst ]    [ Veil Strike ]      [ Pinning Shot ]
Elementalist R        Shadowblade M         Ranger R
+burst on combo fin.  +gap close on Q hit   +slow on Q crit
```

---

## 5. MVP T2 Minimal

**1-Altar MVP: YES — school demo için yeterli**
- "Echo Cascade" (%20 sabit, 0.8s ICD): T1'in deterministik ritmi + T2'nin sürpriz element = zengin combo
- T1 + T2 minimal: "her saldırı önemli + ritim de önemli" hissi

**T2 full açılma sırası:**
1. Echo Cascade (Altar 4, Echo tag) — MVP
2. Shatter Vein (Altar 5, Fracture tag) — T4 feed öncelikli
3. Whisper of Embers (Altar 1, Burn/Bleed)
4. Frost Pact (Altar 2, Chill)
5. Rift Hunger (Altar 3, Rift)
6. Altar 6-10 Faz 2 detay sonrası

---

## 6. T3 Weapon Sprite Swap (Yol A Bridge)

**Kod scope: 14-18h gerçekci (8h underestimate, 20-30h range içinde)**
- Swap mekanizması + T3 unlock per-run persist + SO referansları + Karar #99 canon tutarlılığı: ~14-18h

**Gen maliyet: 28-32 gen gerçekci (20 initial + 8-12 revision)**
- "20-30 gen" belgedeki üst bound makul — ONAYLA

**Öneri: MVP SKIP, Faz 2 son adım — ONAYLA**

---

## Implementation Risk Top 3

1. **Per-enemy tag dictionary (T4):** Despawn cleanup atlanırsa memory leak. `OnEnemyDeath` explicit cleanup ZORUNLU.
2. **T3 family tag dağılımı:** 80 SO verify edilmeden lock edilirse Faz 2 büyük refactor. Alpha: Warblade + Elementalist 8 entry ile validate.
3. **T2/T4 double-proc aynı frame:** Beat 3 T1 + T2 aynı anda tetiklenirse tag double-refresh riski. Mitigation: frame-end tag resolve (batch işle, immediate değil).

---

## Final Recommendation

- **MVP addition:** T2 minimal (Echo Cascade, ~6h) — EVET, school demo için T1 üzerine ekle
- **Faz 2 start order:** T4 tag system önce → T2 full 10 Altar → T4 UI → T3 data + dialog → T3 echo bond algorithm → T3 weapon swap
- **Tek cümle:** Sistem RIMA roguelite döngüsüyle uyumlu, algorithmic tag matching scalable, per-enemy tag cleanup + T3 family tag dağılımı validate edilirse Faz 2'de iş görür.

---
*rima-sonnet analysis S70 gece. Codex dispatch öncesi per-enemy tag cleanup notu ekle.*
