# Council — RIMA Oda Tasarımı + Prop Yerleşimi + Room Designer (DEEP / design lens)

Sen ax Gemini 3.1 Pro High'sın. DERİN TASARIM görüşü. Kod yazma. Kısa, net, madde madde.

## KONU
Kullanıcı RIMA odalarının "güzel tasarlanmış" hissini istiyor (küçük çıplak elmas-platform DEĞİL). Karar: ISO yok, Wang yok → ışık + dekor + derinlik. Prop katmanları: T1 decal 32px / T2 küçük 64px / T3 focal 128px / T4 landmark. Karakter ref = 64px. RIMA = room-based roguelite (açık-dünya değil), demo-finalizasyon, deadline. Sanat kilidi S59 = HIGH TOP-DOWN 3/4 (Hades/Children of Morta ref).

## GROUND-TRUTH
- Dekorasyon-pass kuruldu (RoomDecorationPass, flag-off): CompositionRoleMap zone'ları (CleanCenter/DecoratedEdge/DoorSafety radius=3/FocalCluster/WallBand) + BridsonPoissonAutoPlacer (seeded Poisson) + PropFootprintValidator.
- Oda boyutları var: teardrop 24×18, donut 28×20, boss-oval 36×28; chamber küçük.

## SORULAR
1. **ODA BOYUTU:** Top-down ARPG roguelite'ta bir oda "tasarlanmış" (boş/çok-büyük değil) hissetsin diye oda-tipi başına ideal boyut (tile) nedir? Combat / Elite / Boss / Shop. Boyut + dövüş-alanı + dekor-kenar dengesini açıkla. Hades/CoM nasıl çözüyor?
2. **MANTIKSAL PROP YERLEŞİMİ:** Katmanlı prop'ları (decal/küçük/focal/landmark) zone'lara nasıl dağıtırsın ki oda "el-yapımı tasarlanmış" okunur, rastgele-saçılmış değil? Focal-point seçimi, oda-kimliği landmark (1/oda), asimetri, kenar-hikayesi, yoğunluk-gradyanı. "Decoration intent" modeli nedir?
3. **ROOM DESIGNER İŞLEVSELLİĞİ:** Tekrarlanabilir güzel oda üretmek için bir oda-authoring workflow'unun MİNİMUM hangi özelliklere ihtiyacı var (auto-decorate önizleme, zone-overlay, ışık-önizleme, prop-paleti, validator-feedback)? Tasarımcı verimliliği için olmazsa-olmaz 3 özellik.
4. **IŞIK AUTHORING:** Per-oda mı per-environment mi ışık? Authorable + sahneyi kalabalıklaştırmadan nasıl kurulur? Atmosfer için ışık-kompozisyon ilkeleri (key/fill/accent, brazier point-light, oda-kimliği rengi).

Kapanış: "Deadline'da RIMA'ya en yüksek 'tasarlanmış oda' hissi katacak TEK hamle" — tek cümle.
