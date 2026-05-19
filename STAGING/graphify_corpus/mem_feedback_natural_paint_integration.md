---
name: natural-paint-integration
description: "Agent'lar (Codex/Opus/Claude) Brush V1 + Sprint 10-11 entegrasyonunda sadece spec uygulamasın — RIMA'ya özgü doğal yerleşim, Hades-tone okunabilirlik ve combat-readable bir sonuç gözetilsin."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: dacb3dfe-b4f5-4efb-af7d-48afdea966cd
---

# Natural Paint Integration — RIMA-Specific Aesthetic Judgement

## Rule

Agent'lar (Codex, Opus, Claude orchestrator) Brush V1 + Sprint 10/11/12 entegrasyonunda **sadece spec'i mekanik uygulamayacak**. RIMA'ya özgü "daha doğal, daha güzel" sonuca yön verecek şekilde değer üretecekler. Yani:

- Compose role tanımları (clean center / decorated edge / focal cluster / wall band / door safety / encounter avoid) yerleşim için **primary model** — noise + Poisson + minDistance bunu desteklemek için var, override etmek için değil.
- Wang context-aware paint (Sprint 11 Composite stroke): komşu tile-grid hücrelerini okuyarak Wang case'i belirleyecek — kullanıcı manuel köşe seçimi yapmayacak.
- Bucket pick + flip + rotation: rastgele değil, **stroke seed deterministic** olacak ki regenerate aynı sonucu verebilsin (Auto-Dress / Regenerate Decorative integration).
- Density curve + featureMask + wallProximityCurve birlikte değerlendirilecek — tek-eksik bir sinyale dayanma.
- L4-L6 katmanlar doğal görünmeli: kare grid hissi YASAK, edge bias + jitter + radius variation aktif.

## Why

**Why:** Kullanıcı RIMA'da Hades-tone Fractured Epic okunabilirlik istiyor: combat lane'i temiz, decoration kenara, focal accent'ler nadir + deliberate. Mekanik random scatter veya symmetrical grid ile bu hissiyat ölmüyor — agent'lar bunu gözetmeli. Bu memory Codex review'larında çıkan "composition roles primary model" kararının day-to-day enforcement'ı.

## How to apply

- **Codex implement görevlerinde:** spec'in dışında composition-aware default'lar koy (örn. encounterAvoidRadius var ama spec value vermemişse 64f başlat, L6 sparse spec ama yoğunluk vermemişse density 0.15 başlat).
- **Opus implement görevlerinde:** Hades / Dead Cells / Hyper Light Drifter referansları aklında olsun — yeni dosya yazarken default değerleri "looks good in shipped pixel art" perspektifinden seç.
- **Claude orchestrator dispatch'lerde:** rima-design (Opus sub-agent) çağrılabilir 2+ sistem kesen aesthetic karar gerekirse. Mekanik task → Codex / rima-codex; aesthetic judgement → rima-design.
- **rima-design çağırma threshold:** "X katmanın yoğunluğu doğal mı?", "bu Wang case seçimi okunabilir mi?", "L6 hero placement room composition'ı bozuyor mu?" gibi sorularda.
- **Test gate:** Vertical slice loop test (M8 Phase 2) sırasında Claude göz kontrolü yapacak — "boyanan oda combat-readable mi?" — değilse Auto-Dress preset'leri ayarla.

## Reference patterns (shipped)

- **Hades:** authored painterly ground breakup, hero-readable accent shapes, no "scaled stamp" feeling. Native size variant pick + bucket weighting (Sprint 9 yapısı bunu zaten enforce ediyor).
- **Dead Cells:** structural tile clarity + decal variety. Kare grid sadece L1/L2'de hissedilmeli; L4-L6 organic.
- **Hyper Light Drifter:** sparse high-contrast accents. L6 rift hero placement bunu hedefler — kullanıcının "ister kare ister yuvarlak" Aseprite-like UX'ine bu felsefe yansıyacak.
- **Spelunky:** layered authorship (scaffold → decoration → gameplay). RoomTemplate prefab → Auto-Dress L4-L6 → Props Mode L7 = bu pipeline.

## Cross-links

- [[brush-tool-v1-design]]
- [[karar-143-layered-pipeline]]
- [[room-library-architecture]]
- [[s86-opus-signoff-decisions]]
- [[codex-vs-opus-split]]
- [[visual-quality]]
