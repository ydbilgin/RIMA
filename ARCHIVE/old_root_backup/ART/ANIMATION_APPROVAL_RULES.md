# ANIMATION_APPROVAL_RULES.md
> **Ne zaman yükle:** Animasyon üretimi veya onay adımı öncesinde.

---

## ANİMASYON AŞAMA SİSTEMİ

| Aşama | Anlam | Üretim yöntemi | Onay gerekli mi? |
|---|---|---|---|
| `animation_test` | Hızlı kalite kontrolü, prototype base üzerinde | MCP template | HAYIR — test amaçlı |
| `candidate_animation` | Approved base üzerinden üretildi, onay bekliyor | MCP template | EVET — sen onaylar |
| `approved_animation` | Sen onayladın, Unity'e alınabilir | — | Tamamlandı |
| `rough_anim_test` | Aseprite'ta elle çizilmiş ham test | Aseprite manuel | HAYIR — referans |

---

## KURAL: ZAYIF BASE ÜZERİNDE FİNAL ANİMASYON KURMA

```
prototype_temp base → animation_test ✅ (test amaçlı, düşük gen harcama)
candidate_base     → animation_test ✅ (review için, tam zincir değil)
approved_base      → candidate_animation ✅ (tam animasyon zinciri buradan başlar)
```

**YASAK:**
```
prototype_temp base → 6 template full set ❌ (base zayıf, gen israfı)
candidate_base → approved_animation ❌ (sen onaylamadan final kabul etme)
```

---

## ONAY ÖNCESİ TEST ANİMASYONU

Senden onay gelmeden önce şunlar yapılabilir:

```
✅ İzin verilen:
- 1-2 template animasyon testi (idle + walk)
- Sadece south yönü için hızlı kontrol
- rough_anim_test Aseprite'ta manuel

❌ Yapılmaz:
- 6 template × 8 yön full set
- confirm_cost=true ile custom animasyon
- Animation → Unity export
```

---

## TEMPLATE vs CUSTOM ANİMASYON KARARI

| Durum | Seçim | Neden |
|---|---|---|
| Standard hareket (walk, idle, death) | Template | 1 gen/yön, hızlı, yeterli |
| Oyunun ihtiyacı özel hareket değilse | Template | Custom çok pahalı |
| Karaktere özgü animasyon gerekiyor | Custom | 20-40 gen/yön, onay al |
| Combat feel kritikse, template yetmiyorsa | Aseprite + PixelLab extension | MCP'ye göre daha kontrollü |

---

## EDITOR ŞART OLAN ANİMASYON DURUMLARI

Şu durumlarda MCP yetmez, Aseprite gerekir:

| Sorun | Çözüm |
|---|---|
| Frame'ler arasında titreme / flickering | Aseprite: Init Image → "Current Layer (Anim)" seç |
| Frame timing yanlış (çok hızlı/yavaş) | Aseprite Timeline: frame sağ tık → Frame Duration |
| Smear frame eksik (saldırı hızı hissi) | Aseprite: manuel ara frame çiz |
| Silhouette okunmaz (karakter kalabalıkta kaybolur) | Aseprite: outline güçlendir |
| Ölüm animasyonu dramatik değil | Aseprite: son 2 frame'i elle düzenle |

---

## ANİMASYON ONAY AKIŞI

```
1. approved_base hazır
2. MCP → 2 template test (idle + walk, south only) → animation_test
3. Ben sana gösteririm → sen izlersin
4. "İyi görünüyor" → full set başlar (6 template × 8 yön)
5. Full set → candidate_animation
6. Sen full ZIP'i kontrol edersin → "onaylıyorum"
7. → approved_animation → Unity export
```

---

## ANİMASYON TEST BATCH ÖNERİSİ (validation için)

```
Karakter: Warblade (approved_base'den)
Test: idle (fight-stance-idle-8-frames) → south only → 1 gen
      walk (walking-6-frames) → south only → 1 gen
Toplam: 2 gen
Süre: ~3 dk
Sonuç: titreme, stil, timing kontrolü
Kararlar: yeterli → full set | yetersiz → Aseprite hybrid
```
