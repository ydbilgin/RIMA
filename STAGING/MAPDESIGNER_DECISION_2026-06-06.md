# KARAR: Map Designer İyileştirme — Faz-1 (gece) + Faz-2 (sonra) (2026-06-06)

Council: cx (envanter) + ax-3.1-Pro (hibrit araç vizyonu) + ax-3.5-Flash (lean kesim) → Opus sentez.

## Teşhis (cx, file:line kanıtlı)
`RIMA/Map Designer` (UnifiedMapDesigner) ESKİ RoomData veri yoluna bağlı: 26 RoomTemplateSO'yu LİSTELEYEMİYOR, açamıyor, kaydedemiyor. Modern yol (RoomTemplateSO→IsoRoomBuilder) ayrı pencerede (Room Browser). PropsTab template-aware ama Legacy menüde. Auto-placer batch menüde çalışıyor.

## Oybirliği
- Gecede TAM RoomData↔RoomTemplateSO dönüşümü YOK (L, riskli) — Faz-2.
- Canlı SO→auto-rebuild bağı KES (599 hücreli odada editör donar; 3.1 önermişti, Flash+cx reddetti) → manuel Rebuild butonu.
- En yüksek ROI: template listesi + Build + **Auto-Props (seed+randomize)** butonu.
- UnifiedDesignerCore/F2/RoomData yollarına ve UnifiedDesignerTests'e DOKUNMA.

## Anlaşmazlık + karar
Flash: Room Browser'ı büyüt. cx: UnifiedMapDesigner'a "Rooms" paneli ekle (Browser mantığını reuse). **KARAR: cx yolu** — kullanıcı "MAP DESIGNER güzel olsun" dedi; Rooms sekmesi Map Designer'ın İLK/varsayılan sekmesi olur, Room Browser fallback kalır. Flash'ın cilaları aynen alınır.

## FAZ-1 (bu gece, cx implement)
1. UnifiedMapDesigner'a yeni "Rooms" sekmesi (İLK sekme, default): RoomTemplateSO listesi (arama + klasör grupları + seçili-template state).
2. Butonlar (renkli, GUI.backgroundColor): **Build in Arena** (yeşil; Browser'ın build yolunu helper'a çıkar+reuse) · **Auto Props** (mavi; seed int alanı + 🎲 randomize + onay dialogu; Undo.RecordObject + SetDirty; sadece seçili template) · **Save Assets** (turuncu; dirty-yıldızı göstergesi).
3. Seçili template için 2D ŞEMATİK önizleme (EditorGUI.DrawRect: walkable/hole/kapı renk kodlu) — 3D render değil.
4. Smoke test: 26 template döngüsü Build + prop-validasyon, 0 exception (sabah provası kanıtı).
5. Kapı toggler/walkable boyama/socket handle = FAZ-2 (defer).

## FAZ-2 (sunum sonrası backlog)
SceneView walkable/hole/overlay fırçası (Undo'lu) · kapı/socket edit handle'ları · flood-fill ada-bütünlüğü validatörü · prop taşması kırmızı işaret · PropsTab entegrasyonu · RoomData→Template dönüşümü + eski sekme temizliği · template thumbnail cache.

## Riskler (cx)
walkableGrid indeksleme bounds'a bağlı `(y-yMin)*w+(x-xMin)` · iki SceneView handler çakışması (PropsTab entegre edilirse — Faz-2) · RuntimeAssetRegistry yoksa palet uyarısı · legacy RoomTemplateLoader prefab-ref ister, KULLANMA.
