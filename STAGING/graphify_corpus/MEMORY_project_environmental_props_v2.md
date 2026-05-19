---
name: environmental-props-v2
description: "V2 future scope — çevre props çeşitliliği (ağaçlar, türlü flora/dekor) biome-aware Brush sistemine entegre edilecek. Şu anın işi değil."
metadata: 
  node_type: memory
  type: project
  originSessionId: dacb3dfe-b4f5-4efb-af7d-48afdea966cd
---

# Environmental Props Variety — V2 Future Scope

**Status:** DEFER — V1 vertical slice + Sprint 9-13 hardening tamamlanmadan başlama.
**Kayıt tarihi:** 2026-05-16 S86 PREP-3 (kullanıcı sözlü not)

---

## Kullanıcı notu (verbatim çeviri):
> "Çevreye ağaçlar falan da eklenecekse tür tür güzel çeşitli eklenebilir. Bu da memory'e ekle, ilerisi için, şimdinin işi değil."

---

## V2 Scope Hint

Brush V1 vertical slice + Sprint 9-13 başarıyla kapanınca, V2 scope'ta:

- **Tür çeşitliliği:** Sadece "tree" değil — birden fazla tree species (oak / pine / dead / blossom / corrupted), biome'a göre varyant pool
- **Flora layers:** Yer örtüsü (small grass tufts, ferns, mushrooms), orta katman (bushes, saplings), üst katman (trees, large foliage)
- **Biome-specific palettes:** ShatteredKeep (corrupted/dead trees) ≠ Forest (lush) ≠ Crypt (dark thorns) ≠ Desert (cacti/dry shrubs) ≠ Volcano (ash-burned skeletons)
- **Seasonal variants (opsiyonel V2+):** Aynı tree species autumn/winter/spring color pass
- **Props Mode kategorileri (Sprint 12 reference):** Tree + Bush + Pile + Crate gibi prop kategorileri zaten Sprint 12'de tanımlanacak; bu V2 expansion onun üzerine bina edilir

---

## Mimari Bağlantılar (V2 implement edilirken)

- **Brush V1 entegrasyonu:** Yeni AssetPoolSO biome+layer pair'leri (örn. `Trees_ShatteredKeep`, `Foliage_Forest`). Hero gating + size bucket aynı sistem. [[brush-tool-v1-design]]
- **Layer routing:** Trees büyük ihtimal **L7 Props** (Sprint 12+) — çünkü collision + sorting anchor + spawn avoidance gerekiyor. Pure decorative değil. [[room-library-architecture]]
- **Composition Roles relevance:** "decoratedEdges" + "wallBand" + "encounterAvoid" zone'ları flora yerleşimini doğal yönlendirir (Sprint 11 Natural Engine).
- **Marketplace forward-compat:** `namespacePrefix` zaten V1'de tanımlı — V2'de "biome_forest_flora.brushpack" gibi pack'ler bağımsız ship edilebilir.

---

## Why (Bu kayıt neden duruyor?)

V1 tamamen "Hybrid Auto-Slice + Wang 16 + Brush V1 8-sprint + Sprint 9-13" üzerine kilitli. Çevre props çeşitliliği şimdi gündeme alınırsa scope creep + vertical slice gecikmesi. Ama V2 planlamasında "ne kadar büyük genişleme?" sorusu cevaplanmalı — bu memory o cevabın seed'i.

## How to apply

V1 ship olduktan sonra V2 backlog grooming'de bu memory'e dön:
1. Biome listesi finalize (kaç biome × kaç prop kategorisi)
2. Tree species sayısı + variant count target
3. PixelLab dispatch cost estimate (her biome × ~3 flora master)
4. Sprint 12 Props Mode'a entegrasyon planı (collision footprint per tree)
5. Sprint 11 Natural Engine composition role binding (trees → decoratedEdges + wallBand olabilir)

---

## Cross-links

- [[brush-tool-v1-design]]
- [[room-library-architecture]]
- [[karar-143-layered-pipeline]]
- [[visual-quality]]
- [[rima-backlog]]
